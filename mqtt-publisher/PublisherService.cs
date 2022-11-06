
using MqttExample.Model;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.ExampleSensorValidator;
using System.Text.Json;

namespace mqtt_publisher
{

    public interface IMqttPublisherService
    {
        public void startPublisher();
    }

    public class ExampleMqttPublisherService : IMqttPublisherService
    {
        private ExampleConsoleLogger m_exampleLogger;
        private ExampleNonFilePersister m_examplePersister;
        private ExampleSensorValidator m_exampleSensorValidator;
        private ExampleMqttStatusMonitor m_exampleStatusMonitor;
        private MqttExampleClient.MqttExampleClient m_client;

        public ExampleMqttPublisherService()
        {
            m_exampleLogger = new ExampleConsoleLogger();
            m_examplePersister = new ExampleNonFilePersister();
            m_exampleSensorValidator = new ExampleSensorValidator(m_exampleLogger, 5000);
            m_exampleStatusMonitor = new ExampleMqttStatusMonitor(m_exampleLogger);

            m_client = new MqttExampleClient.MqttExampleClient(m_exampleLogger, m_examplePersister, m_exampleSensorValidator, m_exampleStatusMonitor);
        }

        public void startPublisher()
        {
            m_client.connect().Wait();
            m_client.attachSubscriber("response", false).Wait();

            sendTemperature();
        }

        private void sendTemperature()
        {
            Random r = new Random();
            double range = 100;


            int sequenceNumber = 0;
            var task = Task.Run(async () =>
            {
                while (true)
                {

                    if (!m_exampleSensorValidator.anyDataReceived())
                    {
                        m_exampleLogger.Log("No subscriber attached.");
                    }
                    
                    if (m_exampleSensorValidator.IsTimeout())
                    {
                        m_exampleLogger.Log("Timeout of subscriber..");
                    }

                    if (m_exampleSensorValidator.anyDataReceived() && !m_exampleSensorValidator.getLastSequenceNumberReceived().Equals(sequenceNumber))
                    {
                        // error resolution.. message lost? client too slow? network slow?
                        m_exampleLogger.Log("Cannot confirm that a subscriber is in sync.");
                    }

                    var randomTemp = (((r.NextDouble() * 2) - 1.0) * range);

                    sequenceNumber = sequenceNumber > Int32.MaxValue ? 0 : ++sequenceNumber;
                    var msg = new ExampleMsg
                    {
                        m_temperature = randomTemp,
                        m_sequenceNumber = sequenceNumber
                    };
                    string jsonString = JsonSerializer.Serialize(msg);
                    m_client.sendMessage(jsonString, "exampleTemp");
                    await Task.Delay(500);
                }
            });
        }
    }
}


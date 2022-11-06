
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
        public void startPublisher()
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");
            var exampleLogger = new ExampleConsoleLogger();
            var examplePersister = new ExampleFilePersister(logfilePath);
            var exampleSensorValidator = new ExampleSensorValidator(exampleLogger, 1000);
            var exampleStatusMonitor = new ExampleMqttStatusMonitor(exampleLogger);

            var publisher = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister, exampleSensorValidator, exampleStatusMonitor);
            publisher.connect().Wait();

            sendTemperature(publisher);
        }

        private void sendTemperature(MqttExampleClient.MqttExampleClient publisher)
        {
            Random r = new Random();
            double range = 100;


            int sequenceNumber = 0;
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    var randomTemp = (((r.NextDouble() * 2) - 1.0) * range);

                    sequenceNumber = sequenceNumber > Int32.MaxValue ? 0 : ++sequenceNumber;
                    var msg = new ExampleMsg
                    {
                        m_temperature = randomTemp,
                        m_sequenceNumber = sequenceNumber
                    };
                    string jsonString = JsonSerializer.Serialize(msg);
                    publisher.sendMessage(jsonString);
                    await Task.Delay(500);
                }
            });
        }
    }
}


using MqttExample.Model;
using MqttExampleClient;
using MqttExampleClient.Services;

namespace mqtt_subscriber
{
    public interface IMqttSubscriberService
    {
        List<ExampleMsg> GetExampleMsgs();
    }

    public class ExampleMqttSubscriberService : IMqttSubscriberService
    {
        private ExampleConsoleLogger m_exampleLogger;
        private ExampleFilePersister m_examplePersister;
        private ExampleSensorValidator m_exampleSensorValidator;
        private ExampleMqttStatusMonitor m_exampleStatusMonitor;
        private MqttExampleClient.MqttExampleClient m_client;

        public ExampleMqttSubscriberService()
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.txt");

            m_exampleLogger = new ExampleConsoleLogger();
            m_examplePersister = new ExampleFilePersister(logfilePath);
            m_exampleSensorValidator = new ExampleSensorValidator(m_exampleLogger);
            m_exampleStatusMonitor = new ExampleMqttStatusMonitor(m_exampleLogger);
            m_client = new MqttExampleClient.MqttExampleClient(m_exampleLogger, m_examplePersister, m_exampleSensorValidator, m_exampleStatusMonitor);

            validateTimeout(m_exampleSensorValidator);
            m_client.connect().Wait();
            m_client.attachSubscriber("exampleTemp", true).Wait();
        }

        public List<ExampleMsg> GetExampleMsgs()
        {
            return m_exampleStatusMonitor.GetExampleMsgs();
        }

        private void validateTimeout(ExampleSensorValidator exampleSensorValidator)
        {
            var task = Task.Run(async () =>
            {
                while (true)
                {
                    exampleSensorValidator.checkTimeOutAndLog();
                    await Task.Delay(500);
                }
            });
        }


    }
}

using MqttExample.Model;
using MqttExampleClient;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.ExampleSensorValidator;

namespace mqtt_subscriber
{
    public interface IMqttSubscriberService
    {
        void startSubscriber();
        List<ExampleMsg> GetExampleMsgs();
    }

    public class ExampleMqttSubscriberService : IMqttSubscriberService
    {
        private ExampleConsoleLogger m_exampleLogger;
        private ExampleFilePersister m_examplePersister;
        private ExampleSensorValidator m_exampleSensorValidator;
        private ExampleMqttStatusMonitor m_exampleStatusMonitor;
        private MqttExampleClient.MqttExampleClient? m_subscriber;

        public ExampleMqttSubscriberService()
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");

            m_exampleLogger = new ExampleConsoleLogger();
            m_examplePersister = new ExampleFilePersister(logfilePath);
            m_exampleSensorValidator = new ExampleSensorValidator(m_exampleLogger);
            m_exampleStatusMonitor = new ExampleMqttStatusMonitor(m_exampleLogger);
            

        }

        public List<ExampleMsg> GetExampleMsgs()
        {
            return m_exampleStatusMonitor.GetExampleMsgs();
        }

        public void startSubscriber()
        {
            if (m_subscriber == null)
            {
                m_subscriber = new MqttExampleClient.MqttExampleClient(m_exampleLogger, m_examplePersister, m_exampleSensorValidator, m_exampleStatusMonitor);
                validateTimeout(m_exampleSensorValidator);
                m_subscriber.connect().Wait();
                m_subscriber.attachSubscriber().Wait();
            }
        }

        private void validateTimeout(MqttExampleClient.ExampleSensorValidator.ExampleSensorValidator exampleSensorValidator)
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

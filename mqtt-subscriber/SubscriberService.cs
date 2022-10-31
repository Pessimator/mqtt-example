using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.ExampleSensorValidator;

namespace mqtt_subscriber
{
    public interface IMqttSubscriberService
    {
        void startSubscriber();
    }

    public class ExampleMqttSubscriberService : IMqttSubscriberService
    {
        public void startSubscriber()
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");
            var exampleLogger = new ExampleConsoleLogger();
            var examplePersister = new ExampleFilePersister(logfilePath);
            var exampleSensorValidator = new ExampleSensorValidator(exampleLogger);


            var subscriber = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister, exampleSensorValidator);
            validateTimeout(exampleSensorValidator);
            subscriber.connect().Wait();
            subscriber.attachSubscriber().Wait();
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

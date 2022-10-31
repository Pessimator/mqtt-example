using mqtt_subscriber;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.ExampleSensorValidator;

namespace mqtt_publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");
            var exampleLogger = new ExampleConsoleLogger();
            var examplePersister = new ExampleFilePersister(logfilePath);
            var exampleSensorValidator = new MqttExampleClient.ExampleSensorValidator.ExampleSensorValidator(exampleLogger, 1000);

            
            var subscriber = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister, exampleSensorValidator);
            validateTimeout(exampleSensorValidator);
            subscriber.connect().Wait();
            subscriber.attachSubscriber().Wait();

            Console.ReadLine();
        }

        private static void validateTimeout(MqttExampleClient.ExampleSensorValidator.ExampleSensorValidator exampleSensorValidator)
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
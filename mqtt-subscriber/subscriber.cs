using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;

namespace mqtt_publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");
            var exampleLogger = new ExampleConsoleLogger();
            var examplePersister = new ExampleFilePersister(logfilePath);

            var subscriber = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister);
            subscriber.connect().Wait();



            subscriber.attachSubscriber().Wait();

            Console.ReadLine();
        }
    }
}
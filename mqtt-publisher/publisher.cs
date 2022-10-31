using MqttExample.Model;
using MqttExampleClient;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using System.Text.Json;


namespace mqtt_publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            string logfilePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "logfile.json");
            var exampleLogger = new ExampleConsoleLogger();
            var examplePersister = new ExampleFilePersister(logfilePath);

            var publisher = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister);
            publisher.connect().Wait();

            sendTemperature(publisher);
            publisher.disconnect();

            Console.ReadLine();
        }

        private static void sendTemperature(MqttExampleClient.MqttExampleClient publisher)
        {
            float[] temperatures = { 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f, 17.0f, 18.0f, 19.0f, -1.0f, 0.0f };
            var index = -1;
            while (true)
            {
                index++;
                index = index > temperatures.Length - 1 ? 0 : index;
                var msg = new ExampleMsg
                {
                    m_temperature = temperatures[index],
                    m_sequenceNumber = index
                };
                string jsonString = JsonSerializer.Serialize(msg);
                publisher.sendMessage(jsonString);  
                Thread.Sleep(5000);
            }

        }
    }
}
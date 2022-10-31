using MqttExample.Model;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.ExampleSensorValidator;
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
            var exampleSensorValidator = new ExampleSensorValidator(exampleLogger, 1000);


            var publisher = new MqttExampleClient.MqttExampleClient(exampleLogger, examplePersister, exampleSensorValidator);
            publisher.connect().Wait();

            sendTemperature(publisher);
            publisher.disconnect();

            Console.ReadLine();
        }

        private static void sendTemperature(MqttExampleClient.MqttExampleClient publisher)
        {
            float[] temperatures = { 10.0f, 11.0f, 12.0f, 13.0f, 14.0f, 15.0f, 16.0f, 17.0f, 18.0f, 19.0f, -1.0f, 0.0f };
            int sequenceNumber = 0;
            var index = -1;
            while (true)
            {
                sequenceNumber = sequenceNumber > Int32.MaxValue ? 0 : ++sequenceNumber;

                index++;
                index = index > temperatures.Length - 1 ? 0 : index;
                var msg = new ExampleMsg
                {
                    m_temperature = temperatures[index],
                    m_sequenceNumber = sequenceNumber
                };
                string jsonString = JsonSerializer.Serialize(msg);
                publisher.sendMessage(jsonString);  
                Thread.Sleep(500);
            }

        }
    }
}
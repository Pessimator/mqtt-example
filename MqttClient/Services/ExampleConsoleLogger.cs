using MqttExampleClient.Interfaces;

namespace MqttExampleClient.Services
{

    public class ExampleConsoleLogger : IExampleLogger
    {
        public void Log(string message, DateTime? timestamp = null)
        {
            string output = timestamp != null ? timestamp.ToString() + ": " : string.Empty;
            output += message;
            Console.WriteLine(output);
        }
    }
}

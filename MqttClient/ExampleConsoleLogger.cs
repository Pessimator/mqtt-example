namespace MqttExampleClient.ExampleLogger
{
    public interface IExampleLogger
    {
        public void Log(string message);
    }

    public class ExampleConsoleLogger : IExampleLogger
    {
        public void Log(string message)
        {
            System.Console.WriteLine(message);  
        }
    }
}

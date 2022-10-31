namespace MqttExampleClient.ExampleLogger
{
    public interface IExampleLogger
    {
        public void Log(string message, DateTime? timestamp = null);
    }

    public class ExampleConsoleLogger : IExampleLogger
    {
        public void Log(string message, DateTime? timestamp = null)
        {
            string output = (timestamp != null) ? timestamp.ToString() + ": " : String.Empty;
            output += message;
            System.Console.WriteLine(output);  
        }
    }
}

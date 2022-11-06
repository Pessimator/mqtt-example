namespace MqttExampleClient.Interfaces
{
    public interface IExampleLogger
    {
        public void Log(string message, DateTime? timestamp = null);
    }
}

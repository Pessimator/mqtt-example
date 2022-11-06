namespace MqttExampleClient.ExamplePersister
{
    public interface IExamplePersister
    {
        public Task WriteOut(string messaage, bool append = true);
    }
}

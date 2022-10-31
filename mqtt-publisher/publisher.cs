namespace mqtt_publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = new ExampleMqttPublisherService();
            publisher.startPublisher();
            Console.ReadLine();
        }
    }
}
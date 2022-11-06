namespace mqtt_subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ExampleMqttSubscriberService();
            service.startSubscriber();
            Console.ReadLine();
        }


    }
}
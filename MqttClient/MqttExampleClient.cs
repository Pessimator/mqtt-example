
using MqttExample.Model;
using MqttExampleClient.ExampleLogger;
using MqttExampleClient.ExamplePersister;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

namespace MqttExampleClient;

public class MqttExampleClient
{
    public MqttExampleClient(IExampleLogger logger, IExamplePersister persister)
    {
        m_mqttFactory = new MqttFactory();
        m_mqttClient = m_mqttFactory.CreateMqttClient();
        m_logger = logger;
        m_persister = persister;
       
    }

    private MqttFactory m_mqttFactory { get; }
    private IMqttClient m_mqttClient { get; }
    private IExampleLogger m_logger { get; }

    private IExamplePersister m_persister;

    public async Task connect()
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost").Build();
        await m_mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        m_logger.Log("The MQTT client is connected.");
    }


    public void disconnect()
    {
        var mqttClientDisconnectOptions = m_mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
        m_mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
        m_logger.Log("The MQTT client is disconnected.");
    }


    public void sendMessage(string message)
    {
        var applicationMessage = new MqttApplicationMessageBuilder()
       .WithTopic("topic")
       .WithPayload(message)
       .Build();
        m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        m_logger.Log("Message sent.");
    }

    public async Task attachSubscriber()
    {
        m_mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            m_logger.Log("Received application message.");
            string decodedPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var msg = JsonSerializer.Deserialize(decodedPayload, typeof(ExampleMsg));
            m_logger.Log(decodedPayload);
            m_persister.WriteOut(decodedPayload);
            return Task.CompletedTask;
        };

        var mqttSubscribeOptions = m_mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(
        f =>
        {
            f.WithTopic("topic");
        })
        .Build();
        await m_mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        m_logger.Log("Subscriber attached.");        
    }
}
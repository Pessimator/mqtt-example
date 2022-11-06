
using MqttExample.Model;
using MqttExampleClient.ExamplePersister;
using MqttExampleClient.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

namespace MqttExampleClient;

public class MqttExampleClient
{
    public MqttExampleClient(IExampleLogger logger, IExamplePersister persister, ISensorValidator validator, IMqttStatusMonitor monitor)
    {
        m_mqttFactory = new MqttFactory();
        m_mqttClient = m_mqttFactory.CreateMqttClient();
        m_logger = logger;
        m_persister = persister;
        m_validator = validator;
        m_monitor = monitor;
    }

    private MqttFactory m_mqttFactory { get; }
    private IMqttClient m_mqttClient { get; }
    private IExampleLogger m_logger { get; }

    private IExamplePersister m_persister { get; }

    private ISensorValidator m_validator { get; }

    private IMqttStatusMonitor m_monitor;

    public async Task connect()
    {
        var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("localhost").WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce).Build();
        await m_mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
        m_logger.Log("The MQTT client is connected.");
    }


    public void disconnect()
    {
        var mqttClientDisconnectOptions = m_mqttFactory.CreateClientDisconnectOptionsBuilder().Build();
        m_mqttClient.DisconnectAsync(mqttClientDisconnectOptions, CancellationToken.None);
        m_logger.Log("The MQTT client is disconnected.");
    }


    public void sendMessage(string message, string topic)
    {
        var applicationMessage = new MqttApplicationMessageBuilder()
       .WithTopic(topic)
       .WithPayload(message)
       .Build();
        m_mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
        m_logger.Log("[" + topic + "] Sent message: " + message);
    }

    public async Task attachSubscriber(string topic, bool subscriber)
    {
        m_mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            var timestamp = DateTime.Now.ToLocalTime();
            string decodedPayload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            var msg = JsonSerializer.Deserialize<ExampleMsg>(decodedPayload);
            m_logger.Log("[" + topic + "] Received message: " + decodedPayload);
            m_persister.WriteOut(decodedPayload);
            m_validator.updateTimestamp(timestamp, msg.m_sequenceNumber);
            m_monitor.addTemperatureMessage(msg);
            
            
            if (subscriber)
            {
                sendMessage(decodedPayload, "response");
            }
            return Task.CompletedTask;
        };

        var mqttSubscribeOptions = m_mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(
        f =>
        {
            f.WithTopic(topic);
        })
        .Build();
        await m_mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
        m_logger.Log("Subscriber attached to tpic: " + topic);        
    }
}
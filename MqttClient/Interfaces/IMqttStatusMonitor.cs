using MqttExample.Model;

namespace MqttExampleClient.Interfaces
{
    public interface IMqttStatusMonitor
    {
        void addTemperatureMessage(ExampleMsg msg);
        List<ExampleMsg> GetExampleMsgs();
    }



}

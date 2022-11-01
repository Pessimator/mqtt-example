using MqttExample.Model;
using MqttExampleClient.ExampleLogger;

namespace MqttExampleClient.ExampleSensorValidator
{

    public interface IMqttStatusMonitor
    {
        void addTemperatureMessage(ExampleMsg msg);
        List<ExampleMsg> GetExampleMsgs();
    }

    public class ExampleMqttStatusMonitor : IMqttStatusMonitor
    {

        public ExampleMqttStatusMonitor(IExampleLogger logger)
        {
            m_logger = logger;
        }

        private IExampleLogger m_logger;
        private List<ExampleMsg> m_temperatureList = new List<ExampleMsg>();

        public void addTemperatureMessage(ExampleMsg msg)
        {
            m_temperatureList.Add(msg);
            if (m_temperatureList.Count() > 20)
            {
                m_temperatureList.RemoveAt(0);
            }
        }

        public List<ExampleMsg> GetExampleMsgs()
        {
            return m_temperatureList;
        }


    }



}

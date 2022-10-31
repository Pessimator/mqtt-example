using MqttExampleClient.ExampleLogger;

namespace MqttExampleClient.ExampleSensorValidator
{

    public interface ISensorValidator
    {
        bool IsTimeout();
        void updateTimestamp(DateTime timestamp);
    }

    public class ExampleSensorValidator : ISensorValidator
    {

        public ExampleSensorValidator(IExampleLogger logger, double timeoutValueInMs = 5000)
        {
            m_logger = logger;
            m_timeoutValueInMs = timeoutValueInMs;

        }

        bool m_dataReceived = false;
        DateTime m_lastTimeStamp = DateTime.MinValue;
        private IExampleLogger m_logger;
        double m_timeoutValueInMs = 1000;


        public void updateTimestamp(DateTime timestamp)
        {
            m_dataReceived = true;
            m_lastTimeStamp = m_lastTimeStamp > timestamp ? m_lastTimeStamp : timestamp;
        } 


        public bool IsTimeout()
        {
            if (m_dataReceived)
            {
                return (Math.Abs((m_lastTimeStamp - DateTime.Now).TotalMilliseconds) > m_timeoutValueInMs) ? true : false;
            }
            return false;
        }

        public void checkTimeOutAndLog()
        {
            if (IsTimeout())
            {
                m_logger.Log("Timeout has been detected!");
            }
        }
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt_subscriber
{

    internal interface ISensorValidator
    {
        bool IsTimeout();
        void updateTimestamp(DateTime timestamp);
    }

    class ExampleSensorValidator : ISensorValidator
    {

        public ExampleSensorValidator(double timeoutValueInMs = 1000)
        {
            m_timeoutValueInMs = timeoutValueInMs;
        }

        bool m_dataReceived = false;
        DateTime m_lastTimeStamp = DateTime.MinValue;
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

    }



}

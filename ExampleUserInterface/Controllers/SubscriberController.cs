using Microsoft.AspNetCore.Mvc;
using MqttExample.Model;
using mqtt_subscriber;

namespace Interface.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {

        private IMqttSubscriberService m_mqqtSubscriber;

        public SubscriberController(IMqttSubscriberService mqqtSubscriber)
        {
            this.m_mqqtSubscriber = mqqtSubscriber;
        }


        [HttpGet]
        public IEnumerable<ExampleMsg> Get()
        {
            var tempMsg = m_mqqtSubscriber.GetExampleMsgs();
            tempMsg.Sort();
            return tempMsg;
        }
    }
}
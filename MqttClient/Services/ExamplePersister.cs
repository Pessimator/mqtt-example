using System.IO;
using MqttExampleClient.ExamplePersister;

namespace MqttExampleClient.Services
{

    public class ExampleFilePersister : IExamplePersister
    {
        private string m_filePath;

        public ExampleFilePersister(string filePath)
        {
            m_filePath = filePath;
            WriteOut(DateTime.Now.ToLocalTime().ToString(), false).Wait();
        }
        public async Task WriteOut(string message, bool append = true)
        {
            using (FileStream stream = new FileStream(m_filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(DateTime.Now.ToLocalTime().ToString() + ": " + message);
            }
        }
    }

    public class ExampleNonFilePersister : IExamplePersister
    {
        public Task WriteOut(string messaage, bool append = true)
        {
            // do nothing by design
            return Task.FromResult(true);
        }
    }
}

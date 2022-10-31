using System.IO;

namespace MqttExampleClient.ExamplePersister
{
    public interface IExamplePersister
    {
        public Task WriteOut(string messaage, bool append = true);
    }

    public class ExampleFilePersister : IExamplePersister
    {
        private string m_filePath;

        public ExampleFilePersister(string filePath)
        {
            m_filePath = filePath;
            this.WriteOut(DateTime.Now.ToLocalTime().ToString(), false).Wait();
        }
        public async Task WriteOut(string message, bool append = true)
        {
            using (FileStream stream = new FileStream(m_filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            using (StreamWriter sw = new StreamWriter(stream))
            {
                await sw.WriteLineAsync(message);
            }
        }
    }
}

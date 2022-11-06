namespace MqttExampleClient.Interfaces
{
    public interface ISensorValidator
    {
        bool IsTimeout();

        void updateTimestamp(DateTime timestamp, int sequenceNumber);

        int getLastSequenceNumberReceived();

        bool anyDataReceived();
    }



}


namespace MqttExample.Model;

public class ExampleMsg : IEquatable<ExampleMsg>, IComparable<ExampleMsg>
{
    public int m_sequenceNumber { get; set; }
    public double m_temperature { get; set; }


    public bool Equals(ExampleMsg? other)
    {
        return other is null ? false : other.m_sequenceNumber.Equals(m_sequenceNumber);
    }

    int IComparable<ExampleMsg>.CompareTo(ExampleMsg? other)
    {
        return other == null ? 1 : m_sequenceNumber.CompareTo(other.m_sequenceNumber);
    }
}

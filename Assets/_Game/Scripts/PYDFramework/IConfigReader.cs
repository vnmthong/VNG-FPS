namespace PYDFramework.Config
{
    public interface IConfigReader
    {
        bool HasNext();
        bool ReadBool();
        int ReadInt();
        long ReadLong();
        float ReadFloat();
        double ReadDouble();
        string ReadString();
        bool[] ReadBoolArr();
        int[] ReadIntArr();
        float[] ReadFloatArr();
        double[] ReadDoubleArr();
        long[] ReadLongArr();
        string[] ReadStringArr();
    }
}
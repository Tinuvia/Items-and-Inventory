using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileReadWrite
{
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite)
    {
        using (Stream stream = File.Open(filePath, FileMode.Create))
            //since we are wrapping this statement in "using", the file will automatically close when the statement ends
            // i.e we don't have to specifically call stream.Close();
        {
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using(Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
}

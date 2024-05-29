using System.Xml.Serialization;

namespace Vids.Service
{
    public class XmlService<T>
    {
        public string SerialiseObject(T obj)
        {
            if (obj != null)
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());

                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj);
                    return writer.ToString();
                }
            }else
            {
                return string.Empty;
            }
        }

        public bool SerialiseObject(string filePath, T? obj)
        {
            string fileDirectoryPath = GetFileDirectoryPath(filePath);
            Directory.CreateDirectory(fileDirectoryPath);

            using (StreamWriter streamWriter = new StreamWriter(filePath, false))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(streamWriter, obj);
            }

            return true;
        }

        public T? DeserialiseObject(string filePath)
        {
            T? obj = default(T);

            if (File.Exists(filePath))
            {
                using (Stream streamReader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    obj = (T?)xmlSerializer.Deserialize(streamReader);
                }
            }

            return obj;
        }

        public T? DeserializeObjectFromString(string xmlStr)
        {
            T? obj = default(T);

            using (StringReader stringReader = new StringReader(xmlStr))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                obj = (T?)xmlSerializer.Deserialize(stringReader);
            }

            return obj;
        }

        private string GetFileDirectoryPath(string filePath)
        {
            int lastSlashIndex = filePath.LastIndexOf('\\');
            string fileDirectoryPath = filePath.Substring(0, lastSlashIndex + 1).Trim();

            return fileDirectoryPath;
        }

    }
}
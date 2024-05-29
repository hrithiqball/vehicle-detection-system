using System.Xml.Serialization;

namespace Vids.Model
{
    public class DbInfo
    {
        public string ServerName { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; } = 5432;

        [XmlIgnore]
        public bool IsOnline { get; set; } = true;
        [XmlIgnore]
        public string ConnectionStr { get; set; } = string.Empty;
    }
}

using System.Text.Json.Serialization;

namespace Project.Controllers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RPGClass : Byte
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3,
        Healer = 4,
    }
}
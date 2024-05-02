using System.Text.Json.Serialization;

namespace Project.Controllers
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RPGClass
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3,
        Healer = 4,
    }
}
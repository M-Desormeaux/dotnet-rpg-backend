using System;
using System.Text.Json.Serialization;

namespace NetRPG.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ClassTypes
    {
        Knight,
        Mage,
        Assassin,
        Cleric,
        Barbarian,
        Theif
    }
}
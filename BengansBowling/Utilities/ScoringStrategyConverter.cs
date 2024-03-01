using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BengansBowling.Strategies;
using System;

public class ScoringStrategyConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) {
        return (objectType == typeof(IScoringStrategy));
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        JObject jo = JObject.Load(reader);

        // Check if TypeName exists and is not null
        JToken typeNameToken = jo["TypeName"];
        if (typeNameToken == null) {
            // Handle the case where TypeName is missing or ScoringStrategy is empty
            // You could return null, or a default instance of a scoring strategy
            return null; // Or return a new DefaultScoringStrategy() if you have a default implementation
        }

        string typeName = typeNameToken.Value<string>();
        switch (typeName) {
            case "ProfessionalScoringStrategy":
            return JsonConvert.DeserializeObject<ProfessionalScoringStrategy>(jo.ToString(), new JsonSerializerSettings { Converters = new[] { this } });
            case "AmateurScoringStrategy":
            return JsonConvert.DeserializeObject<AmateurScoringStrategy>(jo.ToString(), new JsonSerializerSettings { Converters = new[] { this } });
            default:
            throw new Exception($"Unknown type: {typeName}");
        }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        JObject jo = new JObject();
        jo.Add("TypeName", value.GetType().Name);
        foreach (var prop in value.GetType().GetProperties()) {
            if (prop.CanRead) {
                jo.Add(prop.Name, JToken.FromObject(prop.GetValue(value), serializer));
            }
        }
        jo.WriteTo(writer);
    }
}

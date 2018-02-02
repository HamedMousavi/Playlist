using MyMemory.Domain.Abstract;
using Newtonsoft.Json;


namespace MyMemory.Domain
{

    public class JsonStringSerializer : ISerializer<string>
    {

        public string Serialize<TIn>(TIn serializable)
        {
            return JsonConvert.SerializeObject(
                serializable,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                });
        }


        public TIn Deserialize<TIn>(string serialized)
        {
            return string.IsNullOrWhiteSpace(serialized)
                ? default(TIn)
                : JsonConvert.DeserializeObject<TIn>(
                    serialized,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
        }


        public void Deserialize<TIn>(string serialized, TIn serializable)
        {
            if (!string.IsNullOrWhiteSpace(serialized) && serializable != null)
                JsonConvert.PopulateObject(
                    serialized,
                    serializable,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
        }
    }
}
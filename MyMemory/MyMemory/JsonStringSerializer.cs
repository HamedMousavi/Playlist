using Newtonsoft.Json;


namespace MyMemory
{

    public class JsonStringSerializer : ISerializer<string>
    {

        public string Serialize<TIn>(TIn serializable)
        {
            return JsonConvert.SerializeObject(serializable);
        }


        public TIn Deserialize<TIn>(string serialized)
        {
            return string.IsNullOrWhiteSpace(serialized)
                ? default(TIn)
                : JsonConvert.DeserializeObject<TIn>(serialized);
        }


        public void Deserialize<TIn>(string serialized, TIn serializable)
        {
            if (!string.IsNullOrWhiteSpace(serialized) && serializable != null)
                JsonConvert.PopulateObject(serialized, serializable);
        }
    }
}
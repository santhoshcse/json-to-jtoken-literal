namespace JSON_to_ObjectInitializer
{
    using Newtonsoft.Json.Linq;

    public interface IConverter
    {
        public string ConstructObjectInitializerFormat(JToken jObj);
    }
}

namespace JSON2ObjInitializer_BackEnd
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Text;

    public static class JsonSerializerUtil
    {
        public static StringBuilder Sb { get; set; } = new StringBuilder();

        public static string Serialize(object inputObject)
        {
            var jsonString = JsonConvert.SerializeObject(inputObject);
            return jsonString;
        }

        public static JToken Deserialize(string jsonString)
        {
            var jsonObject = JsonConvert.DeserializeObject(jsonString);
            var jObj = JObject.FromObject(jsonObject);
            return jObj;
        }

        public static string ConstructObjectInitializerFormat(JToken jObj, int type = 0)
        {
            // Construction based on JSON.NET types
            if (type == 0)
            {
                Sb = new StringBuilder();
                ConstructObject(jObj, isLast: true);
                string result = Sb.ToString();
                result = result.Replace(@"bool~True", "true");
                result = result.Replace(@"bool~False", "false");
                return result;
            }

            return string.Empty;
        }

        private static void ConstructObject(JToken jObj, bool isLast = false)
        {
            if (jObj.Type == JTokenType.Object)
            {
                Sb.Append("new JObject(");
                foreach (JToken child in jObj.Children())
                {
                    bool isLastChild = child.Next == null;
                    ConstructObject(child, isLastChild);
                }

                Sb.Append(")");
                if (!isLast)
                {
                    Sb.Append(", ");
                }
            }
            else if (jObj.Type == JTokenType.Array)
            {
                Sb.Append("new JArray(");
                foreach (JToken child in jObj.Children())
                {
                    ConstructObject(child);
                    if (child.Next != null)
                    {
                        Sb.Append(", ");
                    }
                }

                Sb.Append(")");
                if (!isLast)
                {
                    Sb.Append(", ");
                }
            }
            else if (jObj.Type == JTokenType.Property)
            {
                Sb.Append("new JProperty(");
                foreach (JToken child in jObj.Children())
                {
                    Sb.Append(string.Format("\"{0}\"", child.Path));
                    Sb.Append(", ");
                    ConstructObject(child, true);
                }

                Sb.Append(")");
                if (!isLast)
                {
                    Sb.Append(", ");
                }
            }
            else if (jObj.Type == JTokenType.String)
            {
                var stringLiteral = string.Format("\"{0}\"", jObj.Value<string>());
                Sb.Append(stringLiteral);
            }
            else if (jObj.Type == JTokenType.Integer)
            {
                Sb.Append(jObj.Value<int>());
            }
            else if (jObj.Type == JTokenType.Float)
            {
                Sb.Append(jObj.Value<float>());
            }
            else if (jObj.Type == JTokenType.Boolean)
            {
                // Adding bool~ to the value to have proper bool literal in the output
                // Ex: To change False/True to false/true
                Sb.Append("bool~" + jObj.Value<bool>());
            }
            else if (jObj.Type == JTokenType.Null)
            {
                string NULL_STR = null;
                Sb.Append(NULL_STR);
            }
        }
    }
}

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
            var jObj = JToken.FromObject(jsonObject);
            return jObj;
        }

        public static string ConstructObjectInitializerFormat(JToken jObj, int type = 0)
        {
            // Construction based on JSON.NET types
            if (type == 0)
            {
                Sb = new StringBuilder();
                ConstructObject(jObj, 0, isLast: true);
                string result = Sb.ToString();
                result = result.Replace(@"bool~True", "true");
                result = result.Replace(@"bool~False", "false");
                return result;
            }

            return string.Empty;
        }

        private static void ConstructObject(JToken jObj, int level, bool isLast = false, bool isNewLine = true)
        {
            if (jObj.Type == JTokenType.Object)
            {
                Indent(level, isNewLine);
                Sb.Append("new JObject\n");
                Indent(level);
                Sb.Append("{\n");
                foreach (JToken child in jObj.Children())
                {
                    bool isLastChild = child.Next == null;
                    ConstructObject(child, level + 1, isLastChild);
                }

                Sb.Append("\n");
                Indent(level);
                Sb.Append("}");
                if (!isLast)
                {
                    Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.Array)
            {
                Indent(level, isNewLine);
                Sb.Append("new JArray\n");
                Indent(level);
                Sb.Append("{\n");
                foreach (JToken child in jObj.Children())
                {
                    bool isLastChild = child.Next == null;
                    ConstructObject(child, level + 1, isLastChild);
                    if (child.Next != null && (child.Type != JTokenType.Object && child.Type != JTokenType.Array && child.Type != JTokenType.Property))
                    {
                        Sb.Append(", ");
                    }
                }

                Sb.Append("\n");
                Indent(level);
                Sb.Append("}");
                if (!isLast)
                {
                    Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.Property)
            {
                Indent(level);
                Sb.Append("new JProperty(");
                foreach (JToken child in jObj.Children())
                {
                    Sb.Append(string.Format("\"{0}\"", GetPropertyName(child.Path)));
                    Sb.Append(", ");
                    ConstructObject(child, level, true, false);
                }

                Sb.Append(")");
                if (!isLast)
                {
                    Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.String)
            {
                Indent(level, isNewLine);
                var stringLiteral = string.Format("\"{0}\"", jObj.Value<string>());
                Sb.Append(stringLiteral);
            }
            else if (jObj.Type == JTokenType.Integer)
            {
                Indent(level, isNewLine);
                Sb.Append(jObj.Value<int>());
            }
            else if (jObj.Type == JTokenType.Float)
            {
                Indent(level, isNewLine);
                Sb.Append(jObj.Value<float>());
            }
            else if (jObj.Type == JTokenType.Boolean)
            {
                Indent(level, isNewLine);

                // Adding `bool~` to the value to have proper boolean literal in the output
                // Ex: To change False/True to false/true
                Sb.Append("bool~" + jObj.Value<bool>());
            }
            else if (jObj.Type == JTokenType.Null)
            {
                Indent(level, isNewLine);
                var nullLiteral = "null";
                Sb.Append(nullLiteral);
            }
        }

        private static string GetPropertyName(string propertyPath)
        {
            int propertyPathLength = propertyPath.Length;
            string propertyName;
            if (propertyPath.EndsWith(']'))
            {
                if (propertyPath[propertyPathLength - 2] == '\'')
                {
                    // abcde['ab'] => 11-5-4
                    int propertyKeyStartingIndex = propertyPath.LastIndexOf("['");
                    propertyName = propertyPath.Substring(propertyKeyStartingIndex + 2, propertyPathLength - propertyKeyStartingIndex - 4);
                }
                else
                {
                    // abcde["ab"] => 11-5-4
                    // to be deleted: this case won't occur
                    int propertyKeyStartingIndex = propertyPath.LastIndexOf("[\"");
                    propertyName = propertyPath.Substring(propertyKeyStartingIndex + 2, propertyPathLength - propertyKeyStartingIndex - 4);
                }

                if (propertyName.Contains('"'))
                {
                    propertyName = propertyName.Replace("\"", "\\\"");
                }
            }
            else
            {
                // abcde.ab => 8-5-1
                int propertyKeyStartingIndex = propertyPath.LastIndexOf(".");
                propertyName = propertyPath.Substring(propertyKeyStartingIndex + 1, propertyPathLength - propertyKeyStartingIndex - 1);
            }

            return propertyName;
        }

        private static void Indent(int tabIndentCount, bool isNewLine = true)
        {
            if (isNewLine)
            {
                for (int i = 0; i < tabIndentCount; i++)
                {
                    Sb.Append("\t");
                }
            }
        }
    }
}

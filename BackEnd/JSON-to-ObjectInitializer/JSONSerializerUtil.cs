namespace JSON_to_ObjectInitializer
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public static class JsonSerializerUtil
    {
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

        public static string GetPropertyName(string propertyPath)
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

        public static StringBuilder Indent(int tabIndentCount, bool isNewLine = true)
        {
            StringBuilder Sb = new StringBuilder();
            if (isNewLine)
            {
                for (int i = 0; i < tabIndentCount; i++)
                {
                    Sb.Append("\t");
                }
            }

            return Sb;
        }

        public static string ConvertToPascalCase(string value)
        {
            value = value.ToLower().Replace("_", " ").Replace("-", " ");
            TextInfo info = CultureInfo.CurrentCulture.TextInfo;
            value = info.ToTitleCase(value).Replace(" ", string.Empty);
            return value;
        }

        public static string Normalize(string value)
        {
            // Removing leading digits from the value
            value = value.TrimStart(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            // Removing unsupported chars for the property name in C#
            value = string.Join("", value.ToCharArray().Where(val => char.IsLetterOrDigit(val)).ToArray());

            // Converting value to PascalCase for the property
            value = ConvertToPascalCase(value);
            return value;
        }
    }
}

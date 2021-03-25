namespace JSON_to_ObjectInitializer
{
    using Newtonsoft.Json.Linq;
    using System.Text;

    public class TypedConverter : IConverter
    {
        private StringBuilder Sb { get; set; } = new StringBuilder();

        private int arrayTypeIndex = 0;

        public string ConstructObjectInitializerFormat(JToken jObj)
        {
            // Construction based on Property values
            // TODO: infer data type of the values and define Types based on that.
            this.ConstructObject(jObj, 0, "Root", isLast: true);
            string result = this.Sb.ToString();
            result = result.Replace(@"bool~True", "true");
            result = result.Replace(@"bool~False", "false");
            return result;
        }

        private void ConstructObject(JToken jObj, int level, string propertyName, bool isLast = false, bool isNewLine = true, JTokenType previousType = JTokenType.Null)
        {
            if (jObj.Type == JTokenType.Object)
            {
                this.Indent(level, isNewLine);
                this.Sb.Append("new " + propertyName + "\n");
                this.Indent(level);
                this.Sb.Append("{\n");
                foreach (JToken child in jObj.Children())
                {
                    bool isLastChild = child.Next == null;
                    this.ConstructObject(child, level + 1, "", isLast: isLastChild);
                }

                this.Sb.Append("\n");
                this.Indent(level);
                this.Sb.Append("}");
                if (!isLast)
                {
                    this.Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.Array)
            {
                this.Indent(level, isNewLine);
                if (previousType == JTokenType.Array)
                {
                    propertyName = propertyName + arrayTypeIndex;
                    arrayTypeIndex++;
                }

                this.Sb.Append("new List<" + propertyName + ">\n");
                this.Indent(level);
                this.Sb.Append("{\n");
                foreach (JToken child in jObj.Children())
                {
                    bool isLastChild = child.Next == null;
                    this.ConstructObject(child, level + 1, propertyName, isLast: isLastChild, previousType: JTokenType.Array);
                    if (child.Next != null && (child.Type != JTokenType.Object && child.Type != JTokenType.Array && child.Type != JTokenType.Property))
                    {
                        this.Sb.Append(", ");
                    }
                }

                this.Sb.Append("\n");
                this.Indent(level);
                this.Sb.Append("}");
                if (!isLast)
                {
                    this.Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.Property)
            {
                this.Indent(level);
                foreach (JToken child in jObj.Children())
                {
                    var normalizedPropertyName = JsonSerializerUtil.Normalize(JsonSerializerUtil.GetPropertyName(child.Path));
                    this.Sb.Append(string.Format("{0}", normalizedPropertyName));
                    this.Sb.Append(" = ");
                    this.ConstructObject(child, level, normalizedPropertyName, isLast: true, isNewLine: false);
                }

                if (!isLast)
                {
                    this.Sb.Append(",\n");
                }
            }
            else if (jObj.Type == JTokenType.String)
            {
                this.Indent(level, isNewLine);
                var stringLiteral = string.Format("\"{0}\"", jObj.Value<string>());
                this.Sb.Append(stringLiteral);
            }
            else if (jObj.Type == JTokenType.Integer)
            {
                this.Indent(level, isNewLine);
                this.Sb.Append(jObj.Value<int>());
            }
            else if (jObj.Type == JTokenType.Float)
            {
                this.Indent(level, isNewLine);
                this.Sb.Append(jObj.Value<float>());
            }
            else if (jObj.Type == JTokenType.Boolean)
            {
                this.Indent(level, isNewLine);

                // Adding `bool~` to the value to have proper boolean literal in the output
                // Ex: To change False/True to false/true
                this.Sb.Append("bool~" + jObj.Value<bool>());
            }
            else if (jObj.Type == JTokenType.Null)
            {
                this.Indent(level, isNewLine);
                var nullLiteral = "null";
                this.Sb.Append(nullLiteral);
            }
        }

        private void Indent(int tabIndentCount, bool isNewLine = true)
        {
            this.Sb.Append(JsonSerializerUtil.Indent(tabIndentCount, isNewLine));
        }
    }
}

using System;

namespace JSON2ObjInitializer_BackEnd
{
    public class Program
    {
        static void Main(string[] args)
        {
            var jObj = new
            {
                Name = "Santhosh",
                Age = 28,
                Teams = new [] { "Services", "UI", "Analytics" },
                Department = "Product",
                IsIntern = false
            };
            string jsonString = JsonSerializerUtil.Serialize(jObj);
            var obj = JsonSerializerUtil.Deserialize(jsonString);
            string objectIniOutput = JsonSerializerUtil.ConstructObjectInitializerFormat(obj);
            Console.WriteLine(objectIniOutput);
            Console.ReadLine();
        }
    }
}

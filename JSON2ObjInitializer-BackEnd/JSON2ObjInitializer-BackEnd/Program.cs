using System;

namespace JSON2ObjInitializer_BackEnd
{
    public class Program
    {
        static void Main(string[] args)
        {
            string jsonString = "{\"data\":[{\"key\":\"Audio\",\"it.e][\\\"ms\":[{\"key\":\"Bluetooth Headphones\",\"items\":null,\"count\":13482,\"summary\":[12099500.9899]}]}],\"totalCount\":1000000,\"summary\":[3638256074.5103]}";
            string jsonString1 = "[[{\"key\":\"Bluetooth Headphones\",\"items\":null,\"count\":13482,\"summary\":[12099500.9899]}],[\"value\"]]";
            var obj = JsonSerializerUtil.Deserialize(jsonString);
            string objectIniOutput = JsonSerializerUtil.ConstructObjectInitializerFormat(obj);
            Console.WriteLine(objectIniOutput);
            Console.ReadLine();
        }
    }
}

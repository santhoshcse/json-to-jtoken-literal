namespace ConsoleApplication
{
    using JSON_to_ObjectInitializer;
    using System;

    public static class Program
    {
        static void Main(string[] args)
        {
            string jsonString1 = "{\"data\":[{\"key\":\"Audio\",\"it.e][\\\"ms\":[{\"key\":\"Bluetooth Headphones\",\"items\":null,\"count\":13482,\"summary\":[12099500.9899]}]}],\"totalCount\":1000000,\"summary\":[3638256074.5103]}";
            string jsonString = "[[{\"key\":\"Bluetooth Headphones\",\"items\":null,\"count\":13482,\"summary\":[12099500.9899]}],[\"value\"]]";
            var obj = JsonSerializerUtil.Deserialize(jsonString);
            Console.WriteLine("Enter conversion option (0: Default, 1: JToken): ");
            var convertOption = Console.ReadLine();
            Console.Clear();
            if (convertOption == "1")
            {
                Console.WriteLine("Converting JSON to Object Initializer using Token types");
                string objectIniOutput = new JTokenConverter().ConstructObjectInitializerFormat(obj);
                Console.WriteLine(objectIniOutput);
            }
            else if (convertOption == "0")
            {
                Console.WriteLine("Converting JSON to Object Initializer using Property names");
                string objectIniOutput = new DefaultConverter().ConstructObjectInitializerFormat(obj);
                Console.WriteLine(objectIniOutput);
            }
            else
            {
                Console.WriteLine("Invalid option provided.");
            }

            Console.ReadLine();
        }
    }
}

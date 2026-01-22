class Helper
{
    public static void SayHello()
    {
        Console.WriteLine("\n===============");
        Console.WriteLine(" Hello, World! ");
        Console.WriteLine("===============\n");
    }

    public static void ChangeFolder(string directoryPath = @"D:\Code-KY-Practice")
    {
        Directory.SetCurrentDirectory(directoryPath);
    }
}
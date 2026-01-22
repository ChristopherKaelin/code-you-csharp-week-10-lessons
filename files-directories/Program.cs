using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

class Program
{
    static void Main()
    {
        Helper.SayHello();
        Helper.ChangeFolder();

        try {
            string path = "example.txt";
            byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello, FileStream!");

            // Writing to the file
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, false))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }

            // Reading from the file
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, false))
            {
                byte[] buffer = new byte[data.Length];
                fs.Seek(0, SeekOrigin.Begin);
                int bytesRead = fs.Read(buffer, 0, buffer.Length);
                string readData = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Data read from file: " + readData);
            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }

    }

}


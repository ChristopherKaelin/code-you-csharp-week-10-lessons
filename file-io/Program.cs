
using System;
using System.IO;
using System.Text;
using File_IO;

class Program
{
    static void Main()
    {

        Console.WriteLine("\nUse the StreamWriter and StreamReader classes.\n");

        // Get the current directory of the executable program
        string currentDirectory = Directory.GetCurrentDirectory();
        Console.WriteLine($"Current directory: {currentDirectory}");

        // Use currentDirectory to create a directory path named TransactionLogs
        string transactionsDirectoryPath = Path.Combine(currentDirectory, "TransactionLogs");
        if (!Directory.Exists(transactionsDirectoryPath))
        {
            Directory.CreateDirectory(transactionsDirectoryPath);
            Console.WriteLine($"Created directory: {transactionsDirectoryPath}");
        }

        // Create a filepath in the TransactionLogs directory for a file named transactions.csv
        string csvFilePath = Path.Combine(transactionsDirectoryPath, "transactions.csv");

        // Simulate one month of transactions for customer Niki Demetriou
        string firstName = "Niki";
        string lastName = "Demetriou";
        BankCustomer customer = new BankCustomer(firstName, lastName);

        // Add CheckingAccount, SavingsAccount, and MoneyMarketAccount to the customer object using the customer.CustomerId
        customer.AddAccount(new CheckingAccount(customer, customer.CustomerId, 5000));
        customer.AddAccount(new SavingsAccount(customer, customer.CustomerId, 15000));
        customer.AddAccount(new MoneyMarketAccount(customer, customer.CustomerId, 90000));

        DateOnly startDate = new DateOnly(2025, 3, 1);
        DateOnly endDate = new DateOnly(2025, 3, 31);
        customer = SimulateDepositsWithdrawalsTransfers.SimulateActivityDateRange(startDate, endDate, customer);

        using (StreamWriter sw = new StreamWriter(csvFilePath))
        {
            sw.WriteLine("TransactionId,TransactionType,TransactionDate,TransactionTime,PriorBalance,TransactionAmount,SourceAccountNumber,TargetAccountNumber,Description");

            Console.WriteLine("\nSimulated transactions:\n");
            foreach (var account in customer.Accounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    Console.WriteLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
                    sw.WriteLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
                }
            }
        }

        // Read the transaction data from the transactions.csv file
        using (StreamReader sr = new StreamReader(csvFilePath))
        {
            string headerLine = sr.ReadLine(); // Read the header line
            Console.WriteLine("\nTransaction data read from the CSV file:\n");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        // Use the FileStream class to perform low-level file I/O operations

        // Create a filepath for the filestream.txt file
        string fileStreamPath = Path.Combine(currentDirectory, "filestream.txt");

        // Prepare transaction data from customer accounts
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("TransactionId,TransactionType,TransactionDate,TransactionTime,PriorBalance,TransactionAmount,SourceAccountNumber,TargetAccountNumber,Description");

        foreach (var account in customer.Accounts)
        {
            foreach (var transaction in account.Transactions)
            {
                // Append transaction data to the StringBuilder object
                sb.AppendLine($"{transaction.TransactionId},{transaction.TransactionType},{transaction.TransactionDate},{transaction.TransactionTime},{transaction.PriorBalance:F2},{transaction.TransactionAmount:F2},{transaction.SourceAccountNumber},{transaction.TargetAccountNumber},{transaction.Description}");
            }
        }

        Console.WriteLine($"\n\nUse the FileStream class to perform file I/O operations.");

        // Write transaction data to file using FileStream
        using (FileStream fileStream = new FileStream(fileStreamPath, FileMode.Create, FileAccess.Write))
        {
            // Convert the StringBuilder object to a byte array and write the byte array to the file
            byte[] transactionDataBytes = new UTF8Encoding(true).GetBytes(sb.ToString());

            // Use the Write method to write the byte array to the file
            fileStream.Write(transactionDataBytes, 0, transactionDataBytes.Length);
            Console.WriteLine($"\nFile length after write: {fileStream.Length} bytes");

            // Use the Flush method to ensure all data is written to the file
            fileStream.Flush();
        }

        Console.WriteLine($"\nTransaction data written using FileStream. File: {fileStreamPath}");

        // Read transaction data from file using FileStream
        using (FileStream fileStream = new FileStream(fileStreamPath, FileMode.Open, FileAccess.Read))
        {
            byte[] readBuffer = new byte[1024];
            UTF8Encoding utf8Decoder = new UTF8Encoding(true);
            int bytesRead;

            Console.WriteLine("\nUsing FileStream to read/display transaction data.\n");

            while ((bytesRead = fileStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
            {
                Console.WriteLine($"bytes read: {utf8Decoder.GetString(readBuffer, 0, bytesRead)}\n");
            }

            Console.WriteLine($"File length: {fileStream.Length} bytes");
            Console.WriteLine($"Current position: {fileStream.Position} bytes");

            // Use the Seek method to move to the beginning of the file
            fileStream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine($"Position after seek: {fileStream.Position} bytes");

            // Use the FileStream.Read method to read the first 100 bytes
            bytesRead = fileStream.Read(readBuffer, 0, 100);
            Console.WriteLine($"Read first 100 bytes: {utf8Decoder.GetString(readBuffer, 0, bytesRead)}");
        }

        // Create a filepath for a binary file named binary.dat
        string binaryFilePath = Path.Combine(currentDirectory, "binary.dat");

        // Create a BinaryWriter object and write binary data to the binary.dat file
        using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(binaryFilePath, FileMode.Create)))
        {
            binaryWriter.Write(1.25);
            binaryWriter.Write("Hello");
            binaryWriter.Write(true);
        }

        Console.WriteLine($"\n\nBinary data written to: {binaryFilePath}");

        // Create a BinaryReader object and read binary data from the binary.dat file
        using (BinaryReader binaryReader = new BinaryReader(File.Open(binaryFilePath, FileMode.Open)))
        {
            double a = binaryReader.ReadDouble();
            string b = binaryReader.ReadString();
            bool c = binaryReader.ReadBoolean();
            Console.WriteLine($"Binary data read from {binaryFilePath}: {a}, {b}, {c}");
        }



        Console.WriteLine("\n");
    }
}

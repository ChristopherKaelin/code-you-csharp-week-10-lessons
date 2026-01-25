using System;

namespace Files_JSON;

public interface IQuarterlyReportGenerator
{
    void GenerateQuarterlyReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
}

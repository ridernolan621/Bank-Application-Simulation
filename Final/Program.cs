using System.Net.NetworkInformation;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using final;

namespace Final;

class Program{
    static List<Customer> customers = CustomerFileLoader.loadCustomerFile("customer_data.csv");
    static List<Employee> employees = EmployeeFileLoader.loadEmployeeFile("employee_data.csv");
    static long loggedInAccountNumber;
    static string empID = "";
    static void Main(){    
        Console.WriteLine("----------\nMAIN MENU\n----------");
        Console.WriteLine("1. Account Login\n2. Create Account\n3. Admin Login\n4. Quit\n\nSelect an option: ");
        string user_input = Console.ReadLine()!;
        
        switch (user_input){
            case "1":
                AccountLogin();
                break;
            case "2":
                CreateAccount();
                break;
            case "3":
                AdminLogin();
                break;
            case "4":
                Console.WriteLine("Have a nice day!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid selection. Exiting program");
                break;
        }
    }

    static void AccountLogin(){
        Console.Clear();
        Console.WriteLine("-----------------\n ACCOUNT LOGIN\n-----------------\n");
        long account_number;
        int pin;

        Console.WriteLine("Enter account number: ");
        account_number = long.Parse(Console.ReadLine()!);

        bool accountFound = false;
        foreach(var customer in customers){

            if (customer.account_number == account_number){

                Console.WriteLine("Enter pin number: ");
                pin = int.Parse(Console.ReadLine()!);
                if (customer.pin == pin){

                    Console.WriteLine($"\n\nWelcome, {customer.firstName} {customer.lastName}.\n");
                    loggedInAccountNumber = account_number;
                    accountServices();
                    accountFound = true;
                    break;
                }

                break;

            }
        }

        if (!accountFound){

            Console.WriteLine("Account not found. Exiting Program...");
            Environment.Exit(0);
        }
    }
    static void makeWithdrawl(){
        float withdrawlAmount = 0;

        foreach(var customer in customers){
            if (customer.account_number == loggedInAccountNumber){
                Console.WriteLine($"Balance: {customer.balance}");

                bool validAmount = false;
                while (!validAmount){
                    Console.WriteLine("Enter amount you would like to withdraw:");
                    if (!float.TryParse(Console.ReadLine(), out withdrawlAmount)){
                        Console.WriteLine("Invalid amount entered.");
                        continue;
                    }
                    if (withdrawlAmount <= 0){
                        Console.WriteLine("Invalid amount. Please enter a positive value.");
                        continue;
                    }
                    if (withdrawlAmount > customer.balance){
                        Console.WriteLine($"Insufficient funds. Please enter an amount smaller than {customer.balance}.");
                        continue;
                    }

                    validAmount = true;
                }

                float newBalance = customer.balance - withdrawlAmount;
                Console.WriteLine($"Withdrawal successful. New balance: {newBalance:N2}");
                customer.UpdateBalance(newBalance);
            }
        }

        using (StreamWriter fileWrite = new StreamWriter("customer_data.csv")){
            foreach(var customer in customers){
                fileWrite.WriteLine($"{customer.account_number},{customer.pin},{customer.firstName},{customer.lastName},{customer.balance},{customer.account_type},{customer.loan_type},{customer.loan_balance}");
            }
        }
        Console.WriteLine("Directing to ACCOUNT SERVICES...");
        accountServices();
    }
    static void accountServices(){
            Console.WriteLine("----------------\nACCOUNT SERVICES\n----------------\n");
            Console.WriteLine("1. Make a withdrawl\n2. Make a deposit\n3. Transfer Funds to another user\n4. Make loan payment\n5. Customer balance inquiry\n6. Back to main menu\n");
            Console.WriteLine("Select an option: ");
            string option = Console.ReadLine()!;

            switch (option){
            case "1":
                makeWithdrawl();
                break;
            case "2":
                makeDeposit();
                break;
            case "3":
                transferFunds();
                break;
            case "4":
                loanPayment();
                break;
            case "5":
                balanceInquiry();
                break;
            case "6":
                MainMenu();
                break;
            default:
                Environment.Exit(0);
                break;
        }
    }
    static void makeDeposit(){
        float depositAmount = 0;

        foreach(var customer in customers){
            if (customer.account_number == loggedInAccountNumber){
                Console.WriteLine($"Balance: {customer.balance}");

                bool validAmount = false;
                while (!validAmount){
                    Console.WriteLine("Enter amount you would like to deposit:");
                    if (!float.TryParse(Console.ReadLine(), out depositAmount)){
                        Console.WriteLine("Invalid amount entered.");
                        continue;
                    }
                    if (depositAmount < 0){
                        Console.WriteLine("Invalid amount. Please enter a positive value.");
                        continue;
                    }

                    validAmount = true;
                }

                float newBalance = customer.balance + depositAmount;
                Console.WriteLine($"Deposit successful. New balance: {newBalance:N2}");
                customer.UpdateBalance(newBalance);
            }
        }
        using (StreamWriter fileWrite = new StreamWriter("customer_data.csv")){
            foreach(var customer in customers){
                fileWrite.WriteLine($"{customer.account_number},{customer.pin},{customer.firstName},{customer.lastName},{customer.balance},{customer.account_type},{customer.loan_type},{customer.loan_balance}");
            }
        }
        Console.WriteLine("Directing to ACCOUNT SERVICES...");
        accountServices();
    }
    static void transferFunds(){
        float transferAmount = 0;

        foreach(var customer in customers){

            if (customer.account_number == loggedInAccountNumber){

                Console.WriteLine($"Balance: {customer.balance}");

                bool validAmount = false;
                while (!validAmount){
                    Console.WriteLine("Enter amount you would like to transfer:");
                    if (!float.TryParse(Console.ReadLine(), out transferAmount)){

                        Console.WriteLine("Invalid amount entered.");
                        continue;
                    }
                    if (transferAmount < 0){

                        Console.WriteLine("Invalid amount. Please enter a positive value.");
                        continue;
                    }
                    if (transferAmount > customer.balance){

                        Console.WriteLine($"Amount is larger than {customer.balance}");
                        continue;
                    }

                    validAmount = true;
                }

                float newBalance = customer.balance - transferAmount;
                customer.UpdateBalance(newBalance);
            }
        }

        long transferAccount;
        bool validAccount = false;

        while (!validAccount){

            Console.WriteLine("Enter account you would like to transfer to: ");
            if (!long.TryParse(Console.ReadLine(), out transferAccount)){

                Console.WriteLine("Invalid amount entered.");
            }
            foreach(var customer in customers){

                if (transferAccount == customer.account_number){

                    Console.WriteLine("Account found, transfering amount now..");

                    float newBalance = customer.balance + transferAmount;
                    Console.WriteLine($"Transfer successful.");
                    customer.UpdateBalance(newBalance);
                }
            }
            validAccount = true;
        }

        using (StreamWriter fileWrite = new StreamWriter("customer_data.csv")){

            foreach(var customer in customers){

                fileWrite.WriteLine($"{customer.account_number},{customer.pin},{customer.firstName},{customer.lastName},{customer.balance},{customer.account_type},{customer.loan_type},{customer.loan_balance}");
            }
        }
        Console.WriteLine("Directing to ACCOUNT SERVICES...");
        accountServices();
    }

    static void loanPayment(){
        float loanPayment = 0;

        foreach(var customer in customers){

            if (customer.account_number == loggedInAccountNumber){
                if (customer.loan_balance == 0){
                    Console.WriteLine("You do not have a loan balance.");
                    Console.WriteLine("Directing to ACCOUNT SERVICES...");
                    accountServices();
                }

                Console.WriteLine($"Balance: {customer.balance}, Loan type and Balance: {customer.loan_type}, {customer.loan_balance}");

                bool validAmount = false;
                while (!validAmount){

                    Console.WriteLine("Enter amount you would like to pay:");
                    if (!float.TryParse(Console.ReadLine(), out loanPayment)){

                        Console.WriteLine("Invalid amount entered.");
                        continue;
                    }
                    if (loanPayment < 0){

                        Console.WriteLine("Invalid amount. Please enter a positive value.");
                        continue;
                    }
                    if (loanPayment > customer.balance){

                        Console.WriteLine($"Amount is larger than {customer.balance}");
                        continue;
                    }

                    validAmount = true;
                }

                float newBalance = customer.balance - loanPayment;
                float newLoanBalance = customer.loan_balance - loanPayment;
                customer.UpdateBalance(newBalance);
                customer.UpdateLoanBalance(newLoanBalance);
                Console.WriteLine($"New balance: {customer.balance}\nNew loan balance: {customer.loan_balance}");
            }
        }

        using (StreamWriter fileWrite = new StreamWriter("customer_data.csv")){

            foreach(var customer in customers){

                fileWrite.WriteLine($"{customer.account_number},{customer.pin},{customer.firstName},{customer.lastName},{customer.balance},{customer.account_type},{customer.loan_type},{customer.loan_balance}");
            }
        }

        Console.WriteLine("\nDirecting to ACCOUNT SERVICES...");
        accountServices();
    }

    static void balanceInquiry(){
        Console.Clear();
        Console.WriteLine("-----------------\n BALANCE INQUIRY\n-----------------\n");

        foreach(var customer in customers){
            
            if (customer.account_number == loggedInAccountNumber){
                
                Console.WriteLine($"Balance: {customer.balance}");
                Console.WriteLine($"Account Type: {customer.account_type}");
                Console.WriteLine($"Loan Type: {customer.loan_type}");
                Console.WriteLine($"Loan Balance: {customer.loan_balance}");
                continue;
            }
        }

        Console.WriteLine("\nDirecting to ACCOUNT SERVICES...");
        accountServices();
    }

    static void CreateAccount(){
        Console.Clear();
        Console.WriteLine("-----------------\n CREATE ACCOUNT\n-----------------\n");

        long accountNumber = 0;
        int pin = 0;
        string firstName = "";
        string lastName = "";
        int balance = 0;
        string accountType = "";
        string loanType = "None";
        int loanBalance = 0;

        bool accountCreate = true;
        while(accountCreate){
            Console.WriteLine("Enter your first name: ");
            firstName = Console.ReadLine()!;
            if (firstName == ""){

                Console.WriteLine("Must enter a name");
                continue;

            }

            Console.WriteLine("Enter your last name: ");
            lastName = Console.ReadLine()!;
            if (lastName == ""){

                Console.WriteLine("Must enter a name");
                continue;

            }

            Console.WriteLine("Set your pin (must be 4 numbers)");
            string s = Console.ReadLine()!;
            if (s == ""){

                Console.WriteLine("Must enter a pin");

            }
            if (s.Length != 4){

                Console.WriteLine("Must be 4 numbers");
                continue;

            }
            else{
                
                if (int.TryParse(s, out pin)){

                    Console.WriteLine("PIN successfully set: " + pin);

                }
                else{

                    Console.WriteLine("Invalid PIN format. Must be numeric.");
                    continue;

                }
            }

            Console.WriteLine("What type of account would you like to set up?\n1. Savings \n2. Checking");
            int a = int.Parse(Console.ReadLine()!);
            
            if (a == 1){

                accountType = "Savings";
            }else if(a == 2){

                accountType = "Checking";
            }else{
                Console.WriteLine("Invalid Input");
                Console.WriteLine("Setting account type to default: Checking...");
                accountType = "Checking";
            }

            balance = 100;

            Console.WriteLine($"Creating account number now... Initial Balance: {balance}\n");
            string firstNumbers = "183977";
            int rdNumbers = 0;

            Random rd = new Random();
            string concatenatedNumbers = firstNumbers.ToString();


            for(int i = 0; i < 10; i ++){

                rdNumbers = rd.Next(0, 9);
                concatenatedNumbers += rdNumbers;

            } 
            
            accountNumber = long.Parse(concatenatedNumbers);
            Console.WriteLine($"Account Number: {accountNumber}\n");

            customers.Add(new Customer(accountNumber, pin, firstName, lastName, balance, accountType, loanType, loanBalance));

            using (StreamWriter fileWrite = new StreamWriter("customer_data.csv")){

                foreach(var customer in customers){

                    fileWrite.WriteLine($"{customer.account_number},{customer.pin},{customer.firstName},{customer.lastName},{customer.balance},{customer.account_type},{customer.loan_type},{customer.loan_balance}");

                }
            }

            accountCreate = false;
        }

        Console.WriteLine("Do you want to login? y/n: ");
        string yn = Console.ReadLine()!.ToLower();
        if (yn == "y"){

            Console.WriteLine("\nDirecting to ACCOUNT LOGIN...");
            AccountLogin();
        }else if (yn == "n"){

            Console.WriteLine("Directing to MAIN MENU...");
            MainMenu();
        }else{

            Console.WriteLine("Invalid Entry:");
            Console.WriteLine("Directing to MAIN MENU...");
            MainMenu();
        }
    }

    static void MainMenu(){    
        Console.WriteLine("----------\nMAIN MENU\n----------");
        Console.WriteLine("1. Account Login\n2. Create Account\n3. Admin Login\n4. Quit\n\nSelect an option:");
        string user_input = Console.ReadLine()!;
        
        switch (user_input){
            case "1":
                AccountLogin();
                break;
            case "2":
                CreateAccount();
                break;
            case "3":
                AdminLogin();
                break;
            case "4":
                Console.WriteLine("Have a nice day!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid selection. Exiting program");
                break;
        }
    }

    static void AdminLogin(){
        
        Console.WriteLine("--------------\n ADMIN LOGIN\n--------------\n");

        bool validAccount = true;
        while (validAccount){
            Console.WriteLine("Enter employee ID");
            string username = Console.ReadLine()!;

            foreach(var employee in employees){
                
                if (employee.username == username){

                    Console.WriteLine("Enter passwrord: ");
                    int pin = int.Parse(Console.ReadLine()!);

                    if (employee.password == pin){
                        
                        adminServices();
                        validAccount = false;
                        break;

                    }else if(empID != employee.username){

                        Console.WriteLine("Account not found...");
                        Console.WriteLine("Directing to MAIN MENU...");
                        MainMenu();

                    }
                }
            }
        }
    }

    public static void adminServices(){

        Console.WriteLine("--------------- \nADMIN SERVICES\n---------------");

        foreach(var employee in employees){

            if (employee.username == empID){

                Console.WriteLine($"\nWelcome, {employee.firstName} {employee.lastName}.\n");
                continue;
            }
        }
        
        Console.WriteLine("\nWhat would you like to do today?");
        Console.WriteLine("1. Show average savings account balance \n2. Show total savings account balance \n3. Show average checking account balance \n4. Show checking account total balance \n5. Show the number of account for each type \n6. Show the number of each loan type \n7. Show total outstanding loan balance \n8. Show average outstanding loan balance \n9. Show all employee information \n10. Main Menu ");
        string option = Console.ReadLine()!;

        switch (option){
            case "1":
                avgSavings();
                break;
            case "2":
                totalSavings();
                break;
            case "3":
                avgChecking();
                break;
            case "4":
                totalChecking();
                break;
            case "5":
                numAccountType();
                break;
            case "6":
                numLoanType();
                break;
            case "7":
                totalLoanBalance();
                break;
            case "8":
                avgLoanBalance();
                break;
            case "9":
                employeeInfo();
                break;
            case "10":
                MainMenu();
                break;
            default:
                Console.WriteLine("Invalid Selection, directing to main menu...");
                MainMenu();
                break;

        }
    }

    static void avgSavings(){

        List<float> savings = new List<float>();
        
        foreach(var customer in customers){

            if (customer.account_type == "Savings"){

                foreach (var account in customers){

                    savings.Add(account.balance);

                }

                float totalSaving = savings.Sum();
                float totalAccounts = savings.Count();

                float avgSavings = totalSaving / totalAccounts;

                Console.WriteLine($"------------------------\nAverage savings balance\n-----------------------");
                Console.WriteLine($"$ {avgSavings}");
                adminServices();
            }
        }
    }

    static void totalSavings(){

        List<float> savings = new List<float>();

        foreach(var customer in customers){

            if (customer.account_type == "Savings"){

                foreach (var account in customers){

                    savings.Add(account.balance);

                }

                float totalSaving = savings.Sum();
                float total = totalSaving;

                Console.WriteLine($"------------------------\nTotal savings balance\n-----------------------");
                Console.WriteLine($"$ {total}");
                adminServices();

            }
        }
    }

    static void avgChecking(){

        List<float> checking = new List<float>();

        foreach(var customer in customers){

            if (customer.account_type == "Checking"){

                foreach (var account in customers){

                    checking.Add(account.balance);

                }

                float totalChecking = checking.Sum();
                float numChecking = checking.Count();

                float avgCheckings = totalChecking / numChecking;

                Console.WriteLine($"------------------------\nAverage Checking balance\n-----------------------");
                Console.WriteLine($"$ {avgCheckings}");
                adminServices();

            }
        }
    }

    static void totalChecking(){

        List<float> checking = new List<float>();

        foreach(var customer in customers){

            if (customer.account_type == "Checking"){

                foreach (var account in customers){

                    checking.Add(account.balance);

                }

                float totalCheckings = checking.Sum();

                Console.WriteLine($"------------------------\nTotal Checking balance\n-----------------------");
                Console.WriteLine($"$ {totalCheckings}");
                adminServices();

            }
        }
    }
    
    static void numAccountType(){

        Console.WriteLine("---------------------------------------\nNumber of Checking and Saving accounts\n---------------------------------------");
        
        var savings = from Customer in customers
                            where Customer.account_type == "Savings"
                            select Customer;
        Console.WriteLine($"Saving accounts: {savings.Count()}");
        
        var checking = from Customer in customers
                            where Customer.account_type == "Checking"
                            select Customer;
        Console.WriteLine($"Checking accounts: {checking.Count()}");

        adminServices();

    }

    static void numLoanType(){

        Console.WriteLine("----------------------------------\nNumber of each loan type\n---------------------------------");

        var auto = from Customer in customers
                            where Customer.loan_type == "Auto"
                            select Customer;
        Console.WriteLine($"Auto loans: {auto.Count()}");
        
        var home = from Customer in customers
                            where Customer.loan_type == "Home"
                            select Customer;
        Console.WriteLine($"Home loans: {home.Count()}");

        var personal = from Customer in customers
                            where Customer.loan_type == "Personal"
                            select Customer;
        Console.WriteLine($"Personal loans: {personal.Count()}");

        adminServices();
    }

    static void totalLoanBalance(){

        Console.WriteLine("----------------------------------\nTotal loan balance\n---------------------------------");

        List<float> loanBalace = new List<float>();

        foreach(var loan in customers){

            loanBalace.Add(loan.loan_balance);

        }   

        float totalLoan = loanBalace.Sum();
            
        Console.WriteLine($"$ {totalLoan}");
        adminServices();
    }

    static void avgLoanBalance(){

        Console.WriteLine("----------------------------------\nAverage loan balance\n---------------------------------");

        List<float> loanBalace = new List<float>();

        foreach(var loan in customers){

            loanBalace.Add(loan.loan_balance);

        }   

        float totalLoan = loanBalace.Sum();
        float totalLoans = loanBalace.Count();
        float avgLoan = totalLoan / totalLoans;
        Console.WriteLine($"$ {avgLoan}");
        adminServices();
    }

    public static void employeeInfo(){

        Console.WriteLine("-----------------\nEmployee information\n-----------------");

        foreach(var employee in employees){

            Console.WriteLine($"{employee.firstName} {employee.lastName}, {employee.title}\n");

        }

        adminServices();
        

    }
}
namespace final;

static class CustomerFileLoader{
    public static List<Customer> loadCustomerFile(string filePath){
        List<Customer> CustomerList = new List<Customer>();

        using (StreamReader fileRead = new StreamReader(filePath)) {
            int lineNumber = 0;
            string lineData = "";

            while (!fileRead.EndOfStream) {
                lineNumber ++;
                lineData = fileRead.ReadLine()!;

                string[] parts = lineData.Split(",");

                try{
                    long   account_number  = long.Parse(parts[0]);
                    int    pin             = int.Parse(parts[1]);
                    string first_name       = parts[2];
                    string lastName        = parts[3];
                    float  balance         = float.Parse(parts[4]);
                    string account_type    = parts[5];
                    string loan_type       = parts[6];
                    float  loan_balance    = float.Parse(parts[7]);

                    CustomerList.Add(new Customer(account_number, pin, first_name, lastName, balance, account_type, loan_type, loan_balance));
                }
                catch(Exception error){
                    Console.WriteLine(error.Message);
                }
            }    
        }
        return CustomerList;
    }
}
static class EmployeeFileLoader{
    public static List<Employee> loadEmployeeFile(string filePath){
        List<Employee> EmployeeList = new List<Employee>();

        using (StreamReader fileRead = new StreamReader(filePath)) {
            fileRead.ReadLine();

            while (!fileRead.EndOfStream) {
                string lineData = fileRead.ReadLine()!;
                string[] parts = lineData.Split(",");

                try{
                    string username   = parts[0];
                    int password   = int.Parse(parts[1]);
                    string first_name = parts[2];
                    string last_name  = parts[3];
                    string title      = parts[4];

                    EmployeeList.Add(new Employee(username, password, first_name, last_name, title));
                }
                catch(Exception error){
                    Console.WriteLine(error.Message);
                    continue;
                }
            }    
        }
        return EmployeeList;
    }
}

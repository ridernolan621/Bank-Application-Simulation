using System.Diagnostics.Contracts;

namespace final;

abstract class Person{
    public string firstName;
    public string lastName;

    public Person(string firstName, string lastName) {
        this.firstName = firstName;
        this.lastName = lastName;
    }

    public string FirstName{
        get { return this.firstName; }
        set { firstName = value; }
    }
}

class Customer: Person{
    public long account_number {get; set;}
    public int pin {get; set;}
    public float balance {get; set;}
    public string account_type {get; set;}
    public string loan_type {get; set;}
    public float loan_balance {get; set;}

    public Customer(
        long account_number, 
        int pin,
        string first_name,
        string last_name,
        float balance,
        string account_type, 
        string loan_type, 
        float loan_balance) : base(first_name, last_name)
        {
            this.account_number = account_number;
            this.pin = pin;
            this.balance = balance;
            this.account_type = account_type;
            this.loan_type = loan_type;
            this.loan_balance = loan_balance;
        }

    public void UpdateBalance(float newBalance)
    {
        balance = newBalance;
    }
    public void UpdateLoanBalance(float newLoanBalance)
    {
        loan_balance = newLoanBalance;
    }
}

class Employee: Person{
    public string username {get; set;}
    public int password {get; set;}
    public string title {get; set;}

    public Employee(
        string username,
        int password,
        string first_name,
        string last_name,
        string title) : base(first_name, last_name)
        {
            this.username = username;
            this.password = password;
            this.title = title;
        }
}
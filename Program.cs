using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    private const string DataFilePath = "employees.txt";
    private static List<Employee> employees = new List<Employee>();

    static void Main(string[] args)
    {
        LoadDataFromFile();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create a new employee");
            Console.WriteLine("2. View all employees");
            Console.WriteLine("3. Search for an employee by ID");
            Console.WriteLine("4. Search for employees by name");
            Console.WriteLine("5. Delete an employee by ID");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice (1-6): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateEmployee();
                    break;
                case "2":
                    ViewAllEmployees();
                    break;
                case "3":
                    SearchEmployeeByID();
                    break;
                case "4":
                    SearchEmployeesByName();
                    break;
                case "5":
                    DeleteEmployeeByID();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }

            Console.WriteLine();
        }

        SaveDataToFile();
    }

    static void LoadDataFromFile()
    {
        if (File.Exists(DataFilePath))
        {
            string[] lines = File.ReadAllLines(DataFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 4)
                {
                    Employee employee = new Employee
                    {
                        Name = parts[0],
                        EmployeeID = int.Parse(parts[1]),
                        Title = parts[2],
                        StartDate = DateTime.Parse(parts[3])
                    };
                    employees.Add(employee);
                }
            }
        }
    }

    static void SaveDataToFile()
    {
        using (StreamWriter file = new StreamWriter(DataFilePath))
        {
            foreach (Employee employee in employees)
            {
                file.WriteLine($"{employee.Name}|{employee.EmployeeID}|{employee.Title}|{employee.StartDate}");
            }
        }
    }

    static void CreateEmployee()
    {
        Console.WriteLine("Enter employee details:");
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Title: ");
        string title = Console.ReadLine();
        Console.Write("Start Date (DD-MM-YYYY): ");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
        {
            Console.WriteLine("Invalid date format. Employee not created.");
            return;
        }

        Random random = new Random();
        int employeeID;
        do
        {
            employeeID = random.Next(1000, 10000);
        } while (employees.Any(e => e.EmployeeID == employeeID));

        Employee employee = new Employee
        {
            Name = name,
            EmployeeID = employeeID,
            Title = title,
            StartDate = startDate
        };
        employees.Add(employee);

        Console.WriteLine("Employee created successfully." + "Employee Id Number: " + employeeID);
    }

    static void ViewAllEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees found.");
            return;
        }

        Console.WriteLine("All employees:");
        foreach (Employee employee in employees)
        {
            Console.WriteLine($"Name: {employee.Name}");
            Console.WriteLine($"Employee ID: {employee.EmployeeID}");
            Console.WriteLine($"Title: {employee.Title}");
            Console.WriteLine($"Start Date: {employee.StartDate.ToShortDateString()}");
            Console.WriteLine("------------------------");
        }
    }

    static void SearchEmployeeByID()
    {
        Console.Write("Enter employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeID))
        {
            Console.WriteLine("Invalid employee ID.");
            return;
        }

        Employee employee = employees.FirstOrDefault(e => e.EmployeeID == employeeID);
        if (employee != null)
        {
            Console.WriteLine($"Employee found:");
            Console.WriteLine($"Name: {employee.Name}");
            Console.WriteLine($"Employee ID: {employee.EmployeeID}");
            Console.WriteLine($"Title: {employee.Title}");
            Console.WriteLine($"Start Date: {employee.StartDate.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }

    static void SearchEmployeesByName()
    {
        Console.Write("Enter name or part of the name: ");
        string searchTerm = Console.ReadLine();

        List<Employee> matchingEmployees = employees.Where(e => e.Name.Contains(searchTerm)).ToList();
        if (matchingEmployees.Count > 0)
        {
            Console.WriteLine($"Employees found ({matchingEmployees.Count}):");
            foreach (Employee employee in matchingEmployees)
            {
                Console.WriteLine($"Name: {employee.Name}");
                Console.WriteLine($"Employee ID: {employee.EmployeeID}");
                Console.WriteLine($"Title: {employee.Title}");
                Console.WriteLine($"Start Date: {employee.StartDate.ToShortDateString()}");
                Console.WriteLine("------------------------");
            }
        }
        else
        {
            Console.WriteLine("No employees found.");
        }
    }

    static void DeleteEmployeeByID()
    {
        Console.Write("Enter employee ID: ");
        if (!int.TryParse(Console.ReadLine(), out int employeeID))
        {
            Console.WriteLine("Invalid employee ID.");
            return;
        }

        Employee employee = employees.FirstOrDefault(e => e.EmployeeID == employeeID);
        if (employee != null)
        {
            employees.Remove(employee);
            Console.WriteLine("Employee deleted successfully.");
        }
        else
        {
            Console.WriteLine("Employee not found.");
        }
    }
}
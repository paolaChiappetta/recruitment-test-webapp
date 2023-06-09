﻿using InterviewTest.Model;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace InterviewTest.Stores
{
    public class EmployeeStore: IEmployeeStore
    {
        private string connectionString;

        public EmployeeStore()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" };
            connectionString = connectionStringBuilder.ConnectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Id, Name, Lastname, Value, Address, Phone FROM Employees";
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Lastname = reader.GetString(2),
                            Value = reader.GetInt32(3),
                            Address = reader.GetString(4),
                            Phone = reader.GetString(5)
                        });
                    }
                }
            }

            return employees;
        }

        public void AddEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"INSERT INTO Employees(Name, Lastname, Value, Address, Phone) VALUES(@n, @l, @v, @a, @p)";
                queryCmd.Parameters.AddWithValue("@n", employee.Name);
                queryCmd.Parameters.AddWithValue("@l", employee.Lastname);
                queryCmd.Parameters.AddWithValue("@v", employee.Value);
                queryCmd.Parameters.AddWithValue("@a", employee.Address);
                queryCmd.Parameters.AddWithValue("@p", employee.Phone);
                queryCmd.ExecuteNonQuery();
            }
        }

        public void DeleteEmployee(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"DELETE FROM Employees WHERE Id=@i";
                queryCmd.Parameters.AddWithValue("@i", id);
                queryCmd.ExecuteNonQuery();
            }
        }

        public void UpdateEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"UPDATE Employees SET Value=@v, Name=@n , Lastname=@l, Address=@a, Phone=@p WHERE Id=@i";
                queryCmd.Parameters.AddWithValue("@i", employee.Id);
                queryCmd.Parameters.AddWithValue("@n", employee.Name);
                queryCmd.Parameters.AddWithValue("@l", employee.Lastname);
                queryCmd.Parameters.AddWithValue("@v", employee.Value);
                queryCmd.Parameters.AddWithValue("@a", employee.Address);
                queryCmd.Parameters.AddWithValue("@p", employee.Phone);
                queryCmd.ExecuteNonQuery();
            }
        }
    }
}

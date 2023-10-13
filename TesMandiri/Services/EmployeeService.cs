using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using TesMandiri.Interfaces;
using TesMandiri.Models;
using System.Xml.Linq;

namespace TesMandiri.Services;

public class EmployeeService : IEmployeeService
{
    SqlConnection conn;
    public EmployeeService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<EmployeeBase> GetEmployee()
    {
        conn.Open();
        List<EmployeeBase> employees = new();
        try
        {
            SqlCommand command = new SqlCommand("Select id, [name] empName from Employee", conn);
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                EmployeeBase employee = new();
                employee.EmployeeId = Convert.ToInt32(reader["id"]);
                employee.EmployeeName = Convert.ToString(reader["empName"]) ?? string.Empty;

                employees.Add(employee);
            }
            conn.Close();

            return employees;
        }
        catch
        {
            if (conn is not null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            throw;
        }
    }

    public EmployeeBase GetEmployeeById(int id)
    {
        conn.Open();
        EmployeeBase employee = new();
        try
        {
            SqlCommand command = new SqlCommand("Select id, [name] empName from Employee where id = @Id", conn);
            command.Parameters.Add(new SqlParameter("Id", id));

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                employee.EmployeeId = Convert.ToInt32(reader["id"]);
                employee.EmployeeName = Convert.ToString(reader["empName"]) ?? string.Empty;
            }
            conn.Close();

            return employee;
        }
        catch
        {
            if (conn is not null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            throw;
        }
    }

    public int CreateEmployee(string name)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO Employee ([name]) output INSERTED.Id VALUES(@EmpName)", conn, transaction);
            command.Parameters.Add(new SqlParameter("EmpName", name));

            int id = (int)command.ExecuteScalar();
            transaction.Commit();
            conn.Close();

            return id;
        }
        catch
        {
            transaction.Rollback();
            if (conn is not null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            throw;
        }
    }

    public bool UpdateEmployee(EmployeeBase employee)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Update Employee Set [name] = @EmpName Where id = @Id", conn, transaction);
            command.Parameters.Add(new SqlParameter("Id", employee.EmployeeId));
            command.Parameters.Add(new SqlParameter("EmpName", employee.EmployeeName));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return true;
        }
        catch
        {
            transaction.Rollback();
            if (conn is not null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            throw;
        }
    }

    public bool DeleteEmployee(int id)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Delete From Employee Where id = @Id", conn, transaction);
            command.Parameters.Add(new SqlParameter("Id", id));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return true;
        }
        catch
        {
            transaction.Rollback();
            if (conn is not null && conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
            throw;
        }
    }
}

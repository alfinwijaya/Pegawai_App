using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Services;

public class EmployeeCardService : IEmployeeCardService
{
    SqlConnection conn;
    public EmployeeCardService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<EmployeeCard> Get()
    {
        conn.Open();
        List<EmployeeCard> employees = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select EmployeeCard.EmployeeId, Employee.[Name] EmployeeName, EmployeeCard.CardNumber , IdCard.Description 
                from EmployeeCard
                Left Join Employee On Employee.Id = EmployeeCard.EmployeeId 
                Left Join IdCard On IdCard.Number = EmployeeCard.CardNumber
            ", conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EmployeeCard employeeCard = new();
                employeeCard.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                employeeCard.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;
                employeeCard.Card = new();
                employeeCard.Card.CardNumber = Convert.ToInt32(reader["CardNumber"]);
                employeeCard.Card.CardDescription = Convert.ToString(reader["Description"]) ?? string.Empty;

                employees.Add(employeeCard);
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

    public EmployeeCard? GetById(int id)
    {
        conn.Open();
        EmployeeCard employeeCard = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select EmployeeCard.EmployeeId, Employee.[Name] EmployeeName, EmployeeCard.CardNumber , IdCard.Description 
                from EmployeeCard
                Left Join Employee On Employee.Id = EmployeeCard.EmployeeId 
                Left Join IdCard On IdCard.Number = EmployeeCard.CardNumber
                Where EmployeeCard.EmployeeId = @Id
            ", conn);
            command.Parameters.Add(new SqlParameter("Id", id));

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                employeeCard.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                employeeCard.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;
                employeeCard.Card = new();
                employeeCard.Card.CardNumber = Convert.ToInt32(reader["CardNumber"]);
                employeeCard.Card.CardDescription = Convert.ToString(reader["Description"]) ?? string.Empty;
            } 
            else
            {
                conn.Close();
                return null;
            }
            conn.Close();

            return employeeCard;
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

    public int Create(EmployeeCardDto employeeCard)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO EmployeeCard (EmployeeId, CardNumber) VALUES(@Id, @Number)", conn, transaction);
            command.Parameters.Add(new SqlParameter("Id", employeeCard.EmployeeId));
            command.Parameters.Add(new SqlParameter("Number", employeeCard.CardNumber));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return employeeCard.EmployeeId;
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

    public bool Update(EmployeeCardDto employeeCard)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Update EmployeeCard Set CardNumber = @Number Where EmployeeId = @Id", conn, transaction);
            command.Parameters.Add(new SqlParameter("Id", employeeCard.EmployeeId));
            command.Parameters.Add(new SqlParameter("Number", employeeCard.CardNumber));

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

    public bool Delete(int id)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Delete From EmployeeCard Where EmployeeId = @Id", conn, transaction);
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

using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Services;

public class DivisionMemberService : IDivisionMemberService
{
    SqlConnection conn;
    public DivisionMemberService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<Division> Get()
    {
        conn.Open();
        List<Division> divMember = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select DivisionMember.DivisionCode, Division.[Name] DivisionName, 
                    DivisionMember.EmployeeId, Employee.[Name] EmployeeName 
                from DivisionMember
                Left Join Division On Division.Code = DivisionMember.DivisionCode  
                Left Join Employee On Employee.Id = DivisionMember.EmployeeId
            ", conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Division div = new();
                EmployeeBase employee = new();

                employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                employee.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;

                var divCode = Convert.ToString(reader["DivisionCode"]) ?? string.Empty;
                if (divMember.Any(d => d.DivisionCode == divCode))
                {
                    divMember
                    .FirstOrDefault(d => d.DivisionCode == divCode)!
                    .Employee
                    .Add(employee);
                }
                else
                {
                    div.DivisionCode = divCode;
                    div.DivisionName = Convert.ToString(reader["DivisionName"]) ?? string.Empty;
                    div.Employee.Add(employee);

                    divMember.Add(div);

                }
            }
            conn.Close();

            return divMember;
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

    public Division? GetById(string code)
    {
        conn.Open();
        Division div = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select DivisionMember.DivisionCode, Division.[Name] DivisionName, 
                    DivisionMember.EmployeeId, Employee.[Name] EmployeeName 
                from DivisionMember
                Left Join Division On Division.Code = DivisionMember.DivisionCode  
                Left Join Employee On Employee.Id = DivisionMember.EmployeeId
                Where DivisionMember.DivisionCode = @Code
            ", conn);
            command.Parameters.Add(new SqlParameter("Code", code));

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bool first = true;
                EmployeeBase employee = new();

                employee.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                employee.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;

                if (first)
                {
                    div.DivisionCode = Convert.ToString(reader["DivisionCode"]) ?? string.Empty;
                    div.DivisionName = Convert.ToString(reader["DivisionName"]) ?? string.Empty;
                    first = false;
                }
                
                div.Employee.Add(employee);
                
            }
            conn.Close();

            return div;
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

    public bool Create(DivisionMemberDto division)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO DivisionMember (DivisionCode, EmployeeId) VALUES(@Code, @EmpId)", conn, transaction);

            foreach(var employee in division.Employee)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("Code", division.DivisionCode));
                command.Parameters.Add(new SqlParameter("EmpId", employee.EmployeeId));

                command.ExecuteNonQuery();
            }
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

    public bool Update(DivisionMemberDto division)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Delete From DivisionMember Where DivisionCode = @Code", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", division.DivisionCode));
            command.ExecuteNonQuery();

            command = new SqlCommand("INSERT INTO DivisionMember (DivisionCode, EmployeeId) VALUES(@Code, @EmpId)", conn, transaction);

            foreach (var employee in division.Employee)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("Code", division.DivisionCode));
                command.Parameters.Add(new SqlParameter("EmpId", employee.EmployeeId));

                command.ExecuteNonQuery();
            }

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

    public bool Delete(string code)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand(@"If Exists (Select 1 From DivisionMember Where DivisionCode = @Code)
                                                    Begin
                                                        Select 1
                                                    END
                                                    ELSE BEGIN Select 0 End
            
            ", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", code));

            int exist = (int)command.ExecuteScalar();
            if (exist == 0)
            {
                conn.Close();
                return false;
            }

            command = new SqlCommand("Delete From DivisionMember Where DivisionCode = @Code", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", code));

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

    public bool CheckEmpExist(int id, string? code = null)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand(string.Format(
                    @"
                    If Exists (Select 1 From DivisionMember Where EmployeeId = @Id {0})
                    Begin
                        Select 1
                    END
                    ELSE BEGIN Select 0 End
            
            ", code is not null ? $" And DivisionCode <> '{code}'" : string.Empty), conn, transaction);
            command.Parameters.Add(new SqlParameter("Id", id));

            int exist = (int)command.ExecuteScalar();

            conn.Close();
            if (exist == 1)
                return true;

            return false;
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

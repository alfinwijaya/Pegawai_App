using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TesMandiri.Interfaces;
using TesMandiri.Models;

namespace TesMandiri.Services;

public class EmployeeTaskService: IEmployeeTaskService
{
    SqlConnection conn;
    public EmployeeTaskService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<EmployeeTask> Get()
    {
        conn.Open();
        List<EmployeeTask> empTask = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select EmployeeTask.EmployeeId, Employee.[Name] EmployeeName, 
                    EmployeeTask.TaskCode, Task.[Name] TaskName 
                from EmployeeTask 
                Left Join Employee On Employee.Id = EmployeeTask.EmployeeId
                Left Join Task On Task.Code = EmployeeTask.TaskCode
            ", conn);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EmployeeTask emp = new();
                TaskBase task = new();

                task.TaskCode = Convert.ToString(reader["TaskCode"]) ?? string.Empty;
                task.TaskName = Convert.ToString(reader["TaskName"]) ?? string.Empty;

                var empId = Convert.ToInt32(reader["EmployeeId"]);
                if (empTask.Any(d => d.EmployeeId == empId))
                {
                    empTask
                    .FirstOrDefault(d => d.EmployeeId == empId)!
                    .Tasks
                    .Add(task);
                }
                else
                {
                    emp.EmployeeId = empId;
                    emp.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;
                    emp.Tasks.Add(task);

                    empTask.Add(emp);

                }
            }
            conn.Close();

            return empTask;
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

    public EmployeeTask? GetById(int id)
    {
        conn.Open();
        EmployeeTask empTask = new();
        try
        {
            SqlCommand command = new SqlCommand(@"
                Select EmployeeTask.EmployeeId, Employee.[Name] EmployeeName, 
                    EmployeeTask.TaskCode, Task.[Name] TaskName 
                from EmployeeTask 
                Left Join Employee On Employee.Id = EmployeeTask.EmployeeId
                Left Join Task On Task.Code = EmployeeTask.TaskCode
                Where EmployeeTask.EmployeeId = @Id
            ", conn);
            command.Parameters.Add(new SqlParameter("Id", id));

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                bool first = true;
                TaskBase task = new();

                task.TaskCode = Convert.ToString(reader["TaskCode"]) ?? string.Empty;
                task.TaskName = Convert.ToString(reader["TaskName"]) ?? string.Empty;

                if (first)
                {
                    empTask.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                    empTask.EmployeeName = Convert.ToString(reader["EmployeeName"]) ?? string.Empty;
                    first = false;
                }

                empTask.Tasks.Add(task);

            }
            conn.Close();

            return empTask;
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

    public bool Create(EmployeeTaskDto employee)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO EmployeeTask (EmployeeId, TaskCode) VALUES(@EmpId, @Code)", conn, transaction);

            foreach(var task in employee.Tasks)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("EmpId", employee.EmployeeId));
                command.Parameters.Add(new SqlParameter("Code", task.TaskCode));

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

    public bool Update(EmployeeTaskDto employee)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Delete From EmployeeTask Where EmployeeId = @EmpId", conn, transaction);
            command.Parameters.Add(new SqlParameter("EmpId", employee.EmployeeId));
            command.ExecuteNonQuery();

            command = new SqlCommand("INSERT INTO EmployeeTask (EmployeeId, TaskCode) VALUES(@EmpId, @Code)", conn, transaction);

            foreach (var task in employee.Tasks)
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("EmpId", employee.EmployeeId));
                command.Parameters.Add(new SqlParameter("Code", task.TaskCode));

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

    public bool Delete(int id)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand(@"If Exists (Select 1 From EmployeeTask Where EmployeeId = @EmpId)
                                                    Begin
                                                        Select 1
                                                    END
                                                    ELSE BEGIN Select 0 End
            
            ", conn, transaction);
            command.Parameters.Add(new SqlParameter("EmpId", id));

            int exist = (int)command.ExecuteScalar();
            if (exist == 0)
            {
                conn.Close();
                return false;
            }

            command = new SqlCommand("Delete From EmployeeTask Where EmployeeId = @EmpId", conn, transaction);
            command.Parameters.Add(new SqlParameter("EmpId", id));

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

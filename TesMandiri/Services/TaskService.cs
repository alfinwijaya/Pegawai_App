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

public class TaskService : IMasterString<TaskBase>
{
    SqlConnection conn;
    public TaskService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<TaskBase> Get()
    {
        conn.Open();
        List<TaskBase> tasks = new();
        try
        {
            SqlCommand command = new SqlCommand("Select code, [name] from Task", conn);
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                TaskBase task = new();
                task.TaskCode = Convert.ToString(reader["code"]) ?? string.Empty;
                task.TaskName = Convert.ToString(reader["name"]) ?? string.Empty;

                tasks.Add(task);
            }
            conn.Close();

            return tasks;
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

    public TaskBase GetById(string id)
    {
        conn.Open();
        TaskBase task = new();
        try
        {
            SqlCommand command = new SqlCommand("Select code, [name] from Task where code = @Code", conn);
            command.Parameters.Add(new SqlParameter("Code", id));

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                task.TaskCode = Convert.ToString(reader["code"]) ?? string.Empty;
                task.TaskName = Convert.ToString(reader["name"]) ?? string.Empty;
            }
            conn.Close();

            return task;
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

    public string Create(TaskBase task)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO Task VALUES(@Code, @Name)", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", task.TaskCode));
            command.Parameters.Add(new SqlParameter("Name", task.TaskName is null ? DBNull.Value : task.TaskName));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return task.TaskCode;
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

    public bool Update(TaskBase task)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Update Task Set [name] = @Name Where code = @Code", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", task.TaskCode));
            command.Parameters.Add(new SqlParameter("Name", task.TaskName is null ? DBNull.Value : task.TaskName));

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

    public bool Delete(string id)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Delete From Task Where code = @Code", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", id));

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

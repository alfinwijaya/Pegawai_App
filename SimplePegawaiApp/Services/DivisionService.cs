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

public class DivisionService : IMasterString<DivisionBase>
{
    SqlConnection conn;
    public DivisionService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<DivisionBase> Get()
    {
        conn.Open();
        List<DivisionBase> divisions = new();
        try
        {
            SqlCommand command = new SqlCommand("Select code, [name] from Division", conn);
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                DivisionBase division = new();
                division.DivisionCode = Convert.ToString(reader["code"]) ?? string.Empty;
                division.DivisionName = Convert.ToString(reader["name"]) ?? string.Empty;

                divisions.Add(division);
            }
            conn.Close();

            return divisions;
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

    public DivisionBase GetById(string id)
    {
        conn.Open();
        DivisionBase division = new();
        try
        {
            SqlCommand command = new SqlCommand("Select code, [name] from Division where code = @Code", conn);
            command.Parameters.Add(new SqlParameter("Code", id));

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                division.DivisionCode = Convert.ToString(reader["code"]) ?? string.Empty;
                division.DivisionName = Convert.ToString(reader["name"]) ?? string.Empty;
            }
            conn.Close();

            return division;
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

    public string Create(DivisionBase division)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO Division VALUES(@Code, @Name)", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", division.DivisionCode));
            command.Parameters.Add(new SqlParameter("Name", division.DivisionName is null ? DBNull.Value : division.DivisionName));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return division.DivisionCode;
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

    public bool Update(DivisionBase division)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Update Division Set [name] = @Name Where code = @Code", conn, transaction);
            command.Parameters.Add(new SqlParameter("Code", division.DivisionCode));
            command.Parameters.Add(new SqlParameter("Name", division.DivisionName is null ? DBNull.Value : division.DivisionName));

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
            SqlCommand command = new SqlCommand("Delete From Division Where code = @Code", conn, transaction);
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

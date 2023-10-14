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

public class IdCardService : IMasterInt<IdCardBase>
{
    SqlConnection conn;
    public IdCardService(IOptions<Setting> setting)
    {
        conn = new SqlConnection(setting.Value.ConnectionString);
    }

    public List<IdCardBase> Get()
    {
        conn.Open();
        List<IdCardBase> cards = new();
        try
        {
            SqlCommand command = new SqlCommand("Select number, [description] from IdCard", conn);
            SqlDataReader reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                IdCardBase card = new();
                card.CardNumber = Convert.ToInt32(reader["number"]);
                card.CardDescription = Convert.ToString(reader["description"]) ?? string.Empty;

                cards.Add(card);
            }
            conn.Close();

            return cards;
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

    public IdCardBase GetById(int id)
    {
        conn.Open();
        IdCardBase card = new();
        try
        {
            SqlCommand command = new SqlCommand("Select number, [description] from IdCard where number = @Number", conn);
            command.Parameters.Add(new SqlParameter("Number", id));

            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                card.CardNumber = Convert.ToInt32(reader["number"]);
                card.CardDescription = Convert.ToString(reader["description"]) ?? string.Empty;
            }
            conn.Close();

            return card;
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

    public int Create(IdCardBase card)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("INSERT INTO IdCard VALUES(@Number, @Description)", conn, transaction);
            command.Parameters.Add(new SqlParameter("Number", card.CardNumber));
            command.Parameters.Add(new SqlParameter("Description", card.CardDescription is null ? DBNull.Value : card.CardDescription));

            command.ExecuteNonQuery();
            transaction.Commit();
            conn.Close();

            return card.CardNumber;
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

    public bool Update(IdCardBase card)
    {
        conn.Open();
        SqlTransaction transaction = conn.BeginTransaction();
        try
        {
            SqlCommand command = new SqlCommand("Update IdCard Set [description] = @Description Where number = @number", conn, transaction);
            command.Parameters.Add(new SqlParameter("Number", card.CardNumber));
            command.Parameters.Add(new SqlParameter("Description", card.CardDescription is null ? DBNull.Value : card.CardDescription));

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
            SqlCommand command = new SqlCommand("Delete From IdCard Where number = @Id", conn, transaction);
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

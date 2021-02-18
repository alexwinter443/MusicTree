using Milestone.Models;
using System;
using System.Data.SqlClient;

/*
 * Alex Vergara and Kacey Morris
 * January 31, 2021
 * CST 247
 * Minesweeper Web Application
 * 
 * Security Data Access Service which manipulates and accesses the database.
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Views.Services
{
    public class SecurityDAO
    {
        public string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Minesweeper;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool RegisterUser(UserModel user)
        {
            bool success = false;
            // Prepared Statement
            string sqlStatement = "INSERT INTO dbo.users (firstName, lastName," +
                "gender, age, state, email, username, password) VALUES (@firstname, @lastname, @gender, @age, @state, @email, @username, @password)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar, 50).Value = user.firstName;
                command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar, 50).Value = user.lastName;
                command.Parameters.Add("@gender", System.Data.SqlDbType.NVarChar, 50).Value = user.gender;
                command.Parameters.Add("@age", System.Data.SqlDbType.Int, 100).Value = user.age;
                command.Parameters.Add("@state", System.Data.SqlDbType.NVarChar, 50).Value = user.state;
                command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 50).Value = user.email;
                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = user.username;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 50).Value = user.password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    success = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return success;
        }

        public bool findByUsernamePassword(UserModel user)
        {
            // assume negative result
            bool success = false;

            // prepared statements for increased security
            string sqlStatement = "SELECT * FROM dbo.users WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);

                // define values of placeholders
                cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 50).Value = user.username;
                cmd.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 50).Value = user.password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        success = true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return success;
        }

    }
}

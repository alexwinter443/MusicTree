using Microsoft.Extensions.Hosting;
using Milestone.Models;
using System;
using System.Data.SqlClient;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using DotNet5Crud.Models;

/*
 * Alex Vergara
 * 4/24/2022
 * cst 451
 * capstone
 * 
 * Security Data Access Service which manipulates and accesses the database.
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Views.Services
{
    public class SecurityDAO
    {
        public bool updateAudioFile(AudioFile audioFile)
        {
            bool success = true;
            string sqlStatement = "Update dbo.AudioFiles SET Name = @Name, Genre = @Genre, BPM = @BPM, Description = @Description, [Key] = @Key1 WHERE AudioFileId = @Id";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.Name;
                command.Parameters.Add("@Genre", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.Genre;
                command.Parameters.Add("@BPM", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.BPM;
                command.Parameters.Add("@Description", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.Description;
                command.Parameters.Add("@Key1", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.Key;
                command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.AudioFileId;
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






        public bool deleteComment(int audioFileID)
        {
            bool success = false;
            string sqlStatement = "DELETE FROM dbo.comments WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = audioFileID;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    success = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                return success;

            }
        }

        public bool uploadComment(string comment, int audiofileid)
        {
            bool success = false;

            string sqlStatement = "INSERT INTO dbo.comments (comment, FK_AudioFileID) VALUES (@comment, @FK_AudioFileID)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@comment", System.Data.SqlDbType.NVarChar, 50).Value = comment;
                command.Parameters.Add("@FK_AudioFileID", System.Data.SqlDbType.NVarChar, 50).Value = audiofileid;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    success = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return success;
        }




        public List<Comment> getAllComments(AudioFile audiofile)
        {
            // create a blank list of comments to return
            List<Comment> newComments = new List<Comment>();

            string sqlStatement = "SELECT * FROM dbo.comments WHERE FK_AudioFileID = @Id";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);

                // define values of placeholders
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = audiofile.AudioFileId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            newComments.Add(new Comment
                            {
                                Id = (int)reader["Id"],
                                comment = reader["comment"].ToString(),
                                FK_AudioFileID = (int)reader["FK_AudioFileID"]
                                
                            });

                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return newComments;
        }


        // connection to our database
        public string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                 
        // this method is used to register a user
        public bool RegisterUser(UserModel user)
        {
            bool success = false;
            // Prepared Statement
            string sqlStatement = "INSERT INTO dbo.users (firstName, lastName," +
                "gender, age, email, username, password) VALUES (@firstname, @lastname, @gender, @age, @email, @username, @password)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@firstname", System.Data.SqlDbType.NVarChar, 50).Value = user.firstName;
                command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar, 50).Value = user.lastName;
                command.Parameters.Add("@gender", System.Data.SqlDbType.NVarChar, 50).Value = user.gender;
                command.Parameters.Add("@age", System.Data.SqlDbType.Int, 100).Value = user.age;
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
        
        // this method is used to find if the user is exists in our database.
        public int findByUsernamePassword(UserModel user)
        {
            // send userID up through layers
            int userID = -1;

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
                        while (reader.Read())
                        {
                            userID = (int)reader.GetValue(0);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return userID;
        }


        public List<AudioFile> GetAudioFiles(int id)
        {

            string sqlStatement = "SELECT * FROM dbo.AudioFiles WHERE FK_audioID = @Id";

            List<AudioFile> newAudioFiles = new List<AudioFile>();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);

                // define values of placeholders
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int, 50).Value = id;
            

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            newAudioFiles.Add(new AudioFile { Name = reader["Name"].ToString(),
                                                              Genre = reader["Genre"].ToString(),
                                                              Key = reader["Key"].ToString(),
                                                              BPM = reader["BPM"].ToString(),
                                                              Description = reader["Description"].ToString(),
                                                              FileName = reader["Filename"].ToString(),
                                                              filepath = reader["filepath"].ToString()
                                                              });
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return newAudioFiles;

        }

    }
}

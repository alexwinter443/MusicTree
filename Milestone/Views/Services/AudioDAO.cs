using Azure.Messaging.EventHubs;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DotNet5Crud.Models;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Milestone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using Azure.Identity;
using Microsoft.ServiceBus.Messaging;

namespace Milestone.Views.Services
{
    public class AudioDAO
    {

        private const bool UseInteractiveAuth = false;
        private const string AdaptiveStreamingTransformName = "MyTransformWithAdaptiveStreamingPreset";
        public string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);
        // connection to our database
        public string connectionStr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private const string CustomTransform = "Custom_H264_3Layer";

        private const string OverlayFileName = @"cloud.png";
        private const string OverlayLabel = @"logo";


        public bool deleteComment(int commentId)
        {
            bool executed = false;
            string sqlStatement = "DELETE * FROM dbo.comments WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = commentId;
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    executed = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return executed;
        }



        public UserModel getUser(int userId)
        {
            string sqlStatement = "SELECT * FROM dbo.users WHERE Id = @Id";

            UserModel newAudioFiles = new UserModel();


            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = userId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            /* newAudioFiles. = Int32.Parse(reader["AudioFileId"].ToString());
                             newAudioFiles.Name = reader["Name"].ToString();
                             newAudioFiles.outputassetname = reader["OutputAssetName"].ToString();
 */
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


        // this method is used to find if the like item exists in our database.
        public string getLikeStatus(int userId, int audioFileId)
        {
            // send userID up through layers
            string liked = "";

            // prepared statements for increased security
            string sqlStatement = "SELECT * FROM dbo.Likes WHERE FK_userId = @Id and FK_audioId = @FK_userId";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand cmd = new SqlCommand(sqlStatement, connection);

                // define values of placeholders
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar, 50).Value = userId;
                cmd.Parameters.Add("@FK_userId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;
                try
                {
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            return (string)reader.GetValue(3);
                           
                        }
                    }
                    else
                    {
                        createLikeObject(audioFileId, userId);
                        reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            return (string)reader.GetValue(3);

                        }
                    }
                }
                // possiblly add like creation method here
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return liked;
        }


        public bool doesrowexist(int userId, int audioFileId)
        {
            string sqlStatement = "SELECT COUNT(*) FROM dbo.Likes WHERE FK_userId = @FK_userId AND FK_audioId = @FK_audioId";

            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@FK_userId", System.Data.SqlDbType.NVarChar, 50).Value = userId;
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;

                try
                {
                    
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            success = true;
                            //newAudioFiles.AudioFileId = Int32.Parse(reader["AudioFileId"].ToString());
                            //newAudioFiles.Name = reader["Name"].ToString();
                            //newAudioFiles.outputassetname = reader["OutputAssetName"].ToString();

                        }

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return success;
        }

        public int getNumberOfLikes(int audioFileId)
        {
            string sqlStatement = "Select likes from dbo.AudioFiles WHERE AudioFileId = @audioFileId";

            int result = 0;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@audioFileId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;
                try
                {
                    connection.Open();
                    result = (int)command.ExecuteScalar();
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            return result;
        }




        public bool increaseLikeCounter(int audioFileId)
        {
            string sqlStatement = "Update dbo.AudioFiles SET likes = @likes + 1 WHERE AudioFileId = @FK_audioId";

            bool success = true;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;

                // get number of likes inject method

                int resultofLikes = getNumberOfLikes(audioFileId);

                
                command.Parameters.Add("@likes", System.Data.SqlDbType.NVarChar, 50).Value = resultofLikes;

                try
                {
                    connection.Open();
                    command.ExecuteScalar();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            return success; 
        }



        public bool decreaseLikeCounter(int audioFileId)
        {
            string sqlStatement = "Update dbo.AudioFiles SET likes = @likes - 1 WHERE AudioFileId = @FK_audioId";

            bool success = true;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;

                // get number of likes inject method
                int resultofLikes = getNumberOfLikes(audioFileId);

                command.Parameters.Add("@likes", System.Data.SqlDbType.NVarChar, 50).Value = resultofLikes;

                try
                {
                    connection.Open();
                    command.ExecuteScalar();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return success;
        }




        // userID, and Ai
        public bool toggleLike(int userid, int audioFileId)
        {
            string sqlStatement = "Update dbo.Likes SET liked = @liked WHERE FK_audioId = @FK_AudioId AND FK_userId = @FK_userId";

            bool success = false;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileId;
                command.Parameters.Add("@FK_userId", System.Data.SqlDbType.NVarChar, 50).Value = userid;

                //gets likestatus
                string liked = getLikeStatus(userid, audioFileId);

                if(liked == "true")
                {
                    // inject like counter --
                    decreaseLikeCounter(audioFileId);
                    

                    command.Parameters.Add("@liked", System.Data.SqlDbType.NVarChar, 50).Value = "false";
                }
                else if(liked == "")
                {
                    increaseLikeCounter(audioFileId);


                    command.Parameters.Add("@liked", System.Data.SqlDbType.NVarChar, 50).Value = "true";
                }
                else
                {
                    // inject like counter ++
                    increaseLikeCounter(audioFileId);


                    command.Parameters.Add("@liked", System.Data.SqlDbType.NVarChar, 50).Value = "true";
                }

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


        public bool createLikeObject(int audioFileID, int id)
        {
            bool success = false;
            string sqlStatement = "INSERT INTO dbo.Likes (FK_userId, FK_audioId, liked) VALUES (@FK_userId,@FK_audioId,@liked)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@FK_userId", System.Data.SqlDbType.NVarChar, 50).Value = id;
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileID;
                command.Parameters.Add("@liked", System.Data.SqlDbType.NVarChar, 50).Value = "false";

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

        public bool createLikeObjectTrue(int audioFileID, int id)
        {
            bool success = false;
            string sqlStatement = "INSERT INTO dbo.Likes (FK_userId, FK_audioId, liked) VALUES (@FK_userId,@FK_audioId,@liked)";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@FK_userId", System.Data.SqlDbType.NVarChar, 50).Value = id;
                command.Parameters.Add("@FK_audioId", System.Data.SqlDbType.NVarChar, 50).Value = audioFileID;
                command.Parameters.Add("@liked", System.Data.SqlDbType.NVarChar, 50).Value = "false";

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



        public bool updateAudioFile(AudioFile audioFile)
        {
            bool success = true;
            string sqlStatement = "Update dbo.AudioFiles SET OutputAssetName = @Name, accountname = @account, resourcegroup = @group WHERE AudioFileId = @Id";

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);



                //Define Values of placeholders in SQL Statement string
                command.Parameters.Add("@Name", System.Data.SqlDbType.NVarChar, 50).Value = audioFile.outputassetname;
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


        public AudioFile GetAudioFileAsync(int id)
        {

            string sqlStatement = "SELECT * FROM dbo.AudioFiles WHERE AudioFileId = @Id";

            AudioFile newAudioFile = new AudioFile();

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
                            newAudioFile.AudioFileId = Int32.Parse(reader["AudioFileId"].ToString());
                            newAudioFile.Name = reader["Name"].ToString();
                            newAudioFile.outputassetname = reader["OutputAssetName"].ToString();                                            

                        }
                       
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
            return newAudioFile;

        }



        #region EnsureTransformExists
        /// <summary>
        /// If the specified transform exists, return that transform. If the it does not
        /// exist, creates a new transform with the specified output. In this case, the
        /// output is set to encode a video using a custom preset.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The transform name.</param>
        /// <returns></returns>
        private static async Task<Transform> CreateCustomTransform(IAzureMediaServicesClient client, string resourceGroupName, string accountName, string transformName)
        {

            Console.WriteLine("Creating a custom transform...");

            // Create a new Transform Outputs array - this defines the set of outputs for the Transform
            TransformOutput[] outputs = new TransformOutput[]
            {
                    // Create a new TransformOutput with a custom Standard Encoder Preset
                    // This demonstrates how to create custom codec and layer output settings

                  new TransformOutput(
                        new StandardEncoderPreset(
                           
                            //overlays image ontop of video
                            filters: new Filters
                            {
                                Overlays = new List<Overlay>
                                {
                                    new VideoOverlay
                                    {
                                        InputLabel = OverlayLabel,   // same as the one used in the JobInput to identify which asset is the overlay image
                                        Position = new Rectangle( "1200","670"), // left, top position of the overlay in absolute pixel position relative to the source videos resolution. 
                                        // Percentage based settings are coming soon, but not yet supported. In the future you can set this to "90%","90%" for example to be resolution independent on the source video positioning.
                                        // Opacity = 0.25  // opacity can be adjusted on the overlay.
                                    }
                                }
                            },

                            codecs: new Codec[]
                            {
                                // Add an AAC Audio layer for the audio encoding
                                new AacAudio(
                                    //channels: 2,
                                    //samplingRate: 48000,
                                    //bitrate: 128000,
                                    //profile: AacAudioProfile.AacLc
                                ),
                                // Next, add a H264Video for the video encoding
                               new H264Video (
                                    // Set the GOP interval to 2 seconds for all H264Layers
                                    keyFrameInterval:TimeSpan.FromSeconds(2),
                                     // Add H264Layers. Assign a label that you can use for the output filename

                                    layers:  new List<H264Layer>
                                    {
                                        new H264Layer (
                                            profile: H264VideoProfile.Baseline,
                                            bitrate: 1000000, // Units are in bits per second and not kbps or Mbps - 3.6 Mbps or 3,600 kbps
                                            width: "1280",
                                            height: "720"                                        
                                        ),
                                        new H264Layer (
                                            profile: H264VideoProfile.Baseline,
                                            bitrate: 600000, // Units are in bits per second and not kbps or Mbps - 1.6 Mbps or 1600 kbps
                                            width: "480",
                                            height: "720"                                   
                                        ),                                    
                                    }
                                    //layers:  new H264Layer[]
                                    //{
                                    //    new H264Layer (
                                    //        bitrate: 3600000, // Units are in bits per second and not kbps or Mbps - 3.6 Mbps or 3,600 kbps
                                    //        width: "1280",
                                    //        height: "720",
                                    //        label: "HD-3600kbps" // This label is used to modify the file name in the output formats
                                    //    ),
                                    //    new H264Layer (
                                    //        bitrate: 1600000, // Units are in bits per second and not kbps or Mbps - 1.6 Mbps or 1600 kbps
                                    //        width: "960",
                                    //        height: "540",
                                    //        label: "SD-1600kbps" // This label is used to modify the file name in the output formats
                                    //    ),
                                    //    new H264Layer (
                                    //        bitrate: 600000, // Units are in bits per second and not kbps or Mbps - 0.6 Mbps or 600 kbps
                                    //        width: "640",
                                    //        height: "360",
                                    //        label: "SD-600kbps" // This label is used to modify the file name in the output formats
                                    //    ),
                                    //}
                                ),
                              
                            },
                            // Specify the format for the output files - one for video+audio, and another for the thumbnails
                            formats: new Format[]
                            {
                                // Mux the H.264 video and AAC audio into MP4 files, using basename, label, bitrate and extension macros
                                // Note that since you have multiple H264Layers defined above, you have to use a macro that produces unique names per H264Layer
                                // Either {Label} or {Bitrate} should suffice
                                 
                                new Mp4Format(
                                    filenamePattern:"Video-{Basename}-{Label}-{Bitrate}{Extension}"
                                )
                                //new PngFormat(
                                //    filenamePattern:"Thumbnail-{Basename}-{Index}{Extension}"
                                //)
                            }
                            
                      
                        ),
                        onError: OnErrorType.StopProcessingJob,
                        relativePriority: Priority.Normal
                    )
            };

            string description = "A simple custom encoding transform with 2 MP4 bitrates";

            // Does a Transform already exist with the desired name? This method will just overwrite (Update) the Transform if it exists already. 
            // In production code, you may want to be cautious about that. It really depends on your scenario.
            Transform transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, outputs, description);

            return transform;
        }
        #endregion EnsureTransformExists






























        public enum KnownFolder
        {
            Contacts,
            Downloads,
            Favorites,
            Links,
            SavedGames,
            SavedSearches
        }

        public static class KnownFolders
        {
            private static readonly Dictionary<KnownFolder, Guid> _guids = new()
            {
                [KnownFolder.Contacts] = new("56784854-C6CB-462B-8169-88E350ACB882"),
                [KnownFolder.Downloads] = new("374DE290-123F-4565-9164-39C4925E467B"),
                [KnownFolder.Favorites] = new("1777F761-68AD-4D8A-87BD-30B759FA33DD"),
                [KnownFolder.Links] = new("BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968"),
                [KnownFolder.SavedGames] = new("4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4"),
                [KnownFolder.SavedSearches] = new("7D1D3A04-DEBB-4115-95CF-2F29DA2920DA")
            };

            public static string GetPath(KnownFolder knownFolder)
            {
                return SHGetKnownFolderPath(_guids[knownFolder], 0);
            }

            [DllImport("shell32",
                CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
            private static extern string SHGetKnownFolderPath(
                [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags,
                nint hToken = 0);
        }







        /// <summary>
        /// Run the sample async.
        /// </summary>
        /// <param name="config">The parm is of type ConfigWrapper. This class reads values from local configuration file.</param>
        /// <returns></returns>
        // <RunAsync>
        public static async Task RunAsync(ConfigWrapper config, string mp4name, AudioFile audiofileData)
        {
            

            string downloadsPath = KnownFolders.GetPath(KnownFolder.Downloads);


            IAzureMediaServicesClient client;
            try
            {
                client = await Authentication.CreateMediaServicesClientAsync(config, UseInteractiveAuth);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("TIP: Make sure that you have filled out the appsettings.json file before running this sample.");
                Console.Error.WriteLine($"{e.Message}");
                return;
            }

            // Set the polling interval for long running operations to 2 seconds.
            // The default value is 30 seconds for the .NET client SDK
            client.LongRunningOperationRetryTimeout = 50;

            // Creating a unique suffix so that we don't have name collisions if you run the sample
            // multiple times without cleaning up.
            string uniqueness = Guid.NewGuid().ToString("N");
            string jobName = $"job-{uniqueness}";
            string locatorName = $"locator-{uniqueness}";
            string outputAssetName = $"output-{uniqueness}";
            string inputAssetName = $"input-{uniqueness}";
            string logoAssetName = $"logo-{uniqueness}";
            audiofileData.outputassetname = outputAssetName;
            bool stopEndpoint = false;


            Transform transform = await CreateCustomTransform(client, config.ResourceGroup, config.AccountName, CustomTransform);


            // Ensure that you have the desired encoding Transform. This is really a one time setup operation.
            _ = await AudioDAO.GetOrCreateTransformAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName);

            // Create a new input Asset and upload the specified local video file into it.
            _ = await AudioDAO.CreateInputAssetAsync(client, config.ResourceGroup, config.AccountName, inputAssetName, mp4name);

            //_ = await AudioDAO.CreateInputAssetAsync(client, config.ResourceGroup, config.AccountName, logoAssetName, OverlayFileName);

            Asset overlayImageAsset = await CreateInputAssetAsync(client, config.ResourceGroup, config.AccountName, logoAssetName, OverlayFileName);




            // Use the name of the created input asset to create the job input.
            _ = new JobInputAsset(assetName: inputAssetName);

            // Output from the encoding Job must be written to an Asset, so let's create one
            Asset outputAsset = await AudioDAO.CreateOutputAssetAsync(client, config.ResourceGroup, config.AccountName, outputAssetName);

            _ = await AudioDAO.SubmitJobAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName, jobName, inputAssetName, outputAsset.Name, overlayImageAsset.Name);




            // In this demo code, we will poll for Job status
            // Polling is not a recommended best practice for production applications because of the latency it introduces.
            // Overuse of this API may trigger throttling. Developers should instead use Event Grid.
            Job job = await AudioDAO.WaitForJobToFinishAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName, jobName);

            if (job.State == Microsoft.Azure.Management.Media.Models.JobState.Finished)
            {
                Console.WriteLine("Job finished.");
                if (!Directory.Exists(downloadsPath))
                    Directory.CreateDirectory(downloadsPath);



                //await AudioDAO.DownloadOutputAssetAsync(client, config.ResourceGroup, config.AccountName, outputAsset.Name, downloadsPath);



                StreamingLocator locator = await AudioDAO.CreateStreamingLocatorAsync(client, config.ResourceGroup, config.AccountName, outputAsset.Name, locatorName);

                // Note that the URLs returned by this method include a /manifest path followed by a (format=)
                // parameter that controls the type of manifest that is returned. 
                // The /manifest(format=m3u8-aapl) will provide Apple HLS v4 manifest using MPEG TS segments.
                // The /manifest(format=mpd-time-csf) will provide MPEG DASH manifest.
                // And using just /manifest alone will return Microsoft Smooth Streaming format.
                // There are additional formats available that are not returned in this call, please check the documentation
                // on the dynamic packager for additional formats - see https://docs.microsoft.com/azure/media-services/latest/dynamic-packaging-overview
                IList<string> urls = await AudioDAO.GetStreamingUrlsAsync(client, config.ResourceGroup, config.AccountName, locator.Name);
                var firstElement = urls.First();
                audiofileData.filepath = firstElement;

                foreach (var url in urls)
                {
                    Console.WriteLine(url);
                }

            }

            Console.WriteLine("Done. Copy and paste the Streaming URL ending in '/manifest' into the Azure Media Player at 'http://aka.ms/azuremediaplayer'.");
            Console.WriteLine("See the documentation on Dynamic Packaging for additional format support, including CMAF.");
            Console.WriteLine("https://docs.microsoft.com/azure/media-services/latest/dynamic-packaging-overview");
        }
        // </RunAsync>


        /// <summary>
        /// Creates a new input Asset and uploads the specified local video file into it.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The asset name.</param>
        /// <param name="fileToUpload">The file you want to upload into the asset.</param>
        /// <returns></returns>
        // <CreateInputAsset>
        public static async Task<Asset> CreateInputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string assetName,
            string fileToUpload)
        {
            // In this example, we are assuming that the asset name is unique.
            //
            // If you already have an asset with the desired name, use the Assets.Get method
            // to get the existing asset. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).

            // Call Media Services API to create an Asset.
            // This method creates a container in storage for the Asset.
            // The files (blobs) associated with the asset will be stored in this container.
            Asset asset = await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset());

            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the exparation time for the SAS URL.
            var response = await client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime());

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            BlobContainerClient container = new BlobContainerClient(sasUri);
            BlobClient blob = container.GetBlobClient(Path.GetFileName(fileToUpload));

            // Use Strorage API to upload the file into the container in storage.
            await blob.UploadAsync(fileToUpload);

            return asset;
        }



        // </CreateInputAsset>

        /// <summary>
        /// Creates an ouput asset. The output from the encoding Job must be written to an Asset.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The output asset name.</param>
        /// <returns></returns>
        // <CreateOutputAsset>
        public static async Task<Asset> CreateOutputAssetAsync(IAzureMediaServicesClient client, string resourceGroupName, string accountName, string assetName)
        {
            bool existingAsset = true;
            Asset outputAsset;
            try
            {
                // Check if an Asset already exists
                outputAsset = await client.Assets.GetAsync(resourceGroupName, accountName, assetName);
            }
            catch (ErrorResponseException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                existingAsset = false;
            }

            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (existingAsset)
            {
                // Name collision! In order to get the sample to work, let's just go ahead and create a unique asset name
                // Note that the returned Asset can have a different name than the one specified as an input parameter.
                // You may want to update this part to throw an Exception instead, and handle name collisions differently.
                string uniqueness = $"-{Guid.NewGuid():N}";
                outputAssetName += uniqueness;

                Console.WriteLine("Warning – found an existing Asset with name = " + assetName);
                Console.WriteLine("Creating an Asset with this name instead: " + outputAssetName);
            }




            return await client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, outputAssetName, asset);



        }











        // </CreateOutputAsset>

        /// <summary>
        /// If the specified transform exists, get that transform.
        /// If the it does not exist, creates a new transform with the specified output. 
        /// In this case, the output is set to encode a video using one of the built-in encoding presets.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The name of the transform.</param>
        /// <returns></returns>
        // <EnsureTransformExists>
        public static async Task<Transform> GetOrCreateTransformAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName)
        {
            bool createTransform = false;
            Transform transform = null;
            try
            {
                // Does a transform already exist with the desired name? Assume that an existing Transform with the desired name
                // also uses the same recipe or Preset for processing content.
                transform = client.Transforms.Get(resourceGroupName, accountName, transformName);
            }
            catch (ErrorResponseException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                createTransform = true;
            }

            if (createTransform)
            {
                // You need to specify what you want it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
                        new TransformOutput
                        {
                            // The preset for the Transform is set to one of Media Services built-in sample presets.
                            // You can  customize the encoding settings by changing this to use "StandardEncoderPreset" class.
                            Preset = new BuiltInStandardEncoderPreset()
                            {
                                // This sample uses the built-in encoding preset for Adaptive Bitrate Streaming.
                                PresetName = EncoderNamedPreset.AdaptiveStreaming
                            }
                        }
                };

                // Create the Transform with the output defined above
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output);
            }

            return transform;
        }
        // </EnsureTransformExists>

        /// <summary>
        /// Submits a request to Media Services to apply the specified Transform to a given input video.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The name of the transform.</param>
        /// <param name="jobName">The (unique) name of the job.</param>
        /// <param name="inputAssetName">The name of the input asset.</param>
        /// <param name="outputAssetName">The (unique) name of the  output asset that will store the result of the encoding job. </param>
        // <SubmitJob>
        public static async Task<Job> SubmitJobAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName,
            string overlayAssetName)
        {
            // Use the name of the created input asset to create the job input.
            List<JobInput> jobInputs = new List<JobInput>()
            {
                new JobInputAsset(assetName: inputAssetName),
                new JobInputAsset(assetName: overlayAssetName, label: OverlayLabel)
            };

            JobOutput[] jobOutputs =
            {
                    new JobOutputAsset(outputAssetName),
            };

            // In this example, we are assuming that the job name is unique.
            //
            // If you already have a job with the desired name, use the Jobs.Get method
            // to get the existing job. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).
            Job job = await client.Jobs.CreateAsync(
                resourceGroupName,
                accountName,
                transformName,
                jobName,
                new Job
                {
                    Input = new JobInputs(inputs: jobInputs),
                    Outputs = jobOutputs,
                });

            return job;
        }
        // </SubmitJob>

        /// <summary>
        /// Polls Media Services for the status of the Job.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The name of the transform.</param>
        /// <param name="jobName">The name of the job you submitted.</param>
        /// <returns></returns>
        // <WaitForJobToFinish>
        public static async Task<Job> WaitForJobToFinishAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName)
        {
            const int SleepIntervalMs = 20 * 1000;

            Job job;
            do
            {
                job = await client.Jobs.GetAsync(resourceGroupName, accountName, transformName, jobName);

                Console.WriteLine($"Job is '{job.State}'.");
                for (int i = 0; i < job.Outputs.Count; i++)
                {
                    JobOutput output = job.Outputs[i];
                    Console.Write($"\tJobOutput[{i}] is '{output.State}'.");
                    if (output.State == Microsoft.Azure.Management.Media.Models.JobState.Processing)
                    {
                        Console.Write($"  Progress (%): '{output.Progress}'.");
                    }

                    Console.WriteLine();
                }

                if (job.State != Microsoft.Azure.Management.Media.Models.JobState.Finished && job.State != Microsoft.Azure.Management.Media.Models.JobState.Error && job.State != Microsoft.Azure.Management.Media.Models.JobState.Canceled)
                {
                    await Task.Delay(SleepIntervalMs);
                }
            }
            while (job.State != Microsoft.Azure.Management.Media.Models.JobState.Finished && job.State != Microsoft.Azure.Management.Media.Models.JobState.Error && job.State != Microsoft.Azure.Management.Media.Models.JobState.Canceled);

            return job;
        }
        // </WaitForJobToFinish>

        /// <summary>
        /// Creates a StreamingLocator for the specified asset and with the specified streaming policy name.
        /// Once the StreamingLocator is created the output asset is available to clients for playback.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The name of the output asset.</param>
        /// <param name="locatorName">The StreamingLocator name (unique in this case).</param>
        /// <returns></returns>
        // <CreateStreamingLocator>
        public static async Task<StreamingLocator> CreateStreamingLocatorAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string locatorName)
        {
            StreamingLocator locator = await client.StreamingLocators.CreateAsync(
                resourceGroup,
                accountName,
                locatorName,
                new StreamingLocator
                {
                    AssetName = assetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                });

            return locator;
        }
        // </CreateStreamingLocator>

        /// <summary>
        /// Checks if the "default" streaming endpoint is in the running state,
        /// if not, starts it.
        /// Then, builds the streaming URLs.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="locatorName">The name of the StreamingLocator that was created.</param>
        /// <returns></returns>
        // <GetStreamingURLs>
        public static async Task<IList<string>> GetStreamingUrlsAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            String locatorName)
        {
            const string DefaultStreamingEndpointName = "default";

            IList<string> streamingUrls = new List<string>();

            StreamingEndpoint streamingEndpoint = await client.StreamingEndpoints.GetAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);

            if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
            {
                await client.StreamingEndpoints.StartAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);
            }

            ListPathsResponse paths = await client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName);

            foreach (StreamingPath path in paths.StreamingPaths)
            {
                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,

                    Path = path.Paths[0]
                };
                streamingUrls.Add(uriBuilder.ToString());
            }
            return streamingUrls;
        }







        // </GetStreamingURLs>

        /// <summary>
        ///  Downloads the results from the specified output asset, so you can see what you got.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The output asset.</param>
        /// <param name="outputFolderName">The name of the folder into which to download the results.</param>
        // <DownloadResults>
        public static async Task DownloadOutputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string outputFolderName)
        {
            if (!Directory.Exists(outputFolderName))
            {
                Directory.CreateDirectory(outputFolderName);
            }

            AssetContainerSas assetContainerSas = await client.Assets.ListContainerSasAsync(
                resourceGroup,
                accountName,
                assetName,
                permissions: AssetContainerPermission.Read,
                expiryTime: DateTime.UtcNow.AddHours(1).ToUniversalTime());

            Uri containerSasUrl = new Uri(assetContainerSas.AssetContainerSasUrls.FirstOrDefault());
            BlobContainerClient container = new BlobContainerClient(containerSasUrl);

            string directory = Path.Combine(outputFolderName, assetName);
            Directory.CreateDirectory(directory);

            Console.WriteLine($"Downloading output results to '{directory}'...");

            string continuationToken = null;
            IList<Task> downloadTasks = new List<Task>();

            do
            {
                var resultSegment = container.GetBlobs().AsPages(continuationToken);

                foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        var blobClient = container.GetBlobClient(blobItem.Name);
                        string filename = Path.Combine(directory, blobItem.Name);

                        downloadTasks.Add(blobClient.DownloadToAsync(filename));
                    }
                    // Get the continuation token and loop until it is empty.
                    continuationToken = blobPage.ContinuationToken;
                }

            } while (continuationToken != "");

            await Task.WhenAll(downloadTasks);

            Console.WriteLine("Download complete.");
        }
        // </DownloadResults>
        



        /// <summary>
        /// Deletes the jobs, assets and potentially the content key policy that were created.
        /// Generally, you should clean up everything except objects 
        /// that you are planning to reuse (typically, you will reuse Transforms, and you will persist output assets and StreamingLocators).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="resourceGroupName"></param>
        /// <param name="accountName"></param>
        /// <param name="transformName"></param>
        /// <param name="jobName"></param>
        /// <param name="assetNames"></param>
        /// <param name="contentKeyPolicyName"></param>
        /// <returns></returns>
        // <CleanUp>
        public static async Task CleanUpAsync(
           IAzureMediaServicesClient client,
           string resourceGroupName,
           string accountName,
           string transformName,
           string jobName,
           List<string> assetNames,
           string contentKeyPolicyName = null
           )
        {
            await client.Jobs.DeleteAsync(resourceGroupName, accountName, transformName, jobName);

            foreach (var assetName in assetNames)
            {
                await client.Assets.DeleteAsync(resourceGroupName, accountName, assetName);
            }

            if (contentKeyPolicyName != null)
            {
                client.ContentKeyPolicies.Delete(resourceGroupName, accountName, contentKeyPolicyName);
            }
        }
        // </CleanUp






    }
}

using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Identity.Client;
using Microsoft.Rest;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Milestone.Models
{
    public class Authentication
    {
        public static readonly string TokenType = "Bearer";

        /// <summary>
        /// Creates the AzureMediaServicesClient object based on the credentials
        /// supplied in local configuration file.
        /// </summary>
        /// <param name="config">The param is of type ConfigWrapper, which reads values from local configuration file.</param>
        /// <returns>A task.</returns>
        // <CreateMediaServicesClientAsync>
        public static async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(ConfigWrapper config, bool interactive = false)
        {
            ServiceClientCredentials credentials;
            if (interactive)
                credentials = await GetCredentialsInteractiveAuthAsync(config);
            else
                credentials = await GetCredentialsAsync(config);

            return new AzureMediaServicesClient(config.ArmEndpoint, credentials)
            {
                SubscriptionId = config.SubscriptionId,
            };
        }
        // </CreateMediaServicesClientAsync>

        /// <summary>
        /// Create the ServiceClientCredentials object based on the credentials
        /// supplied in local configuration file.
        /// </summary>
        /// <param name="config">The param is of type ConfigWrapper. This class reads values from local configuration file.</param>
        /// <returns></returns>
        // <GetCredentialsAsync>
        private static async Task<ServiceClientCredentials> GetCredentialsAsync(ConfigWrapper config)
        {
            // Use ConfidentialClientApplicationBuilder.AcquireTokenForClient to get a token using a service principal with symmetric key

            var scopes = new[] { config.ArmAadAudience + "/.default" };

            var app = ConfidentialClientApplicationBuilder.Create(config.AadClientId)
                .WithClientSecret(config.AadSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.AadTenantId)
                .Build();

            var authResult = await app.AcquireTokenForClient(scopes)
                                                     .ExecuteAsync()
                                                     .ConfigureAwait(false);

            return new TokenCredentials(authResult.AccessToken, TokenType);
        }
        // </GetCredentialsAsync>

        /// <summary>
        /// Create the ServiceClientCredentials object based on interactive authentication done in the browser
        /// </summary>
        /// <param name="config">The param is of type ConfigWrapper. This class reads values from local configuration file.</param>
        /// <returns></returns>
        // <GetCredentialsInteractiveAuthAsync>
        private static async Task<ServiceClientCredentials> GetCredentialsInteractiveAuthAsync(ConfigWrapper config)
        {
            var scopes = new[] { config.ArmAadAudience + "/user_impersonation" };

            // client application of Az Cli
            string ClientApplicationId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46";

            AuthenticationResult result = null;

            IPublicClientApplication app = PublicClientApplicationBuilder.Create(ClientApplicationId)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.AadTenantId)
                .WithRedirectUri("http://localhost")
                .Build();

            var accounts = await app.GetAccountsAsync();

            try
            {
                result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
            }
            catch (MsalUiRequiredException ex)
            {
                try
                {
                    result = await app.AcquireTokenInteractive(scopes).ExecuteAsync();
                }
                catch (MsalException maslException)
                {
                    Console.Error.WriteLine($"ERROR: MSAL interactive authentication exception with code '{maslException.ErrorCode}' and message '{maslException.Message}'.");
                }
            }
            catch (MsalException maslException)
            {
                Console.Error.WriteLine($"ERROR: MSAL silent authentication exception with code '{maslException.ErrorCode}' and message '{maslException.Message}'.");
            }

            return new TokenCredentials(result.AccessToken, TokenType);
        }


        /*/// <summary>
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
                            codecs: new Codec[]
                            {
                                // Add an AAC Audio layer for the audio encoding
                                new AacAudio(
                                    channels: 2,
                                    samplingRate: 48000,
                                    bitrate: 128000,
                                    profile: AacAudioProfile.AacLc
                                ),
                                // Next, add a H264Video for the video encoding
                               new H264Video (
                                    // Set the GOP interval to 2 seconds for both H264Layers
                                    keyFrameInterval:TimeSpan.FromSeconds(2),
                                     // Add H264Layers, one at HD and the other at SD. Assign a label that you can use for the output filename
                                    layers:  new H264Layer[]
                                    {
                                        new H264Layer (
                                            bitrate: 1000000, // Units are in bits per second
                                            width: "1280",
                                            height: "720",
                                            label: "HD" // This label is used to modify the file name in the output formats
                                        ),
                                        new H264Layer (
                                            bitrate: 600000,
                                            width: "640",
                                            height: "360",
                                            label: "SD"
                                        )
                                    }
                                ),
                                // Also generate a set of thumbnails in one Jpg file (thumbnail sprite)
                                new JpgImage(
                                    start: "0%",
                                    step: "5%",
                                    range: "100%",
                                    spriteColumn: 10,
                                    layers: new JpgLayer[]{
                                        new JpgLayer(
                                            width: "20%",
                                            height: "20%",
                                            quality : 90
                                        )
                                    }
                                )
                            },
                            // Specify the format for the output files - one for video+audio, and another for the thumbnail sprite
                            formats: new Format[]
                            {
                                // Mux the H.264 video and AAC audio into MP4 files, using basename, label, bitrate and extension macros
                                // Note that since you have multiple H264Layers defined above, you have to use a macro that produces unique names per H264Layer
                                // Either {Label} or {Bitrate} should suffice
                                 
                                new Mp4Format(
                                    filenamePattern:"Video-{Basename}-{Label}-{Bitrate}{Extension}"
                                ),
                                new JpgFormat(
                                    filenamePattern:"ThumbnailSprite-{Basename}-{Index}{Extension}"
                                )
                            }
                        ),
                        onError: OnErrorType.StopProcessingJob,
                        relativePriority: Priority.Normal
                    )
            };

            string description = "A simple custom encoding transform with 2 MP4 bitrates and thumbnail sprite";
            // Create the custom Transform with the outputs defined above
            // Does a Transform already exist with the desired name? This method will just overwrite (Update) the Transform if it exists already. 
            // In production code, you may want to be cautious about that. It really depends on your scenario.
            Transform transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, outputs, description);

            return transform;
        }*/



    }
}

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DotNet5Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Milestone.Models;
using Milestone.Views.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;
using Microsoft.WindowsAzure.MediaServices.Client;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net.Http;

namespace DotNet5Crud.Controllers
{
    /* Alex Vergara
    *  Capstone
    *  4/24/2022
    *  This is the controller for the audiofile 
    */
    public class AudioFileController : Controller
    {
        //variables
        private readonly AudioFileDBContext _context;
        private readonly IWebHostEnvironment env;



        public async Task<IActionResult> DownloadFile(int audioFileId)
        {
            AudioDAO securityDAO = new AudioDAO();
            AudioFile audiofile = securityDAO.GetAudioFileAsync(audioFileId);
            bool UseInteractiveAuth = false;


            ConfigWrapper config = new(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables() // parses the values from the optional .env file at the solution root
                .Build());

            IAzureMediaServicesClient client;

            client = await Authentication.CreateMediaServicesClientAsync(config, UseInteractiveAuth);

            await AudioDAO.DownloadOutputAssetAsync(client, "capstoneresourcegroup","capstoneservices", audiofile.outputassetname, securityDAO.downloadsPath, audiofile.Name);
            return RedirectToAction("Index", "AudioFile");

        }


        [HttpPost]
        public async Task<IActionResult> uploadComment(string comment, AudioFile audioFile, int userid)
        {

            SecurityDAO securityDAO = new SecurityDAO();

            if(securityDAO.uploadComment(comment, audioFile.AudioFileId,userid))
            {
                return RedirectToAction("Index", "AudioFile");
            }

            else
            {
                return NotFound();
            }
        }

        //[HttpPost]
        public async Task<IActionResult> deleteComment(int audioFileId)
        {
            SecurityService securityserv = new SecurityService();
            if (securityserv.deleteComment((int)audioFileId))
            {
                return RedirectToAction("Index", "AudioFile");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> getComments(AudioFile audioFile)
        {
            SecurityService securityserv = new SecurityService();
            List<Comment> audiofilecomments = securityserv.getComments(audioFile);
            return View(audiofilecomments);
        }
        
        // cosntructor
        public AudioFileController(AudioFileDBContext context)
        {
            _context = context;
        }

        // index
        public async Task<IActionResult> Index()
        {
            var audioFiles = await _context.AudioFiles.ToListAsync();
            return View(audioFiles);
        }


        public async Task<IActionResult> MyFiles1()
        {
            int id1 = (int)HttpContext.Session.GetInt32("userID");
            SecurityService securityserv = new SecurityService();
            List<AudioFile> newfiles = securityserv.GetAudioFiles(id1);
            return View(newfiles);
        }
     


        //Add Or Edit Get Method
        public async Task<IActionResult> AddOrEdit(int? audioFileId)
        {
            // passes values to form
            ViewBag.PageName = audioFileId == null ? "Create Instrumental" : "Edit Instrumental";
            ViewBag.IsEdit = audioFileId == null ? false : true;
            // if audiofile id is empty
            if (audioFileId == null)
            {
                /* MultipleFilesModel model = new MultipleFilesModel();
                 return View(model);*/
         
                return View();
            }
            
            else
            {
                // fetch audio file associated with ID
                var audioFile = await _context.AudioFiles.FindAsync(audioFileId);
                
                // if audiofile does not exists
                if (audioFile == null)
                {
                    return NotFound();
                }
                //else return view associated with audikofile
                return View(audioFile);
            }        
        }


        public async Task<IActionResult> EditAudioOnly(int? audioFileId)
        {
            // passes values to form
            ViewBag.PageName = audioFileId == null ? "Create Instrumental" : "Edit Instrumental";
            ViewBag.IsEdit = audioFileId == null ? false : true;
            // if audiofile id is empty
            if (audioFileId == null)
            {
               /* CombinedModel model = new CombinedModel();
                return View(model);*/
                return View();
            }

            else
            {
                // fetch audio file associated with ID
                var audioFile = await _context.AudioFiles.FindAsync(audioFileId);

                // if audiofile does not exists
                if (audioFile == null)
                {
                    return NotFound();
                }
                //else return view associated with audikofile
                return View(audioFile);
            }
        }



        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> AddOrEdit2(IFormCollection formCollection, IFormFile file, IFormFile file2)
        {
            SecurityService securityserv = new SecurityService();

            int userID1 = (int)HttpContext.Session.GetInt32("userID");

            AudioFile audioFile = new AudioFile();

            Account account = new Account(
                "dawmedia",
                "948836387591992",
                "39ghSoIH3vubnAu35VZnL7tqxh8");

            Cloudinary cloudinary = new Cloudinary(account);

            HttpClient client = new HttpClient();
            var formContent = new MultipartFormDataContent();


            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
           // FileInfo fileInfo = new FileInfo(file2.FileName);
            // creates filename for our mp4
            string fileName = file2.FileName;
            // self describing
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file2.CopyTo(stream);


            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileNameWithPath),
                PublicId = file2.FileName
            };

            var uploadResult2 = cloudinary.Upload(uploadParams);

            var res = "https://res.cloudinary.com/dawmedia/image/fetch/c_scale,h_400,w_500/" + uploadResult2.Url;

            try
            {
                // sets values of audiofile
                audioFile.Name = Request.Form["Name"];
                audioFile.Description = Request.Form["Description"];
                audioFile.BPM = Request.Form["BPM"];
                audioFile.Genre = Request.Form["Genre"];
                audioFile.FileName = Request.Form["filename"];
                audioFile.Key = Request.Form["Key"];
                audioFile.File = file;
                audioFile.File2 = file2;
                audioFile.FK_audioID = userID1;
                audioFile.filepath = "";
                audioFile.likes = 0;
                audioFile.jpgassetname = res;
                //     string hello2 = file.FileName;
                // gets extension of file mp3 or mp4
                audioFile.fileformat = file.FileName.Substring(file.FileName.Length - 3);
                

                //var idasd = Request.Form[""];
                //audioFile.AudioFileId = Int32.Parse(Request.Form["AudioFileId"].ToString());



                _context.Add(audioFile);

                //audioFile.AudioFileId = Int32.Parse(Request.Form["AudioFileId"].ToString());

                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(audioFile.Name);
                // creates filename for our mp4
                fileName = "b" + fileInfo.Extension;
                // self describing
                fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    audioFile.File.CopyTo(stream);

                }

                try
                {
                    DotEnv.Load(".env");
                }
                catch
                {

                }

                ConfigWrapper config = new(new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables() // parses the values from the optional .env file at the solution root
                    .Build());



                try
                {
                    await NewMethod(config);
                }

                catch (Exception exception)
                {
                    if (exception.Source.Contains("ActiveDirectory"))
                    {
                        Console.Error.WriteLine("TIP: Make sure that you have filled out the appsettings.json file before running this sample.");
                    }

                    Console.Error.WriteLine($"{exception.Message}");

                    if (exception.GetBaseException() is ErrorResponseException apiException)
                    {
                        Console.Error.WriteLine(
                            $"ERROR: API call failed with error code '{apiException.Body.Error.Code}' and message '{apiException.Body.Error.Message}'.");
                    }
                }

                Task NewMethod(ConfigWrapper config)
                {
                    return AudioDAO.RunAsync(config, fileNameWithPath, audioFile);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            try
            {
                AudioDAO audioDAO = new AudioDAO();
                try
                {
                    audioDAO.createLikeObject(audioFile.AudioFileId, userID1);
                }
                catch (Exception ex)
                {
                    return NotFound();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));

        }



        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<IActionResult> EditAudioOnly(int? audioFileId, IFormCollection formCollection, IFormFile file)
        {
            
            AudioFile audioFile = new AudioFile();

            audioFile.Name = Request.Form["Name"];
            audioFile.Description = Request.Form["Description"];
            audioFile.BPM = Request.Form["BPM"];
            audioFile.Genre = Request.Form["Genre"];
            audioFile.FileName = Request.Form["filename"];
            audioFile.Key = Request.Form["Key"];
            audioFile.AudioFileId = Int32.Parse(Request.Form["AudioFileId"].ToString());

            SecurityService securityService = new SecurityService();
            if (securityService.updateAudioFile(audioFile))
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        
        }


        public async Task<IActionResult> LikeAudio(int audioFileId)
        {
            int id1 = (int)HttpContext.Session.GetInt32("userID");
            AudioDAO audioDAO = new AudioDAO();

            SecurityService securityserv = new SecurityService();

            // passes in userID, and audioFileID
            if (audioDAO.toggleLike(id1, audioFileId))
            {
                return RedirectToAction("Index", "AudioFile");
            }
            else
            {
                return NotFound();
            }
        }


        // Audio File Details
        public async Task<IActionResult> Details(int? audioFileId)
        {
            // If  audio file is null
            if (audioFileId == null)
            {
                return NotFound();
            }

            
      
            // gets audio file details for singluar audio file
            var audioFile = await _context.AudioFiles.FirstOrDefaultAsync(m => m.AudioFileId == audioFileId);
            if (audioFile == null)
            {
                return NotFound();
            }


            SecurityService securityserv = new SecurityService();
            AudioDAO audioDAO = new AudioDAO();
            List<Comment> audiofilecomments = securityserv.getComments(audioFile);
            int userID1 = (int)HttpContext.Session.GetInt32("userID");

            string getStatus = audioDAO.getLikeStatus(userID1, audioFile.AudioFileId);
            

            var combinedModel = new ViewModel()
            {
                audiofile = audioFile,
                comments = audiofilecomments,
                userID = userID1,
                liked = getStatus,
            };

            return View(combinedModel);
        }



        // GET: audioFiles/Delete/1
        public async Task<IActionResult> Delete(int? audioFileId)
        {
            // error catching
            if (audioFileId == null)
            {
                return NotFound();
            }
            // retrieves audio file values to pass through
            var audioFile = await _context.AudioFiles.FirstOrDefaultAsync(m => m.AudioFileId == audioFileId);

            if (audioFile == null)
            {
                return NotFound();
            }

            return View(audioFile);
        }

        // POST: audioFiles/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int audioFileId)
        {
            // 
            var audioFile = await _context.AudioFiles.FindAsync(audioFileId);
            _context.AudioFiles.Remove(audioFile);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}

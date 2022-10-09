using DotNet5Crud.Models;
using Milestone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Security Service which acts as a business service for the functions which require security. 
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Views.Services
{
    public class SecurityService
    {

        SecurityDAO SecurityDAO = new SecurityDAO();

        public bool IsValid(UserModel user)
        {
            return SecurityDAO.RegisterUser(user);
        }

        public int loginUser(UserModel user)
        {
            return SecurityDAO.findByUsernamePassword(user);
        }

        public List<AudioFile> GetAudioFiles(int userId)
        {
            return SecurityDAO.GetAudioFiles(userId);
        }

        public List<Comment> getComments(AudioFile audioFile)
        {
            return SecurityDAO.getAllComments(audioFile);
        }

        public bool deleteComment(int audioFileID)
        {
            return SecurityDAO.deleteComment(audioFileID);
        }

        public bool updateAudioFile(AudioFile audioFile)
        {
            return SecurityDAO.updateAudioFile(audioFile);
        }
    }
}

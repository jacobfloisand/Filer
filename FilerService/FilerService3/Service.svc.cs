﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FilerService3
{
    
    public class FilerService : IFilerService
    {
        private static string FilerDB;

        static FilerService()
        {
            FilerDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
        }

        /// <summary>
        /// Adds a file to the data base. Usable fields are:
        /// File/Address - The file or web address to be stored in the DB.
        /// Date -         The date for this file(usually today's date)
        /// Name/Link -    The name of the file or link
        /// Class -        the class of the file or link.
        /// Unit -         the unit of the file or link.
        /// Section
        /// Type
        /// isLink
        /// Override - true if this name is to override any file with the same information
        ///     in the DB.
        /// </summary>
        /// <param name="data"></param>
        public void AddFile(ResourceData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new task to the data base. Usable fields are:
        /// Task - the task to be added to the DB.
        /// </summary>
        /// <param name="task"></param>
        public void AddNewTask(OneTask task)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes a file/link from the database. Usable fields are:
        /// Name
        /// Class
        /// Unit
        /// Section
        /// </summary>
        /// <param name="Nickname"></param>
        public void Delete(ResourceData Nickname)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches DB using specified criteria. Usable fields are:
        /// Name
        /// Date
        /// Class
        /// Unit
        /// Section
        /// Type
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public SearchData DoSearch(ResourceData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns all of the tags. No input
        /// </summary>
        /// <returns></returns>
        public TagData GetAllTags()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a file from the DB. Usable fields are:
        /// Name
        /// Class
        /// Unit
        /// Section
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ResourceData GetFullFile(ResourceData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the last tags used in a search or save. No input.
        /// </summary>
        /// <returns></returns>
        public ResourceData GetLastTags()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the information for the last 5 items that were filed. No input.
        /// </summary>
        /// <returns></returns>
        public SearchData GetRecentlyFiled()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the full to-do list. No input.
        /// </summary>
        /// <returns></returns>
        public TaskData GetToDoList()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a new tag. Usable fields are:
        /// Class
        /// Unit
        /// Section
        /// Type
        /// </summary>
        /// <returns></returns>
        public void MakeNewTag(ResourceData data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a to do list item from DB. Usable field is:
        /// Task
        /// </summary>
        /// <param name="task"></param>
        public void RemoveTask(OneTask task)
        {
            throw new NotImplementedException();
        }
    }
}

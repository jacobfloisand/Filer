using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace FilerService
{
    
    public class FilerService : IFilerService
    {
        private static string FilerDB;

        static FilerService()
        {
            FilerDB = ConfigurationManager.ConnectionStrings["FilerDB"].ConnectionString;
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
        /// Note: This method does not update the tag file with the DB.
        /// </summary>
        /// <param name="data"></param>
        public void AddFile(ResourceData data)
        {
            //Dub our local variables:
            string linkName = data.LinkName;
            string fileName = data.FileName;
            string link = data.Link;
            string file = data.File;
            string date = data.Date;
            string myClass = data.Class;
            string unit = data.Unit;
            string section = data.Section;
            string type = data.Type;
            int dataID = 0;
            string isLink = data.isLink;

            if(!CanBeAdded(data)) //Should only be run if override is false and the item exists in DB.
            {
                SetStatus(HttpStatusCode.Conflict);
                return;
            }
            //Check to see if data is a link or file.
            if(isLink.Equals("true"))
            {
                //add the link to the database. Another transaction is used for the tags.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("insert into Links (Link, Name, Date)" +
                                            "values(@link, @linkName, @date)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@link", link);
                            command.Parameters.AddWithValue("@linkName", linkName);
                            command.Parameters.AddWithValue("@date", date);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                dataID = reader.GetInt32(0);
                            }

                            trans.Commit();
                        }
                    }
                }


                //Now add the tag information for the file.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("insert into Classes(DataID, Class)" +
                                            "values(@DataID, @myClass)" +
                                            "insert into Units(DataID, Unit)" +
                                            "values(@DataID, @unit)" +
                                            "insert into Sections(DataID, Section)" +
                                            "values(@DataID, @section)" +
                                            "insert into Types(DataID, Type)" +
                                            "values(@DataID, @type)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@myClass", myClass);
                            command.Parameters.AddWithValue("@unit", unit);
                            command.Parameters.AddWithValue("@section", section);
                            command.Parameters.AddWithValue("@type", type);
                            command.Parameters.AddWithValue("@DataID", dataID);

                            command.ExecuteNonQuery();
                            trans.Commit();
                        }
                    }
                }
                SetStatus(HttpStatusCode.Accepted);
                return;
            }

            else
            {
                //add the file to the database. We will add the file tags in another transaction.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("insert into Files (Archive, Name, Date)" +
                                            "values(@file, @fileName, @date)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@file", file);
                            command.Parameters.AddWithValue("@fileName", fileName);
                            command.Parameters.AddWithValue("@date", date);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                dataID = reader.GetInt32(0);
                            }
                            
                            trans.Commit();
                        }
                    }
                }


                //Now add the tag information for the file.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("insert into Classes(DataID, Class)" +
                                            "values(@DataID, @myClass)" +
                                            "insert into Units(DataID, Unit)" +
                                            "values(@DataID, @unit)" +
                                            "insert into Sections(DataID, Section)" +
                                            "values(@DataID, @section)" +
                                            "insert into Types(DataID, Type)" +
                                            "values(@DataID, @type)", conn, trans))
                        {
                            command.Parameters.AddWithValue("@myClass", myClass);
                            command.Parameters.AddWithValue("@unit", unit);
                            command.Parameters.AddWithValue("@section", section);
                            command.Parameters.AddWithValue("@type", type);
                            command.Parameters.AddWithValue("@DataID", dataID);

                            command.ExecuteNonQuery();
                            trans.Commit();
                        }
                    }
                }
                SetStatus(HttpStatusCode.Accepted);
            }

        }

        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }


        private bool CanBeAdded(ResourceData data)
        {
            string linkName = data.LinkName;
            string fileName = data.FileName;
            string link = data.Link;
            string file = data.File;
            string date = data.Date;
            string myClass = data.Class;
            string unit = data.Unit;
            string section = data.Section;
            string type = data.Type;
            int dataID = 0;
            string isLink = data.isLink;
            bool isMatch = false;

            //Check DB to see if name, class, unit, and section all match entry in DB. If Override is true we just delete right away. If nothing is deleted it wasn't there to begin with.
            //Only do this first one if it's a link.
            if (isLink.Equals(true))
            {


                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand("Select Links.DataID from Links, Classes, Units, Sections where Links.Name = @linkName AND Classes.Class = @myClass AND Units.Unit = @unit AND Sections.Section = @section", conn, trans))
                        {
                            command.Parameters.AddWithValue("@myClass", myClass);
                            command.Parameters.AddWithValue("@unit", unit);
                            command.Parameters.AddWithValue("@section", section);
                            command.Parameters.AddWithValue("@type", type);
                            command.Parameters.AddWithValue("@DataID", dataID);
                            command.Parameters.AddWithValue("@linkName", linkName);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                isMatch = reader.Read();
                            }
                            trans.Commit();
                        }
                    }
                    if (!isMatch) //If we didn't find a match in the DB.
                    {
                        return true;
                    }
                    if (data.Override.Equals(true)) //If we found a match and override is true...
                    {
                        //Delete the item we found.
                        Delete(data);
                        return true;
                    }
                    //If we found a match and override is not true...
                    return false;
                }
            }

            //If the item we are adding is not a link...
            


            using (SqlConnection conn = new SqlConnection(FilerDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("Select Files.DataID from Files, Classes, Units, Sections where Files.Name = @fileName AND Classes.Class = @myClass AND Units.Unit = @unit AND Sections.Section = @section", conn, trans))
                    {
                        command.Parameters.AddWithValue("@myClass", myClass);
                        command.Parameters.AddWithValue("@unit", unit);
                        command.Parameters.AddWithValue("@section", section);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@DataID", dataID);
                        command.Parameters.AddWithValue("@fileName", fileName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isMatch = reader.Read();
                        }
                        trans.Commit();
                    }
                }
                if (!isMatch) //If we didn't find a match in the DB.
                {
                    return true;
                }
                if (data.Override.Equals(true)) //If we found a match and override is true...
                {
                    //Delete the item we found.
                    Delete(data);
                    return true;
                }
                //If we found a match and override is not true...
                return false;
            }
            
            //If so check to see if we override this time.
            //If we don't override this time return false.
            //If we do perform a delete operation and return true.
            return false;
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
        public SearchData DoSearch(string Class, string Unit, string Section, string Name, string Date, string Type)
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
        public ResourceData GetFullFile(string Class, String Unit, String Section, String Name)
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

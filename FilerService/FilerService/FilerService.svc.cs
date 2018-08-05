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

            if (data == null || isLink == null || data.Override == null)
            {
                SetStatus(HttpStatusCode.Forbidden);
                return;
            }
            if(isLink.Equals("true") && (link == null || linkName == null))
            {
                SetStatus(HttpStatusCode.Forbidden);
                return;
            }
            if (isLink.Equals("false") && (file == null || fileName == null))
            {
                SetStatus(HttpStatusCode.Forbidden);
                return;
            }

            if (!CanBeAdded(data)) //Should only be run if override is false and the item exists in DB.
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

                string queryString = "";
                if(myClass != null)
                {
                    queryString = queryString + "insert into Classes(DataID, Class) values(@dataID, @myClass) ";
                }
                if(unit != null)
                {
                    queryString = queryString + "insert into Units(DataID, Unit) values(@DataID, @unit) ";
                }
                if(section != null)
                {
                    queryString = queryString + "insert into Sections(DataID, Section) values(@DataID, @section) ";
                }
                if(type != null)
                {
                    queryString = queryString + "insert into Types(DataID, Type) values(@DataID, @type) ";
                }

                //Now add the tag information for the file.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@DataID", dataID);
                            if (myClass != null)
                            {
                                command.Parameters.AddWithValue("@myClass", myClass);
                            }
                            if (unit != null)
                            {
                                command.Parameters.AddWithValue("@unit", unit);
                            }
                            if (section != null)
                            {
                               command.Parameters.AddWithValue("@section", section);
                            }
                            if (type != null)
                            {
                                command.Parameters.AddWithValue("@type", type);
                            }


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

                string queryString = "";
                if (myClass != null)
                {
                    queryString = queryString + "insert into Classes(DataID, Class) values(@dataID, @myClass) ";
                }
                if (unit != null)
                {
                    queryString = queryString + "insert into Units(DataID, Unit) values(@DataID, @unit) ";
                }
                if (section != null)
                {
                    queryString = queryString + "insert into Sections(DataID, Section) values(@DataID, @section) ";
                }
                if (type != null)
                {
                    queryString = queryString + "insert into Types(DataID, Type) values(@DataID, @type) ";
                }

                //Now add the tag information for the file.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@DataID", dataID);
                            if (myClass != null)
                            {
                                command.Parameters.AddWithValue("@myClass", myClass);
                            }
                            if (unit != null)
                            {
                                command.Parameters.AddWithValue("@unit", unit);
                            }
                            if (section != null)
                            {
                                command.Parameters.AddWithValue("@section", section);
                            }
                            if (type != null)
                            {
                                command.Parameters.AddWithValue("@type", type);
                            }

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
            if (isLink.Equals("true"))
            {
                //Make our query string here. Complete with if statements depending on if inputs are null.
                string myQueryString = "Select Links.DataID from Links, Classes, Units, Sections where Links.Name = @linkName ";
                if (myClass != null)
                {
                    myQueryString = myQueryString + "AND Classes.Class = @myClass ";
                }
                if (unit != null)
                {
                    myQueryString = myQueryString + "AND Units.Unit = @unit ";
                }
                if (section != null)
                {
                    myQueryString = myQueryString + "AND Sections.Section = @section ";
                }

                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(myQueryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@linkName", linkName);
                            command.Parameters.AddWithValue("@DataID", dataID);
                            if (myClass != null)
                            {
                                command.Parameters.AddWithValue("@myClass", myClass);
                            }
                            if (unit != null)
                            {
                                command.Parameters.AddWithValue("@unit", unit);
                            }
                            if (section != null)
                            {
                                command.Parameters.AddWithValue("@section", section);
                            }

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
            
            //Make query string here so we can have if statements that say if some values are null.
            string queryString = "Select Files.DataID from Files, Classes, Units, Sections where Files.Name = @fileName ";
            if(myClass != null)
            {
                queryString = queryString + "AND Classes.Class = @myClass ";
            }
            if(unit != null)
            {
                queryString = queryString + "AND Units.Unit = @unit ";
            }
            if (section != null)
            {
                queryString = queryString + "AND Sections.Section = @section ";
            }



            using (SqlConnection conn = new SqlConnection(FilerDB))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                    {
                        command.Parameters.AddWithValue("@fileName", fileName);
                        command.Parameters.AddWithValue("@DataID", dataID);
                        if (myClass != null)
                        {
                            command.Parameters.AddWithValue("@myClass", myClass);
                        }
                        if (unit != null)
                        {
                            command.Parameters.AddWithValue("@unit", unit);
                        }
                        if (section != null)
                        {
                            command.Parameters.AddWithValue("@section", section);
                        }

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
        /// <param name="data"></param>
        public void Delete(ResourceData data)
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

            if(isLink == null || (linkName == null && fileName == null))
            {
                SetStatus(HttpStatusCode.Forbidden);
                return;
            }

            //This part is if what we are deleting is a link.
            if (isLink.Equals("true"))
            {
                string queryString = "Select Links.DataID from Links, Classes, Units, Sections where Links.Name = @linkName ";
                if (myClass != null)
                {
                    queryString = queryString + "AND Classes.Class = @myClass ";
                }
                if (unit != null)
                {
                    queryString = queryString + "AND Units.Unit = @unit ";
                }
                if (section != null)
                {
                    queryString = queryString + "AND Sections.Section = @section ";
                }
                
                //First we need to find the DataID of the information that we want to remove.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@linkName", linkName);
                            command.Parameters.AddWithValue("@DataID", dataID);
                            if (myClass != null)
                            {
                                command.Parameters.AddWithValue("@myClass", myClass);
                            }
                            if (unit != null)
                            {
                                command.Parameters.AddWithValue("@unit", unit);
                            }
                            if (section != null)
                            {
                                command.Parameters.AddWithValue("@section", section);
                            }
                            if (type != null)
                            {
                                command.Parameters.AddWithValue("@type", type);
                            }

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    dataID = reader.GetInt32(0);
                                }
                                else
                                {
                                    SetStatus(HttpStatusCode.Conflict);
                                    return;
                                }
                            }
                            trans.Commit();
                        }
                    }
                }

                queryString = "";
                //If statements here
                if (myClass != null)
                {
                    queryString = queryString + "Delete from Classes where DataID = @DataID ";
                }
                if (unit != null)
                {
                    queryString = queryString + "Delete from Units where DataID = @DataID ";
                }
                if (section != null)
                {
                    queryString = queryString + "Delete from Sections where DataID = @DataID ";
                }
                if (type != null)
                {
                    queryString = queryString + "Delete from Types where DataID = @DataID ";
                }
                queryString += "Delete from Links where DataID = @DataID";
                //Now we want to use the DataID we found to actually delete the information.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@DataID", dataID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                int numDeleted = reader.RecordsAffected; //This is where we need to check if something was deleted or not. 1 = deleted. 0 = not deleted.
                                if (numDeleted == 1)
                                {
                                    SetStatus(HttpStatusCode.OK);
                                }
                                if (numDeleted == 0)
                                {
                                    SetStatus(HttpStatusCode.Conflict);
                                }
                            }
                            trans.Commit();
                        }
                    }
                }
                return;
            }
            else //If the piece of data is a file.
            {
                string queryString = "Select Files.DataID from Files, Classes, Units, Sections where Files.Name = @FileName ";
                if (myClass != null)
                {
                    queryString = queryString + "AND Classes.Class = @myClass ";
                }
                if (unit != null)
                {
                    queryString = queryString + "AND Units.Unit = @unit ";
                }
                if (section != null)
                {
                    queryString = queryString + "AND Sections.Section = @section ";
                }
                //First we need to find the DataID of the information that we want to remove.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@FileName", fileName);
                            command.Parameters.AddWithValue("@DataID", dataID);
                            if (myClass != null)
                            {
                                command.Parameters.AddWithValue("@myClass", myClass);
                            }
                            if (unit != null)
                            {
                                command.Parameters.AddWithValue("@unit", unit);
                            }
                            if (section != null)
                            {
                                command.Parameters.AddWithValue("@section", section);
                            }
                            if (type != null)
                            {
                                command.Parameters.AddWithValue("@type", type);
                            }

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if(reader.Read())
                                {
                                    dataID = reader.GetInt32(0);
                                }
                                else
                                {
                                    SetStatus(HttpStatusCode.Conflict);
                                    return;
                                }

                            }
                            trans.Commit();
                        }
                    }
                }

                queryString = "";
                //If statements here
                if (myClass != null)
                {
                    queryString = queryString + "Delete from Classes where DataID = @DataID ";
                }
                if (unit != null)
                {
                    queryString = queryString + "Delete from Units where DataID = @DataID ";
                }
                if (section != null)
                {
                    queryString = queryString + "Delete from Sections where DataID = @DataID ";
                }
                if (type != null)
                {
                    queryString = queryString + "Delete from Types where DataID = @DataID ";
                }
                queryString += "Delete from Files where DataID = @DataID";
                //Now we want to use the DataID we found to actually delete the information.
                using (SqlConnection conn = new SqlConnection(FilerDB))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        using (SqlCommand command = new SqlCommand(queryString, conn, trans))
                        {
                            command.Parameters.AddWithValue("@DataID", dataID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                int numDeleted = reader.RecordsAffected; //This is where we need to check if something was deleted or not. 1 = deleted. 0 = not deleted.
                                if (numDeleted == 1)
                                {
                                    SetStatus(HttpStatusCode.OK);
                                }
                                if (numDeleted == 0)
                                {
                                    SetStatus(HttpStatusCode.Conflict);
                                }
                            }
                            trans.Commit();
                        }
                    }
                }
                return;
            }

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

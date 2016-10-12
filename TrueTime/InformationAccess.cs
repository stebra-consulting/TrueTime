using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using TrueTime.Models;

namespace TrueTime
{
    /// <summary>
    /// Holds summarized information about a consultant's time spent on a requested time period.
    /// The time period parameters may vary, and are outside of this class.
    /// </summary>
    public class ConsultantTime
    {
        public string Name { get; set; }
        public double TimeSpent { get; set; }
    }

    /// <summary>
    /// Holds summarized information about time spent on a project.
    /// Like ConsultantTime, the time period parameters may vary, and are outside of this class.
    /// </summary>
    public class ProjectTime
    {
        public string Name { get; set; }
        public double TimeSpent { get; set; }
    }

    /// <summary>
    /// Class that acts as an abstraction layer between the storage and the application logic
    /// </summary>
     public class InformationAccess
    {
        CloudStorageAccount _storageAccount = null;
        const string _projectTableName = "Projects";
        const string _projectPartitionKey = "project";
        const string _userTableName = "Users";
        const string _userPartitionKey = "user";
        const string _userTimesTableName = "UserTimes";
        const string _userTimesPartitionKey = "usertime";
        const string _userProjectTableName = "UserProjects";
        const string _userProjectPartitionKey = "userproject";

        /// <summary>
        /// Validates the connection string information in web.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">Connection string for the storage service</param>
        /// <returns>CloudStorageAccount object</returns>
        CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
                return storageAccount;
            }
            catch (FormatException)
            {
                throw new Exception("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the web.config file.");
            }
            catch (ArgumentException)
            {
                throw new Exception("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the web.config file.");
            }
            catch(Exception)
            {
                throw; //we don't know what went wrong here.
            }            
        }

        public bool Initialize()
        {
            string azureKey = System.Web.Configuration.WebConfigurationManager.AppSettings["StorageConnectionString"];

            try
            {
                // Parse the connection string and return a reference to the storage account.
                _storageAccount = CloudStorageAccount.Parse(azureKey);
                return true;
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        ///////////////////////////////////////
        /// PROJECTS
        ///////////////////////////////////////

        List<AzureProject> GetAllProjects(bool includeHidden = false)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_projectTableName);
                var query = table.CreateQuery<AzureProject>();
                var res = table.ExecuteQuery(query).Where(r => (includeHidden == true) || (r.Hidden == false));
                return res.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Retrieves a project by its name and returns its entity or null upon error
        /// </summary>
        public AzureProject GetProject(string projectName)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_projectTableName);
                
                var query = from entity in table.CreateQuery<AzureProject>()
                            where entity.PartitionKey == _projectPartitionKey && entity.RowKey == projectName
                            select entity;
                return query.First();
                
                /*
                var query = table.CreateQuery<AzureProject>().Where(
                                p => p.PartitionKey == _projectPartitionKey && p.RowKey == projectName);
                var res = table.ExecuteQuery(query);
                return res.First();
                */
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts or updates a project into the table Projects
        /// </summary>
        /// <param name="systemUser">a pre-filled object of data</param>
        /// <returns>true if the operation was successful, else false</returns>
        public async Task<bool> InsertUpdateProject(AzureProject project)
        {
            try
            {
                project.PartitionKey = _projectPartitionKey;
                // Create the table handle
                CloudTable table = await CreateTableAsync(_projectTableName);
                // Create the InsertOrReplace TableOperation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(project);
                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

                return result.Result != null;
            }
            catch(Exception)
            {
                return false;
            }            
        }

        bool DeleteProject(string projectName)
        {
            return false;
            /*
            CloudTableClient table = _storageAccount.CreateCloudTableClient();
            Project deleteEntity = GetProject(projectName);
            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
            await table.   ExecuteAsync(deleteOperation);
            */
        }

        ///////////////////////////////////////
        /// USERS
        ///////////////////////////////////////

        public List<AzureUser> GetAllUsers(UserType typeOfUser, bool includeDeleted = false)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTableName);
                var query = table.CreateQuery<AzureUser>();
                var res = table.ExecuteQuery(query);
                return res.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public AzureUser GetUser(string name)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTableName);
                var query = table.CreateQuery<AzureUser>();
                var res = table.ExecuteQuery(query).Where(
                    u => u.PartitionKey == _userPartitionKey && u.RowKey == name);
                return res.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Inserts or updates a user into the table Users
        /// </summary>
        /// <param name="systemUser">a pre-filled object of data</param>
        /// <returns>true if the operation was successful, else false</returns>
        /// <remarks>This function should only be accessible to the administrators</remarks>
        public async Task<bool> InsertUpdateUser(AzureUser systemUser)
        {
            try
            {
                systemUser.PartitionKey = _userPartitionKey;
                // Create the table handle
                CloudTable table = await CreateTableAsync(_userTableName);
                // Create the InsertOrReplace TableOperation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(systemUser);
                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

                return result.Result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }
        bool DeleteUser(AzureUser user)
        {
            return false;
        }

        ///////////////////////////////////////
        /// USERTIMES
        ///////////////////////////////////////

        /// <summary>
        /// Inserts or updates a UserTime into the table UserTimes
        /// </summary>
        /// <param name="updatingUser">The user filling in the time report, usually the consultant himself,
        ///                            but may be the adminstrator too
        /// </param>
        /// <param name="userTime">a pre-filled object of data</param>
        /// <returns>true if the operation was successful, else false</returns>
        public async Task<bool> InsertUpdateUserTime(string updatingUser, AzureUserTime userTime)
        {
            try
            {
                userTime.PartitionKey = _userTimesPartitionKey;
                // Create the table handle
                CloudTable table = await CreateTableAsync(_userTimesTableName);
                // Create the InsertOrReplace TableOperation
                TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(userTime);
                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insertOrMergeOperation);

                return result.Result != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        ///////////////////////////////////////
        /// USERPROJECTS
        ///////////////////////////////////////


        /// <summary>
        ///  Returns a list of all projects for the given consultant. If no consultant is specified,
        ///  all projects for all consultants are returned.
        /// </summary>
        public List<AzureUserProject> GetAllProjects(string consultant = "")
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userProjectTableName);

                var query = table.CreateQuery<AzureUserProject>();
                var res = table.ExecuteQuery(query).Where(s => consultant == string.Empty || s.RowKey == consultant);
                return res.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///////////////////////////////////////
        /// REPORT FUNCTIONS
        ///////////////////////////////////////

        /// <summary>
        /// Given a DateTime, for which we only use the Year and Month component of, 
        /// it returns a list of each consultant and his/her summed reported time 
        /// </summary>
        public List<ConsultantTime> GetConsultantTimePerMonth(DateTime aMonth)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTimesTableName);
                var query = table.CreateQuery<AzureUserTime>();
                var q = table.ExecuteQuery(query).Where(
                    ut => ut.PartitionKey == _userTimesPartitionKey && 
                    ut.WorkDate.Year == aMonth.Year &&
                    ut.WorkDate.Month == aMonth.Month);

                List<AzureUserTime> lut = q.ToList();
                Dictionary<string, double> consultantTimeDict = new Dictionary<string, double>();
                //transfer list into a dictionary, keyed by the consultant's name, value is the summed hours
                foreach (AzureUserTime aut in lut)
                {
                    double time;

                    if (consultantTimeDict.ContainsKey(aut.UserRowKey))
                    {
                        time = aut.TimeSpent + consultantTimeDict[aut.UserRowKey];
                        consultantTimeDict[aut.UserRowKey] = time;
                    }
                    else
                    {
                        time = aut.TimeSpent;
                        consultantTimeDict.Add(aut.UserRowKey, time);
                    }
                }

                List<ConsultantTime> res = new List<ConsultantTime>();

                foreach (KeyValuePair<string, double> kvp in consultantTimeDict)
                {
                    ConsultantTime ct = new ConsultantTime();

                    ct.Name = kvp.Key;
                    ct.TimeSpent = kvp.Value;
                    res.Add(ct);
                }

                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Given a DateTime, for which we only use the week which the date belongs to, 
        /// it returns a list of each consultant and his/her summed reported time 
        /// </summary>
        public List<ConsultantTime> GetConsultantTimePerWeek(DateTime aWeek)
        {
            try
            {
                List<DateTime> aWeekRange = new List<DateTime>();
                DateCalculator dc = new DateCalculator(aWeek.Year);

                aWeekRange = dc.GetWeekRange(aWeek);

                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTimesTableName);
                var query = table.CreateQuery<AzureUserTime>();
                var q = table.ExecuteQuery(query).Where(
                    ut => ut.PartitionKey == _userTimesPartitionKey &&
                    ut.WorkDate.Date >= aWeekRange[0].Date &&
                    ut.WorkDate.Date <= aWeekRange[1].Date);

                List<AzureUserTime> lut = q.ToList();
                Dictionary<string, double> consultantTimeDict = new Dictionary<string, double>();
                //transfer list into a dictionary, keyed by the consultant's name, value is the summed hours
                foreach (AzureUserTime aut in lut)
                {
                    double time;

                    if (consultantTimeDict.ContainsKey(aut.UserRowKey))
                    {
                        time = aut.TimeSpent + consultantTimeDict[aut.UserRowKey];
                        consultantTimeDict[aut.UserRowKey] = time;
                    }
                    else
                    {
                        time = aut.TimeSpent;
                        consultantTimeDict.Add(aut.UserRowKey, time);
                    }
                }

                List<ConsultantTime> res = new List<ConsultantTime>();

                foreach (KeyValuePair<string, double> kvp in consultantTimeDict)
                {
                    ConsultantTime ct = new ConsultantTime();

                    ct.Name = kvp.Key;
                    ct.TimeSpent = kvp.Value;
                    res.Add(ct);
                }

                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ProjectTime> GetProjectTimePerYear(DateTime aYear)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTimesTableName);
                var query = table.CreateQuery<AzureUserTime>();
                var q = table.ExecuteQuery(query).Where(
                    ut => ut.PartitionKey == _userTimesPartitionKey &&
                    ut.WorkDate.Year == aYear.Year);
                List<AzureUserTime> lut = q.ToList();
                Dictionary<string, double> projectTimeDict = new Dictionary<string, double>();
                //transfer list into a dictionary, keyed by the project name, value is the summed hours
                foreach (AzureUserTime aut in lut)
                {
                    double time;

                    if (projectTimeDict.ContainsKey(aut.ProjectRowKey))
                    {
                        time = aut.TimeSpent + projectTimeDict[aut.ProjectRowKey];
                        projectTimeDict[aut.ProjectRowKey] = time;
                    }
                    else
                    {
                        time = aut.TimeSpent;
                        projectTimeDict.Add(aut.ProjectRowKey, time);
                    }
                }

                List<ProjectTime> res = new List<ProjectTime>();

                foreach (KeyValuePair<string, double> kvp in projectTimeDict)
                {
                    ProjectTime ct = new ProjectTime();

                    ct.Name = kvp.Key;
                    ct.TimeSpent = kvp.Value;
                    res.Add(ct);
                }

                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Given a DateTime, for which we only use the Year and Month component of, 
        /// it returns a list of each project and the summed reported time for the month
        /// </summary>
        public List<ProjectTime> GetProjectTimePerMonth(DateTime aMonth)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_userTimesTableName);
                var query = table.CreateQuery<AzureUserTime>();
                var q = table.ExecuteQuery(query).Where(
                    ut => ut.PartitionKey == _userTimesPartitionKey &&
                    ut.WorkDate.Year == aMonth.Year &&
                    ut.WorkDate.Month == aMonth.Month);

                List<AzureUserTime> lut = q.ToList();
                Dictionary<string, double> projectTimeDict = new Dictionary<string, double>();
                //transfer list into a dictionary, keyed by the project name, value is the summed hours
                foreach (AzureUserTime aut in lut)
                {
                    double time;

                    if (projectTimeDict.ContainsKey(aut.ProjectRowKey))
                    {
                        time = aut.TimeSpent + projectTimeDict[aut.ProjectRowKey];
                        projectTimeDict[aut.ProjectRowKey] = time;
                    }
                    else
                    {
                        time = aut.TimeSpent;
                        projectTimeDict.Add(aut.ProjectRowKey, time);
                    }
                }

                List<ProjectTime> res = new List<ProjectTime>();

                foreach (KeyValuePair<string, double> kvp in projectTimeDict)
                {
                    ProjectTime ct = new ProjectTime();

                    ct.Name = kvp.Key;
                    ct.TimeSpent = kvp.Value;
                    res.Add(ct);
                }

                return res;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region TABLE CREATIONS

        /// <summary>
        /// Creates all tables needed by the application
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateTables()
        {
            CloudTable t1, t2, t3, t4, t5;

            try
            {
                t1 = await CreateTableAsync("AuditTrail");
                t2 = await CreateTableAsync("Users");
                t3 = await CreateTableAsync("Projects");
                t4 = await CreateTableAsync("UserTimes");
                t5 = await CreateTableAsync("UserProjects");
            }
            catch (Exception)
            {
                return false;
            }
            return t1.Exists() && t2.Exists() && t3.Exists() && t4.Exists() && t5.Exists();
        }

#if false
        /// <summary>
        /// Creates an empty table with only the standard columns 
        /// </summary>
        /// <param name="tableName">the name of the table</param>
        /// <returns>true if the table could be created else false</returns>
        public async System.Threading.Tasks.Task<bool> CreateTable(string tableName)
        {
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            try
            {
                return await table.CreateIfNotExistsAsync();
            }
            catch (StorageException)
            {
                throw;
            }
        }
#endif
        async Task<CloudTable> CreateTableAsync(string tableName)
        {
            // Create a table client for interacting with the table service
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create a table client for interacting with the table service 
            CloudTable table = tableClient.GetTableReference(tableName);
            try
            {
                if (await table.CreateIfNotExistsAsync())
                {
                    //table didn't exist previously
                    return table;
                }
                else
                {
                    //table existed previously
                    return table;
                }
            }
            catch (StorageException)
            {
                throw;
            }
        }
#endregion

        //TODO: this function should not be here
        /// <summary>
        /// This submits a weekly timesheet to the system. 
        /// </summary>
        /// <returns></returns>
        bool SubmitWeeklyTimeSheet(AzureUser aConsultant, DateTime weekForSubmission)
        {
            return false;
        }
    }
}
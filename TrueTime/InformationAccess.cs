using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace TrueTime
{
    /// <summary>
    /// Class that acts as an abstraction layer between the storage and the application logic
    /// </summary>
     public class InformationAccess
    {
        CloudStorageAccount _storageAccount = null;
        const string _projectTableName = "Project";
        const string _projectPartitionKey = "project";
        const string _userTableName = "Users";
        const string _userPartitionKey = "user";

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

        List<Project> GetAllProjects(bool includeHidden = false)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_projectTableName);
                var query = table.CreateQuery<Project>();
                var res = table.ExecuteQuery(query);
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
        public Project GetProject(string projectName)
        {
            try
            {
                CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(_projectTableName);
                var query = table.CreateQuery<Project>()/*.Where(
                                p => p.PartitionKey == _projectPartitionKey && p.Name == projectName)*/;
                var res = table.ExecuteQuery(query).Where(
                                p => p.PartitionKey == _projectPartitionKey && p.RowKey == projectName);
                return res.First();
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
        public async Task<bool> InsertUpdateProject(Project project)
        {
            try
            {
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
        AzureUser GetUser(string name)
        {
            return null;
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
                throw;
            }
        }
        bool DeleteUser(AzureUser user)
        {
            return false;
        }
        bool CreateUser(AzureUser user)
        {
            return false;
        }

        #region TABLE CREATIONS

        /// <summary>
        /// Creates all tables needed by the application
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CreateTables()
        {
            CloudTable t1, t2, t3, t4;

            try
            {
                t1 = await CreateTableAsync("AuditTrail");
                t2 = await CreateTableAsync("Users");
                t3 = await CreateTableAsync("Projects");
                t4 = await CreateTableAsync("UserTimes");
            }
            catch (Exception)
            {
                return false;
            }
            return t1.Exists() && t2.Exists() && t3.Exists() && t4.Exists();
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
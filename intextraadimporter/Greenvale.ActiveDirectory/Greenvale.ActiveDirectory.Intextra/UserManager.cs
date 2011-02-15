using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace Greenvale.ActiveDirectory.Intextra
{
    public class UserManager
    {
        private string connectionString;
        private log4net.ILog log;

        public UserManager(string connectionString)
        {
            this.connectionString = connectionString;
            log = log4net.LogManager.GetLogger("Default");
        }

        public User AddUser(string username, string password, string firstName, string surname, int createdBy,
            string nTDomainName, string nTUserName, string primaryEmailAddress, string defaultUrl, string theme,
            string activeDirectoryGuid)
        {
            log.Info("Adding user:");
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@UserName", username));
            sqlParameters.Add(new SqlParameter("@FirstName", firstName));
            sqlParameters.Add(new SqlParameter("@Surname", surname));
            sqlParameters.Add(new SqlParameter("@Password", password));
            sqlParameters.Add(new SqlParameter("@NTDomainName", nTDomainName));
            sqlParameters.Add(new SqlParameter("@NTUserName", nTUserName));
            sqlParameters.Add(new SqlParameter("@EmailAddress", primaryEmailAddress));
            sqlParameters.Add(new SqlParameter("@CreatedBy", createdBy));
            sqlParameters.Add(new SqlParameter("@Theme", theme));
            sqlParameters.Add(new SqlParameter("@Url", defaultUrl));
            sqlParameters.Add(new SqlParameter("@ActiveDirectoryGuid", activeDirectoryGuid));

            foreach (SqlParameter parameter in sqlParameters)
            {
                log.InfoFormat("Paramter: {0}, Value: {1}", parameter.ParameterName, parameter.Value);
            }

            SqlHelper.ExecuteScalar(this.connectionString,
                CommandType.Text,
                @"  INSERT INTO awsUsers (UserName, FirstName, Surname, Password, NTDomainName, NTUserName, 
	                    PrimaryEmailAddress, CreatedBy, Theme, DefaultURL, ActiveDirectoryGUID) 
                    VALUES (@UserName, @FirstName, @Surname, @Password, @NTDomainName, @NTUserName, 
	                    @EmailAddress, @CreatedBy, @Theme, @Url, @ActiveDirectoryGuid)",
               sqlParameters.ToArray());

            // Now return the user;
            return GetUser(username);
        }


        public User GetUser(string username)
        {
            DataSet results = SqlHelper.ExecuteDataset(this.connectionString,
                CommandType.Text,
                @"  SELECT *
                    FROM awsUsers
                    WHERE Username = @Username;",
                new SqlParameter("@Username", username));
            if (results.Tables[0].Rows.Count < 1)
                return null;
            else
                return UserFromDataRow(results.Tables[0].Rows[0]);
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            DataSet results = SqlHelper.ExecuteDataset(this.connectionString,
                CommandType.Text,
                @"  SELECT *
                    FROM awsUsers
                    WHERE ActiveDirectoryGuid IS NOT NULL;");
            foreach (DataRow dataRow in results.Tables[0].Rows)
            {
                users.Add(UserFromDataRow(dataRow));
            }
            return users;
        }

        public void DisableUser(User user)
        {
            SqlHelper.ExecuteNonQuery(this.connectionString,
                CommandType.Text,
                @"  UPDATE awsUsers
                    SET Disabled = 1
                    WHERE UserID = @UserID;",
                new SqlParameter("@UserID", user.UserID));
        }

        public User UserFromDataRow(DataRow dataRow)
        {
            int userId = (int)dataRow["UserID"];
            string userName = dataRow["UserName"].ToString();
            string password = dataRow["Password"].ToString();
            string firstName = dataRow["FirstName"].ToString();
            string surname = dataRow["Surname"].ToString();
            bool disabled = (bool)dataRow["Disabled"];
            DateTime creationDate = (DateTime)dataRow["CreationDate"];
            int createdBy = (int)dataRow["CreatedBy"];
            DateTime? lastEditDate = (dataRow["LastEditDate"] != DBNull.Value) ? (DateTime)dataRow["LastEditDate"] : new DateTime?();
            int? lastEditedBy = (dataRow["LastEditedBy"] != DBNull.Value) ? (int)dataRow["LastEditedBy"] : new int?();
            DateTime? expiryDate = (dataRow["ExpiryDate"] != DBNull.Value) ? (DateTime)dataRow["ExpiryDate"] : new DateTime?();
            string nTDomainName = dataRow["NTDomainName"].ToString();
            string nTUserName = dataRow["NTUserName"].ToString();
            string primaryEmailAddress = dataRow["PrimaryEmailAddress"].ToString();
            string defaultURL = dataRow["DefaultURL"].ToString();
            string theme = dataRow["Theme"].ToString();
            string activeDirectoryGUID = dataRow["ActiveDirectoryGUID"].ToString();

            return new User(userId, userName, password, firstName, surname, disabled, creationDate, 
                createdBy, lastEditDate, lastEditedBy, expiryDate, nTDomainName, nTUserName, 
                primaryEmailAddress, defaultURL, theme, activeDirectoryGUID);
        }

        public User EnsureUser(System.DirectoryServices.DirectoryEntry searchResult, string aDUsername, string aDDomain)
        {
            // We need to check if the user exists, if not add it - no need to return the object as we can log in here.
            log.InfoFormat("Checking for user: {0}", aDUsername);
            User user = GetUser(aDUsername);

            if (user == null)
            {
                // Add the user
                log.InfoFormat("User Not Found (adding): {0}", aDUsername);
                string firstName = null;
                string surname = null;
                int createdBy = 1;
                string primaryEmailAddress = "";
                string defaultUrl = "Home";
                string theme = "default";
                string activeDirectoryGuid = searchResult.Guid.ToString();

                if (searchResult.Properties["givenName"].Count > 0)
                {
                    firstName = searchResult.Properties["givenName"][0].ToString();
                }
                else
                {
                    log.ErrorFormat("givenName (Forename) cannot be blank in AD for user {0}", aDUsername);
                    return null;
                }

                if (searchResult.Properties["sn"].Count > 0)
                {
                    surname = searchResult.Properties["sn"][0].ToString();
                }
                else
                {
                    log.ErrorFormat("sn (Surname) cannot be blank in AD for user {0}", aDUsername);
                    return null;
                }

                return AddUser(aDUsername, aDUsername, firstName, surname, createdBy, aDDomain, aDUsername, primaryEmailAddress, defaultUrl, theme, activeDirectoryGuid);
            }
            else
            {
                log.InfoFormat("User Found: {0}", aDUsername);
                // Update user details
                return user;
            }
        }

        public void EnableUser(User user)
        {
            SqlHelper.ExecuteNonQuery(this.connectionString,
                CommandType.Text,
                @"  UPDATE awsUsers
                    SET Disabled = 0
                    WHERE UserID = @UserID;",
                new SqlParameter("@UserID", user.UserID));
        }

        public void EnsurePermissions(User user)
        {
            int groupId = Convert.ToInt32(SqlHelper.ExecuteScalar(this.connectionString,
                CommandType.Text,
                @"  SELECT GroupID
                    FROM awsGroups
                    WHERE GroupName = 'intextra.users';"));

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            sqlParameters.Add(new SqlParameter("@UserID", user.UserID));
            sqlParameters.Add(new SqlParameter("@GroupID", groupId));

            SqlHelper.ExecuteNonQuery(this.connectionString,
                CommandType.Text,
                @"IF NOT EXISTS (
	                SELECT *
	                FROM awsGroupMembership
	                WHERE UserID = @UserID
		                AND GroupID = @GroupID 
	                )
	                INSERT INTO awsGroupMembership (UserID, GroupID) 
	                VALUES (@UserID, @GroupID)",
                sqlParameters.ToArray());
        }
    }
}

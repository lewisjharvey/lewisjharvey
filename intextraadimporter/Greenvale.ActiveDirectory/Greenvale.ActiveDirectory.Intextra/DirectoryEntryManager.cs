using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using log4net;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;

namespace Greenvale.ActiveDirectory.Intextra
{
    public class DirectoryEntryManager
    {
        private string databaseConnectionString;
        private int directoryId;
        private readonly ILog log = log4net.LogManager.GetLogger("Default");

        // Attribute Name Consts
        private const string FORENAME = "Forename";
        private const string SURNAME = "Surname";
        private const string SITE = "Site";
        private const string WORK_TELEPHONE = "Work Telephone";
        private const string BUSINESS_EMAIL = "Business Email";
        private const string MOBILE_PHONE = "Mobile Phone";
        private const string JOB_TITLE = "Job Title";
        private const string DEPARTMENT = "Department";
        private const string COMPANY = "Company";
        private const string MANAGER = "Manager";

        public DirectoryEntryManager(int directoryId, string databaseConnectionString)
        {
            this.databaseConnectionString = databaseConnectionString;
            this.directoryId = directoryId;
        }

        private List<DirectoryEntryAttribute> SetPropertiesFromEntry(System.DirectoryServices.DirectoryEntry searchResult)
        {
            List<DirectoryEntryAttribute> directoryEntryAttributes = new List<DirectoryEntryAttribute>();

            foreach (string attribute in searchResult.Properties.PropertyNames)
            {
                if (searchResult.Properties[attribute].Count > 0)
                {
                    // Find the matching DirectoryEntryAttribute type
                    switch (attribute.ToLower())
                    {
                        case "givenname":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(FORENAME, searchResult.Properties[attribute][0].ToString(), AttributeType.Forename));
                            break;
                        case "sn":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(SURNAME, searchResult.Properties[attribute][0].ToString(), AttributeType.Surname));
                            break;
                        case "physicaldeliveryofficename":
                            // Do a lookup to get the directory entry id for the location based on the name
                            DirectoryEntryAttribute dea = GetSiteDirectoryEntryAttribute(searchResult.Properties[attribute][0].ToString(), searchResult.Properties["sAMAccountName"][0].ToString());
                            if (dea != null)
                            {
                                directoryEntryAttributes.Add(dea);
                            }
                            break;
                        case "telephonenumber":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(WORK_TELEPHONE, searchResult.Properties[attribute][0].ToString(), AttributeType.WorkTelephone));
                            break;
                        case "mail":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(BUSINESS_EMAIL, searchResult.Properties[attribute][0].ToString(), AttributeType.BusinessEmail));
                            break;
                        case "mobile":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(MOBILE_PHONE, searchResult.Properties[attribute][0].ToString(), AttributeType.MobilePhone));
                            break;
                        case "title":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(JOB_TITLE, searchResult.Properties[attribute][0].ToString(), AttributeType.JobTitle));
                            break;
                        case "department":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(DEPARTMENT, searchResult.Properties[attribute][0].ToString(), AttributeType.Department));
                            break;
                        case "company":
                            directoryEntryAttributes.Add(new DirectoryEntryAttribute(COMPANY, searchResult.Properties[attribute][0].ToString(), AttributeType.Company));
                            break;
                    }
                }
            }

            return directoryEntryAttributes;
        }

        private DirectoryEntryAttribute GetSiteDirectoryEntryAttribute(string siteName, string username)
        {
            // Look up the database based on the name passed.
            DirectoryEntryAttribute directoryEntryAttribute = new DirectoryEntryAttribute(SITE, null, AttributeType.Site);
            switch (siteName.ToLower())
            {
                case "march":
                    directoryEntryAttribute.Value = "1864";
                    break;
                case "burrelton":
                    directoryEntryAttribute.Value = "1865";
                    break;
                case "duns":
                    directoryEntryAttribute.Value = "1868";
                    break;
                case "floods ferry":
                    directoryEntryAttribute.Value = "1925";
                    break;
                case "colton":
                    directoryEntryAttribute.Value = "3805";
                    break;
                case "swancote":
                    directoryEntryAttribute.Value = "2487";
                    break;
                case "tern hill":
                    directoryEntryAttribute.Value = "1869";
                    break;
                case "wisbech":
                    directoryEntryAttribute.Value = "2052";
                    break;
                case "nacton":
                    directoryEntryAttribute.Value = "4634";
                    break;
                case "malton":
                    directoryEntryAttribute.Value = "4635";
                    break;
                case "remote":
                    // Do nothing
                    return null;
                default:
                    log.ErrorFormat("User: {0} - invalid site '{1}'", username, siteName);
                    return null;
            }
            return directoryEntryAttribute;
        }

        public DirectoryEntry GetDirectoryEntry(int userId)
        {
            DataSet results = SqlHelper.ExecuteDataset(this.databaseConnectionString,
                CommandType.Text,
                @"  SELECT *
                    FROM awsdDirectoryEntry
                    WHERE UserID = @UserId;",
                new SqlParameter("@UserId", userId));
            if (results.Tables[0].Rows.Count < 1)
                return null;
            else
                return DirectoryEntryFromDataRow(results.Tables[0].Rows[0]);
        }

        private DirectoryEntry DirectoryEntryFromDataRow(DataRow dataRow)
        {
            // Get all the directory entry attributes and populate them
            int directoryEntryID = (int)dataRow["DirectoryEntryID"];
            int userID = (int)dataRow["userID"];
            int directoryID = (int)dataRow["DirectoryID"];
            DateTime createdOn = (DateTime)dataRow["CreatedOn"];
            DateTime? lastModifiedOn = (dataRow["LastModifiedOn"] != DBNull.Value) ? (DateTime)dataRow["LastModifiedOn"] : new DateTime?();
            int? lastModifiedByUserID = (dataRow["LastModifiedByUserID"] != DBNull.Value) ? (int)dataRow["LastModifiedByUserID"] : new int?();
            int createdByUserID = (int)dataRow["CreatedByUserID"];

            DirectoryEntry directoryEntry = new DirectoryEntry(directoryEntryID, userID, directoryID, createdOn, lastModifiedOn, createdByUserID, lastModifiedByUserID);
            return directoryEntry;
        }

        public void EnsureDirectoryEntry(System.DirectoryServices.DirectoryEntry searchResult, string aDUsername, int userId)
        {
            // We need to check if the entry exists
            // If not add it, if it does update it
            int directoryEntryId = -1;
            DirectoryEntry directoryEntry = GetDirectoryEntry(userId);
            if (directoryEntry == null)
            {
                log.Info("Adding Directory Entry:");
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                sqlParameters.Add(new SqlParameter("@UserID", userId));
                sqlParameters.Add(new SqlParameter("@DirectoryID", directoryId));
                sqlParameters.Add(new SqlParameter("@CreatedOn", DateTime.Now));
                sqlParameters.Add(new SqlParameter("@CreatedByUserID", 1));

                foreach (SqlParameter parameter in sqlParameters)
                {
                    log.InfoFormat("Parameter: {0}, Value: {1}", parameter.ParameterName, parameter.Value);
                }

                directoryEntryId = Convert.ToInt32(SqlHelper.ExecuteScalar(this.databaseConnectionString,
                    CommandType.Text,
                    @"  INSERT INTO awsdDirectoryEntry (UserID, DirectoryID, CreatedOn, CreatedByUserID)
                    VALUES (@UserID, @DirectoryID, @CreatedOn, @CreatedByUserID)
                    SELECT SCOPE_IDENTITY();",
                   sqlParameters.ToArray()));
            }
            else
            {
                directoryEntryId = directoryEntry.DirectoryEntryID;
            }

            // Now add all the directory entry attributes
            List<DirectoryEntryAttribute> directoryEntryAttributes = this.SetPropertiesFromEntry(searchResult);
            log.InfoFormat("Adding the attributes ({0})", directoryEntryAttributes.Count);
            foreach (DirectoryEntryAttribute directoryEntryAttribute in directoryEntryAttributes)
            {
                EnsureDirectoryEntryAttribute(directoryEntryAttribute.AttributeType, directoryEntryAttribute.Value, directoryEntryId);
            }
        }

        public void EnsureDirectoryEntryAttribute(AttributeType attributeType, string value, int directoryEntryId)
        {
            List<SqlParameter> attributeSqlParameters = new List<SqlParameter>();
            attributeSqlParameters.Add(new SqlParameter("@AttributeTypeID", (int)attributeType));
            attributeSqlParameters.Add(new SqlParameter("@Value", value));
            attributeSqlParameters.Add(new SqlParameter("@DirectoryEntryID", directoryEntryId));
            attributeSqlParameters.Add(new SqlParameter("@UserId", 1));

            foreach (SqlParameter parameter in attributeSqlParameters)
            {
                log.InfoFormat("Attribute Parameter: {0}, Value: {1}", parameter.ParameterName, parameter.Value);
            }

            SqlHelper.ExecuteNonQuery(this.databaseConnectionString,
                    CommandType.StoredProcedure,
                    "EnsureDirectoryEntryAttribute",
                    attributeSqlParameters.ToArray());
        }

        public void DeleteDirectoryEntryByUserId(int userId)
        {
            SqlHelper.ExecuteNonQuery(this.databaseConnectionString,
                CommandType.Text,
                @"  DELETE
                    FROM awsAttribute
                    WHERE AttributeID IN (
	                    SELECT AttributeID
	                    FROM awsdDirectoryEntryAttribute
	                    WHERE DirectoryEntryID IN (
			                    SELECT DirectoryEntryID
			                    FROM awsdDirectoryEntry
			                    WHERE UserID = @UserID
		                    )
	                    )

                    -- Now delete all the directory entry attributes
                    DELETE
                    FROM awsdDirectoryEntryAttribute
                    WHERE DirectoryEntryID IN (
		                    SELECT DirectoryEntryID
		                    FROM awsdDirectoryEntry
		                    WHERE UserID = @UserID
	                    )
	
                    -- Now delete all the directory entry attributes
                    DELETE
                    FROM awsdDirectoryEntry
                    WHERE UserID = @UserID",
                new SqlParameter("@UserID", userId));
        }
    }
}

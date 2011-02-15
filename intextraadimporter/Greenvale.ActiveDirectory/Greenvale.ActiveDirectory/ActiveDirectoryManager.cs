using System.DirectoryServices;
using System;

namespace Greenvale.ActiveDirectory
{
    public class ActiveDirectoryManager
    {
        private DirectoryEntry connectionDirectoryEntry;
        public DirectoryEntry ConnectionDirectoryEntry
        {
            get
            {
                if (this.connectionDirectoryEntry != null)
                {
                    return this.connectionDirectoryEntry;
                }
                else
                {
                    throw new ApplicationException("The connectionDirectoryEntry has not been set up.");
                }
            }
            set
            {
                this.connectionDirectoryEntry = value;
            }
        }

        public ActiveDirectoryManager(string connectionString, string username, string password)
        {
            this.ConnectionDirectoryEntry = this.GetDirectoryEntryConnection(connectionString, username, password);
        }

        public DirectoryEntry GetDirectoryEntryConnection(string connectionString, string username, string password)
        {
            DirectoryEntry directoryEntry = new DirectoryEntry();
            directoryEntry.Path = connectionString;
            directoryEntry.Username = username;
            directoryEntry.Password = password;
            return directoryEntry;
        }

        public SearchResultCollection GetLDAPDirectoryEntries(string ldapFilter)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher(this.ConnectionDirectoryEntry);
            directorySearcher.PageSize = 500;
            directorySearcher.Filter = ldapFilter;
            directorySearcher.SearchScope = SearchScope.Subtree;
            directorySearcher.PropertiesToLoad.Add("givenName");
            directorySearcher.PropertiesToLoad.Add("sn");
            directorySearcher.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            directorySearcher.PropertiesToLoad.Add("telephoneNumber");
            directorySearcher.PropertiesToLoad.Add("mail");
            directorySearcher.PropertiesToLoad.Add("mobile");
            directorySearcher.PropertiesToLoad.Add("title");
            directorySearcher.PropertiesToLoad.Add("department");
            directorySearcher.PropertiesToLoad.Add("company");
            directorySearcher.PropertiesToLoad.Add("manager");
            directorySearcher.PropertiesToLoad.Add("userPrincipalName");
            directorySearcher.PropertiesToLoad.Add("userAccountControl");

            SearchResultCollection results = directorySearcher.FindAll();
            return results;
        }

        public DirectoryUser CreateUserFromEntry(System.DirectoryServices.DirectoryEntry searchResult)
        {
            DirectoryUser directoryUser = new DirectoryUser();

            foreach (string attribute in searchResult.Properties.PropertyNames)
            {
                if (searchResult.Properties[attribute].Count > 0)
                {
                    // Find the matching DirectoryEntryAttribute type
                    string value = searchResult.Properties[attribute][0].ToString();
                    switch (attribute.ToLower())
                    {
                        case "givenname":
                            directoryUser.Forename = value;
                            break;
                        case "sn":
                            directoryUser.Surname = value;
                            break;
                        case "physicaldeliveryofficename":
                            directoryUser.Site = value;
                            break;
                        case "telephonenumber":
                            directoryUser.WorkTelephone = value;
                            break;
                        case "mail":
                            directoryUser.EmailAddress = value;
                            break;
                        case "mobile":
                            directoryUser.MobileNo = value;
                            break;
                        case "title":
                            directoryUser.JobTitle = value;
                            break;
                        case "department":
                            directoryUser.Department = value;
                            break;
                        case "company":
                            directoryUser.Company = value;
                            break;
                    }
                }
            }

            return directoryUser;
        }

        public bool UserExistsInAD(string userPrincipalName)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher(this.ConnectionDirectoryEntry);
            directorySearcher.PageSize = 500;
            directorySearcher.Filter = string.Format("(&(objectClass=user) (userPrincipalName={0}))", userPrincipalName);
            //TODO
            directorySearcher.PropertiesToLoad.Add("userAccountControl");
            //directorySearcher.PropertiesToLoad.Add("msDS-UserAccountDisabled");
            SearchResult result = directorySearcher.FindOne();
            bool exists = false;
            if (result != null)
            {
                //TODO
                int userAccountControl = Convert.ToInt32(result.Properties["userAccountControl"][0]);
                //int userAccountControl = Convert.ToInt32(result.Properties["msDS-UserAccountDisabled"][0]);
                //TODO
                bool disabled = ((userAccountControl & 2) > 0);
                //bool disabled = (userAccountControl == 1) ? true : false;
                if(!disabled)
                    exists = true;
            }
            return exists;
        }
    }
}

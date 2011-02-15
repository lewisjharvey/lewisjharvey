using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

using Greenvale.ActiveDirectory.Intextra;
using log4net;
using log4net.Config;

namespace Greenvale.ActiveDirectory.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ILog log = log4net.LogManager.GetLogger("Default");
            XmlConfigurator.Configure();

            try
            {

                log.Info("===============================");
                log.Info(" Import process started");
                log.Info(DateTime.Now.ToString());
                log.Info("===============================");


                // Set up the configuration items
                string connectionString = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.LDAPConnectionString;
                string username = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.LDAPUsername;
                string password = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.LDAPPassword;
                string ldapFilter = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.LDAPFilter;
                string regExFilter = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.RegExFilter;
                int directoryId = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.DirectoryId;
                string databaseConnectionString = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.DatabaseConnectionString;
                string aDDomain = Greenvale.ActiveDirectory.Console.Properties.Settings.Default.Domain;

                log.InfoFormat("ConnectionString: {0}", connectionString);
                log.InfoFormat("Username: {0}", username);
                log.InfoFormat("Password: {0}", password);
                log.InfoFormat("LDAPFilter: {0}", ldapFilter);
                log.InfoFormat("RegExFilter: {0}", regExFilter);
                log.InfoFormat("DirectoryId: {0}", directoryId);

                // Build a manager for return Intextra modelled objects.
                DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(directoryId, databaseConnectionString);
                ActiveDirectoryManager activeDirectoryManager = new ActiveDirectoryManager(connectionString, username, password);
                UserManager userManager = new UserManager(databaseConnectionString);

                SearchResultCollection collection = activeDirectoryManager.GetLDAPDirectoryEntries(ldapFilter);

                List<string> usernames = new List<string>();
                for (int q = 0; q < collection.Count; q++)
                {
                    System.DirectoryServices.DirectoryEntry myEntry = collection[q].GetDirectoryEntry();
                    string theUsername = myEntry.Properties["sAMAccountName"][0].ToString().ToLower();
                    int userAccountControl = Convert.ToInt32(myEntry.Properties["userAccountControl"][0]);
                    bool disabled = ((userAccountControl & 2) > 0);
                    if (!disabled)
                        usernames.Add(theUsername);
                }

                // Firstly we must disable all IX users not in AD with a Guid
                // and then remove their directory entry
                log.Info("Starting clear out of existing users:");
                List<User> users = userManager.GetAllUsers();
                foreach (User user in users)
                {
                    log.InfoFormat("Checking user: {0}", user.UserName);

                    if (!usernames.Contains(user.UserName.ToLower()))
                    {
                        userManager.DisableUser(user);
                        directoryEntryManager.DeleteDirectoryEntryByUserId(user.UserID);
                        log.InfoFormat("User {0} disabled", user.UserName);
                    }
                    else
                    {
                        userManager.EnableUser(user);

                        // Check they are in the users group for permissioning
                        userManager.EnsurePermissions(user);

                        log.InfoFormat("User {0} enabled", user.UserName);
                    }
                    log.InfoFormat("Finished Checking user: {0}", user.UserName);
                }

                // Now add/update all new/existing
                
                // Output the total count
                log.InfoFormat("AD Users Found: {0}", collection.Count);
                for (int i = 0; i < collection.Count; i++)
                {
                    log.Info("----------------------");
                    log.Info("- PROCESSING AD USER -");
                    log.Info("----------------------");

                    System.DirectoryServices.DirectoryEntry directoryEntry = collection[i].GetDirectoryEntry();

                    // Filter based on the username
                    string aDUsername = directoryEntry.Properties["sAMAccountName"][0].ToString();
                    if (!string.IsNullOrEmpty(regExFilter.ToString()))
                    {
                        if (new Regex(regExFilter).Match(aDUsername).Success)
                        {
                            log.InfoFormat("User Filtered: {0}", aDUsername);
                            log.Info("----------------------");
                            log.Info("- ENDING AD USER     -");
                            log.Info("----------------------");
                            log.Info("");
                            continue;
                        }
                    }

                    int userAccountControl = Convert.ToInt32(directoryEntry.Properties["userAccountControl"][0]);
                    bool disabled = ((userAccountControl & 2) > 0);
                    if (disabled)
                        continue;

                    // We've passed all the filters so add/update the details.
                    User user = userManager.EnsureUser(directoryEntry, aDUsername, aDDomain);
                    userManager.EnsurePermissions(user);
                    if (user != null)
                    {
                        directoryEntryManager.EnsureDirectoryEntry(directoryEntry, aDUsername, user.UserID);
                    }

                    log.Info("----------------------");
                    log.Info("- ENDING AD USER     -");
                    log.Info("----------------------");
                    log.Info("");
                }

                // Now we must go through and set up all the managers. This must be done now to ensure the
                // entry lookup works.
                for (int j = 0; j < collection.Count; j++)
                {
                    log.Info("---------------------------");
                    log.Info("- PROCESSING MANAGER USER -");
                    log.Info("---------------------------");
                    
                    // Get the account
                    System.DirectoryServices.DirectoryEntry directoryEntry = collection[j].GetDirectoryEntry();
                    log.InfoFormat("Checking User For Manager: {0}", directoryEntry.Properties["sAMAccountName"][0].ToString());

                    int userAccountControl = Convert.ToInt32(directoryEntry.Properties["userAccountControl"][0]);
                    bool disabled = ((userAccountControl & 2) > 0);
                    if (disabled)
                        continue;

                    if (directoryEntry.Properties["manager"].Count > 0)
                    {
                        // Filter based on the username
                        string aDUsername = directoryEntry.Properties["sAMAccountName"][0].ToString();
                        if (!string.IsNullOrEmpty(regExFilter.ToString()))
                        {
                            if (new Regex(regExFilter).Match(aDUsername).Success)
                            {
                                log.InfoFormat("User Filtered: {0}", aDUsername);
                                log.Info("----------------------");
                                log.Info("- ENDING AD USER     -");
                                log.Info("----------------------");
                                log.Info("");
                                continue;
                            }
                        }

                        // Get the manager property
                        string managerCN = directoryEntry.Properties["manager"].Value.ToString();

                        // Find the user in AD.
                        ActiveDirectoryManager managerActiveDirectoryManager = new ActiveDirectoryManager(connectionString + "/" + managerCN, username, password);
                        SearchResultCollection managers = managerActiveDirectoryManager.GetLDAPDirectoryEntries("");

                        // Get their username.
                        if (managers.Count > 0)
                        {
                            System.DirectoryServices.DirectoryEntry manager = managers[0].GetDirectoryEntry();
                            string managerUsername = manager.Properties["sAMAccountName"].Value.ToString();

                            // Use this to look up PD entry in IX
                            User managerUser = userManager.GetUser(managerUsername);
                            User reportUser = userManager.GetUser(aDUsername);
                            if (managerUser == null || reportUser == null)
                                continue;
                            else
                            {
                                // Get directory entry id
                                Greenvale.ActiveDirectory.Intextra.DirectoryEntry managerDE = directoryEntryManager.GetDirectoryEntry(managerUser.UserID);
                                Greenvale.ActiveDirectory.Intextra.DirectoryEntry reportDE = directoryEntryManager.GetDirectoryEntry(reportUser.UserID);

                                if (managerDE == null)
                                {
                                    log.ErrorFormat("User: {0} Manager: {1} does not exist", aDUsername, managerDE);
                                    continue;
                                }
                                
                                // set the manager attribute to this.
                                directoryEntryManager.EnsureDirectoryEntryAttribute(AttributeType.Manager, managerDE.DirectoryEntryID.ToString(), reportDE.DirectoryEntryID);
                            }

                            
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        log.Info("No Manager Set");
                    }

                    log.Info("-----------------------");
                    log.Info("- ENDING MANAGER USER -");
                    log.Info("-----------------------");
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }
    }
}

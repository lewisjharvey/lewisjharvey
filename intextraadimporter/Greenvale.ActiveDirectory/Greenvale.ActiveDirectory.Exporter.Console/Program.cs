using System;
using System.Collections.Generic;
using System.DirectoryServices;

using log4net;
using log4net.Config;

namespace Greenvale.ActiveDirectory.Exporter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Must provide path for CSV output as arg 1");

            ILog log = log4net.LogManager.GetLogger("Default");
            XmlConfigurator.Configure();

            try
            {

                log.Info("===============================");
                log.Info(" Import process started");
                log.Info(DateTime.Now.ToString());
                log.Info("===============================");


                // Set up the configuration items
                string connectionString = Greenvale.ActiveDirectory.Exporter.Console.Properties.Settings.Default.LDAPConnectionString;
                string username = Greenvale.ActiveDirectory.Exporter.Console.Properties.Settings.Default.LDAPUsername;
                string password = Greenvale.ActiveDirectory.Exporter.Console.Properties.Settings.Default.LDAPPassword;
                string ldapFilter = Greenvale.ActiveDirectory.Exporter.Console.Properties.Settings.Default.LDAPFilter;
                string regExFilter = Greenvale.ActiveDirectory.Exporter.Console.Properties.Settings.Default.RegExFilter;

                log.DebugFormat("ConnectionString: {0}", connectionString);
                log.DebugFormat("Username: {0}", username);
                log.DebugFormat("Password: {0}", password);
                log.DebugFormat("LDAPFilter: {0}", ldapFilter);
                log.DebugFormat("RegExFilter: {0}", regExFilter);

                // Build a manager for return Intextra modelled objects.
                ActiveDirectoryManager activeDirectoryManager = new ActiveDirectoryManager(connectionString, username, password);
                SearchResultCollection collection = activeDirectoryManager.GetLDAPDirectoryEntries(ldapFilter);

                List<DirectoryUser> directoryUsers = new List<DirectoryUser>();
                for (int q = 0; q < collection.Count; q++)
                {
                    System.DirectoryServices.DirectoryEntry myEntry = collection[q].GetDirectoryEntry();
                    //string theUsername = myEntry.Properties["sAMAccountName"][0].ToString().ToLower();

                    int userAccountControl = Convert.ToInt32(myEntry.Properties["userAccountControl"][0]);
                    bool disabled = ((userAccountControl & 2) > 0);

                    //bool disabled = Convert.ToBoolean(myEntry.Properties["msDS-UserAccountDisabled"][0]);
                    
                    if (!disabled)
                    {
                        // Write out the username
                        directoryUsers.Add(activeDirectoryManager.CreateUserFromEntry(myEntry));
                    }
                }

                // Now produce the CSV
                CSVManager csvManager = new CSVManager();
                csvManager.GenerateCSVFile(args[0], directoryUsers);

                log.Info("-----------------------");
                log.Info("- ENDING MANAGER USER -");
                log.Info("-----------------------");
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }
    }
}

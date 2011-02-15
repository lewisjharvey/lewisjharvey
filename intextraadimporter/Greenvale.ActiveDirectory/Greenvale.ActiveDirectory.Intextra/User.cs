using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Greenvale.ActiveDirectory.Intextra
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public bool Disabled { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? LastEditedBy { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string NTDomainName { get; set; }
        public string NTUserName { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string DefaultURL { get; set; }
        public string Theme { get; set; }
        public string ActiveDirectoryGUID { get; set; }

        public User(int userID, string userName, string password, string firstName, string surname, bool disabled, 
            DateTime creationDate, int createdBy, DateTime? lastEditDate, int? lastEditedBy, DateTime? expiryDate, 
            string nTDomainName, string nTUserName, string primaryEmailAddress, string defaultURL, string theme, string activeDirectoryGUID)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.Password = password;
            this.FirstName = firstName;
            this.Surname = surname;
            this.Disabled = disabled;
            this.CreationDate = creationDate;
            this.CreatedBy = createdBy;
            this.LastEditDate = lastEditDate;
            this.LastEditedBy = lastEditedBy;
            this.ExpiryDate = expiryDate;
            this.NTDomainName = nTDomainName;
            this.NTUserName = nTUserName;
            this.PrimaryEmailAddress = primaryEmailAddress;
            this.DefaultURL = defaultURL;
            this.Theme = theme;
            this.ActiveDirectoryGUID = activeDirectoryGUID;
        }
    }
}

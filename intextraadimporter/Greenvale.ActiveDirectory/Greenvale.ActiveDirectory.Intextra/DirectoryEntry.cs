using System.Collections.Generic;
using System;

namespace Greenvale.ActiveDirectory.Intextra
{
    public class DirectoryEntry
    {
        public int DirectoryEntryID
        {
            get;
            set;
        }

        public int DirectoryID
        {
            get;
            set;
        }

        public List<DirectoryEntryAttribute> DirectoryEntryAttributes
        {
            get;
            set;
        }

        public DateTime CreatedOn
        {
            get;
            set;
        }

        public DateTime? LastModifiedOn
        {
            get;
            set;
        }

        public int CreatedByUserID
        {
            get;
            set;
        }

        public int? LastModifiedByUserID
        {
            get;
            set;
        }

        public int UserID
        { 
            get; 
            set; 
        }

        public DirectoryEntry(int directoryEntryID, int userID, int directoryID, DateTime createdOn, DateTime? lastModifiedOn, int createdByUserID, int? lastModifiedByUserID)
        {
            this.DirectoryEntryID = directoryEntryID;
            this.UserID = userID;
            this.DirectoryEntryID = directoryEntryID;
            this.DirectoryID = directoryID;
            this.CreatedOn = createdOn;
            this.LastModifiedOn = lastModifiedOn;
            this.CreatedByUserID = createdByUserID;
            this.LastModifiedByUserID = lastModifiedByUserID;
        }
    }
}

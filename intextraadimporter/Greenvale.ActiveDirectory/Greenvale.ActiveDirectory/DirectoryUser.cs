using System;

using FileHelpers;

namespace Greenvale.ActiveDirectory
{
    [DelimitedRecord(",")]
    public class DirectoryUser
    {
        [FieldQuoted] 
        public string Forename;
        [FieldQuoted]
        public string Surname;
        [FieldQuoted]
        public string Site;
        [FieldQuoted]
        public string WorkTelephone;
        [FieldQuoted]
        public string EmailAddress;
        [FieldQuoted]
        public string MobileNo;
        [FieldQuoted]
        public string JobTitle;
        [FieldQuoted]
        public string Department;
        [FieldQuoted] 
        public string Company;
    }
}

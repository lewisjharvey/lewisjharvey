using System.Collections.Generic;

using FileHelpers;

namespace Greenvale.ActiveDirectory
{
    public class CSVManager
    {
        public void GenerateCSVFile(string path, List<DirectoryUser> directoryUsers)
        {
            FileHelperEngine<DirectoryUser> engine = new FileHelperEngine<DirectoryUser>();
            engine.WriteFile(path, directoryUsers);
        }
    }
}

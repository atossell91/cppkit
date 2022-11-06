using System;
using System.Collections.Generic;
using System.Text;

namespace cppkit
{
    interface IFileManager
    {
        bool CreateFiles();
        bool DeleteFiles();

        bool RenameFiles(string newname);
    }
}

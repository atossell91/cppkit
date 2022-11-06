using System;
using System.Collections.Generic;

namespace cppkit
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Arg> argsList = ArgParser.ParseArgs(args);
            foreach (Arg arg in argsList)
            {
                IFileManager cpp = new CppClassManager(arg.Parameters[0]);
                if (arg.ArgName == "class")
                {
                    if (!cpp.CreateFiles())
                    {
                        Console.WriteLine("Error");
                    }
                    else
                    {
                        Console.WriteLine("File created.");
                    }
                }
                else if (arg.ArgName == "delete")
                {
                    if (!cpp.DeleteFiles())
                    {
                        Console.WriteLine("Delete error");
                    }
                    else
                    {
                        Console.WriteLine("Files deleted.");
                    }
                }
                else if (arg.ArgName == "rename")
                {
                    if (!cpp.RenameFiles(arg.Parameters[1]))
                    {
                        Console.WriteLine("Rename error");
                    }
                    else
                    {
                        Console.WriteLine("File renamed");
                    }
                }
            }
        }
    }
}

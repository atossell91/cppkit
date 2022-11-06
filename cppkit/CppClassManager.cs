using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace cppkit
{
    class CppClassManager : IFileManager
    {
        public CppClassManager(string name)
        {
            Name = name;

            Dir = System.IO.Directory.GetCurrentDirectory();

            HeaderPath = Path.Combine(new string[] { Dir, INCLUDE_DIR_NAME, Name + HEAD_EXT });
            ImplementationPath = Path.Combine(new string[] { Dir, SRC_DIR_NAME, Name + CPP_EXT });
        }
        public string Name { get; set; }

        public string Dir { get; private set; }

        public string HeaderPath { get; private set; }
        public string ImplementationPath { get; private set; }

        public bool HasInclude { get; private set; } = false;
        public bool HasSrc { get; private set; } = false;

        const string SRC_DIR_NAME = "src";
        const string INCLUDE_DIR_NAME = "include";
        const string CPP_EXT = ".cpp";
        const string HEAD_EXT = ".h";
        const string COPYRIGHT_FORM = "//  Copyright {0} Anthony Tossell";
        const string DATE_STR = "yyyy";
        const string PREP_FORM = "INCLUDE_{0}_H_";
        const string IFNDEF_FORM = "#ifndef {0}";
        const string DEFINE_FORM = "#define {0}";
        const string CLASS_FORM = "class {0} {{\n private:\n public:\n}};\n";
        const string ENDIF_FORM = "#endif  // {0}";
        const string INCLUDE_FORM = "#include \"../include/{0}" + HEAD_EXT + "\"";

        private bool FoundDirs(string dr)
        {
            string[] dirs = System.IO.Directory.GetDirectories(dr);
            string includePath = Path.Combine(dr, INCLUDE_DIR_NAME);
            string srcPath = Path.Combine(dr, SRC_DIR_NAME);

            foreach (string dir in dirs)
            {
                if (dir == includePath)
                {
                    HasInclude = true;
                }
                else if (dir == srcPath)
                {
                    HasSrc = true;
                }
            }
            return HasSrc && HasInclude;
        }

        void autoFillHeader(string path)
        {
            string prepStr = String.Format(PREP_FORM, Name.ToUpper());
            //Console.WriteLine(prepStr);
            string[] fileContent = new string[]
            {
                String.Format(COPYRIGHT_FORM, DateTime.Now.ToString(DATE_STR)),
                String.Format(IFNDEF_FORM, prepStr),
                String.Format(DEFINE_FORM, prepStr),
                "",
                String.Format(CLASS_FORM, Name),
                String.Format(ENDIF_FORM, prepStr)
            };
            File.WriteAllLines(path, fileContent);
        }

        void autoFillImp(string path)
        {
            string[] fileContent = new string[]
            {
                String.Format(COPYRIGHT_FORM, DateTime.Now.ToString(DATE_STR)),
                String.Format(INCLUDE_FORM,Name),
                ""
            };
            File.WriteAllLines(path, fileContent);
        }

        public bool CreateFiles()
        {
            if (!FoundDirs(Dir))
            {
                Console.WriteLine("Directories not found in " + Dir);
                return false;
            }

            if (File.Exists(HeaderPath))
            {
                Console.WriteLine("File " + HeaderPath + " aldready exists.");
                return false;
            }
            FileStream fHeader = File.Create(HeaderPath);
            fHeader.Close();

            if (File.Exists(ImplementationPath))
            {
                Console.WriteLine("File " + ImplementationPath + " aldready exists.");
                return false;
            }
            FileStream fImp = File.Create(ImplementationPath);
            fImp.Close();

            autoFillHeader(HeaderPath);
            autoFillImp(ImplementationPath);

            return true;
        }

        public bool DeleteFiles()
        {

            if (!File.Exists(HeaderPath) || !File.Exists(ImplementationPath)) {
                return false;
            }

            File.Delete(HeaderPath);
            File.Delete(ImplementationPath);
            return true;
        }

        public void ReplaceIncludes(string oldName)
        {
            List<string> files = new List<string>();

            string srcDir = Path.Combine(Dir, SRC_DIR_NAME);
            string incDir = Path.Combine(Dir, INCLUDE_DIR_NAME);

            files.AddRange(Directory.GetFiles(incDir));
            files.AddRange(Directory.GetFiles(srcDir));

            Console.WriteLine("Pulled " + files.Count + " files from directories,");
            Console.WriteLine("  " + incDir);
            Console.WriteLine("  " + srcDir);

            string oldInclude = String.Format(INCLUDE_FORM, oldName);
            string newInclude = String.Format(INCLUDE_FORM, Name);

            Console.WriteLine("Replacing, " + oldInclude);
            Console.WriteLine("     with, " + newInclude);

            foreach (string file in files)
            {
                Console.WriteLine("Working on file: " + file);
                string txt = File.ReadAllText(file);
                File.WriteAllText(file, txt.Replace(oldInclude, newInclude));
            }
        }

        public bool RenameFiles(string newname)
        {
            string name = newname;
            string newHeader = Path.Combine(new string[] { Dir, INCLUDE_DIR_NAME, name + HEAD_EXT });
            string newImp = Path.Combine(new string[] { Dir, SRC_DIR_NAME, name + CPP_EXT });

            if (!File.Exists(HeaderPath) || !File.Exists(ImplementationPath))
            {
                return false;
            }

            File.Move(HeaderPath, newHeader);
            File.Move(ImplementationPath, newImp);

            if (!File.Exists(newHeader) || !File.Exists(newImp))
            {
                return false;
            }

            string oldName = Name;
            Name = name;
            ImplementationPath = newImp;
            HeaderPath = newHeader;

            ReplaceIncludes(oldName);

            return true;
        }
    }
}

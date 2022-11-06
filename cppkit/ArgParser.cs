using System;
using System.Collections.Generic;
using System.Text;

namespace cppkit
{
    public class ArgParser
    {
        public static bool IsArg(string cmd)
        {
            if (cmd.Length > 2 &&
                cmd[0] == '-' &&
                cmd[1] == '-' &&
                Char.IsLetterOrDigit(cmd[2]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string RemoveDashes(string arg)
        {
            int start = 0;
            for (; start < arg.Length && arg[start] == '-'; ++start);
            return arg.Substring(start);
        }
        public static List<Arg> ParseArgs(string[] cmd)
        {
            List<Arg> argsList = new List<Arg>();
            Arg a = new Arg();

            if (!IsArg(cmd[0])) {
                throw new ArgumentException("First argument must be an argument.");
            }

            a.ArgName = RemoveDashes(cmd[0]);

            for (int n =1; n < cmd.Length; ++n)
            {
                string tok = cmd[n];

                if (IsArg(tok))
                {
                    argsList.Add(a);
                    a = new Arg();
                    a.ArgName = RemoveDashes(tok);
                }
                else
                {
                    a.AddParam(tok);
                }
            }

            argsList.Add(a);

            return argsList;
        }
    }
}

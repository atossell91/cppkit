using System;
using System.Collections.Generic;
using System.Text;

namespace cppkit
{
    public class Arg
    {
        public Arg()
        {
            Parameters = new List<string>();
        }
        public string ArgName { get; set; }
        public List<string> Parameters { get; private set;}

        public void AddParam(string param)
        {
            Parameters.Add(param);
        }

        public void ClearParams()
        {
            Parameters.Clear();
        }
    }
}

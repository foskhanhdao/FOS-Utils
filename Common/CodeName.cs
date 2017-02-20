using System;
using System.Collections.Generic;
using System.Text;

namespace FOS_Utils
{
    public class CodeName
    {
        private string code;
        private string name;
        public CodeName(string c, string n)
        {
            this.code = c;
            this.name = n;
        }
        public string Code
        {
            get
            {
                return code;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public override string ToString()
        {
            return name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Classes
{
    public class CodeMaps
    {
        private Dictionary<int, string> codeNames;
        public CodeMaps()
        {
            codeNames = new Dictionary<int, string>();
            codeNames[0] = "CODE_ANALOG";
            codeNames[1] = "CODE_CUSTOM";
            codeNames[2] = "CODE_SINGLENODE";
            codeNames[3] = "CODE_CONSUMER";
            codeNames[4] = "CODE_MOTION";
            codeNames[5] = "CODE_DIGITAL";
            codeNames[6] = "CODE_LIMITSET";
            codeNames[7] = "CODE_MULTIPLENODE";
            codeNames[8] = "CODE_SOURCE";
            codeNames[9] = "CODE_SENSOR";
        }
        public string GetNameForCode(Codes code)
        {
            if(!codeNames.Keys.Contains((int)code))
            {
                return null;
            }
            return codeNames[(int)code];
        }
    }
}

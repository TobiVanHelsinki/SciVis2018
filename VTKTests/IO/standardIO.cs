using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VTKTests.IO
{
    class standardIO
    {
        public static string ReadFile_TextContent(string fileName)
        {
            FileStream file = File.OpenRead(fileName);
            StreamReader filereader = new StreamReader(file);
            StringBuilder builder = new StringBuilder();
            while (true)
            {
                string line = filereader.ReadLine();
                if (line == null)
                {
                    break;
                }
                builder.Append(line);
            }
            return builder.ToString();
        }
    }
}

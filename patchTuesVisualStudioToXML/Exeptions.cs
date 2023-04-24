using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML
{
    public class MSRCStatusCodeExeption:Exception
    {
        public MSRCStatusCodeExeption(string message) : base(string.Format("Connection error. Status code {0}", message) )
        {
        }
        public MSRCStatusCodeExeption(string message, Exception inner)
    : base(string.Format("Connection error. Status code {0}", message), inner)
        {
        }
    }
    public class GeneratorValueExeption : Exception
    {

        public GeneratorValueExeption(string name):base(name)
        {
        }
        public GeneratorValueExeption(string message, Exception inner)
    : base(message, inner)
        {
        }

    }
    
}

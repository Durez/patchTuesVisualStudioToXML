using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace patchTuesVisualStudioToXML
{
    public class MSRCAPIControllerStatusCodeExeption:Exception
    {
        public MSRCAPIControllerStatusCodeExeption()
        {

        }
        public MSRCAPIControllerStatusCodeExeption(string message) : base(string.Format("Connection error. Status code {0}", message) )
        {
        }
        public MSRCAPIControllerStatusCodeExeption(string message, Exception inner)
    : base(string.Format("Connection error. Status code {0}", message), inner)
        {
        }
    }
    public class GeneratorValueExeption : Exception
    {
        public GeneratorValueExeption()
        {

        }

        public GeneratorValueExeption(string name):base(name)
        {
        }
        public GeneratorValueExeption(string message, Exception inner)
    : base(message, inner)
        {
        }

    }
    
}

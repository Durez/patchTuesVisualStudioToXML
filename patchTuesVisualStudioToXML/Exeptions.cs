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
        public MSRCStatusCodeExeption(string argName, Exception inner)
    : base(string.Format("Connection error. Status code {0}", argName), inner)
        {
        }
    }
    public class GeneratorValueExeption : Exception
    {

        public GeneratorValueExeption(string argName) :base(string.Format("Empty value in {0}", argName))
        {
        }
        public GeneratorValueExeption(string argName, Exception inner)
    : base(string.Format("Empty value in {0}", argName), inner)
        {
        }

    }



}

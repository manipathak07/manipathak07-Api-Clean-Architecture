using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singularity.Common.Exceptions
{
    public class NotImplementedException:Exception
    {
        public NotImplementedException(string msg): base(msg)
        {

        }
    }
}

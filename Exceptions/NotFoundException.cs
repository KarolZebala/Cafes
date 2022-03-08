using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Exceptions
{
    public class NotFoundEcxeption : Exception
    {
        public NotFoundEcxeption(string message) : base(message)
        {

        }
    }
    
}

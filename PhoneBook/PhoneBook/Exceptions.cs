using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    /// <summary>
    /// 
    /// Exceptions defined for my application.
    /// 
    /// </summary>
    class FailedLogin : ApplicationException {

        public FailedLogin(string message) : base(message)
        {

        }

    }
    class InvalidInput : ApplicationException {
        public InvalidInput(string message) : base(message)
        {

        }
    }
    class ItemNotFound : ApplicationException {
        public ItemNotFound(string message) : base(message)
        {

        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Exceptions
{
    public class NotFoundException : Exception
    {
        private string _errorMessage;
        public NotFoundException(string msg)
        {
            _errorMessage = msg;
        }

        public override string Message => _errorMessage;
    }
}

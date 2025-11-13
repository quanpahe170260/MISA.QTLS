using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Core.Exceptions
{
    public class BusinessException : Exception
    {
        private string _errorMessage;
        public BusinessException(string msg)
        {
            _errorMessage = msg;
        }

        public override string Message => _errorMessage;
    }
}

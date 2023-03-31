using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GestaoComercio.Domain.Utils
{
    public class MyExceptionApi : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public MyExceptionApi(string message, HttpStatusCode statusCode) : base (message)
        {
            StatusCode = statusCode;
        }
    }
}

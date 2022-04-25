using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TVmazeScrapper.Domain.Models.Exceptions
{
    public class ScrapException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public ScrapException(HttpStatusCode httpStatusCode) : base($"The TV API return HttpStatus: {httpStatusCode}")
        {
            this.StatusCode = httpStatusCode;
        }
    }
}

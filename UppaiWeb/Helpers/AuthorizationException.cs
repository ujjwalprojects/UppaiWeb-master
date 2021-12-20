using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UppaiWeb.Helpers
{
    [Serializable]
    public class AuthorizationException : Exception
    {
        public AuthorizationException()
            : base() { }
    }
}
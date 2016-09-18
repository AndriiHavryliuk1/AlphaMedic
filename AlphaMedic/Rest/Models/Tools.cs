using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Rest.Models
{
    public  static class Tools
    {

       public static bool AnyRole(IPrincipal user,List<string> roles)
        {
            foreach(var r in roles)
            {
                if (user.IsInRole(r)) return true;
            }
            return false;
        }
    }
}
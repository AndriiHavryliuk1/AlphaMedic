using Rest.Models.AlphaMedicContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthorizationServer.Models
{
    public class UserStore
    {
        AlphaMedicContext db = new AlphaMedicContext(); 
    }
}
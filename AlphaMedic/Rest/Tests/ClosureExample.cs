using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Tests
{
    public class ClosureExample
    {
        public static Func<string,string> Hello()
        {
            string previous = "Alex";
            Func<string, string> func = (name) =>
            {
               
                var res= "Hello, " + name + " previous user " + previous;
                previous = name;
                return res;
            }; 
            return func;
        }
    }
}
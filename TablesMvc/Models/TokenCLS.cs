using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TablesMvc.Models
{
    public class TokenCLS
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}

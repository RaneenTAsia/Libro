using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configurations
{
    public static class Extensions
    {
        public static string AddCurrency(this decimal value, string Currency)
        {
            return value + " " + Currency;
        }
    }
}

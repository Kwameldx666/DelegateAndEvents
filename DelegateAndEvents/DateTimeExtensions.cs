using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateTimeExtensions_
{
    // Extension method for DateTime to check if a date is within a specified range
    public static class DateTimeExtensions
    {
        public static bool IsWithinRange(this DateTime date, DateTime? startDate, DateTime? endDate)
        {
            return (!startDate.HasValue || date >= startDate) &&
                   (!endDate.HasValue || date <= endDate);
        }
    }

}

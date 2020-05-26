using System;

namespace MyBlog.Admin.Models.Utilities
{
    public static class DateFormatExtensions
    {
        public static string ToLiteralDate(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy hh:mm");    
        }

        // public static string ToLiteralDateTime(this DateTime dateTime)
        // {
        //     DateTime todayUtc = DateTime.UtcNow.Date;
        //     DateTime date = dateTime.Date;

        //     const string timeFormat = "hh:mm tt";
        //     if (date == todayUtc)
        //         return "Today, " + date.ToString(timeFormat);
        //     else if (date == todayUtc.AddDays(-1.0))
        //         return "Yesterday, " + date.ToString(timeFormat);
        //     else if (date > todayUtc.AddDays(-7.0)
        //         && date < todayUtc)
        //         return date.ToString("ddd, " + timeFormat);
        //     else if (date > todayUtc.AddMonths(-1)
        //         && date < todayUtc)
        //         return date.ToString("d MMM " + timeFormat);
        //     else if (date.Year == todayUtc.Year)
        //         return date.ToString("d MMM " + timeFormat);

        // }
    } 
}
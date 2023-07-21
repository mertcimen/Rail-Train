using System;
using System.Globalization;
using UnityEngine;

namespace FunGames.Tools.Utils
{
    public class DateUtils
    {
        public static DateTime ConvertInvariant(string dateTime)
        {
            try
            {
                return Convert.ToDateTime(dateTime, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Current date is not stored with InvariantCulture format!");
                return ConvertSimple(dateTime);
            }
        }

        private static DateTime ConvertSimple(string dateTime)
        {
            try
            {
                return Convert.ToDateTime(dateTime);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return DateTime.Now;
            }
        }
    }
}
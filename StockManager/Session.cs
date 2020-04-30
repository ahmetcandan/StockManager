using Newtonsoft.Json;
using StockManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace StockManager
{
    public static class Session
    {
        public static DataAccess Entities;
        public static Account DefaultAccount;
        public static User User;

        public static T DeepCopy<T>(this T item)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(item));
        }

        public static List<T> DeepCopy<T>(this List<T> item)
        {
            return JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(item));
        }

        public static DateTime SmallDate(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateTime DayStart(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }

        public static DateTime DayEnd(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static void SaveChanges()
        {
            Entities.Save();
        }

        public static string ToMoneyStirng(this decimal value, int d)
        {
         
            return value.ToString("N" + d);
        }

        public static string ComputeSha256Hash(this string rawData, string salt)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData + salt));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string MoneyTypeToString(this MoneyType moneyType)
        {
            switch (moneyType)
            {
                case MoneyType.TRY:
                    return "TRY";
                case MoneyType.USD:
                    return "USD";
                case MoneyType.EUR:
                    return "EUR";
                default:
                    return "";
            }
        }
    }
}

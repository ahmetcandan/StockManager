using Borsa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Borsa
{
    public static class DB
    {
        public static DataAccess Entities;
        public static Account DefaultAccount;
        public static User User;

        public static void Save()
        {
            Entities.Save();
        }

        public static string ToMoneyStirng(this decimal value, int d)
        {
         
            return value.ToString("N" + d);
        }

        public static string ComputeSha256Hash(this string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

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

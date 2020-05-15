using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace StockManager
{
    public class DovizComApi
    {
        public List<StockCurrent> StockCurrents { get; set; }
        string html;
        string hisseUrl = @"<a href=""//borsa.doviz.com/hisseler/";

        public DovizComApi()
        {
            try
            {
                html = getRequestToString();
                StockCurrents = new List<StockCurrent>();

                string text = html.Substring(html.IndexOf("Tüm Hisse Senetleri"));

                while (text.IndexOf(hisseUrl) > -1)
                {
                    text = text.Substring(text.IndexOf(hisseUrl));
                    StockCurrent stockCurrent = new StockCurrent();
                    stockCurrent.StockName = text.Substring(text.IndexOf(@""">") + 2, text.IndexOf("</a>") - (text.IndexOf(@""">") + 2)).Trim();
                    stockCurrent.StockCode = getStockName(text.Substring(text.IndexOf(hisseUrl) + hisseUrl.Length, 7));
                    stockCurrent.Price = getStockValue(text.Substring(text.IndexOf("<td>") + 4, 20));
                    text = text.Substring(text.IndexOf("<td>"));
                    stockCurrent.StockName = stockCurrent.StockName.Substring(stockCurrent.StockCode.Length + 3);
                    stockCurrent.UpdateDate = DateTime.Now;
                    stockCurrent.CreatedDate = DateTime.Now;
                    StockCurrents.Add(stockCurrent);
                }
            }
            catch (Exception ex)
            {

            }
        }

        string getStockName(string text)
        {
            return text.Substring(0, text.IndexOf(@""">"));
        }

        decimal getStockValue(string text)
        {
            decimal result = 0;
            decimal.TryParse(text.Substring(0, text.IndexOf("</td>")).Replace(",", "."), out result);
            return result;
        }

        public string getRequestToString()
        {
            string result = string.Empty;
            WebRequest request = WebRequest.Create("https://borsa.doviz.com/hisseler");
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                result = reader.ReadToEnd();
            }
            response.Close();
            return result;
        }
    }
}

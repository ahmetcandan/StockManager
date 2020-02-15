using Borsa.Model;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Borsa
{
    public class DataAccess
    {
        public DataAccess()
        {
            fileExistCreate();
            read();
            GetStockService();
        }

        public List<Account> Accounts { get; set; }
        public List<AccountTransaction> AccountTransactions { get; set; }
        public List<Stock> Stocks { get; set; }
        public List<StockTransaction> StockTransactions { get; set; }
        public List<User> Users { get; set; }
        public Setting Setting { get; set; }

        public int GenerateAccontTransactionId()
        {
            if (AccountTransactions.Count > 0)
                return AccountTransactions.Max(c => c.AccountTransactionId) + 1;
            return 1;
        }

        public int GenerateStockTransactionId()
        {
            if (StockTransactions.Count > 0)
                return StockTransactions.Max(c => c.StockTransactionId) + 1;
            return 1;
        }

        public int GenerateAccountId()
        {
            if (Accounts.Count > 0)
                return Accounts.Max(c => c.AccountId) + 1;
            return 1;
        }

        public StockTransaction GetStockTransaction(int stockTransactionId)
        {
            if (StockTransactions.Any(c => c.StockTransactionId == stockTransactionId))
            {
                var result = StockTransactions.First(c => c.StockTransactionId == stockTransactionId);
                if (result.AccountTransaction == null)
                    result.AccountTransaction = AccountTransactions.FirstOrDefault(c => c.AccountTransactionId == result.AccountTransactionId);
                if (result.Stock == null)
                    result.Stock = GetStock(result.StockCode);
                return result;
            }
            return null;
        }

        public StockTransaction GetStockTransactionByAccountTransactionId(int accountTransactionId)
        {
            if (StockTransactions.Any(c => c.AccountTransactionId == accountTransactionId))
            {
                var result = StockTransactions.First(c => c.AccountTransactionId == accountTransactionId);
                if (result.AccountTransaction == null)
                    result.AccountTransaction = GetAccountTransaction(result.AccountTransactionId);
                if (result.Stock == null)
                    result.Stock = GetStock(result.StockCode);
                return result;
            }
            return null;
        }

        public AccountTransaction GetAccountTransaction(int accountTransactionId)
        {
            if (AccountTransactions.Any(c => c.AccountTransactionId == accountTransactionId))
            {
                var result = AccountTransactions.First(c => c.AccountTransactionId == accountTransactionId);
                if (result.Account == null)
                    result.Account = GetAccount(result.AccountId);
                if (result.StockTransaction == null && result.StockTransactionId.HasValue)
                    result.StockTransaction = StockTransactions.FirstOrDefault(c => c.StockTransactionId == result.StockTransactionId.Value);
                return result;
            }
            return null;
        }

        public AccountTransaction GetAccountTransactionByStockTransactionId(int stockTransactionId)
        {
            if (AccountTransactions.Any(c => c.StockTransactionId == stockTransactionId))
            {
                var result = AccountTransactions.First(c => c.StockTransactionId == stockTransactionId);
                if (result.Account == null)
                    result.Account = GetAccount(result.AccountId);
                if (result.StockTransaction == null && result.StockTransactionId.HasValue)
                    result.StockTransaction = GetStockTransaction(result.StockTransactionId.Value);
                return result;
            }
            return null;
        }

        public User GetUser(string userName, string passwordHash)
        {
            var user = Users.FirstOrDefault(c => c.IsActive && c.UserName == userName && c.Password == passwordHash);
            if (user == null)
                return null;
            user.Accounts = (from a in Accounts
                             join ua in user.Accounts on a.AccountId equals ua.AccountId
                             select new Account
                             {
                                 AccountId = a.AccountId,
                                 AccountName = a.AccountName,
                                 DefaultAccount = ua.DefaultAccount,
                                 MoneyType = a.MoneyType,
                                 TotalAmount = a.TotalAmount,
                                 AccountTransactions = a.AccountTransactions
                             }).ToList();
            return user;
        }

        public Stock GetStock(string stockCode)
        {
            if (Stocks.Any(c => c.StockCode == stockCode))
            {
                return Stocks.First(c => c.StockCode == stockCode);
            }
            return null;
        }

        public Account GetAccount(int accountId)
        {
            if (Accounts.Any(c => c.AccountId == accountId))
                return Accounts.First(c => c.AccountId == accountId);
            return null;
        }

        public StockTransaction PostStockTransaction(StockTransaction stockTransaction)
        {
            var item = StockTransactions.FirstOrDefault(c => c.StockTransactionId == stockTransaction.StockTransactionId);
            var accountTransaction = GetAccountTransactionByStockTransactionId(stockTransaction.StockTransactionId);

            if (item != null)
                item = stockTransaction;
            else
            {
                stockTransaction.StockTransactionId = GenerateStockTransactionId();
                StockTransactions.Add(stockTransaction);
            }

            if (accountTransaction == null)
                accountTransaction = PostAccountTransaction(new AccountTransaction
                {
                    Amount = stockTransaction.Amount * stockTransaction.UnitPrice * (stockTransaction.TransactionType == TransactionType.Buy ? -1 : 1),
                    Date = stockTransaction.Date,
                    StockTransactionId = stockTransaction.StockTransactionId,
                    StockTransaction = stockTransaction,
                    AccountId = DB.DefaultAccount.AccountId
                });
            else
                accountTransaction.Amount = stockTransaction.Amount * stockTransaction.UnitPrice * (stockTransaction.TransactionType == TransactionType.Buy ? -1 : 1);

            stockTransaction.AccountTransactionId = accountTransaction.AccountTransactionId;
            return stockTransaction;
        }

        public AccountTransaction PostAccountTransaction(AccountTransaction accountTransaction)
        {
            var item = AccountTransactions.FirstOrDefault(c => c.AccountTransactionId == accountTransaction.AccountTransactionId);
            if (item != null)
            {
                item = accountTransaction;
                return item;
            }
            else
            {
                accountTransaction.AccountTransactionId = GenerateAccontTransactionId();
                AccountTransactions.Add(accountTransaction);
                return accountTransaction;
            }
        }

        public User PostUser(User user, bool isNew, string currentPasswordHash = "")
        {

            User item = Users.FirstOrDefault(c => c.UserName == user.UserName);

            if (isNew && item != null)
                throw new Exception("This user name is used");

            if (!isNew && string.IsNullOrEmpty(currentPasswordHash))
                throw new Exception("Current password is empty");

            if (!isNew && item == null)
                throw new Exception("User name is not found");

            if (!isNew && item.Password != currentPasswordHash)
                throw new Exception("Current password is in correct");

            foreach (var account in user.Accounts)
                DB.Entities.PostAccount(account);

            if (item != null)
            {
                item = user;
                return item;
            }
            else
            {
                Users.Add(user);
                return user;
            }
        }

        public Stock PostStock(Stock stock)
        {
            var item = Stocks.FirstOrDefault(c => c.StockCode == stock.StockCode);
            if (item != null)
            {
                item = stock;
                return item;
            }
            else
            {
                Stocks.Add(stock);
                return stock;
            }
        }

        public Account PostAccount(Account account)
        {
            var item = Accounts.FirstOrDefault(c => c.AccountId == account.AccountId);
            var userAccount = DB.User.Accounts.FirstOrDefault(c => c.AccountId == account.AccountId);
            if (userAccount != null)
                userAccount.DefaultAccount = account.DefaultAccount;
            if (account.DefaultAccount)
                foreach (var acc in DB.User.Accounts.Where(c => c.AccountId != account.AccountId))
                    acc.DefaultAccount = false;
            if (item != null)
            {
                item = account;
                getUserAccounts();
                return item;
            }
            else
            {
                account.AccountId = GenerateAccountId();
                DB.User.Accounts.Add(account);
                Accounts.Add(account);
                getUserAccounts();
                return account;
            }
        }

        private void getUserAccounts()
        {
            DB.User.Accounts = (from a in Accounts
                                join ua in DB.User.Accounts on a.AccountId equals ua.AccountId
                                select new Account
                                {
                                    AccountId = a.AccountId,
                                    AccountName = a.AccountName,
                                    DefaultAccount = ua.DefaultAccount,
                                    MoneyType = a.MoneyType,
                                    TotalAmount = a.TotalAmount,
                                    AccountTransactions = a.AccountTransactions
                                }).ToList();
        }

        public void DeleteStockTransaction(int stockTransactionId)
        {
            var stockTransaction = GetStockTransaction(stockTransactionId);
            if (AccountTransactions.Any(c => c.AccountTransactionId == stockTransaction.AccountTransactionId))
                AccountTransactions.Remove(AccountTransactions.First(c => c.AccountTransactionId == stockTransaction.AccountTransactionId));
            StockTransactions.Remove(stockTransaction);
            Save();
        }

        public void Save()
        {
            foreach (var account in Accounts)
                account.TotalAmount = AccountTransactions.Where(c => c.AccountId == account.AccountId).Sum(c => c.Amount);
            write();
        }

        public void GetStockService()
        {
            foreach (var stockCode in (from s in StockTransactions select s.StockCode).Distinct())
            {
                Stock stock = new Stock();
                if (Stocks.Any(c => c.StockCode == stockCode))
                    stock = Stocks.First(c => c.StockCode == stockCode);
                dynamic data = getStock(stockCode);
                if (data == null)
                    continue;
                stock.Value = data.data.hisseYuzeysel.alis;
                stock.Name = data.data.hisseYuzeysel.aciklama;
                stock.HighestValueOfDay = data.data.hisseYuzeysel.yuksek;
                stock.LowestValueOfDay = data.data.hisseYuzeysel.dusuk;
                stock.UpdateDate = data.data.hisseYuzeysel.tarih;
            }
        }

        public Stock GetStockService(string stockCode)
        {
            stockCode = stockCode.ToUpper();
            Stock stock = new Stock();
            if (Stocks.Any(c => c.StockCode == stockCode))
                stock = Stocks.First(c => c.StockCode == stockCode);
            dynamic data = getStock(stockCode);
            if (data == null)
            {
                if (DB.Entities.Stocks.Any(c => c.StockCode == stockCode))
                    return DB.Entities.GetStock(stockCode);
                return new Stock();
            }
            stock.Value = data.data.hisseYuzeysel.alis;
            stock.Name = data.data.hisseYuzeysel.aciklama;
            stock.HighestValueOfDay = data.data.hisseYuzeysel.yuksek;
            stock.LowestValueOfDay = data.data.hisseYuzeysel.dusuk;
            stock.UpdateDate = data.data.hisseYuzeysel.tarih;
            return stock;
        }

        private dynamic getStock(string stockCode)
        {
            try
            {
                return null;
                var client = new RestClient("http://bigpara.hurriyet.com.tr/api/v1/borsa/hisseyuzeysel/" + stockCode.ToUpper());
                var request = new RestRequest("", Method.GET);
                var response = client.Execute(request);
                return JsonConvert.DeserializeObject<dynamic>(response.Content);
            }
            catch
            {
                return null;
            }
        }

        private void write()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("Data/Accounts.json"))
                {
                    if (Accounts == null || Accounts.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from a in Accounts select new { a.AccountId, a.AccountName, a.MoneyType, a.TotalAmount, a.DefaultAccount }));
                }
                using (StreamWriter sw = new StreamWriter("Data/AccountTransactions.json"))
                {
                    if (AccountTransactions == null || AccountTransactions.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from c in AccountTransactions select new { c.AccountId, c.AccountTransactionId, c.Amount, c.Date, c.StockTransactionId }));
                }
                using (StreamWriter sw = new StreamWriter("Data/Stocks.json"))
                {
                    if (Stocks == null || Stocks.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from s in Stocks select new { s.StockCode, s.HighestValueOfDay, s.LowestValueOfDay, s.Name, s.Value }));
                }
                using (StreamWriter sw = new StreamWriter("Data/StockTransactions.json"))
                {
                    if (StockTransactions == null || StockTransactions.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from st in StockTransactions select new { st.StockTransactionId, st.UnitPrice, st.StockCode, st.TotalPrice, st.Date, st.Amount, st.AccountTransactionId, st.TransactionType }));
                }
                using (StreamWriter sw = new StreamWriter("Data/Users.json"))
                {
                    if (Users == null || Users.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from u in Users select new User { UserName = u.UserName, Password = u.Password, IsActive = u.IsActive, Accounts = u.Accounts.Select(c => new Account { AccountId = c.AccountId, DefaultAccount = c.DefaultAccount }).ToList() }));
                }
                using (StreamWriter sw = new StreamWriter("Data/Settings.json"))
                {
                    if (Setting == null)
                        sw.Write("{}");
                    else
                        sw.Write(JsonConvert.SerializeObject(Setting));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kayıt yazma sırasında bir hata oluştu.", ex);
            }
        }

        private void read()
        {
            try
            {
                using (StreamReader sw = new StreamReader("Data/Accounts.json"))
                {
                    Accounts = JsonConvert.DeserializeObject<List<Account>>(sw.ReadToEnd());
                    if (Accounts == null)
                        Accounts = new List<Account>();
                }
                using (StreamReader sw = new StreamReader("Data/AccountTransactions.json"))
                {
                    AccountTransactions = JsonConvert.DeserializeObject<List<AccountTransaction>>(sw.ReadToEnd());
                    if (AccountTransactions == null)
                        AccountTransactions = new List<AccountTransaction>();
                }
                using (StreamReader sw = new StreamReader("Data/Stocks.json"))
                {
                    Stocks = JsonConvert.DeserializeObject<List<Stock>>(sw.ReadToEnd());
                    if (Stocks == null)
                        Stocks = new List<Stock>();
                }
                using (StreamReader sw = new StreamReader("Data/StockTransactions.json"))
                {
                    StockTransactions = JsonConvert.DeserializeObject<List<StockTransaction>>(sw.ReadToEnd());
                    if (StockTransactions == null)
                        StockTransactions = new List<StockTransaction>();
                }
                using (StreamReader sw = new StreamReader("Data/Users.json"))
                {
                    Users = JsonConvert.DeserializeObject<List<User>>(sw.ReadToEnd());
                    if (Users == null)
                        Users = new List<User>();
                }
                using (StreamReader sw = new StreamReader("Data/Settings.json"))
                {
                    Setting = JsonConvert.DeserializeObject<Setting>(sw.ReadToEnd());
                    if (Setting == null)
                        Setting = new Setting();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kayıt okuma sırasında bir hata oluştu.", ex);
            }
        }

        private void fileExistCreate()
        {
            bool save = false;
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
                save = true;
            }

            if (!File.Exists("Data/Accounts.json"))
            {
                using (var file = File.Create("Data/Accounts.json"))
                {
                    save = true;
                    Accounts = new List<Account>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/AccountTransactions.json"))
            {
                using (var file = File.Create("Data/AccountTransactions.json"))
                {
                    save = true;
                    AccountTransactions = new List<AccountTransaction>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/Stocks.json"))
            {
                using (var file = File.Create("Data/Stocks.json"))
                {
                    save = true;
                    Stocks = new List<Stock>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/StockTransactions.json"))
            {
                using (var file = File.Create("Data/StockTransactions.json"))
                {
                    save = true;
                    StockTransactions = new List<StockTransaction>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/Users.json"))
            {
                using (var file = File.Create("Data/Users.json"))
                {
                    save = true;
                    Users = new List<User>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/Settings.json"))
            {
                using (var file = File.Create("Data/Settings.json"))
                {
                    save = true;
                    Setting = new Setting();
                    file.Close();
                }
            }

            if (save) Save();
        }
    }
}

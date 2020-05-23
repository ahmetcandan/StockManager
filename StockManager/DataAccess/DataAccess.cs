using Newtonsoft.Json;
using RestSharp;
using StockManager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StockManager
{
    public class DataAccess
    {
        public DataAccess()
        {
            fileExistCreate();
            read();
        }

        private List<Account> Accounts { get; set; }
        private List<AccountTransaction> AccountTransactions { get; set; }
        private List<Stock> Stocks { get; set; }
        private List<StockTransaction> StockTransactions { get; set; }
        private List<User> Users { get; set; }
        private Setting Setting { get; set; }
        private List<StockCurrent> CurrentStocks { get; set; }
        private List<Period> Periods { get; set; }
        private List<TranslateMessage> TranslateMessages { get; set; }

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

        public int GeneratePeriodId()
        {
            if (Periods.Count > 0)
                return Periods.Max(c => c.PeriodId) + 1;
            return 1;
        }

        public List<string> GetLanguageCodeList()
        {
            return TranslateMessages.Select(c => c.LanguageCode).Distinct().ToList();
        }

        public List<StockCurrent> GetCurrentStocks()
        {
            return CurrentStocks;
        }

        public Setting GetSetting()
        {
            return Setting;
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

        public List<StockTransaction> GetStockTransactions()
        {
            return (from st in StockTransactions
                    join at in AccountTransactions on st.StockTransactionId equals at.StockTransactionId
                    where at.AccountId == Session.DefaultAccount.AccountId
                    select st).ToList();
        }

        public Period GetPeriod(int periodId)
        {
            if (Periods.Any(c => c.PeriodId == periodId))
            {
                return Periods.First(c => c.PeriodId == periodId);
            }
            return null;
        }

        public List<TranslateMessage> GetTranslateMessages()
        {
            return TranslateMessages;
        }

        public List<Period> GetPeriods()
        {
            return Periods.Where(c => c.AccountId == Session.DefaultAccount.AccountId).ToList();
        }

        public TranslateMessage GetMessage(string code)
        {
            if (TranslateMessages.Any(c => c.Code == code && c.LanguageCode == Session.Entities.GetSetting().LanguageCode))
            {
                return TranslateMessages.First(c => c.Code == code && c.LanguageCode == Session.Entities.GetSetting().LanguageCode);
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

        public List<Stock> GetStocks()
        {
            return Stocks;
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
                    AccountId = Session.DefaultAccount.AccountId
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
                throw new Exception(Translate.GetMessage("this-username-is-used"));

            if (!isNew && string.IsNullOrEmpty(currentPasswordHash))
                throw new Exception(Translate.GetMessage("current-password-is-empty"));

            if (!isNew && item == null)
                throw new Exception(Translate.GetMessage("username-is-not-found"));

            if (!isNew && item.Password != currentPasswordHash)
                throw new Exception(Translate.GetMessage("current-password-is-incorrect"));

            foreach (var account in user.Accounts)
                Session.Entities.PostAccount(account);

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
            var userAccount = Session.User.Accounts.FirstOrDefault(c => c.AccountId == account.AccountId);
            if (userAccount != null)
                userAccount.DefaultAccount = account.DefaultAccount;
            if (account.DefaultAccount)
                foreach (var acc in Session.User.Accounts.Where(c => c.AccountId != account.AccountId))
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
                Session.User.Accounts.Add(account);
                Accounts.Add(account);
                getUserAccounts();
                return account;
            }
        }

        public Period PostPeriod(Period period)
        {
            period.AccountId = Session.DefaultAccount.AccountId;
            var item = Periods.FirstOrDefault(c => c.PeriodId == period.PeriodId);
            if (item != null)
            {
                item = period;
                return item;
            }
            else
            {
                period.PeriodId = GeneratePeriodId();
                Periods.Add(period);
                return period;
            }
        }

        public TranslateMessage PostMessage(TranslateMessage message)
        {
            var item = TranslateMessages.FirstOrDefault(c => c.Code == message.Code && c.LanguageCode == message.LanguageCode);
            if (item != null)
            {
                item = message;
                return item;
            }
            else
            {
                if (Session.Entities.GetTranslateMessages().Any(c => c.Code == message.Code && c.LanguageCode == message.LanguageCode))
                    throw new Exception(Translate.GetMessage("dublicate-message"));
                TranslateMessages.Add(message);
                return message;
            }
        }

        private void getUserAccounts()
        {
            Session.User.Accounts = (from a in Accounts
                                     join ua in Session.User.Accounts on a.AccountId equals ua.AccountId
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

        public void DeletePeriod(int periodId)
        {
            var period = GetPeriod(periodId);
            if (period != null)
                Periods.Remove(period);
        }

        public void DeleteMessage(string code)
        {
            var message = GetMessage(code);
            if (message != null)
                TranslateMessages.Remove(message);
        }

        public void DeleteAccount(int accountId)
        {
            var account = GetAccount(accountId);
            if (account != null)
                Accounts.Remove(account);
        }

        public void Save()
        {
            foreach (var account in Accounts)
                account.TotalAmount = AccountTransactions.Where(c => c.AccountId == account.AccountId).Sum(c => c.Amount);
            write();
        }

        public StockCurrent GetStockService(string stockCode)
        {
            stockCode = stockCode.ToUpper();
            var stock = GetStock(stockCode);
            if (CurrentStocks.Any(c => c.StockCode == stockCode && ((c.CreatedDate.AddMinutes(30) > DateTime.Now) || c.CreatedDate >= DateTime.Now.SmallDate().AddHours(18).AddMinutes(10))))
                return CurrentStocks.OrderByDescending(c => c.CreatedDate).FirstOrDefault(c => c.StockCode == stockCode);
            dynamic stockService = getStock(stockCode);
            if (stockService == null) return null;
            var result = new StockCurrent()
            {
                StockCode = stockCode,
                Price = stockService.data.hisseYuzeysel.alis,
                StockName = stockService.data.hisseYuzeysel.aciklama,
                UpdateDate = stockService.data.hisseYuzeysel.tarih,
                CreatedDate = DateTime.Now
            };
            stock.Name = result.StockName;
            stock.Value = result.Price;
            stock.UpdateDate = result.UpdateDate;
            CurrentStocks.Add(result);
            Save();
            return result;
        }

        private dynamic getStock(string stockCode)
        {
            return null;
            try
            {
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
                        sw.Write(JsonConvert.SerializeObject(from a in Accounts select new { a.AccountId, a.AccountName, a.MoneyType, a.TotalAmount, a.DefaultAccount, }));
                }
                using (StreamWriter sw = new StreamWriter("Data/CurrentStocks.json"))
                {
                    if (CurrentStocks == null || CurrentStocks.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from a in CurrentStocks select new { a.StockCode, a.StockName, a.Price, a.UpdateDate, a.CreatedDate }));
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
                        sw.Write(JsonConvert.SerializeObject(from u in Users select new User { UserName = u.UserName, LanguageCode = u.LanguageCode, Password = u.Password, IsActive = u.IsActive, Accounts = u.Accounts.Select(c => new Account { AccountId = c.AccountId, DefaultAccount = c.DefaultAccount }).ToList() }));
                }
                using (StreamWriter sw = new StreamWriter("Data/TranslateMessages.json"))
                {
                    if (TranslateMessages == null || TranslateMessages.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject(from u in TranslateMessages select new TranslateMessage { Code = u.Code, LanguageCode = u.LanguageCode, Value = u.Value }));
                }
                using (StreamWriter sw = new StreamWriter("Data/Periods.json"))
                {
                    if (Periods == null || Periods.Count == 0)
                        sw.Write("[]");
                    else
                        sw.Write(JsonConvert.SerializeObject((from u in Periods select new Period { PeriodId = u.PeriodId, PeriodName = u.PeriodName, StartDate = u.StartDate, EndDate = u.EndDate, AccountId = u.AccountId, IsPublic = u.IsPublic }).ToList()));
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
                using (StreamReader sw = new StreamReader("Data/CurrentStocks.json"))
                {
                    CurrentStocks = JsonConvert.DeserializeObject<List<StockCurrent>>(sw.ReadToEnd());
                    if (CurrentStocks == null)
                        CurrentStocks = new List<StockCurrent>();
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
                using (StreamReader sw = new StreamReader("Data/TranslateMessages.json"))
                {
                    TranslateMessages = JsonConvert.DeserializeObject<List<TranslateMessage>>(sw.ReadToEnd());
                    if (TranslateMessages == null)
                        TranslateMessages = new List<TranslateMessage>();
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
                using (StreamReader sw = new StreamReader("Data/Periods.json"))
                {
                    Periods = JsonConvert.DeserializeObject<List<Period>>(sw.ReadToEnd());
                    if (Periods == null)
                        Periods = new List<Period>();
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

            if (!File.Exists("Data/CurrentStocks.json"))
            {
                using (var file = File.Create("Data/CurrentStocks.json"))
                {
                    save = true;
                    CurrentStocks = new List<StockCurrent>();
                    file.Close();
                }
            }

            if (!File.Exists("Data/Periods.json"))
            {
                using (var file = File.Create("Data/Periods.json"))
                {
                    save = true;
                    Periods = new List<Period>();
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

            if (!File.Exists("Data/TranslateMessages.json"))
            {
                using (var file = File.Create("Data/TranslateMessages.json"))
                {
                    save = true;
                    TranslateMessages = new List<TranslateMessage>();
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

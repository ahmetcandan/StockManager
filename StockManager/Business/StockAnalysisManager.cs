using StockManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockManager.Business
{
    public class StockAnalysisManager
    {
        public StockAnalysisManager(StockAnalysisRequest request)
        {
            this.request = request;
        }

        public List<StockAnalysis> StockAnalyses { get; set; }
        private StockAnalysisRequest request;

        public decimal TotalGain { get; set; }
        public decimal TotalConst { get; set; }
        public decimal AvailableValue { get; set; }
        public decimal ExpectedGain { get; set; }

        public List<StockAnalysis> RefreshList()
        {
            List<Stock> stocks = new List<Stock>();
            if (request.StockCodes.Count == 0)
                stocks = Session.Entities.GetStocks();
            else
                foreach (var stockCode in request.StockCodes)
                {
                    var stock = Session.Entities.GetStock(stockCode);
                    if (stock != null) stocks.Add(stock);
                }
            StockAnalyses = new List<StockAnalysis>();
            StockAnalysis stockAnalysis = new StockAnalysis();
            List<StockTransaction> stockTransactions = new List<StockTransaction>();
            stockTransactions = Session.Entities.GetStockTransactions().Where(c => c.Date >= request.Period.StartDate && c.Date <= request.Period.EndDate && stocks.Select(s => s.StockCode).Contains(c.StockCode)).ToList();

            #region Last Period Stock Transaction
            {
                var lastPeriodStockTransaciton = from st in Session.Entities.GetStockTransactions().DeepCopy()
                                                 where st.Date < request.Period.StartDate
                                                    && stocks.Select(c => c.StockCode).Contains(st.StockCode)
                                                 group st by st.StockCode into StockTransaction
                                                 select new { StockCode = StockTransaction.Key, StockTransaction };

                foreach (var stock in lastPeriodStockTransaciton)
                {
                    StockTransaction previousTransaction = null;
                    foreach (var stockTransaction in stock.StockTransaction)
                    {
                        if (previousTransaction != null && previousTransaction.TransactionType == stockTransaction.TransactionType)
                        {
                            stockTransaction.Amount += previousTransaction.Amount;
                            stockTransaction.TotalPrice += previousTransaction.TotalPrice;
                            previousTransaction.Amount = 0;
                            previousTransaction.TotalPrice = 0;
                            previousTransaction.UnitPrice = 0;
                            stockTransaction.UnitPrice = stockTransaction.TotalPrice / stockTransaction.Amount;
                        }
                        previousTransaction = stockTransaction;
                    }
                }

                foreach (var stock in lastPeriodStockTransaciton)
                {
                    if (stock.StockTransaction.Where(c => c.Amount > 0).Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1)) > 0)
                    {
                        foreach (var stockTransaction in stock.StockTransaction.OrderBy(c => c.Date).Where(c => c.TransactionType == TransactionType.Sell && c.Amount > 0))
                        {
                            decimal sellAmount = stockTransaction.Amount;

                            while (sellAmount > 0)
                            {
                                var buyTransaction = stock.StockTransaction.OrderBy(c => c.Date).FirstOrDefault(c => c.TransactionType == TransactionType.Buy && c.Amount > 0);
                                if (sellAmount > buyTransaction.Amount)
                                {
                                    sellAmount -= buyTransaction.Amount;
                                    buyTransaction.Amount = 0;
                                }
                                else
                                {
                                    buyTransaction.Amount -= sellAmount;
                                    sellAmount = 0;
                                }
                            }
                        }

                        decimal sumBuyAmount = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Sum(c => c.Amount);
                        decimal sumBuyPrice = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Sum(c => c.UnitPrice * c.Amount);
                        DateTime buyDate = stock.StockTransaction.Where(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).Min(c => c.Date);
                        int stockTransactionId = stock.StockTransaction.OrderBy(c => c.Date).FirstOrDefault(c => c.TransactionType == TransactionType.Buy && c.Amount > 0).StockTransactionId;
                        stockTransactions.Add(new StockTransaction
                        {
                            Amount = sumBuyAmount,
                            UnitPrice = sumBuyPrice / sumBuyAmount,
                            TotalPrice = sumBuyPrice,
                            StockCode = stock.StockCode,
                            Date = buyDate,
                            TransactionType = TransactionType.Buy,
                            StockTransactionId = stockTransactionId,
                        });
                    }
                }
            }
            #endregion

            #region Process
            var result = from st in stockTransactions
                         join s in Session.Entities.GetStocks() on st.StockCode equals s.StockCode
                         join s1 in stocks.Distinct() on s.StockCode equals s1.StockCode
                         where s1 != null
                         orderby st.Date
                         group st by s into StockTransactions
                         select new { Stock = StockTransactions.Key, StockTransactions };

            foreach (var item in result.Where(c => c.StockTransactions.Sum(t => t.Amount * (t.TransactionType == TransactionType.Sell ? -1 : 1)) > 0))
                Session.Entities.GetStockService(item.Stock.StockCode);

            foreach (var stock in result)
            {
                stockAnalysis = new StockAnalysis();
                foreach (var stockTransaction in stock.StockTransactions)
                {
                    stockAnalysis.StockCode = stock.Stock.StockCode;
                    stockAnalysis.StockTransactions.Add(new StockAnalysisTransaction
                    {
                        Amount = stockTransaction.Amount,
                        Date = stockTransaction.Date,
                        StockTransactionId = stockTransaction.StockTransactionId,
                        TransactionType = stockTransaction.TransactionType,
                        UnitPrice = stockTransaction.UnitPrice
                    });

                    if (stockAnalysis.TotalAmount == 0)
                    {
                        StockAnalyses.Add(stockAnalysis);
                        stockAnalysis = new StockAnalysis();
                    }
                }

                if (stockAnalysis.StockTransactions.Count > 0)
                {
                    var partialStockAnalysis = new StockAnalysis();
                    partialStockAnalysis.StockCode = stockAnalysis.StockCode;
                    decimal diffarenceAmount = stockAnalysis.StockTransactions.Sum(c => c.Amount * (c.TransactionType == TransactionType.Sell ? -1 : 1));
                    bool isPartial = diffarenceAmount > 0 && stockAnalysis.StockTransactions.Any(c => c.TransactionType == TransactionType.Sell);
                    if (isPartial)
                    {
                        foreach (var stockTransaction in stockAnalysis.StockTransactions.Where(c => c.TransactionType == TransactionType.Buy).OrderByDescending(c => c.Date))
                        {
                            var partialStockTransaction = new StockAnalysisTransaction()
                            {
                                Amount = 0,
                                Date = stockTransaction.Date,
                                StockTransactionId = stockTransaction.StockTransactionId,
                                TransactionType = stockTransaction.TransactionType,
                                UnitPrice = stockTransaction.UnitPrice
                            };
                            if (stockTransaction.Amount >= diffarenceAmount)
                            {
                                stockTransaction.Amount -= diffarenceAmount;
                                partialStockTransaction.Amount = diffarenceAmount;
                                partialStockAnalysis.StockTransactions.Add(partialStockTransaction);
                                break;
                            }
                            else
                            {
                                diffarenceAmount -= stockTransaction.Amount;
                                partialStockTransaction.Amount = stockTransaction.Amount;
                                partialStockAnalysis.StockTransactions.Add(partialStockTransaction);
                                stockTransaction.Amount = 0;
                            }
                        }
                        stockAnalysis.StockTransactions.RemoveAll(c => c.Amount == 0);
                    }

                    StockAnalyses.Add(stockAnalysis);
                    if (isPartial) StockAnalyses.Add(partialStockAnalysis);
                }
            }
            #endregion

            calculate();

            return StockAnalyses.OrderBy(c => c.LastTransactionDate).ToList();
        }

        private void calculate()
        {
            decimal totalValue = 0;
            decimal totalGain = 0;
            decimal expectedGain = 0;
            foreach (var item in StockAnalyses.OrderBy(c => c.LastTransactionDate))
            {
                decimal? curValue = null;
                if (Session.Entities.GetCurrentStocks().Any(c => c.StockCode == item.StockCode && c.UpdateDate <= request.Period.EndDate))
                    curValue = Session.Entities.GetCurrentStocks().Where(c => c.UpdateDate <= request.Period.EndDate).OrderByDescending(c => c.UpdateDate).FirstOrDefault(c => c.StockCode == item.StockCode).Price;
                decimal value = item.TotalAmount == 0 ? (item.TotalSellAmount * item.SellPrice - item.TotalConst) : (item.TotalBuyAmount * (curValue.HasValue ? curValue.Value : item.BuyPrice));
                if (item.TotalAmount > 0)
                    totalValue += value;

                decimal gain = item.TotalAmount == 0 ? (item.TotalValue - item.TotalConst) : (curValue.HasValue ? (item.TotalBuyAmount * curValue.Value - item.TotalBuyAmount * item.BuyPrice - item.TotalConst) : 0);

                if (item.TotalAmount > 0)
                    expectedGain += gain;
                else
                    totalGain += gain;
            }
            TotalGain = totalGain;
            AvailableValue = totalValue;
            TotalConst = StockAnalyses.Sum(c => c.StockTransactions.Where(d => d.Date >= request.Period.StartDate && d.Date <= request.Period.EndDate).Sum(d => d.Const));
            ExpectedGain = expectedGain;
        }
    }
}

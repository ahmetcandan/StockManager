using System;

namespace StockManager
{
    public class Period
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public int PeriodId { get; set; }
        public string PeriodName { get; set; }
        public DateTime StartDate
        {
            get
            {
                return _startDate.DayStart();
            }
            set
            {
                _startDate = value;
            }
        }
        public DateTime EndDate
        {
            get
            {
                return _endDate.DayEnd();
            }
            set
            {
                _endDate = value;
            }
        }
        public int AccountId { get; set; }
        public bool IsPublic { get; set; }
    }
}

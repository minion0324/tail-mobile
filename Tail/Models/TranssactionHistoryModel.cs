using System;
namespace Tail.Models
{
    public class TranssactionHistoryModel : BaseModel
    {
        private string _dateString;
        public string Title { get; set;}
        public DateTime Date {
            get;
            set;
        }
        public string DateString { get=>_dateString;
            set
            {
                DateTime dateTime = DateTime.Parse(value);
                var dateString = dateTime.ToString("MMM dd, yyyy");
                SetProperty(ref _dateString, dateString);
            }
        }
        public int Coins { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsRefunded { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace prototype.Models
{
    public class TradeStreamData : ReactiveObject
    {
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }
    }
}

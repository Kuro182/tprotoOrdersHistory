using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace prototype.Models
{
    public class Order : ReactiveObject
    {
        private long _id;
        public long Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set => this.RaiseAndSetIfChanged(ref _symbol, value);
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }

        private decimal _originalQuantity;
        public decimal OriginalQuantity
        {
            get => _originalQuantity;
            set => this.RaiseAndSetIfChanged(ref _originalQuantity, value);
        }

        private decimal _executedQuantity;
        public decimal ExecutedQuantity
        {
            get => _executedQuantity;
            set => this.RaiseAndSetIfChanged(ref _executedQuantity, value);
                //RaisePropertyChangedEvent("Fullfilled");
        }

        public string FullFilled => ExecutedQuantity + "/" + OriginalQuantity;

        private OrderStatus status;
        public OrderStatus Status
        {
            get => status;
            set
            {
                status = value;
                RaisePropertyChangedEvent("Status");
                RaisePropertyChangedEvent("CanCancel");
            }
        }

        private OrderSide _side;
        public OrderSide Side
        {
            get { return _side; }
            set => this.RaiseAndSetIfChanged(ref _side, value);
        }

        private OrderType _type;
        public OrderType Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }

        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }

        public bool CanCancel => Status == OrderStatus.New || Status == OrderStatus.PartiallyFilled;
    }
}

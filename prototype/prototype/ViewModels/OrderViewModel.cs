using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Objects;
using ReactiveUI;

namespace prototype.ViewModels
{
    public class OrderViewModel : ReactiveObject
    {
        #region Order
        private long _id;
        public long Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }
        #endregion

        #region Symbol
        private string _symbol;
        public string Symbol
        {
            get => _symbol;
            set => this.RaiseAndSetIfChanged(ref _symbol, value);
        }
        #endregion

        #region Price
        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
        }
        #endregion

        #region OriginalQuantity
        private decimal _originalQuantity;
        public decimal OriginalQuantity
        {
            get => _originalQuantity;
            set => this.RaiseAndSetIfChanged(ref _originalQuantity, value);
        }
        #endregion

        #region ExecutedQuantity
        private decimal _executedQuantity;
        public decimal ExecutedQuantity
        {
            get => _executedQuantity;
            set
            {
                this.RaiseAndSetIfChanged(ref _executedQuantity, value);
                this.RaisePropertyChanged("ExecutedQuantity");
                this.RaisePropertyChanged("Fullfilled");
            }
        }
        #endregion

        #region FullFilled
        public string FullFilled => ExecutedQuantity + "/" + OriginalQuantity;
        #endregion

        #region OrderStatus
        private OrderStatus _status;
        public OrderStatus Status
        {
            get => _status;
            set
            {
                this.RaiseAndSetIfChanged(ref _status, value);
                this.RaisePropertyChanged("Status");
                this.RaisePropertyChanged("CanCancel");
            }
        }
        #endregion

        #region OrderSide
        private OrderSide _side;
        public OrderSide Side
        {
            get => _side;
            set => this.RaiseAndSetIfChanged(ref _side, value);
        }
        #endregion

        #region OrderType
        private OrderType _type;
        public OrderType Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }
        #endregion

        #region Time
        private DateTime _time;
        public DateTime Time
        {
            get => _time;
            set => this.RaiseAndSetIfChanged(ref _time, value);
        }
        #endregion

        public bool CanCancel => Status == OrderStatus.New || Status == OrderStatus.PartiallyFilled;
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace prototype.ViewModels
{
    public class BinanceSymbolViewModel : ReactiveObject
    {

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

        #region PriceChangePercent
        private decimal _priceChangePercent;
        public decimal PriceChangePercent
        {
            get => _priceChangePercent;
            set => this.RaiseAndSetIfChanged(ref _priceChangePercent, value);
        }
        #endregion

        #region HighPrice
        private decimal _highPrice;
        public decimal HighPrice
        {
            get => _highPrice;
            set => this.RaiseAndSetIfChanged(ref _highPrice, value);
        }
        #endregion

        #region LowPrice
        private decimal _lowPrice;
        public decimal LowPrice
        {
            get => _lowPrice;
            set => this.RaiseAndSetIfChanged(ref _lowPrice, value);
        }
        #endregion

        #region Volume
        private decimal _volume;
        public decimal Volume
        {
            get => _volume;
            set => this.RaiseAndSetIfChanged(ref _volume, value);
        }
        #endregion

        #region TradeAmount
        private decimal _tradeAmount;
        public decimal TradeAmount
        {
            get => _tradeAmount;
            set => this.RaiseAndSetIfChanged(ref _tradeAmount, value);
        }
        #endregion

        #region TradePrice
        private decimal _tradePrice;
        public decimal TradePrice
        {
            get => _tradePrice;
            set => this.RaiseAndSetIfChanged(ref _tradePrice, value);
        }
        #endregion

        #region Orders Collection
        private ObservableCollection<OrderViewModel> _orders;
        public ObservableCollection<OrderViewModel> Orders
        {
            get => _orders;
            set => this.RaiseAndSetIfChanged(ref _orders, value);
        }
        #endregion

        #region .ctor
        public BinanceSymbolViewModel(string symbol, decimal price)
        {
            this._symbol = symbol;
            this._price = price;
        }

        public BinanceSymbolViewModel(string symbol, decimal price, decimal highprice, decimal lowprice, decimal tradeamount, decimal tradeprice)
        {
            this._symbol = symbol;
            this._price = price;
            _tradeAmount = tradeamount;
            _tradePrice = tradeprice;
            _highPrice = highprice;
            _lowPrice = lowprice;
        }
        #endregion

        #region MyRegion
        public void AddOrder(OrderViewModel order)
        {
            Orders.Add(order);
            Orders.OrderByDescending(o => o.Time);
            this.RaisePropertyChanged("Orders");
        }
        #endregion
    }
}

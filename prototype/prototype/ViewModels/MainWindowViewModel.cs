using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using prototype.Models;
using Telerik.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;

namespace prototype.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region private fields

        private BinanceSocketClient _socketClient;
        private const string _selectedSymbolStatic = "BNBBTC";
        private object orderLock;

        #endregion

        #region ApiKey

        private string _apiKey = "qDalyQCEJgWW62Sc1LDJfltLycgoyj4ow1ak9wo4DWUKwWo0IRf0x82bYXr7MQbG";
        public string ApiKey
        {
            get => _apiKey;
            set => this.RaiseAndSetIfChanged(ref _apiKey, value);
            //if (value != null && apiSecret != null)
            //    BinanceDefaults.SetDefaultApiCredentials();
        }

        #endregion

        #region ApiSecret

        private string _apiSecret = "RgnfcrMpvcffhz8m7BNjrK4X2tv41S90nddThx7F9Dnb0EpKOStJuxxCmVXpUZyt";
        public string ApiSecret
        {
            get => _apiSecret;
            set => this.RaiseAndSetIfChanged(ref _apiSecret, value);

            //if (value != null && apiKey != null)
            //    BinanceDefaults.SetDefaultApiCredentials(apiKey, value);
        }

        #endregion

        #region SelectedSymbolName

        private string _selectedSymbolName;

        public string SelectedSymbolName
        {
            get => _selectedSymbolName;
            set => this.RaiseAndSetIfChanged(ref _selectedSymbolName, value);
        }

        #endregion

        #region SelectedSymbol24High

        private decimal _selectedSymbol24High;

        public decimal SelectedSymbol24High
        {
            get => _selectedSymbol24High;
            set => this.RaiseAndSetIfChanged(ref _selectedSymbol24High, value);
        }

        #endregion

        #region SelectedSymbol24Low

        private decimal _selectedSymbol24Low;

        public decimal SelectedSymbol24Low
        {
            get => _selectedSymbol24Low;
            set => this.RaiseAndSetIfChanged(ref _selectedSymbol24Low, value);
        }

        #endregion

        #region DepositPercentOnBuying

        private string _depositPercentOnBuying;

        public string DepositPercentOnBuying
        {
            get => _depositPercentOnBuying;
            set => this.RaiseAndSetIfChanged(ref _depositPercentOnBuying, value);
        }

        #endregion

        #region BuySellCommand

        private DelegateCommand _buySellCommand;
        /// <summary>
        /// Buy or Sell commands
        /// </summary>
        public DelegateCommand BuySellCommand => _buySellCommand ??
                       (_buySellCommand = new DelegateCommand(obj =>
                       {
                           switch (obj)
                           {
                               //case "ButtonFastForwardBack":
                               //    ShiftTime(Enums.ShiftTime.Back1H);
                               //    break;
                               //case "ButtonNextTrackBack":
                               //    ShiftTime(Enums.ShiftTime.Back10M);
                               //    break;
                           }
                       },
                           (obj) =>
                           {
                               return true;
                           }
                       ));
        #endregion

        #region BuySellCommand

        private DelegateCommand _closeApp;
        /// <summary>
        /// Close application
        /// </summary>
        public DelegateCommand CloseApp => _closeApp ??
                       (_closeApp = new DelegateCommand(obj =>
                       {
                           Application.Current.Shutdown();
                       },
                           (obj) =>
                           {
                               return true;
                           }
                       ));
        #endregion

        #region CancelBtnCommand

        private DelegateCommand _cancelBtnCommand;
        /// <summary>
        /// Cancel All Cancel Limit Cancel StopLimit commands
        /// </summary>
        public DelegateCommand CancelBtnCommand => _cancelBtnCommand ??
                       (_cancelBtnCommand = new DelegateCommand(obj =>
                       {
                           switch (obj)
                           {
                               //case "ButtonFastForwardBack":
                               //    ShiftTime(Enums.ShiftTime.Back1H);
                               //    break;
                               //case "ButtonNextTrackBack":
                               //    ShiftTime(Enums.ShiftTime.Back10M);
                               //    break;
                               //case "ButtonPlayArrowBack":
                               //    ShiftTime(Enums.ShiftTime.Back3M);
                               //    break;
                           }
                       },
                           (obj) =>
                           {
                               return true;
                           }
                       ));
        #endregion

        #region SetTimeRange

        private DelegateCommand _setTimeRange;
        /// <summary>
        /// Set time range for charts
        /// </summary>
        public DelegateCommand SetTimeRange => _setTimeRange ??
                       (_setTimeRange = new DelegateCommand(obj =>
                           {
                               Int32.TryParse((string)obj, out int valueToSet);

                               switch (valueToSet)
                               {
                                   case 1:
                                       GettingKlines(KlineInterval.OneMinute, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-1));
                                       break;
                                   case 60:
                                       GettingKlines(KlineInterval.OneMinute, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-1));
                                       break;
                                   case 300:
                                       GettingKlines(KlineInterval.FiveMinutes, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-5));
                                       break;
                                   case 900:
                                       GettingKlines(KlineInterval.FifteenMinutes, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-15));
                                       break;
                                   case 1800:
                                       GettingKlines(KlineInterval.ThirtyMinutes, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(-30));
                                       break;
                                   case 3600:
                                       GettingKlines(KlineInterval.OneHour, DateTime.UtcNow, DateTime.UtcNow.AddHours(-1));
                                       break;
                                   case 7200:
                                       GettingKlines(KlineInterval.TwoHour, DateTime.UtcNow, DateTime.UtcNow.AddHours(-2));
                                       break;
                                   case 14400:
                                       GettingKlines(KlineInterval.FourHour, DateTime.UtcNow, DateTime.UtcNow.AddHours(-4));
                                       break;
                                   case 21600:
                                       GettingKlines(KlineInterval.SixHour, DateTime.UtcNow, DateTime.UtcNow.AddHours(-6));
                                       break;
                                   case 43200:
                                       GettingKlines(KlineInterval.TwelveHour, DateTime.UtcNow, DateTime.UtcNow.AddHours(-12));
                                       break;
                                   case 86400:
                                       GettingKlines(KlineInterval.OneDay, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1));
                                       break;
                                   case 604800:
                                       GettingKlines(KlineInterval.OneWeek, DateTime.UtcNow, DateTime.UtcNow.AddDays(-7));
                                       break;
                                   case 2592000:
                                       GettingKlines(KlineInterval.OneMonth, DateTime.UtcNow, DateTime.UtcNow.AddDays(-30));
                                       break;
                               }
                           },
                           (obj) =>
                           {
                               return true;
                           }
                       ));
        #endregion

        #region SortCoinlist

        private DelegateCommand _sortCoinlist;
        /// <summary>
        /// Sorting coin list
        /// </summary>
        public DelegateCommand SortCoinlist => _sortCoinlist ??
                       (_sortCoinlist = new DelegateCommand(obj =>
                       {
                           switch (obj)
                           {
                               //case "ButtonFastForwardBack":
                               //    ShiftTime(Enums.ShiftTime.Back1H);
                               //    break;
                               //case "ButtonNextTrackBack":
                               //    ShiftTime(Enums.ShiftTime.Back10M);
                               //    break;
                               //case "ButtonPlayArrowBack":
                               //    ShiftTime(Enums.ShiftTime.Back3M);
                               //    break;
                           }
                       },
                           (obj) =>
                           {
                               return true;
                           }
                       ));
        #endregion

        #region SearchCoinBoxValue

        private string _searchCoinBoxValue;

        public string SearchCoinBoxValue
        {
            get => _searchCoinBoxValue;
            set => this.RaiseAndSetIfChanged(ref _searchCoinBoxValue, value);
        }

        #endregion

        #region SelectedSymbol

        private BinanceSymbolViewModel _selectedSymbol;

        public BinanceSymbolViewModel SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedSymbol, value);
                SelectedSymbolName = _selectedSymbol.Symbol;
                ChangeSymbol();
                GettingKlines(KlineInterval.OneMinute, DateTime.UtcNow, DateTime.UtcNow.AddHours(-1));

            }
        }

        #endregion

        #region Assets

        private ObservableCollection<AssetViewModel> assets;
        public ObservableCollection<AssetViewModel> Assets
        {
            get { return assets; }
            set => this.RaiseAndSetIfChanged(ref assets, value);
        }

        #endregion

        #region AllPrices

        private ObservableCollectionExtended<BinanceSymbolViewModel> _allPrices;
        public ObservableCollectionExtended<BinanceSymbolViewModel> AllPrices
        {
            get => _allPrices;
            set => this.RaiseAndSetIfChanged(ref _allPrices, value);
        }

        #endregion

        #region Klines

        private ObservableCollectionExtended<BinanceKline> _klines;
        public ObservableCollectionExtended<BinanceKline> Klines
        {
            get => _klines;
            set => this.RaiseAndSetIfChanged(ref _klines, value);
        }

        #endregion

        #region StreamData

        private readonly SourceList<BinanceStreamTick> _tickData = new SourceList<BinanceStreamTick>();

        public IObservableCollection<BinanceStreamTick> TickData { get; } =
            new ObservableCollectionExtended<BinanceStreamTick>();

        #endregion

        #region ctor
        public MainWindowViewModel()
        {
            orderLock = new object();

            _tickData
                    .Connect()
                    .ObserveOn(RxApp.TaskpoolScheduler)
                    .Bind(TickData)
                    .Subscribe();


            Task.Run(() => GetAllSymbols());
        }

        #endregion

        private void Get24HourStats()
        {
            using (var client = new BinanceClient())
            {
                var result = client.Get24HPrice(SelectedSymbol.Symbol);
                if (result.Success)
                {
                    SelectedSymbol.HighPrice = result.Data.HighPrice;
                    SelectedSymbol.LowPrice = result.Data.LowPrice;
                    SelectedSymbol.Volume = result.Data.Volume;
                    SelectedSymbol.PriceChangePercent = result.Data.PriceChangePercent;
                }
                //else
                //messageBoxService.ShowMessage($"Error requesting data: {result.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task GetAllSymbols()
        {
            using (var client = new BinanceClient())
            {

                var result = await client.GetAllPricesAsync();
                if (result.Success)
                {
                    AllPrices = new ObservableCollectionExtended<BinanceSymbolViewModel>(result.Data.Select(r => new BinanceSymbolViewModel(r.Symbol, r.Price)).ToList());

                }

                //TODO убрать
                //GettingKlines(DateTime.UtcNow, DateTime.UtcNow.AddHours(-1));
            }

            _socketClient = new BinanceSocketClient();
            _socketClient.SubscribeToAllSymbolTicker(data =>
            {
                foreach (var ud in data)
                {
                    var symbol = AllPrices.SingleOrDefault(p => p.Symbol == ud.Symbol);
                    if (symbol == null)
                        return;

                    symbol.Price = ud.CurrentDayClosePrice;
                    symbol.PriceChangePercent = ud.PriceChangePercentage;
                }
            });

            _socketClient.SubscribeToSymbolTicker("bnbbtc", (data) =>
            {
                if (_tickData.Count > 3600)
                {
                    _tickData.RemoveRange(0, 1);
                }

                _tickData.Add(data);
            });

            //_socketClient.SubscribeToTradesStream("bnbbtc", (data) =>
            //{

            //    SelectedSymbol.Orders = new ObservableCollection<OrderViewModel>(result.Data.OrderByDescending(d => d.Time).Select(o => new OrderViewModel()
            //    {
            //        Id = o.OrderId,
            //        ExecutedQuantity = o.ExecutedQuantity,
            //        OriginalQuantity = o.OriginalQuantity,
            //        Price = o.Price,
            //        Side = o.Side,
            //        Status = o.Status,
            //        Symbol = o.Symbol,
            //        Time = o.Time,
            //        Type = o.Type
            //    }));
            //});
        }

        private void GettingKlines(KlineInterval klineInterval, DateTime startTime, DateTime endTime)
        {
            using (var client = new BinanceClient())
            {
                if (SelectedSymbol == null || SelectedSymbol.Symbol == null || SelectedSymbol.Symbol == string.Empty)
                    return;

                var re = client.GetKlines(SelectedSymbol.Symbol, klineInterval/*, startTime, endTime*/);
                if (re.Success)
                {
                    Klines = new ObservableCollectionExtended<BinanceKline>(re.Data.Select(r => new BinanceKline()
                    {
                        High = r.High,
                        Low = r.Low,
                        Open = r.Open,
                        Close = r.Close,
                        CloseTime = r.CloseTime,
                    }).ToList());
                }
            }
        }

        private void GetOrders()
        {
            using (var client = new BinanceClient())
            {
                var result = client.GetAllOrders(SelectedSymbol.Symbol);
                if (result.Success)
                {
                    SelectedSymbol.Orders = new ObservableCollection<OrderViewModel>(result.Data.OrderByDescending(d => d.Time).Select(o => new OrderViewModel()
                    {
                        Id = o.OrderId,
                        ExecutedQuantity = o.ExecutedQuantity,
                        OriginalQuantity = o.OriginalQuantity,
                        Price = o.Price,
                        Side = o.Side,
                        Status = o.Status,
                        Symbol = o.Symbol,
                        Time = o.Time,
                        Type = o.Type
                    }));
                }
                else
                    Debug.WriteLine($"Error requesting data: {result.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SubscribeUserStream()
        {
            if (ApiKey == null || ApiSecret == null)
                return;

            Task.Run(() =>
            {
                using (var client = new BinanceClient())
                {
                    var startOkay = client.StartUserStream();
                    if (!startOkay.Success)
                    {
                        Debug.WriteLine($"Error starting user stream: {startOkay.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    //TODO Посмотреть два последних аргумента в методе
                    var subOkay = _socketClient.SubscribeToUserStream(startOkay.Data, OnAccountUpdate, OnOrderUpdate, null, null);
                    if (!subOkay.Success)
                    {
                        Debug.WriteLine($"Error subscribing to user stream: {subOkay.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var accountResult = client.GetAccountInfo();
                    if (accountResult.Success)
                        Assets = new ObservableCollection<AssetViewModel>(accountResult.Data.Balances.Where(b => b.Free != 0 || b.Locked != 0)
                            .Select(b => new AssetViewModel() { Asset = b.Asset, Free = b.Free, Locked = b.Locked }).ToList());
                    else
                        Debug.WriteLine($"Error requesting account info: {accountResult.Error.Message}", "error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void ChangeSymbol()
        {
            if (SelectedSymbol != null)
            {
                _selectedSymbol.TradeAmount = 0;
                _selectedSymbol.TradePrice = _selectedSymbol.Price;
                Task.Run(() =>
                {
                    GetOrders();
                    Get24HourStats();
                });
            }
        }

        private void OnAccountUpdate(BinanceStreamAccountInfo data)
        {
            Assets = new ObservableCollection<AssetViewModel>(data.Balances.Where(b => b.Free != 0 || b.Locked != 0).Select(b => new AssetViewModel() { Asset = b.Asset, Free = b.Free, Locked = b.Locked }).ToList());
        }

        private void OnOrderUpdate(BinanceStreamOrderUpdate data)
        {
            var symbol = AllPrices.SingleOrDefault(a => a.Symbol == data.Symbol);
            if (symbol == null)
                return;

            lock (orderLock)
            {
                var order = symbol.Orders.SingleOrDefault(o => o.Id == data.OrderId);
                if (order == null)
                {
                    if (data.RejectReason != OrderRejectReason.None || data.ExecutionType != ExecutionType.New)
                        // Order got rejected, no need to show
                        return;

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        symbol.AddOrder(new OrderViewModel()
                        {
                            ExecutedQuantity = data.AccumulatedQuantityOfFilledTrades,
                            Id = data.OrderId,
                            OriginalQuantity = data.Quantity,
                            Price = data.Price,
                            Side = data.Side,
                            Status = data.Status,
                            Symbol = data.Symbol,
                            Time = data.Time,
                            Type = data.Type
                        });
                    });
                }
                else
                {
                    order.ExecutedQuantity = data.AccumulatedQuantityOfFilledTrades;
                    order.Status = data.Status;
                }
            }
        }
    }
}

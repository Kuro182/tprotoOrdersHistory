using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphTest.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {

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

        #region MainCollection

        private readonly SourceList<TrendViewModel> _mainCollection = new SourceList<TrendViewModel>();

        public IObservableCollection<TrendViewModel> MainCollection { get; } =
            new ObservableCollectionExtended<TrendViewModel>();

        #endregion

        #region OrdersData

        private readonly SourceList<BinanceStreamTick> _ordersData = new SourceList<BinanceStreamTick>();

        public IObservableCollection<BinanceStreamTick> OrdersData { get; } =
            new ObservableCollectionExtended<BinanceStreamTick>();

        #endregion

        public MainWindowViewModel()
        {
            _mainCollection
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(MainCollection)
                .Subscribe();

            for (int k = 0; k < 3; k++)
            {
                _mainCollection.Add(new TrendViewModel());
            }

            int i = 0;

            foreach (var item in _mainCollection.Items)
            {
                i++;

                item.Name = $"trend {i}";
                //item.Color = new SolidColorBrush(Color.FromRgb());
            }



            MainMethod();
        }


        void MainMethod()
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(ApiKey, ApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials(ApiKey, ApiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                // Public
                var ping = client.Ping();
                var exchangeInfo = client.GetExchangeInfo();
                var serverTime = client.GetServerTime();
                var orderBook = client.GetOrderBook("BNBBTC", 10);
                var aggTrades = client.GetAggregatedTrades("BNBBTC", startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10);
                var klines = client.GetKlines("BNBBTC", KlineInterval.OneHour, startTime: DateTime.UtcNow.AddHours(-10), endTime: DateTime.UtcNow, limit: 10);
                var price = client.GetPrice("BNBBTC");
                var prices24h = client.Get24HPrice("BNBBTC");
                var allPrices = client.GetAllPrices();
                var allBookPrices = client.GetAllBookPrices();
                var historicalTrades = client.GetHistoricalTrades("BNBBTC");


                // Private
                var openOrders = client.GetOpenOrders("BNBBTC");
                var allOrders = client.GetAllOrders("BNBBTC");
                var testOrderResult = client.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, 1, price: 1, timeInForce: TimeInForce.GoodTillCancel);
                //var queryOrder = client.GetOrder("BNBBTC", allOrders.Data[0].OrderId);
                //var orderResult = client.PlaceOrder("BNBBTC", OrderSide.Sell, OrderType.Limit, 10, price: 0.0002m, timeInForce: TimeInForce.GoodTillCancel);
                //var cancelResult = client.CancelOrder("BNBBTC", orderResult.Data.OrderId);
                var accountInfo = client.GetAccountInfo();
                var myTrades = client.GetMyTrades("BNBBTC");

                //// Withdrawal/deposit
                //var withdrawalHistory = client.GetWithdrawHistory();
                //var depositHistory = client.GetDepositHistory();
                //var withdraw = client.Withdraw("ASSET", "ADDRESS", 0);
            }

            var socketClient = new BinanceSocketClient();
            // Streams
            var successDepth = socketClient.SubscribeToDepthStream("bnbbtc", updateInterval: 1000, onMessage: (data) =>
            {
                // handle data
            });
            var successTrades = socketClient.SubscribeToTradesStream("bnbbtc", (data) =>
            {
                // handle data
            });
            var successKline = socketClient.SubscribeToKlineStream("bnbbtc", KlineInterval.OneMinute, (data) =>
            {
                // handle data
            });
            var successTicker = socketClient.SubscribeToAllSymbolTicker((data) =>
            {
                // handle data

            });
            var successSingleTicker = socketClient.SubscribeToSymbolTicker("ltcbnb", (data) =>
            {
                _mainCollection.Items.ElementAt(0).StreamData.Add(data);
            });

            var ssuccessSingleTicker = socketClient.SubscribeToSymbolTicker("dashbnb", (data) =>
            {
                _mainCollection.Items.ElementAt(1).StreamData.Add(data);
            });

            var sssuccessSingleTicker = socketClient.SubscribeToSymbolTicker("xmrbnb", (data) =>
            {
                _mainCollection.Items.ElementAt(2).StreamData.Add(data);
            });
            
            string listenKey;
            using (var client = new BinanceClient())
                listenKey = client.StartUserStream().Data;

            //var successAccount = socketClient.SubscribeToUserStream(listenKey,, data =>
            //{
            //    // Handle account info data
            //},
            //    data =>
            //    {
            //        // Handle order update info data
            //    });
            //socketClient.UnsubscribeAll();

            //Console.ReadLine();
        }
    }
}

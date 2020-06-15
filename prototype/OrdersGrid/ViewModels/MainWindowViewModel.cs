using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace OrdersGrid.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private static readonly string _apiKey = "xZjNacAWF1VYln2VTcRRY6KVN7IFNSiJuGtio1p2N0EjaLr3nQBRMiuftAb5zNer";
        private static readonly string _secretKey = "uzOoZeW9uoRtqXirs2GHhVGSuhC20T18crnuiQmrxD10446h1xYwtD3fQDBEPeLV";
        private static readonly string _symbol = "BNBBTC";

        public MainWindowViewModel()
        {
            TradingHistory = new ObservableCollectionExtended<BinanceOrder>();

            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(_apiKey, _secretKey),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials(_apiKey, _secretKey),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });

            using (var client = new BinanceClient())
            {
                var allOrders = client.GetAllOrders(_symbol);
                
                Application.Current.Dispatcher.Invoke(() =>
                    {
                        TradingHistory.AddRange(allOrders.Data);
                    });
            }

            //var socketClient = new BinanceSocketClient();

            //var successTrades = socketClient.subs("bnbbtc", (data) =>
            //{
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        TradingHistory.Add(data);
            //    });
            //});
        }

        private ObservableCollectionExtended<BinanceOrder> _tradingHistory = new ObservableCollectionExtended<BinanceOrder>();
        public ObservableCollectionExtended<BinanceOrder> TradingHistory
        {
            get => _tradingHistory;
            set => this.RaiseAndSetIfChanged(ref _tradingHistory, value);
        }

    }
}

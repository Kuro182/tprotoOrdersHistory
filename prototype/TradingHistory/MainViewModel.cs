using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using DynamicData.Binding;
using ReactiveUI;

namespace TradingHistory
{
    public class MainViewModel : ReactiveObject
    {


        public MainViewModel()
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials("bMNcUXrq4ZzUpMTM3jrNVqNIihI0sAeK6Zytudd7qEhCfyHdUPtL2uDmZSmEQ7J5 ", "qkA8T5Q6lNcUSTAhwi9jYaEbZpxmpvfoD0xB9ZdwLEEEQhYEqim1cJBjv2m1dNma"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            BinanceSocketClient.SetDefaultOptions(new BinanceSocketClientOptions()
            {
                ApiCredentials = new ApiCredentials("bMNcUXrq4ZzUpMTM3jrNVqNIihI0sAeK6Zytudd7qEhCfyHdUPtL2uDmZSmEQ7J5 ", "qkA8T5Q6lNcUSTAhwi9jYaEbZpxmpvfoD0xB9ZdwLEEEQhYEqim1cJBjv2m1dNma"),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });


            using (var client = new BinanceClient())
            {
                var historicalTrades = client.GetHistoricalSymbolTrades("BNBBTC");
            }

            var socketClient = new BinanceSocketClient();

            var successTrades = socketClient.SubscribeToTradeUpdates("bnbbtc", (data) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AllPrices.Add(data);
                });
            });
        }

        private ObservableCollectionExtended<BinanceStreamTrade> _allPrices  = new ObservableCollectionExtended<BinanceStreamTrade>();
        public ObservableCollectionExtended<BinanceStreamTrade> AllPrices
        {
            get { return _allPrices; }
            set => this.RaiseAndSetIfChanged(ref _allPrices, value);
        }
    }
}

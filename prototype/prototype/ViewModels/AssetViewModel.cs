using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace prototype.ViewModels
{
    public class AssetViewModel : ReactiveObject
    {
        #region Asset
        private string _asset;
        public string Asset
        {
            get => _asset;
            set => this.RaiseAndSetIfChanged(ref _asset, value);
        }
        #endregion

        #region Free
        private decimal _free;
        public decimal Free
        {
            get => _free;
            set => this.RaiseAndSetIfChanged(ref _free, value);
        }
        #endregion

        #region Locked
        private decimal _locked;
        public decimal Locked
        {
            get => _locked;
            set => this.RaiseAndSetIfChanged(ref _locked, value);
        }
        #endregion
    }
}

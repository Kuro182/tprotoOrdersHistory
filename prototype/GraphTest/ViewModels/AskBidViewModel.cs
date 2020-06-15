using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphTest.ViewModels
{
    public class AskBidViewModel : ReactiveObject
    {
        #region MainCollection

        private readonly SourceList<TrendViewModel> _mainCollection = new SourceList<TrendViewModel>();

        public IObservableCollection<TrendViewModel> MainCollection { get; } =
            new ObservableCollectionExtended<TrendViewModel>();

        #endregion


    }
}

using Bookinist.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {

        #region Title 

        private string? _title = "Книжный магазин";

        public string? Title { get => _title; set => Set(ref _title, value); }

        #endregion // Title

    }
}

using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        private DataService _DataService;

        public MainWindowViewModel MainModel { get; }


        public CountriesStatisticViewModel(MainWindowViewModel mainModel)
        {
            MainModel = MainModel;
            _DataService = new DataService();
        }
    }
}

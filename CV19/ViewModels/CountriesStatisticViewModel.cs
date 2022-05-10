using CV19.Models;
using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CV19.Infrastructure.Commands;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        private DataService _DataService;

        public MainWindowViewModel MainModel { get; }

        #region Contries IEnumerable<CountryInfo> Статистика по странам

        /// <summary> Статистика по странам </summary>
        private IEnumerable<CountryInfo> _Contries;

        /// <summary> Статистика по странам </summary>
        public IEnumerable<CountryInfo> Countries
        {
            get => _Contries;
            private set => Set(ref _Contries, value); 
        }

        #endregion

        #region Команды

        public ICommand RefreshDataCommand { get; }

        private void OnRefreshDataCommandExecuted(object p)
        {
            Countries = _DataService.GetData();
        }

        #endregion

        public CountriesStatisticViewModel(MainWindowViewModel mainModel)
        {
            MainModel = mainModel;
            _DataService = new DataService();

            #region Команды

            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted);

            #endregion
        }
    }
}

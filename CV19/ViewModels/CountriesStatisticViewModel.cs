using CV19.Models;
using CV19.Services;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CV19.Infrastructure.Commands;
using System.Linq;
using System.Windows;

namespace CV19.ViewModels
{
    internal class CountriesStatisticViewModel : ViewModel
    {
        private DataService _DataService;

        public MainWindowViewModel MainModel { get; internal set; }

        #region Contries IEnumerable<CountryInfo> Статистика по странам

        /// <summary> Статистика по странам </summary>
        private IEnumerable<CountryInfo> _Countries;

        /// <summary> Статистика по странам </summary>
        public IEnumerable<CountryInfo> Countries
        {
            get => _Countries;
            private set => Set(ref _Countries, value); 
        }

        #endregion

        #region SelectedCountry: CountryInfo - выбранная страна

        /// <summary> SelectedCountry: CountryInfo </summary>
        private CountryInfo _SelectedCountry;


        /// <summary> SelectedCountry: CountryInfo </summary>
        public CountryInfo SelectedCountry
        {
            get => _SelectedCountry;
            set => Set(ref _SelectedCountry, value);
        }

        #endregion

        #region Команды

        public ICommand RefreshDataCommand { get; }

        private void OnRefreshDataCommandExecuted(object p)
        {
            Countries = _DataService.GetData();
        }

        #endregion

        /// <summary> Отладочный конструктор, используеммый в процессе разработки в визуальном дизайнере </summary>
        //public CountriesStatisticViewModel() : this(null)
        //{
        //    if (!App.IsDesingMode)
        //        throw new InvalidOperationException("Вызов конструктора, предназначенного для использования в обычном режиме");

        //    /*_Countries = Enumerable.Range(1, 3)
        //       .Select(i => new CountryInfo
        //       {
        //           Name = $"Country {i}",
        //           ProvinceCounts = Enumerable.Range(1, 3).Select(j => new PlaceInfo
        //           {
        //               Name = $"Province {i}",
        //               Location = new Point(i, j),
        //               Counts = Enumerable.Range(1, 3).Select(k => new ConfirmedCount
        //               {
        //                   Date = DateTime.Now.Subtract(TimeSpan.FromDays(100 - k)),
        //                   Count = k
        //               }).ToArray()
        //           }).ToArray()
        //       }).ToArray();*/
        //}


        public CountriesStatisticViewModel(DataService dataService)
        {
            _DataService = dataService;

            #region Команды

            RefreshDataCommand = new LambdaCommand(OnRefreshDataCommandExecuted);

            #endregion
        }
    }
}

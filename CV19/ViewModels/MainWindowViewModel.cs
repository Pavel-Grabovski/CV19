using CV19.Infrastructure.Commands.Base;
using CV19.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CV19.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        #region Заголовок окна
        private string _Title = "Анализ статистики CV19";
        /// <summary>Заголовок окна</summary>

        
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

        #endregion

        #region Status: string - статус программы
        /// <summary>Статус программы</summary>
        private string _Status = "Готов!";


        /// <summary>Статус программы</summary>
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        #region Команды
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecuted(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }
        #endregion



        public MainWindowViewModel()
        {
            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);

            #endregion



        }


    }
}

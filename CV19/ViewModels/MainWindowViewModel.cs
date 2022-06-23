using CV19.Infrastructure.Commands;
using CV19.Models.Decanat;
using CV19.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using DataPoint = CV19.Models.DataPoint;

namespace CV19.ViewModels
{
    internal class MainWindowViewModel: ViewModel
    {
        /*----------------------------------------------------------------------------------------------------------------------------*/

        public CountriesStatisticViewModel CountriesStatistic { get; }

        /*----------------------------------------------------------------------------------------------------------------------------*/

        /*----------------------------------------------------------------------------------------------------------------------------*/

        public ObservableCollection<Group> Groups { get; }


        public object[] CompositeCollection { get; }

        #region SelectedCompositeValue Выбранный непонятный объект
        private object _SelectedCompositeValue;
        public object SelectedCompositeValue
        {
            get => _SelectedCompositeValue;
            set => Set(ref _SelectedCompositeValue, value);
        }
        #endregion

        #region SelectedGroup - Выбранная группа
        private Group _SelectedGroup;
        public Group SelectedGroup
        {
            get => _SelectedGroup;
            set
            {
                if(!Set(ref _SelectedGroup, value)) return;

                _SelectedGroupStudents.Source = value?.Students;
                OnPropertyChanged(nameof(SelectedGroupStudents));
            }
        }
        #endregion

        #region StudentFilterText:string -Текст фильтра студентов

        private string _StudentFilterText;
        public string StudentFilterText
        {
            get => _StudentFilterText;
            set
            {
                if (!Set(ref _StudentFilterText, value)) return;
                _SelectedGroupStudents.View.Refresh();
            }
        }
        #endregion

        #region SelectedGroupStudents - Выбрать группу студентов
        private readonly CollectionViewSource _SelectedGroupStudents = new CollectionViewSource();

        private void OnStudentFiltred(object sender, FilterEventArgs e)
        {
            if(!(e.Item is Student student))
            {
                e.Accepted = false;
                return;
            }

            var filter_text = _StudentFilterText;
            if (string.IsNullOrWhiteSpace(filter_text))
                return;

            if(student.Name is null || student.Surname is null || student.Patronymic is null)
            {
                e.Accepted = false;
                return;
            }

            if (student.Name.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (student.Surname.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;
            if (student.Patronymic.Contains(filter_text, StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        public ICollectionView SelectedGroupStudents => _SelectedGroupStudents?.View;
        #endregion

        #region SelectedPagendex: int - номер выбранной вкладки

        ///<summary> Номер выбранной вкладки</summary>
        private int _SelectedPageIndex = 2;

        ///<summary> Номер выбранной вкладки</summary>
        public int SelectedPageIndex
        {
            get => _SelectedPageIndex;
            set => Set(ref _SelectedPageIndex, value);
        }


        #endregion

        #region TestDataPoints

        /// <summary> Тестовый набор данных для визуализации графиков </summary>
        private IEnumerable<DataPoint> _TestDataPoints;

        /// <summary> Тестовый набор данных для визуализации графиков </summary>
        public IEnumerable<DataPoint> TestDataPoints
        {
            get => _TestDataPoints;
            set => Set(ref _TestDataPoints, value);
        }
        #endregion

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

       
        public string Status
        {
            get => _Status;
            set => Set(ref _Status, value);
        }
        #endregion

        public IEnumerable<Student> TestStudents => Enumerable.Range(1, App.IsDesingMode ? 10 : 100000)
            .Select(i => new Student
            {
                Name = $"Имя {i}",
                Surname = $"Фамилия {i}"
            });

        public DirectoryViewModel DiskRootDir { get; } = new DirectoryViewModel("c:\\");

        #region SelectedDirectory: DirectoryViewModel - Выбранная директория

        /// <summary> Выбранная директория  </summary>
        private DirectoryViewModel _SelectedDirectory;

        /// <summary> Выбранная директория  </summary>
        public DirectoryViewModel SelectedDirectory
        {
            get => _SelectedDirectory;
            set => Set(ref _SelectedDirectory, value);
        }

        #endregion

        /*-------------------------------------------------------------------------------------------------------------------------*/

        #region Команды
        #region CloseApplicationCommand
        public ICommand CloseApplicationCommand { get; }

        private bool CanCloseApplicationCommandExecuted(object p) => true;
        private void OnCloseApplicationCommandExecuted(object p)
        {
            (RootObject as Window)?.Close();
            //Application.Current.Shutdown();
        }
        #endregion

        #region ChangeTabIndexCommand
        public ICommand ChangeTabIndexCommand { get; }
        private bool CanChangeTabIndexCommandExecute(object p) => _SelectedPageIndex >= 0;
        private void OnChangeTabIndexCommandExecute(object p)
        {
            if (p is null) return;
            SelectedPageIndex += Convert.ToInt32(p);
        }
        #endregion

        #region CreateGroupCommand
        public ICommand CreateGroupCommand { get; }
        private bool CanCreateGroupCommandExecute(object p) => true;

        private void OnCreateGroupCommandExecute(object p)
        {
            var group_max_index = Groups.Count + 1;
            var new_group = new Group
            {
                Name = $"Группа {group_max_index}",
                Students = new ObservableCollection<Student>()
            };
            Groups.Add(new_group);
        }
        #endregion

        #region DeleteGroupCommand
        public ICommand DeleteGroupCommand { get; }
        private bool CanDeleteGroupCommandExecute(object p) => p is Group group && Groups.Contains(group) ;
        private void OnDeleteGroupCommandExecute(object p)
        {
            if (!(p is Group group)) return;
            var group_index = Groups.IndexOf(group);
            Groups.Remove(group);
            if(group_index < Groups.Count)
            {
                SelectedGroup = Groups[group_index];
            }
        }
        #endregion
        #endregion

        /*-------------------------------------------------------------------------------------------------------------------------*/
        public MainWindowViewModel(CountriesStatisticViewModel statistic)
        {
            CountriesStatistic = statistic;
            statistic.MainModel = this;
            //CountriesStatistic = new CountriesStatisticViewModel(this);


            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecuted);
            ChangeTabIndexCommand = new LambdaCommand(OnChangeTabIndexCommandExecute, CanChangeTabIndexCommandExecute);
            CreateGroupCommand = new LambdaCommand(OnCreateGroupCommandExecute, CanCreateGroupCommandExecute);
            DeleteGroupCommand = new LambdaCommand(OnDeleteGroupCommandExecute, CanDeleteGroupCommandExecute);

            #endregion

            #region Фильтры
            var data_points = new List<DataPoint>((int)(360 / 0.1));
            for (var x = 0d; x <= 360; x += 0.1)
            {
                const double to_rad = Math.PI / 180;
                var y = Math.Sin(x * to_rad);

                data_points.Add(new DataPoint { XValue = x, YValue = y });
            }

            TestDataPoints = data_points;

            var students_index = 1;
            var students = Enumerable.Range(1, 10).Select(i => new Student
            {
                Name = $"Name {students_index}",
                Surname = $"Surname {students_index}",
                Patronymic = $"Patronymic {students_index++}",
                Birthday = DateTime.Now,
                Rating = 0
            });

            var groups = Enumerable.Range(1, 20).Select(i => new Group
            {
                Name = $"Группа {i}",
                Students = new ObservableCollection<Student>(students)
            });


            Groups = new ObservableCollection<Group>(groups);

            var data_list = new List<object>();
            data_list.Add("Привет мир");
            data_list.Add(42);
            var group = Groups[1];
            data_list.Add(group);
            data_list.Add(group.Students[0]);

            CompositeCollection = data_list.ToArray();

            _SelectedGroupStudents.Filter += OnStudentFiltred;


            //_SelectedGroupStudents.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            //_SelectedGroupStudents.GroupDescriptions.Add(new PropertyGroupDescription("Birthday"));

            #endregion

        }

        /*------------------------------------------------------------------------------------------------------------------------*/
    }
}

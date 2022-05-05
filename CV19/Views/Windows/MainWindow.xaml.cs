using CV19.Models.Decanat;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CV19
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GroupsCollection_OnFilter(object sender, FilterEventArgs e)
        {
            if (!(e.Item is Group group )) return;
            if (group.Name is null) return;

            var filter_text = GroupNameFilterText.Text;
            if (filter_text.Length == 0) return;


            if (group.Name.Contains(filter_text, System.StringComparison.OrdinalIgnoreCase)) return;

            if (group.Description != null && group.Description.Contains(filter_text, System.StringComparison.OrdinalIgnoreCase)) return;

            e.Accepted = false;
        }

        private void GroupNameFilterText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var text_box = (TextBox)sender;
            var collection = (CollectionViewSource)text_box.FindResource("GroupsCollection");
            collection.View.Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfElasticSearch.Views.Controls
{
    /// <summary>
    /// Логика взаимодействия для SearchString.xaml
    /// </summary>
    public partial class SearchString : UserControl
    {
        private WpfElasticSearch.ViewModels.SearchStringControlViewModel _searchStringViewModel;

        public SearchString()
        {
            _searchStringViewModel = new WpfElasticSearch.ViewModels.SearchStringControlViewModel();

            DataContext = _searchStringViewModel;
            InitializeComponent();
        }

        public void FocusToListOfWorker(object sender, KeyEventArgs e)
        {
            if (Key.Down == e.Key)
            {
                SimilarWords.Focus();
            }
        }

        public void FocusToSimilarWords(object sender, KeyEventArgs e)
        {
            if (Key.Up != e.Key && Key.Down != e.Key && Key.Enter != e.Key)
            {
                FocusToTextBox();
            }
        }

        private void FocusToTextBox(object sender, DependencyPropertyChangedEventArgs e)
        {
            FocusToTextBox();
        }

        private void FocusToTextBox()
        {
            txtSearchString.SelectionStart = txtSearchString.Text.Length;
            txtSearchString.Focus();
        }
    }
}

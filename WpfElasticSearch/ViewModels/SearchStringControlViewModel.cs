using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;

namespace WpfElasticSearch.ViewModels
{
    class SearchStringControlViewModel : Notifier
    {
        public SearchStringControlViewModel()
        {
            ChangedText = new ViewModels.Command(arg => ChangedTextMethod(arg));
            ChangeSearchWord = new ViewModels.Command(arg => ChangeSearchWordMethod(arg));

            Enable = Visibility.Collapsed;
        }

        private String _name;
        public ICommand ChangedText { set; get; }
        private Visibility _enable;
        private IEnumerable<String> _similarWords = new List<String>();
        private String _selectedWord;
        public ICommand ChangeSearchWord { get; set; }

        public Visibility Enable
        {
            set
            {
                _enable = value;
                OnPropertyChanged(nameof(Enable));
            }
            get { return _enable; }
        }

        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private String SelectedWord
        {
            get
            {
                return _selectedWord;
            }
            set
            {
                _selectedWord = value;
                OnPropertyChanged(nameof(SelectedWord));
            }
        }

        private void ChangedTextMethod(Object arg)
        {
            String searchWord = Convert.ToString(arg);

            Enable = Visibility.Visible;

            if (!String.IsNullOrEmpty(searchWord))
            {
                Enable = Visibility.Visible;

                ElasticSearch.CElasticsearch el = new ElasticSearch.CElasticsearch();
                el.InitializeElasticsearch("http://localhost:9200");

                SimilarWords = el.GetSimilarWords(searchWord, 1);
            }
            else
            {
                Enable = Visibility.Hidden;
            }
        }

        public IEnumerable<String> SimilarWords
        {
            set
            {
                _similarWords = value;
                OnPropertyChanged(nameof(SimilarWords));
            }
            get { return _similarWords; }
        }

        
        private void ChangeSearchWordMethod(object arg)
        {
            String selectedWord = Convert.ToString(arg);

            if (selectedWord != null)
            {
                Name = selectedWord;
                Enable = Visibility.Hidden;
            }
        }

    }
}

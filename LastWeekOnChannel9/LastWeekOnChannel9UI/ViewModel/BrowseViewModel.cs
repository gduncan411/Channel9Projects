using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LastWeekOnChannel9UI.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace LastWeekOnChannel9UI.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class BrowseViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the BrowseViewModel class.
        /// </summary>
        /// 

        private int _lastLoadedPage = 0;
        public BrowseViewModel()
        {
            if (!base.IsInDesignMode)
                LoadEntries(1);
        }

        private void LoadEntries(int page)
        {
            if (C9Entries == null)
                C9Entries = new ObservableCollection<C9Entry>();

            var htmWeb = new HtmlAgilityPack.HtmlWeb();
            var htmDoc = htmWeb.Load("http://channel9.msdn.com/Browse/AllContent?page=" + page.ToString());

            var links = htmDoc.DocumentNode.SelectNodes("//div[@class='entry-image']");

            foreach (var link in links)
            {
                //string imageurl = link.SelectSingleNode("//img[@class='thumb']").GetAttributeValue("src", "");
                //string altTitle = link.SelectSingleNode("//img[@class='thumb']").GetAttributeValue("alt", "");
                //string url = "http://channel9.msdn.com" + link.SelectSingleNode("//a").GetAttributeValue("href", "");

                string imageurl = link.ChildNodes[1].ChildNodes[1].GetAttributeValue("src", "");
                string altTitle = link.ChildNodes[1].ChildNodes[1].GetAttributeValue("alt", "");
                string url = "http://channel9.msdn.com" + link.ChildNodes[1].GetAttributeValue("href", "");
                string pubdate = link.NextSibling.NextSibling.ChildNodes[3].ChildNodes[3].InnerText;

                C9Entries.Add(new C9Entry()
                    {
                        Title = altTitle,
                        ImageUrl = imageurl,
                        EntryUrl = url,
                        PubDate = pubdate
                    });
            }

            _lastLoadedPage = page;
        }

        /// <summary>
        /// The <see cref="C9Entries" /> property's name.
        /// </summary>
        public const string C9EntriesPropertyName = "C9Entries";

        private ObservableCollection<C9Entry> _c9Entries;

        /// <summary>
        /// Sets and gets the C9Entries property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<C9Entry> C9Entries
        {
            get
            {
                return _c9Entries;
            }

            set
            {
                if (_c9Entries == value)
                {
                    return;
                }

                _c9Entries = value;
                RaisePropertyChanged(() => C9Entries);
            }
        }

        /// <summary>
        /// The <see cref="SelectedC9Entries" /> property's name.
        /// </summary>
        public const string SelectedC9EntriesPropertyName = "SelectedC9Entries";

        private ObservableCollection<C9Entry> _selectedC9Entries ;

        /// <summary>
        /// Sets and gets the SelectedC9Entries property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<C9Entry> SelectedC9Entries
        {
            get
            {
                return _selectedC9Entries;
            }

            set
            {
                if (_selectedC9Entries == value)
                {
                    return;
                }

                _selectedC9Entries = value;
                RaisePropertyChanged(() => SelectedC9Entries);
            }
        }
        private RelayCommand _loadNextPageCommand;

        /// <summary>
        /// Gets the LoadNextPageCommand.
        /// </summary>
        public RelayCommand LoadNextPageCommand
        {
            get
            {
                return _loadNextPageCommand
                    ?? (_loadNextPageCommand = new RelayCommand(ExecuteLoadNextPageCommand));
            }
        }

        private void ExecuteLoadNextPageCommand()
        {
            LoadEntries(++_lastLoadedPage);
        }

    }
}
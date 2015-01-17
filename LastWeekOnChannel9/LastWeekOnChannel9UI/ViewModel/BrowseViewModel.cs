using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using HtmlAgilityPack;
using LastWeekOnChannel9UI.Model;

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
        public Action CloseAction { get; set; }

        public BrowseViewModel()
        {

        }

        private async Task LoadEntries(int page)
        {
            
            Task t = new Task (() =>
            {

                if (C9Entries == null)
                    C9Entries = new ObservableCollection<C9Entry>();

                var htmWeb = new HtmlWeb();
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

                    DispatcherHelper.CheckBeginInvokeOnUI(
                    () =>
                    {
                        C9Entries.Add(new C9Entry()
                        {
                            Title = altTitle,
                            ImageUrl = imageurl,
                            EntryUrl = url,
                            PubDate = pubdate
                        });
                    });

                }

                _lastLoadedPage = page;
            });

            // start the task
            t.Start();

            await t;

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
        /// The <see cref="SelectedC9Entry" /> property's name.
        /// </summary>
        public const string SelectedC9EntryPropertyName = "SelectedC9Entry";

        private C9Entry _selectedC9Entry;

        /// <summary>
        /// Sets and gets the SelectedC9Entry property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public C9Entry SelectedC9Entry
        {
            get
            {
                return _selectedC9Entry;
            }

            set
            {
                if (_selectedC9Entry == value)
                {
                    return;
                }

                _selectedC9Entry = value;
                RaisePropertyChanged(() => SelectedC9Entry);
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

        private async void ExecuteLoadNextPageCommand()
        {
            IsBusy = true;

            Task t = LoadEntries(++_lastLoadedPage);

            await t;

            IsBusy = false;
        }

        private RelayCommand _viewStoryInBrowserCommand;

        /// <summary>
        /// Gets the ViewStoryInBrowserCommand.
        /// </summary>
        public RelayCommand ViewStoryInBrowserCommand
        {
            get
            {
                return _viewStoryInBrowserCommand ?? (_viewStoryInBrowserCommand = new RelayCommand(
                    ExecuteViewStoryInBrowserCommand,
                    CanExecuteViewStoryInBrowserCommand));
            }
        }

        private void ExecuteViewStoryInBrowserCommand()
        {
            if (!ViewStoryInBrowserCommand.CanExecute(null))
            {
                return;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = SelectedC9Entry.EntryUrl,
                UseShellExecute = true
            });

        }

        private bool CanExecuteViewStoryInBrowserCommand()
        {
            if (SelectedC9Entry == null)
                return false;
            else
                return true;
        }


        /// <summary>
        /// The <see cref="IsBusy" /> property's name.
        /// </summary>
        public const string IsBusyPropertyName = "IsBusy";

        private bool _isBusy = false;

        /// <summary>
        /// Sets and gets the IsBusy property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        private RelayCommand _closeCommand;

        /// <summary>
        /// Gets the CloseCommand.
        /// </summary>
        public RelayCommand CloseCommand
        {
            get
            {
                return _closeCommand
                    ?? (_closeCommand = new RelayCommand(ExecuteCloseCommand));
            }
        }

        private void ExecuteCloseCommand()
        {
            CloseAction();
        }

        private RelayCommand<CancelEventArgs> _windowClosingCommand;

        /// <summary>
        /// Gets the WindowClosingCommand.
        /// </summary>
        public RelayCommand<CancelEventArgs> WindowClosingCommand
        {
            get
            {
                return _windowClosingCommand
                    ?? (_windowClosingCommand = new RelayCommand<CancelEventArgs>(ExecuteWindowClosingCommand));
            }
        }

        private void ExecuteWindowClosingCommand(CancelEventArgs parameter)
        {
            if (IsBusy)
                parameter.Cancel = true;
            else
                parameter.Cancel = false;
        }

    }
}
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Threading;
using LastWeekOnChannel9UI.Model;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace LastWeekOnChannel9UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {

            
        }

        /// <summary>
        /// The <see cref="ImageUrl" /> property's name.
        /// </summary>
        public const string ImageURLPropertyName = "ImageUrl";

        private string _imageUrl = string.Empty;

        /// <summary>
        /// Sets and gets the ImageUrl property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return _imageUrl;
            }

            set
            {
                if (_imageUrl == value)
                {
                    return;
                }

                _imageUrl = value;
                AddStoryCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => ImageUrl);
            }
        }

        /// <summary>
        /// The <see cref="EntryUrl" /> property's name.
        /// </summary>
        public const string EntryUrlPropertyName = "EntryUrl";

        private string _entryUrl = string.Empty;

        /// <summary>
        /// Sets and gets the EntryUrl property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string EntryUrl
        {
            get
            {
                return _entryUrl;
            }

            set
            {
                if (_entryUrl == value)
                {
                    return;
                }

                _entryUrl = value;
                AddStoryCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => EntryUrl);
            }
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = string.Empty;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                AddStoryCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => Title);
            }
        }

        /// <summary>
        /// The <see cref="BodyHtml" /> property's name.
        /// </summary>
        public const string BodyHtmlPropertyName = "BodyHtml";

        private string _bodyHtml = string.Empty;

        /// <summary>
        /// Sets and gets the BodyHtml property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string BodyHtml
        {
            get
            {
                return _bodyHtml;
            }

            set
            {
                if (_bodyHtml == value)
                {
                    return;
                }

                _bodyHtml = value;
                AddStoryCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => BodyHtml);
            }
        }

        private RelayCommand _addStoryCommand;

        /// <summary>
        /// Gets the AddStoryCommand.
        /// </summary>
        public RelayCommand AddStoryCommand
        {
            get
            {
                return _addStoryCommand ?? (_addStoryCommand = new RelayCommand(
                    ExecuteAddStoryCommand,
                    CanExecuteAddStoryCommand));
            }
        }

        private void ExecuteAddStoryCommand()
        {
            if (!AddStoryCommand.CanExecute(null))
            {
                return;
            }

            if (Stories == null)
                Stories = new ObservableCollection<Story>();

            Stories.Add(new Story
            {
                Title = Title,
                ImageUrl = ImageUrl,
                EntryUrl = EntryUrl,
                BodyHtml = BodyHtml
            });

            Title = "";
            ImageUrl = "";
            EntryUrl = "";
            BodyHtml = "";
            
        }

        private bool CanExecuteAddStoryCommand()
        {
            if (string.IsNullOrWhiteSpace(Title))
                return false;
            if (string.IsNullOrWhiteSpace(ImageUrl))
                return false;
            if (string.IsNullOrWhiteSpace(EntryUrl))
                return false;
            if (string.IsNullOrWhiteSpace(BodyHtml))
                return false;
            else
                return true;
        }

        /// <summary>
        /// The <see cref="Stories" /> property's name.
        /// </summary>
        public const string StoriesPropertyName = "Stories";

        private ObservableCollection<Story>  _Stories = null;

        /// <summary>
        /// Sets and gets the Stories property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Story>  Stories
        {
            get
            {
                return _Stories;
            }

            set
            {
                if (_Stories == value)
                {
                    return;
                }

                _Stories = value;
                CopyPostToClipboardCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => Stories);
            }
        }

        /// <summary>
        /// The <see cref="SelectedStory" /> property's name.
        /// </summary>
        public const string SelectedStoryPropertyName = "SelectedStory";

        private Story _selectedStory = null;

        /// <summary>
        /// Sets and gets the SelectedStory property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Story SelectedStory
        {
            get
            {
                return _selectedStory;
            }

            set
            {
                if (_selectedStory == value)
                {
                    return;
                }

                _selectedStory = value;
                MoveSelectedStoryUpCommand.RaiseCanExecuteChanged();
                MoveSelectedStoryDownCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged(() => SelectedStory);
            }
        }

        private RelayCommand _moveSelectedStoryUpCommand;

        /// <summary>
        /// Gets the MoveSelectedStoryUpCommand.
        /// </summary>
        public RelayCommand MoveSelectedStoryUpCommand
        {
            get
            {
                return _moveSelectedStoryUpCommand ?? (_moveSelectedStoryUpCommand = new RelayCommand(
                    ExecuteMoveSelectedStoryUpCommand,
                    CanExecuteMoveSelectedStoryUpCommand));
            }
        }

        private void ExecuteMoveSelectedStoryUpCommand()
        {
            if (!MoveSelectedStoryUpCommand.CanExecute(null))
            {
                return;
            }

            var workingstory = SelectedStory;
            var startingindex = Stories.IndexOf(SelectedStory);
            Stories.Remove(SelectedStory);
            Stories.Insert(startingindex - 1, workingstory);

        }

        private bool CanExecuteMoveSelectedStoryUpCommand()
        {
            if (SelectedStory == null)
                return false;
            else if (Stories.IndexOf(SelectedStory) == 0)
                return false;
            else
                return true;
        }

        private RelayCommand _moveSelectedStoryDownCommand;

        /// <summary>
        /// Gets the MoveSelectedStoryDownCommand.
        /// </summary>
        public RelayCommand MoveSelectedStoryDownCommand
        {
            get
            {
                return _moveSelectedStoryDownCommand ?? (_moveSelectedStoryDownCommand = new RelayCommand(
                    ExecuteMoveSelectedStoryDownCommand,
                    CanExecuteMoveSelectedStoryDownCommand));
            }
        }

        private void ExecuteMoveSelectedStoryDownCommand()
        {
            if (!MoveSelectedStoryDownCommand.CanExecute(null))
            {
                return;
            }

            var workingstory = SelectedStory;
            var startingindex = Stories.IndexOf(SelectedStory);
            Stories.Remove(SelectedStory);
            Stories.Insert(startingindex + 1, workingstory);

        }

        private bool CanExecuteMoveSelectedStoryDownCommand()
        {
            if (SelectedStory == null)
                return false;
            else if (Stories.IndexOf(SelectedStory) == Stories.Count - 1 )
                return false;
            else
             return true;
        }

        private RelayCommand _copyPostToClipboardCommand;

        /// <summary>
        /// Gets the CopyPostToClipboardCommand.
        /// </summary>
        public RelayCommand CopyPostToClipboardCommand
        {
            get
            {
                return _copyPostToClipboardCommand ?? (_copyPostToClipboardCommand = new RelayCommand(
                    ExecuteCopyPostToClipboardCommand,
                    CanExecuteCopyPostToClipboardCommand));
            }
        }

        private void ExecuteCopyPostToClipboardCommand()
        {
            if (!CopyPostToClipboardCommand.CanExecute(null))
            {
                return;
            }

            var post = new StringBuilder();
            post.AppendLine("<p>[intro]</p> <dl class=\"twoCol\">");

            foreach (var story in Stories)
            {
                post.AppendLine("<dt>");
                post.AppendFormat("<a href=\"{0}\" target=\"_blank\"><img border=\"0\" alt=\"\" src=\"{1}\" width=\"220\" height=\"123\"></a> ", story.EntryUrl, story.ImageUrl);
                post.AppendLine("</dt>");
                post.AppendLine("<dd>");
                post.AppendFormat("<a href=\"{0}\"><strong>{1}</strong></a>", story.EntryUrl, story.Title);
                post.AppendLine("</dd>");
                post.AppendLine("<dd>");
                post.AppendFormat("<p>{0}</p>", story.BodyHtml);
                post.AppendLine("</dd>");
            }

            post.AppendLine("</dl>");
            post.AppendLine("<p><a class=\"twitter-follow-button\" href=\"https://twitter.com/ch9\">Follow @CH9</a> <br />");
            post.AppendLine("<a class=\"twitter-follow-button\" href=\"https://twitter.com/gduncan411\">Follow @gduncan411</a></p>");

            ClipboardHelper.CopyHtmlToClipBoard(post.ToString());
            System.Windows.MessageBox.Show("Copied");
            
        }

        private bool CanExecuteCopyPostToClipboardCommand()
        {
            if (Stories == null || Stories.Count == 0)
                return false;
            else
                return true;
        }

        private RelayCommand _pasteImageHtmlCommand;

        /// <summary>
        /// Gets the PasteImageHtmlCommand.
        /// </summary>
        public RelayCommand PasteImageHtmlCommand
        {
            get
            {
                return _pasteImageHtmlCommand
                    ?? (_pasteImageHtmlCommand = new RelayCommand(ExecutePasteImageHtmlCommand));
            }
        }

        private void ExecutePasteImageHtmlCommand()
        {
             if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                var rawhtml = Clipboard.GetText(TextDataFormat.Html);

                int start = rawhtml.IndexOf("<!--StartFragment-->");
                int end = rawhtml.IndexOf("<!--EndFragment-->");
                var html = rawhtml.Substring(start + 20, end - start - 20);
                string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
                MatchCollection matchesImgSrc = Regex.Matches(html, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in matchesImgSrc)
                {
                    string href = m.Groups[1].Value;
                    ImageUrl = href;
                    break;
                }

                          //Clipboard.SetText(replacementHtmlText, TextDataFormat.Html);
            }
        }

        private RelayCommand _pasteBodyHtmlCommand;

        /// <summary>
        /// Gets the PasteBodyHtmlCommand.
        /// 
        /// </summary>
        public RelayCommand PasteBodyHtmlCommand
        {
            get
            {
                return _pasteBodyHtmlCommand
                    ?? (_pasteBodyHtmlCommand = new RelayCommand(ExecutePasteBodyHtmlCommand));
            }
        }

        private void ExecutePasteBodyHtmlCommand()
        {
            if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                var rawhtml = Clipboard.GetText(TextDataFormat.Html);

                int start = rawhtml.IndexOf("<!--StartFragment-->");
                int end = rawhtml.IndexOf("<!--EndFragment-->");
                BodyHtml = rawhtml.Substring(start + 20, end - start - 20);

                //Clipboard.SetText(replacementHtmlText, TextDataFormat.Html);
            }
        }


        private RelayCommand _showBrowseWindowCommand;

        /// <summary>
        /// Gets the ShowBrowseWindowCommand.
        /// </summary>
        public RelayCommand ShowBrowseWindowCommand
        {
            get
            {
                return _showBrowseWindowCommand
                    ?? (_showBrowseWindowCommand = new RelayCommand(ExecuteShowBrowseWindowCommand));
            }
        }

        private async void ExecuteShowBrowseWindowCommand()
        {
            var b = new BrowseView();
            b.ShowDialog();

            var vm = (BrowseViewModel)b.DataContext;

            if (vm.C9Entries == null)
                return;

            IsBusy = true;

            await Task.Factory.StartNew(() =>
            {
                if (Stories == null)
                    Stories = new ObservableCollection<Story>();

                foreach (var item in vm.C9Entries)
                {
                    if (item.Selected)
                    {
                        var entryBody = GetEntryBody(item.EntryUrl);

                        DispatcherHelper.CheckBeginInvokeOnUI(
                        () =>
                        {
                            Stories.Add(new Story
                            {
                                Title = item.Title,
                                ImageUrl = item.ImageUrl,
                                EntryUrl = item.EntryUrl,
                                BodyHtml = entryBody
                            });
                        });

                    }
                }
            });


            IsBusy = false;
            
        }

        private string GetEntryBody(string entryUrl)
        {
            string result = string.Empty;

            var htmWeb = new HtmlAgilityPack.HtmlWeb();
            var htmDoc = htmWeb.Load(entryUrl);

            //var links = htmDoc.DocumentNode.SelectSingleNode("//div[@class='entry-body']");
            result = htmDoc.DocumentNode.SelectSingleNode("//div[@id='entry-body']").InnerHtml;
            
            return result;
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
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}
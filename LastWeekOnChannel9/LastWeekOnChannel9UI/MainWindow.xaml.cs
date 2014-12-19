using System.Windows;
using LastWeekOnChannel9UI.ViewModel;
using System.Text.RegularExpressions;

namespace LastWeekOnChannel9UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            Title = Title + " (" + System.Windows.Forms.Application.ProductVersion + ")";
        }

        private void MenuItemPasteHtml_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemPasteHtl_Click(object sender, RoutedEventArgs e)
        {
            //BodyTextbox.Text = 
            if (Clipboard.ContainsText(TextDataFormat.Html))
            {
                var rawhtml = Clipboard.GetText(TextDataFormat.Html);

                int start = rawhtml.IndexOf("<!--StartFragment-->");
                int end = rawhtml.IndexOf("<!--EndFragment-->");
                BodyTextbox.Text = rawhtml.Substring(start + 20, end - start - 20);

                //Clipboard.SetText(replacementHtmlText, TextDataFormat.Html);
            }
        }

        private void MenuItemImagePasteHtlm_Click(object sender, RoutedEventArgs e)
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
                    ImageUrlTextbox.Text = href;
                    break;
                }

                          //Clipboard.SetText(replacementHtmlText, TextDataFormat.Html);
            }
        }

        private void DataGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            //http://andora.us/blog/2011/01/11/wpf-4-datagrid-getting-the-row-number-into-the-rowheader/
            e.Row.Header = (e.Row.GetIndex() + 1).ToString(); 
        }
    }
}
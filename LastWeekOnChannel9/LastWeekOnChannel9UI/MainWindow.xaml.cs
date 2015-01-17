using System.Windows;
using System.Windows.Controls;
using LastWeekOnChannel9UI.ViewModel;
using Application = System.Windows.Forms.Application;

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
            Title = Title + " (" + Application.ProductVersion + ")";
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //http://andora.us/blog/2011/01/11/wpf-4-datagrid-getting-the-row-number-into-the-rowheader/
            e.Row.Header = (e.Row.GetIndex() + 1).ToString(); 
        }
    }
}
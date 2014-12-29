using LastWeekOnChannel9UI.ViewModel;
using System;
using System.Windows;

namespace LastWeekOnChannel9UI
{
    /// <summary>
    /// Description for BrowseView.
    /// </summary>
    public partial class BrowseView : Window
    {
        /// <summary>
        /// Initializes a new instance of the BrowseView class.
        /// </summary>
        public BrowseView()
        {
            InitializeComponent();
            BrowseViewModel vm = (BrowseViewModel)this.DataContext;
            vm.CloseAction = new Action(() => this.Close());

        }
    }
}
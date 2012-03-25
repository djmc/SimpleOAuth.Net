using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SimpleOAuthTester.WP.Mango.ViewModels;

namespace SimpleOAuthTester.WP.Mango
{
	public partial class TwitterControl : UserControl
    {
        private TwitterViewModel ViewModel
        {
            get
            {
                return DataContext as TwitterViewModel;
            }
        }

        public TwitterControl()
		{
			// Required to initialize variables
			InitializeComponent();
		}

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadedCommand.Execute();
        }
	}
}
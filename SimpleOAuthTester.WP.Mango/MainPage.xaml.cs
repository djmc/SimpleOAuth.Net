using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using SimpleOAuthTester.WP.Mango.ViewModels;
using Microsoft.Phone.Shell;
using SimpleOAuthTester.WP.Mango.Classes;
using System.ComponentModel;

namespace SimpleOAuthTester.WP.Mango
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ProgressIndicator indicator = new ProgressIndicator();

        private MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, indicator);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "HasIndeterminateProgress")
            {
                if (ViewModel.HasIndeterminateProgress != indicator.IsIndeterminate)
                {
                    indicator.IsIndeterminate = ViewModel.HasIndeterminateProgress;
                    indicator.IsVisible = ViewModel.HasIndeterminateProgress;
                }
            }
            else if (e.PropertyName == "IndeterminateProgressMessage")
            {
                if (ViewModel.IndeterminateProgressMessage != indicator.Text)
                {
                    indicator.Text = ViewModel.IndeterminateProgressMessage;
                }
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadedCommand.Execute();
        }

        private void EnableProgressIndicator(string text)
        {
            UIHelper.SafeDispatch(() =>
                {
                    indicator.IsIndeterminate = true;
                    indicator.Text = text;
                    indicator.IsVisible = true;
                });
        }

        private void DisableProgressIndicator()
        {
            UIHelper.SafeDispatch(() =>
            {
                indicator.IsVisible = false;
            });
        }
    }
}
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
using SimpleOAuthTester.WP.Mango.Classes;
using Microsoft.Phone.Shell;
using SimpleOAuthTester.WP.Mango.ViewModels;
using System.ComponentModel;

namespace SimpleOAuthTester.WP.Mango
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ProgressIndicator indicator = new ProgressIndicator();

        private TermIeViewModel TermIeViewModel
        {
            get
            {
                return DataContext as TermIeViewModel;
            }
        }

        private TwitterViewModel TwitterViewModel
        {
            get
            {
                return DataContext as TwitterViewModel;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, indicator);
            TwitterViewModel.PropertyChanged += ViewModel_PropertyChanged;
            TermIeViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IDisplaysIndeterminateProgress vm = sender as IDisplaysIndeterminateProgress;

            if (vm == null)
            {
                throw new ArgumentException("Sender was not an IDisplaysIndeterminateProgress.", "sender");
            }

            if (e.PropertyName == "HasIndeterminateProgress")
            {
                if (vm.HasIndeterminateProgress != indicator.IsIndeterminate)
                {
                    indicator.IsIndeterminate = vm.HasIndeterminateProgress;
                    indicator.IsVisible = vm.HasIndeterminateProgress;
                }
            }
            else if (e.PropertyName == "IndeterminateProgressMessage")
            {
                if (vm.IndeterminateProgressMessage != indicator.Text)
                {
                    indicator.Text = vm.IndeterminateProgressMessage;
                }
            }
        }
    }
}
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

namespace SimpleOAuthTester.WP.Mango
{
    public partial class AuthenticateToTwitter : BasePhoneApplicationPage
    {
        private AuthenticateToTwitterViewModel ViewModel
        {
            get
            {
                return DataContext as AuthenticateToTwitterViewModel;
            }
        }

        public AuthenticateToTwitter()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += HandleProgressPropertyChanged;
        }

        private void WebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.BrowserNavigated.Execute();
        }

        private void WebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            ViewModel.BrowserNavigating.Execute(e);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ViewModel.NavigatedTo.Execute();
            base.OnNavigatedTo(e);
        }
    }
}
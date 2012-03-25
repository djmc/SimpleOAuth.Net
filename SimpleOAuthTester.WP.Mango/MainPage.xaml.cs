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
    public partial class MainPage : BasePhoneApplicationPage
    {
        private MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        private TermIeViewModel TermIeViewModel
        {
            get
            {
                return ViewModel.TermIeViewModel;
            }
        }

        private TwitterViewModel TwitterViewModel
        {
            get
            {
                return ViewModel.TwitterViewModel;
            }
        }

        public MainPage()
        {
            InitializeComponent();
            TwitterViewModel.PropertyChanged += HandleProgressPropertyChanged;
            TermIeViewModel.PropertyChanged += HandleProgressPropertyChanged;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.NavigatedToCommand.Execute();
        }
    }
}
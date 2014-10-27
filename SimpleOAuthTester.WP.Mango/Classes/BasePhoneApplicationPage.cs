using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using Microsoft.Phone.Controls;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public class BasePhoneApplicationPage : PhoneApplicationPage
    {
        private readonly ProgressIndicator _indicator = new ProgressIndicator();

        /// <summary>
        /// Initializes a new instance of the BasePhoneApplicationPage class.
        /// </summary>
        public BasePhoneApplicationPage()
            : base()
        {
            SystemTray.SetProgressIndicator(this, _indicator);
        }

        protected void HandleProgressPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IDisplaysIndeterminateProgress vm = sender as IDisplaysIndeterminateProgress;

            if (vm == null)
            {
                throw new ArgumentException("Sender was not an IDisplaysIndeterminateProgress.", "sender");
            }

            if (e.PropertyName == "HasIndeterminateProgress")
            {
                if (vm.HasIndeterminateProgress != _indicator.IsIndeterminate)
                {
                    _indicator.IsIndeterminate = vm.HasIndeterminateProgress;
                    _indicator.IsVisible = vm.HasIndeterminateProgress;
                }
            }
            else if (e.PropertyName == "IndeterminateProgressMessage")
            {
                if (vm.IndeterminateProgressMessage != _indicator.Text)
                {
                    _indicator.Text = vm.IndeterminateProgressMessage;
                }
            }
        }
    }
}

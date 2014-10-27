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

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public static class UIHelper
    {
        // From: http://stackoverflow.com/questions/3293137/how-to-run-a-function-on-a-background-thread-for-windows-phone-7
        public static void SafeDispatch(Action action)
        {
            if (Deployment.Current.Dispatcher.CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(action);
            }
        }
    }
}

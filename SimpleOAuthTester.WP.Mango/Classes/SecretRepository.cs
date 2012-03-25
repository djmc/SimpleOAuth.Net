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
    public static class SecretRepository
    {
        /// <summary>
        /// Consumer Key (change to appropriate value)
        /// </summary>
        public static string ConsumerKey { get { return "key"; } }
        /// <summary>
        /// Consumer Secret (change to appropriate value)
        /// </summary>
        public static string ConsumerSecret { get { return "secret"; } }
        /// <summary>
        /// Callback URL (does not need to be changed, I promise)
        /// <br /><br />
        /// This does not need to be a URL within your control. It will
        /// never actually be called. It is instead intercepted by a Navigating event in
        /// the <see cref="Microsoft.Phone.Controls.WebBrowser"/> control.
        /// </summary>
        public static string CallbackUrl { get { return "http://bing.com"; } }
    }
}

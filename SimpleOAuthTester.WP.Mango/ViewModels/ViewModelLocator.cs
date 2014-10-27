using System;

namespace SimpleOAuthTester.WP.Mango.ViewModels
{
    public class ViewModelLocator
    {
        private static readonly TermIeViewModel _termIe = new TermIeViewModel();
        private static readonly TwitterViewModel _twitter = new TwitterViewModel();
        private static readonly MainViewModel _main = new MainViewModel();
        private static readonly AuthenticateToTwitterViewModel _authenticateToTwitter = new AuthenticateToTwitterViewModel();

        public TermIeViewModel TermIe { get { return _termIe; } }
        public TwitterViewModel Twitter { get { return _twitter; } }
        public MainViewModel Main { get { return _main; } }
        public AuthenticateToTwitterViewModel AuthenticateToTwitter { get { return _authenticateToTwitter; } }
    }
}

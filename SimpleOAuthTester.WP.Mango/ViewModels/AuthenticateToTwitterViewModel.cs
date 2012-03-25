using System;
using SimpleOAuthTester.WP.Mango.Classes;

namespace SimpleOAuthTester.WP.Mango.ViewModels
{
    public class AuthenticateToTwitterViewModel : ViewModelBase, IDisplaysIndeterminateProgress
    {
        private bool _hasIndeterminateProgress;
        public bool HasIndeterminateProgress
        {
            get
            {
                return _hasIndeterminateProgress;
            }
            set
            {
                if (_hasIndeterminateProgress != value)
                {
                    _hasIndeterminateProgress = value;
                    OnPropertyChanged("HasIndeterminateProgress");
                }
            }
        }

        private string _indeterminateProgressMessage;
        public string IndeterminateProgressMessage
        {
            get
            {
                return _indeterminateProgressMessage;
            }
            set
            {
                if (_indeterminateProgressMessage != value)
                {
                    _indeterminateProgressMessage = value;
                    OnPropertyChanged("IndeterminateProgressMessage");
                }
            }
        }
    }
}

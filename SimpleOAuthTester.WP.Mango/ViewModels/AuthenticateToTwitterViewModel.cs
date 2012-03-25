using System;
using SimpleOAuthTester.WP.Mango.Classes;
using SimpleOAuth;
using System.Net;
using System.IO;
using Microsoft.Phone.Controls;

namespace SimpleOAuthTester.WP.Mango.ViewModels
{
    public class AuthenticateToTwitterViewModel : ViewModelBase, IDisplaysIndeterminateProgress
    {
        public RelayCommand<NavigatingEventArgs> BrowserNavigating { get; private set; }
        public RelayCommand BrowserNavigated { get; private set; }
        public RelayCommand NavigatedTo { get; private set; }

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

        private string _webUrl;
        public string WebUrl
        {
            get
            {
                return _webUrl;
            }
            set
            {
                if (_webUrl != value)
                {
                    _webUrl = value;
                    OnPropertyChanged("WebUrl");
                }
            }
        }

        private double _webOpacity;
        public double WebOpacity
        {
            get
            {
                return _webOpacity;
            }
            set
            {
                if (_webOpacity != value)
                {
                    _webOpacity = value;
                    OnPropertyChanged("WebOpacity");
                }
            }
        }

        public AuthenticateToTwitterViewModel()
        {
            BrowserNavigated = new RelayCommand(() => { WebOpacity = 1; HasIndeterminateProgress = false; });
            NavigatedTo = new RelayCommand(HandleNavigatedTo);
            BrowserNavigating = new RelayCommand<NavigatingEventArgs>(HandleBrowserNavigating);
        }

        private void HandleNavigatedTo()
        {
            HasIndeterminateProgress = true;
            IndeterminateProgressMessage = "Waiting on Twitter...";

            Tokens tokens = TwitterTokensRepository.Tokens;
            tokens.AccessToken = null;
            tokens.AccessTokenSecret = null;

            var request = WebRequest.Create(Path.Combine(
                TwitterTokensRepository.OAuthRoot,
                TwitterTokensRepository.RequestTokenPath));

            request.Method = "POST";

            request.SignRequest(tokens)
                .WithCallback(SecretRepository.CallbackUrl)
                .InHeader();

            request.GetOAuthTokensAsync((requestTokens, requestException) =>
                {
                    if (requestException != null)
                    {
                        Messenger.Send<SimpleCommand>(new SimpleCommand { CommandType = SimpleCommandType.AuthenticationFailure, Message = requestException.ToString() });
                        return;
                    }

                    tokens.MergeWith(requestTokens);

                    UIHelper.SafeDispatch(() =>
                        WebUrl = Path.Combine(
                            TwitterTokensRepository.OAuthRoot,
                            TwitterTokensRepository.AuthenticatePath)
                            + "?oauth_token=" + tokens.AccessToken);
                });
        }

        private void HandleBrowserNavigating(NavigatingEventArgs x)
        {
            bool successfulAuthentication = false;
            string authenticationFailure = null;
            var tokens = TwitterTokensRepository.Tokens;

            if (x.Uri.ToString().StartsWith(SecretRepository.CallbackUrl))
            {
                x.Cancel = true;
                WebOpacity = 0;
                var oauthToken = x.Uri.Query.QueryStringValue("oauth_token");
                var oauthVerifier = x.Uri.Query.QueryStringValue("oauth_verifier");

                if (oauthToken.HasValue() && oauthVerifier.HasValue())
                {
                    var keySwapRequest = WebRequest.Create(Path.Combine(
                        TwitterTokensRepository.OAuthRoot,
                        TwitterTokensRepository.AccessTokenPath));

                    keySwapRequest.Method = "POST";

                    keySwapRequest.SignRequest(tokens)
                        .WithEncryption(EncryptionMethod.HMACSHA1)
                        .WithVerifier(oauthVerifier)
                        .InHeader();

                    HasIndeterminateProgress = true;
                    IndeterminateProgressMessage = "Waiting on Twitter...";

                    keySwapRequest.GetOAuthTokensAsync((finalAccessTokens, accessException) =>
                        {
                            HasIndeterminateProgress = false;

                            if (accessException != null)
                            {
                                Messenger.Send<SimpleCommand>(new SimpleCommand { CommandType = SimpleCommandType.AuthenticationFailure, Message = accessException.ToString() });
                                return;
                            }

                            tokens.MergeWith(finalAccessTokens);
                            TwitterTokensRepository.HasAccessTokens = true;

                            Messenger.Send<SimpleCommand>(new SimpleCommand { CommandType = SimpleCommandType.SuccessfulAuthentication });
                        });
                }
                else
                {
                    Messenger.Send<SimpleCommand>(new SimpleCommand { CommandType = SimpleCommandType.AuthenticationFailure });
                }
            }
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.ComponentModel;
using SimpleOAuthTester.WP.Mango.Classes;
using SimpleOAuth;
using System.IO;
using Microsoft.Phone.Shell;

namespace SimpleOAuthTester.WP.Mango.ViewModels
{
    public class TwitterViewModel : ViewModelBase, IDisplaysIndeterminateProgress
    {
        private Tokens RequestTokens { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private const string OAuthRoot = "http://term.ie/oauth/example/";

        #region Bindable Properties
        private string _pageTitle;
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                if (_pageTitle != value)
                {
                    _pageTitle = value;
                    OnPropertyChanged("PageTitle");
                }
            }
        }

        private string _authenticationStatus;
        public string AuthenticationStatus
        {
            get
            {
                return _authenticationStatus;
            }
            set
            {
                if (_authenticationStatus != value)
                {
                    _authenticationStatus = value;
                    OnPropertyChanged("AuthenticationStatus");
                }
            }
        }

        private string _responseText;
        public string ResponseText
        {
            get
            {
                return _responseText;
            }
            set
            {
                if (_responseText != value)
                {
                    _responseText = value;
                    OnPropertyChanged("ResponseText");
                }
            }
        }

        private string _httpMethod;
        public string HttpMethod
        {
            get
            {
                return _httpMethod;
            }
            set
            {
                if (_httpMethod != value)
                {
                    _httpMethod = value;
                    OnPropertyChanged("HttpMethod");
                }
            }
        }

        private string _relativeUrl;
        public string RelativeUrl
        {
            get
            {
                return _relativeUrl;
            }
            set
            {
                if (_relativeUrl != value)
                {
                    _relativeUrl = value;
                    OnPropertyChanged("RelativeUrl");
                }
            }
        }

        private string _httpParameters;
        public string HttpParameters
        {
            get
            {
                return _httpParameters;
            }
            set
            {
                if (_httpParameters != value)
                {
                    _httpParameters = value;
                    OnPropertyChanged("HttpParameters");
                }
            }
        }

        private bool _authenticationButtonEnabled;
        public bool AuthenticationButtonEnabled
        {
            get
            {
                return _authenticationButtonEnabled;
            }
            set
            {
                if (_authenticationButtonEnabled != value)
                {
                    _authenticationButtonEnabled = value;
                    OnPropertyChanged("AuthenticationButtonEnabled");
                }
            }
        }

        private bool _getResponseButtonEnabled;
        public bool GetResponseButtonEnabled
        {
            get
            {
                return _getResponseButtonEnabled;
            }
            set
            {
                if (_getResponseButtonEnabled != value)
                {
                    _getResponseButtonEnabled = value;
                    OnPropertyChanged("GetResponseButtonEnabled");
                }
            }
        }

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
        #endregion

        public RelayCommand AuthenticateCommand { get; private set; }
        public RelayCommand GetResponseCommand { get; private set; }
        public RelayCommand LoadedCommand { get; private set; }

        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get
            {
                return _isAuthenticated;
            }
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    AuthenticateCommand.RaiseCanExecuteChanged();
                    GetResponseCommand.RaiseCanExecuteChanged();

                    AuthenticationStatus = "Authenticated with token: " + RequestTokens.AccessToken;
                }
            }
        }

        public TwitterViewModel()
        {
            AuthenticationButtonEnabled = true;
            GetResponseButtonEnabled = true;
            PageTitle = "twitter";
            AuthenticationStatus = "Not Authenticated";
            HttpMethod = "GET";
            RelativeUrl = "echo_api.php";
            HttpParameters = "method=foo&bar=baz";

            if (DesignerProperties.IsInDesignTool)
            {
                ResponseText = "Response text goes here blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah blah";
            }

            AuthenticateCommand = new RelayCommand(Authenticate, () => IsAuthenticated == false);
            GetResponseCommand = new RelayCommand(GetResponse, () => IsAuthenticated == true);
            LoadedCommand = new RelayCommand(HandleLoaded);
        }

        private void Authenticate()
        {
            var request = WebRequest.Create(Path.Combine(OAuthRoot, "request_token.php"));

            request.Method = "POST";

            request.SignRequest()
                .WithTokens(RequestTokens)
                .InHeader();

            EnableProgressIndicator("Waiting on term.ie...");

            request.GetOAuthTokensAsync((accessTokens, exAuth) =>
                {
                    try
                    {
                        if (exAuth != null)
                        {
                            UIHelper.SafeDispatch(() =>
                                MessageBox.Show("Exception:\n" + exAuth, "Error authenticating", MessageBoxButton.OK));
                            return;
                        }

                        RequestTokens.MergeWith(accessTokens);

                        var keySwapRequest = WebRequest.Create(Path.Combine(OAuthRoot, "access_token.php"));

                        keySwapRequest.Method = "POST";

                        keySwapRequest.SignRequest()
                            .WithTokens(RequestTokens)
                            .InHeader();

                        keySwapRequest.GetOAuthTokensAsync((finalAccessTokens, exAccess) =>
                           {
                               if (exAccess != null)
                               {
                                   UIHelper.SafeDispatch(() => 
                                       MessageBox.Show("Exception:\n" + exAccess, "Error getting access tokens", MessageBoxButton.OK));
                                   return;
                               }

                               RequestTokens.MergeWith(finalAccessTokens);

                               IsAuthenticated = true;
                           });
                    }
                    finally
                    {
                        DisableProgressIndicator();
                    }
                });
        }

        private void GetResponse()
        {
            EnableProgressIndicator("Waiting on term.ie...");
            ResponseText = "Waiting...";
            string methodInput = HttpMethod.ToUpper();
            string urlInput = Path.Combine(OAuthRoot, RelativeUrl);
            if (methodInput.Equals("GET") && !String.IsNullOrEmpty(HttpParameters))
            {
                urlInput += "?" + HttpParameters;
            }
            else if (methodInput.Equals("GET") && !methodInput.Equals("POST"))
            {
                MessageBox.Show("Method must be GET or POST");
                return;
            }

            var request = WebRequest.Create(urlInput);
            request.Method = methodInput;
            var signingRequest = request.SignRequest(RequestTokens).WithEncryption(EncryptionMethod.HMACSHA1);

            Stream requestStream = null;
            StreamWriter requestStreamWriter = null;

            if (methodInput.Equals("POST"))
            {
                if (!String.IsNullOrWhiteSpace(HttpParameters))
                {
                    signingRequest.WithPostParameters(HttpParameters).InHeader();
                }

                request.ContentType = "application/x-www-form-urlencoded";

                request.BeginGetRequestStream(requestResult =>
                {
                    try
                    {
                        requestStream = request.EndGetRequestStream(requestResult);
                        requestStreamWriter = new StreamWriter(requestStream);
                        if (!String.IsNullOrWhiteSpace(HttpParameters))
                        {
                            requestStreamWriter.Write(HttpUtility.UrlEncode(HttpParameters));
                        }
                        requestStreamWriter.Close();
                    }
                    finally
                    {
                        try { if (requestStreamWriter != null) requestStreamWriter.Dispose(); }
                        catch { }
                        try { if (requestStream != null) ((IDisposable)requestStream).Dispose(); }
                        catch { }
                    }

                    HandleResponse(request);
                }, null);
            }
            else
            {
                signingRequest.InHeader();

                HandleResponse(request);
            }
        }

        private void HandleResponse(WebRequest request)
        {
            WebResponse response = null;
            Stream responseStream = null;
            StreamReader responseStreamReader = null;

            request.BeginGetResponse((responseResult) =>
                {
                    try
                    {
                        response = request.EndGetResponse(responseResult);

                        responseStream = response.GetResponseStream();
                        responseStreamReader = new StreamReader(responseStream);
                        ResponseText = responseStreamReader.ReadToEnd();
                    }
                    finally
                    {
                        try { if (response != null) response.Dispose(); }
                        catch { }
                        try { if (responseStream != null) responseStream.Dispose(); }
                        catch { }
                        try { if (responseStreamReader != null) ((IDisposable)responseStreamReader).Dispose(); }
                        catch { }

                        DisableProgressIndicator();
                    }
                }, null);
        }

        private void EnableProgressIndicator(string text)
        {
            IndeterminateProgressMessage = text;
            HasIndeterminateProgress = true;
        }

        private void DisableProgressIndicator()
        {
            HasIndeterminateProgress = false;
        }

        private void HandleLoaded()
        {
            RequestTokens = new Tokens() { ConsumerKey = "key", ConsumerSecret = "secret" };
        }
    }
}

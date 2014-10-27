using System;
using System.ComponentModel;
using SimpleOAuthTester.WP.Mango.Classes;

namespace SimpleOAuthTester.WP.Mango.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                UIHelper.SafeDispatch(() => temp(this, new PropertyChangedEventArgs(propertyName)));
            }
        }
    }
}

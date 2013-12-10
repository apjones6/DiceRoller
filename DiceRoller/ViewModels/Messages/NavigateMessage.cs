using System;
using System.Windows.Navigation;

namespace DiceRoller.ViewModels.Messages
{
    public class NavigateMessage
    {
        private readonly NavigationMode mode;
        private readonly Uri uri;

        public NavigateMessage(NavigationMode mode)
        {
            this.mode = mode;
        }

        public NavigateMessage(string uri)
        {
            this.uri = new Uri(uri, UriKind.Relative);
        }

        public NavigationMode Mode
        {
            get { return mode; }
        }

        public Uri Uri
        {
            get { return uri; }
        }
    }
}

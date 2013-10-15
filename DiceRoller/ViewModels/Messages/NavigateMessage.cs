using System;
using System.Windows.Navigation;

namespace DiceRoller.ViewModels.Messages
{
    public class NavigateMessage
    {
        public NavigateMessage(NavigationMode mode)
        {
            Mode = mode;
        }

        public NavigateMessage(string uri)
        {
            Uri = new Uri(uri, UriKind.Relative);
        }

        public NavigationMode Mode { get; private set; }

        public Uri Uri { get; private set; }
    }
}

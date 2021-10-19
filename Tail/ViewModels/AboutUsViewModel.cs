using System;
using Tail.Common;

namespace Tail.ViewModels
{
    public class AboutUsViewModel: PageViewModelBase
    {
        public string AboutUsUrl
        {
            get { return Constants.ABOUT_US_URL; }
        }
        public AboutUsViewModel()
        {
        }
    }
}

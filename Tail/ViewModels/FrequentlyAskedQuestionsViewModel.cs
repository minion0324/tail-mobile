using System;
using Tail.Common;

namespace Tail.ViewModels
{
    public class FrequentlyAskedQuestionsViewModel : PageViewModelBase
    {
        public FrequentlyAskedQuestionsViewModel()
        {
        }
        public string FaqUrl
        {
            get { return Constants.FAQ_URL; }
        }
    }
}

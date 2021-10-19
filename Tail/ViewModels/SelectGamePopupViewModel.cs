using System.Collections.Generic;
using Tail.Models;

namespace Tail.ViewModels
{
    public class SelectGamePopupViewModel: PageViewModelBase
    {
        IList<GameSchedule> _upcomingGames;
        public IList<GameSchedule> UpcomingGames
        {
            get => _upcomingGames;
            set => SetProperty(ref _upcomingGames, value);
        }
        public SelectGamePopupViewModel()
        {
        }

    }
}

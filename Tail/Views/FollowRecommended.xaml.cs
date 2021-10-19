using System.Threading.Tasks;
using Tail.ViewModels;
namespace Tail.Views
{
    public partial class FollowRecommended :AppPageBase
    {
        readonly  FollowRcommendedViewModel _vModel;
    public FollowRecommended()
        {
            InitializeComponent();
            _vModel = new FollowRcommendedViewModel();
            BindingContext = _vModel;
            recommendedFollowers.ItemAppearing += (sender, e) =>
            {
    
                if (_vModel.CurrentPage <= _vModel.TotalPages)
                {

                   Task.Run(async () =>  await _vModel.GetRecommendedFollowers());
                  
                }
            };
        }
    }
}

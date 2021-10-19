using Tail.Models;
using Tail.Views.Templates;
using Xamarin.Forms;

namespace Tail.Controls
{
    public class ViewAllTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate postDataTemplate;
        private readonly DataTemplate pickDataTemplate;

        public ViewAllTemplateSelector()
        {
            this.postDataTemplate = new DataTemplate(typeof(TrendingPostTemplate));
            this.pickDataTemplate = new DataTemplate(typeof(TrendingPicksTemplate));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var trendDetails = item as TrendPostMain;
            if (trendDetails == null)
                return null;

            if (trendDetails.PostItem.Post_Type == Common.PostType.Pick)
            {
                return this.pickDataTemplate;
            }
            else if (trendDetails.PostItem.Post_Type == Common.PostType.Free)
            {
                return this.postDataTemplate;
            }
            else
            {
                return this.pickDataTemplate;
            }

        }
    }
}

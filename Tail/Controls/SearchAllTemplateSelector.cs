using Tail.Models;
using Tail.Views.Templates;
using Xamarin.Forms;
namespace Tail.Controls
{
    public class SearchAllTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate postDataTemplate;
        private readonly DataTemplate pickDataTemplate;
        public SearchAllTemplateSelector()
        {
            this.postDataTemplate = new DataTemplate(typeof(SearchPostTemplate));
            this.pickDataTemplate = new DataTemplate(typeof(SearchResultPicksTemplate));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var trendDetails = item as PostDetailsMainModel;
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

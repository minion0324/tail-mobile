using Tail.Models;
using Tail.Views.Templates;
using Xamarin.Forms;
namespace Tail.Controls
{
    public class ViewAllPeopleTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate peopleDataTemplate;
       
        public ViewAllPeopleTemplateSelector()
        {
            this.peopleDataTemplate = new DataTemplate(typeof(TrendingPeopleTemplate));
           
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var peopleDetails = item as TrendingUserBase;
            if (peopleDetails == null)
                return null;
            return this.peopleDataTemplate;
        }
    }
}

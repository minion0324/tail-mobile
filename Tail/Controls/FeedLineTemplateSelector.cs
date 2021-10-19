using Tail.Views.Templates;
using Tail.Models;
using Xamarin.Forms;

namespace Tail.Controls
{
    public class FeedLineTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate postSomethingDataTemplate;
        private readonly DataTemplate pickDataTemplate;
        private readonly DataTemplate shareDataTemplate;
        private readonly DataTemplate sharePickDataTemplate;
        public FeedLineTemplateSelector()
        {
            this.postSomethingDataTemplate = new DataTemplate(typeof(PostSomething));
           
            this.pickDataTemplate = new DataTemplate(typeof(PostPick));
            this.shareDataTemplate = new DataTemplate(typeof(ShareTemplate));
            this.sharePickDataTemplate = new DataTemplate(typeof(SharePickTemplate));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var postDetails = item as PostDetailsMainModel;
            if (postDetails == null)
                return null;
            if (postDetails.PostItem.IsShare)
            {
                if (postDetails.PostItem.Post_Type == Common.PostType.Free)
                {
                    return this.shareDataTemplate;
                }
                else if (postDetails.PostItem.Post_Type == Common.PostType.Pick)
                {
                    return this.sharePickDataTemplate;
                }
                else
                {
                    return this.shareDataTemplate;
                }
            }
            else
            {
              
                if (postDetails.PostItem.Post_Type == Common.PostType.Free)
                {
                    postSomethingDataTemplate.SetBinding(PostSomething.CreateImageProperty, "PostItem.CreateImage");
                    postSomethingDataTemplate.SetBinding(PostSomething.IsPlayVisibleProperty, "PostItem.IsPlayEnable");
                    return this.postSomethingDataTemplate;
                }
                else if (postDetails.PostItem.Post_Type == Common.PostType.Pick)
                {
                    pickDataTemplate.SetBinding(PostPick.CreateImageProperty, "PostItem.CreateImage");
                    pickDataTemplate.SetBinding(PostSomething.IsPlayVisibleProperty, "PostItem.IsPlayEnable");
                    return this.pickDataTemplate;
                }
                else
                {
                    postSomethingDataTemplate.SetBinding(PostSomething.CreateImageProperty, "postDetails.PostItem.CreateImage");
                    postSomethingDataTemplate.SetBinding(PostSomething.IsPlayVisibleProperty, "PostItem.IsPlayEnable");
                    return this.postSomethingDataTemplate;
                }
            }

          

        }
    }
}

using System;
using System.Diagnostics;
using Tail.Common;
using Tail.Models;
using Tail.Views.Templates;
using Xamarin.Forms;

namespace Tail.Controls
{
    public class PredictionItemSelector: DataTemplateSelector
    {
        private readonly DataTemplate predictionItemDataTemplate;
       


        public PredictionItemSelector()
        {
            try
            {
                this.predictionItemDataTemplate = new DataTemplate(typeof(PredictionTemplate));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(AppResources.AppName, ex.Message);
            }
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var profiledetails = item as Predictions;
            if (profiledetails == null)
                return null;
            else
                return this.predictionItemDataTemplate;
            

        }
    }
}
    


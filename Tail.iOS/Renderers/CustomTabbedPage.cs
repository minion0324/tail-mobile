using System;
using Tail.iOS.Renderers;
using Tail.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using System.Linq;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(CustomTab), typeof(CustomTabbedPage))]
namespace Tail.iOS.Renderers
{
    public class CustomTabbedPage : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            
               
                if (TabBar == null ||TabBar?.Items == null)
                    return;

                var tabs = Element as TabbedPage;
                if (tabs != null)
                {
                    for (int i = 0; i < TabBar.Items.Length; i++)
                    {
                        UpdateTabBarItem(TabBar.Items[i], tabs.Children[i].IconImageSource);
                    }
                }

                base.ViewWillAppear(animated);
           
        }

        private void UpdateTabBarItem(UITabBarItem item, ImageSource icon)
        {
            try
            {

                if (item == null || icon == null)
                    return;
                var source = icon as FileImageSource;

                if (source != null && source.File != "create_post")
                {

                    item.ImageInsets = new UIEdgeInsets(10, 0, -10, 0);
                    string SelectedFileName = source.File + "_selected.png";
                    UIImage Selected_Image = UIImage.FromFile(SelectedFileName);
                    item.SelectedImage = Selected_Image;
                    item.Image = item.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    item.SelectedImage = item.SelectedImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                    item.SelectedImage.AccessibilityIdentifier = SelectedFileName;
                }
                else
                {
                    CreatePlusButton(item, source);
                }

                this.ShouldSelectViewController += (UITabBarController tabBarController, UIViewController viewController) =>
                {
                    if (viewController == tabBarController.ViewControllers[2])
                    {
                        return false;
                    }
                    return true;
                };

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
        private void CreatePlusButton(UITabBarItem item,FileImageSource source)
        {
            item.Image = null;
            double height = UIScreen.MainScreen.Bounds.Height * UIScreen.MainScreen.Scale;
            var btn = this.View.Subviews.OfType<UIButton>().FirstOrDefault();
            if (btn == null && source != null)
            {
                UIButton _plusButton = new UIButton(frame: new CGRect(0, 0, 80, 80));
                CGPoint center = this.TabBar.Center;

                if (height == 1136 || height == 1334 || height == 2208)
                {
                    _plusButton = new UIButton(frame: new CGRect(0, 0, 70, 70));
                    center.Y = center.Y + 45;
                }
                else
                {
                    center.Y = center.Y + 40;
                }
                _plusButton.Center = center;
                this.View.Add(_plusButton);

                //customize button
                _plusButton.ClipsToBounds = true;
                _plusButton.Layer.CornerRadius = 30;
                string _fileName = source.File + ".png";
                UIImage _plusImage = UIImage.FromFile(_fileName);
                _plusButton.SetBackgroundImage(_plusImage, UIControlState.Normal);
                _plusButton.AdjustsImageWhenHighlighted = false;
                _plusButton.TouchUpInside += (sender, ex) =>
                {
                    MessagingCenter.Send<App>((App)Xamarin.Forms.Application.Current, "PlusClick");
                };
            }
        }
    }
}

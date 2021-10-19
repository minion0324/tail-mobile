using Android.Runtime;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using System;
using System.ComponentModel;
using Android.Content;
using Tail.Droid.Renderers;

[assembly: ExportRenderer(typeof(Tail.Controls.CircleImage), typeof(ImageCircleRenderer))]
namespace Tail.Droid.Renderers
{
    /// <summary>
    /// ImageCircle Implementation
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ImageCircleRenderer : ImageRenderer
    {
#pragma warning disable CS0618 // Type or member is obsolete
        public ImageCircleRenderer() : base()
#pragma warning restore CS0618 // Type or member is obsolete
        {
    
        }

        public ImageCircleRenderer(Context context) : base(context)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null && (int)global::Android.OS.Build.VERSION.SdkInt < 21)
            {

                SetLayerType(LayerType.Software, null);

            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


            if (e.PropertyName == Tail.Controls.CircleImage.BorderColorProperty.PropertyName ||
              e.PropertyName == Tail.Controls.CircleImage.BorderThicknessProperty.PropertyName ||
              e.PropertyName == Tail.Controls.CircleImage.FillColorProperty.PropertyName)
            {
                Invalidate();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="child"></param>
        /// <param name="drawingTime"></param>
        /// <returns></returns>
        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            try
            {

                var radius = (float)Math.Min(Width, Height) / 2f;

                var borderThickness = ((Tail.Controls.CircleImage)Element).BorderThickness;

                var strokeWidth = 0f;

                if (borderThickness > 0)
                {
                    var logicalDensity = global::Android.App.Application.Context.Resources.DisplayMetrics.Density;
                    strokeWidth = (float)Math.Ceiling(borderThickness * logicalDensity + .5f);
                }

                radius -= strokeWidth / 2f;




                var path = new Path();
                path.AddCircle(Width / 2.0f, Height / 2.0f, radius, Path.Direction.Ccw);


                canvas.Save();
                canvas.ClipPath(path);



                var paint = new Paint
                {
                    AntiAlias = true
                };
                paint.SetStyle(Paint.Style.Fill);
                paint.Color = ((Tail.Controls.CircleImage)Element).FillColor.ToAndroid();
                canvas.DrawPath(path, paint);
                paint.Dispose();


                var result = base.DrawChild(canvas, child, drawingTime);

                path.Dispose();
                canvas.Restore();

                path = new Path();
                path.AddCircle(Width / 2f, Height / 2f, radius, Path.Direction.Ccw);


                if (strokeWidth > 0.0f)
                {
                    paint = new Paint
                    {
                        AntiAlias = true,
                        StrokeWidth = strokeWidth
                    };
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.Color = ((Tail.Controls.CircleImage)Element).BorderColor.ToAndroid();
                    canvas.DrawPath(path, paint);
                    paint.Dispose();
                }

                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}
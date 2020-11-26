using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Java.Lang;
using Object = Java.Lang.Object;

namespace QuickDate.Activities.UserProfile.Adapters
{
    public class MultiImagePagerAdapter : PagerAdapter
    {
        public List<string> Images;
        public LayoutInflater Inflater;
        private readonly Activity ActivityContext;
        private readonly RequestOptions GlideRequestOptions;

        public MultiImagePagerAdapter(Activity context, List<string> images)
        {
            ActivityContext = context;
            Images = images;
            Inflater = LayoutInflater.From(context);
            GlideRequestOptions = new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).SetPriority(Bumptech.Glide.Priority.High);
        }

        public override bool IsViewFromObject(View view, Object @object)
        {
            return view.Equals(@object);
        }

        public override int Count
        {
            get
            {
                if (Images != null)
                {
                    return Images.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        public override Object InstantiateItem(ViewGroup view, int position)
        {
            try
            {
                View imageLayout = Inflater.Inflate(Resource.Layout.Style_ImageNormalView, view, false);
                ImageView imageView = imageLayout.FindViewById<ImageView>(Resource.Id.image);
               
                Glide.With(ActivityContext).AsBitmap().Apply(GlideRequestOptions).Transition(new BitmapTransitionOptions().CrossFade(100)).Load(Images[position]).Into(imageView);

                view.AddView(imageLayout, 0);
                return imageLayout;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public override void DestroyItem(ViewGroup container, int position, Object @object)
        {
            container.RemoveView((View)@object);
        }

        public override void RestoreState(IParcelable state, ClassLoader loader)
        {

        }

    }
}
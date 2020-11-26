﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AT.Markushi.UI;
using Bumptech.Glide;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Load.Resource.Bitmap;
using Bumptech.Glide.Request;
using Java.Util;
using QuickDate.Helpers.Utils;
using QuickDateClient.Classes.Global;
using IList = System.Collections.IList;
using Object = Java.Lang.Object;

namespace QuickDate.Activities.HotOrNot.Adapters
{
    public class HotOrNotUserAdapter : RecyclerView.Adapter, ListPreloader.IPreloadModelProvider
    {
        #region Variables Basic

        public readonly Activity ActivityContext;
        public ObservableCollection<UserInfoObject> UsersDateList = new ObservableCollection<UserInfoObject>();
        public event EventHandler<HotOrNotUserAdapterClickEventArgs> ImageItemClick;
        public event EventHandler<HotOrNotUserAdapterClickEventArgs> HotItemClick;
        public event EventHandler<HotOrNotUserAdapterClickEventArgs> NotItemClick;
        public event EventHandler<HotOrNotUserAdapterClickEventArgs> OnItemClick;
        public event EventHandler<HotOrNotUserAdapterClickEventArgs> OnItemLongClick;
        private readonly RequestBuilder FullGlideRequestBuilder;
        public RequestOptions GlideRequestOptions;
        #endregion

        public HotOrNotUserAdapter(Activity context)
        {
            try
            {
                ActivityContext = context;
                GlideRequestOptions = new RequestOptions().SetDiskCacheStrategy(DiskCacheStrategy.All).SetPriority(Bumptech.Glide.Priority.High);
                FullGlideRequestBuilder = Glide.With(context).AsBitmap().Apply(GlideRequestOptions).Transition(new BitmapTransitionOptions().CrossFade(100));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override int ItemCount => UsersDateList?.Count ?? 0;

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            try
            {
                //Setup your layout here >> Style_HotOrNotView
                var itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Style_HotOrNotView, parent, false);
                var vh = new HotOrNotUserAdapterViewHolder(itemView, ImageClick, NotClick, HotClick, Click, LongClick);
                return vh;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        // Replace the contents of a view (invoked by the layout manager)
        public int lastPosition = 0;
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            try
            {
                if (viewHolder is HotOrNotUserAdapterViewHolder holder)
                {
                    var item = UsersDateList[position];
                    if (item != null)
                    {
                        FullGlideRequestBuilder.Load(item.Avater).Into(holder.Image); 
                        holder.Name.Text = QuickDateTools.GetNameFinal(item);
                        //if (position > lastPosition)
                        //{

                        //    var animation = AnimationUtils.LoadAnimation(ActivityContext, Resource.Animation.fab_scale_up);
                        //    animation.Duration = (1000);
                        //    holder.ItemView.StartAnimation(animation);
                        //    lastPosition = position;
                        //}     
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
         
        public UserInfoObject GetItem(int position)
        {
            return UsersDateList[position];
        }

        public override long GetItemId(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public override int GetItemViewType(int position)
        {
            try
            {
                return position;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 0;
            }
        }

        public void ImageClick(HotOrNotUserAdapterClickEventArgs args)
        {
            ImageItemClick?.Invoke(this, args);
        }
        public void HotClick(HotOrNotUserAdapterClickEventArgs args)
        {
            HotItemClick?.Invoke(this, args);
        }
        public void NotClick(HotOrNotUserAdapterClickEventArgs args)
        {
            NotItemClick?.Invoke(this, args);
        }
        public void Click(HotOrNotUserAdapterClickEventArgs args)
        {
            OnItemClick?.Invoke(this, args);
        }

        public void LongClick(HotOrNotUserAdapterClickEventArgs args)
        {
            OnItemLongClick?.Invoke(this, args);
        }

        public IList GetPreloadItems(int p0)
        {
            try
            {
                var d = new List<string>();
                var item = UsersDateList[p0];

                if (item == null)
                    return Collections.SingletonList(p0);

                if (item.Avater != "")
                {
                    d.Add(item.Avater);
                    return d;
                }

                return d;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Collections.SingletonList(p0);
            }
        }
         
        public RequestBuilder GetPreloadRequestBuilder(Object p0)
        {
            return Glide.With(ActivityContext).Load(p0.ToString())
                .Apply(GlideRequestOptions);
        } 
    }

    public class HotOrNotUserAdapterViewHolder : RecyclerView.ViewHolder 
    {
        #region Variables Basic
        public View MainView { get; }
        public ImageView Image { get; private set; } 
        public TextView Name { get; private set; }
        public CircleButton CloseButton { get; private set; }
        public CircleButton LikeButton { get; private set; }

        #endregion

        public HotOrNotUserAdapterViewHolder(View itemView, Action<HotOrNotUserAdapterClickEventArgs> imageClickListener, Action<HotOrNotUserAdapterClickEventArgs> NotClickListener, Action<HotOrNotUserAdapterClickEventArgs> HotClickListener, Action<HotOrNotUserAdapterClickEventArgs> clickListener, Action<HotOrNotUserAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            try
            {
                MainView = itemView;

                Image = MainView.FindViewById<ImageView>(Resource.Id.imgUser); 
                Name = MainView.FindViewById<TextView>(Resource.Id.txtName); 
                CloseButton = MainView.FindViewById<CircleButton>(Resource.Id.closebutton1);
                LikeButton = MainView.FindViewById<CircleButton>(Resource.Id.likebutton2);

                Image.Click += (sender, e) => imageClickListener(new HotOrNotUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Image = Image });
                LikeButton.Click += (sender, e) => HotClickListener(new HotOrNotUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Image = Image });
                CloseButton.Click += (sender, e) => NotClickListener(new HotOrNotUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Image = Image }); 
                //itemView.Click += (sender, e) => clickListener(new HotOrNotUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Image = Image });
                //itemView.LongClick += (sender, e) => longClickListener(new HotOrNotUserAdapterClickEventArgs { View = itemView, Position = AdapterPosition, Image = Image });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        } 
    }

    public class HotOrNotUserAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public ImageView Image { get; set; }
    }

    public class HotOrNotUsersClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
        public UserInfoObject UserClass { get; set; }
        public HotOrNotUserAdapterViewHolder Holder { get; set; }
    }
}
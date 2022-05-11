using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeApp
{
    public class MusicRowAdapter : BaseAdapter<MusicRow>
    {

        Context context;
        List<MusicRow> objects;


        public MusicRowAdapter(Android.Content.Context context, System.Collections.Generic.List<MusicRow> objects)
        {
            this.context = context;
            this.objects = objects;
        }
        public List<MusicRow> GetList()
        {
            return this.objects;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count
        {
            get { return this.objects.Count; }

        }
        public override MusicRow this[int position]
        {
            get { return this.objects[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            Android.Views.LayoutInflater layoutInflater = ((ChooseMusicActivity)context).LayoutInflater;
            Android.Views.View view = layoutInflater.Inflate(Resource.Layout.choose_music_row, parent, false);
            TextView musicName = view.FindViewById<TextView>(Resource.Id.tvMusicName);
            MusicRow temp = objects[position];

            if (temp != null)
            {
                musicName.Text = temp.getName();
            }

            return view;

        }


    }

}
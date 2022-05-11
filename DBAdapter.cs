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
    public class DBAdapter : BaseAdapter<SolveRecord>
    {
        Android.Content.Context context;
        List<SolveRecord> objects;

        public DBAdapter(Android.Content.Context context, System.Collections.Generic.List<SolveRecord> objects)
        {
            this.context = context;
            this.objects = objects;
        }
        public List<SolveRecord> GetList()
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
        public override SolveRecord this[int position]
        {
            get { return this.objects[position]; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)

        {
            Android.Views.LayoutInflater layoutInflater = ((RecordsActivity)context).LayoutInflater;
            Android.Views.View view = layoutInflater.Inflate(Resource.Layout.dbRow, parent, false);
            ImageView ivSolverPic = view.FindViewById<ImageView>(Resource.Id.ivSolverPic);
            TextView tvFName = view.FindViewById<TextView>(Resource.Id.tvFName);
            TextView tvLName = view.FindViewById<TextView>(Resource.Id.tvLName);
            TextView tvTime = view.FindViewById<TextView>(Resource.Id.tvTime);
            TextView tvShuffle = view.FindViewById<TextView>(Resource.Id.tvShuffle);
            SolveRecord temp = objects[position];
            if (temp != null)//set up the custom row view.
            {
                Android.Graphics.Bitmap b = DBHelper.Base64ToBitmap(temp.solverPic);
                tvFName.Text = "" + temp.solverFname;
                tvLName.Text = temp.solverLname;
                tvTime.Text = temp.solveTime.ToString(); ;
                tvShuffle.Text = temp.shuffle;
                ivSolverPic.SetImageBitmap(b);
            }
            return view;
        }
    }
}

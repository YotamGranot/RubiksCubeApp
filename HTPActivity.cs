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
    [Activity(Label = "HTPActivity")]
    public class HTPActivity : Activity
    {
        Button btnExit;
        RelativeLayout rltv;
        RubikView rubikView;
        RubiksCube cube;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.how_to_play);
            // Create your application here
            btnExit = FindViewById<Button>(Resource.Id.btnExitHowToPlay);
            rltv = FindViewById<RelativeLayout>(Resource.Id.rltvHTP);
            btnExit.Click += BtnExit_Click;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                if (rubikView == null)
                {
                    int w = rltv.Width;
                    int h = rltv.Height;
                    cube = new RubiksCube(w, h);
                    rubikView = new RubikView(this, cube,true);
                    rltv.AddView(rubikView);// connect to PaintView
                }
            }
        }
    }
}
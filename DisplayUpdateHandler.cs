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
    public class DisplayUpdateHandler : Handler
    {
        Context context;
        RubiksCube cube;
        RubikView rubikView;

        public DisplayUpdateHandler(Context context, RubiksCube cube, RubikView rubikView)

        {
            this.cube = cube;
            this.rubikView = rubikView;
            this.context = context;
        }

        public override void HandleMessage(Message msg)
        {
            rubikView.draw(); //draw the cube after a move is made.
        }
    }
}
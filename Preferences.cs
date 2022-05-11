using Android;
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
    public class Preferences
    {
        public static bool musicState = false; //is the music on or off? false if off true if on.
        public static bool cubeHelpButtons = true;////is the nevigation buttons on cube screen on or off? false if off true if on.
        public static int exampleSolveTps = 1; //what is the tps when the computer solves the cube.
        public static int bgMusicResId = Resource.Raw.Mozart; // what is the music.
    }
}
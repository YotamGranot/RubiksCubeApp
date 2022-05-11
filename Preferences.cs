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
        public static void uploadToSP(Android.Content.ISharedPreferences sp)
        {
            var editor = sp.Edit();
            editor.PutBoolean("musicState", musicState);
            editor.PutBoolean("cubeHelpButtons", cubeHelpButtons);
            editor.PutInt("exampleSolveTps", exampleSolveTps);
            editor.PutInt("bgMusicResId", bgMusicResId);

            editor.Commit();
        }
        public static void downloadFromSP(Android.Content.ISharedPreferences sp)
        {
            musicState  = sp.GetBoolean("musicState", false);
            cubeHelpButtons = sp.GetBoolean("cubeHelpButtons", true);
            exampleSolveTps = sp.GetInt("exampleSolveTps", 1);
            bgMusicResId = sp.GetInt("bgMusicResId", Resource.Raw.Mozart);
        }
    }
}
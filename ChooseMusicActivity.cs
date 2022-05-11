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
    [Activity(Label = "ChooseMusicActivity")]
    public class ChooseMusicActivity : Activity, ListView.IOnItemClickListener
    {
        public static List<MusicRow> musicList { get; set; }
        MusicRowAdapter musiRowAdapter;
        ListView lv;
        TextView tv;
        Button btnExit;
        Dictionary<int, string> musicIdDict;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.choose_music);
            musicList = new List<MusicRow>();
            //insert all the available bg music to the list
            musicList.Add(new MusicRow(Resource.Raw.A_new_beginning, "A new Beggining"));
            musicList.Add(new MusicRow(Resource.Raw.creative_minds, "creative minds"));
            musicList.Add(new MusicRow(Resource.Raw.Fur_Elise, "Fur elise"));
            musicList.Add(new MusicRow(Resource.Raw.Mozart, "Mozart"));
            musicList.Add(new MusicRow(Resource.Raw.Vivaldi, "Vivaldi"));
            musicIdDict = new Dictionary<int, string>();
            foreach (MusicRow mr in musicList)
            {
                musicIdDict.Add(mr.getResId(), mr.getName());
            }
            musiRowAdapter = new MusicRowAdapter(this, musicList);
            lv = FindViewById<ListView>(Resource.Id.lv);
            tv = FindViewById<TextView>(Resource.Id.tvPlayingNow);
            tv.Text = tv.Text = "Now playing: " + musicIdDict[Preferences.bgMusicResId];
            btnExit = FindViewById<Button>(Resource.Id.btnExitMusicChoice);
            lv.Adapter = musiRowAdapter; //connect to adapter.
            lv.OnItemClickListener = this;
            btnExit.Click += BtnExit_Click;
        }
        protected override void OnPause()
        {
            base.OnPause();
            Preferences.uploadToSP(MainActivity.sp);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish();
        }

        public void OnItemClick(AdapterView parent, View view, int position, long id)
        {
            Preferences.bgMusicResId = musicList[position].getResId();//change the music in prefernces
            tv.Text = "Now playing: " + musicList[position].getName();
            if (Preferences.musicState) //if the music is active restart the service with new music.
            {
                Intent intent = new Intent(this, typeof(MusicService));
                StopService(intent);
                intent = new Intent(this, typeof(MusicService));
                StartService(intent);
            }
        }


    }
}
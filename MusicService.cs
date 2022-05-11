using Android.App;
using Android.Content;
using Android.Media;
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
    [Service(Label = "MusicService")]   //----write service to menifest file 
    [IntentFilter(new String[] { "com.yourname.MusicService" })]
    public class MusicService : Service
    {
        IBinder binder;
        MediaPlayer mp;
        AudioManager am;

        [Obsolete]
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {

            //play mp3 file
            mp = MediaPlayer.Create(this, Preferences.bgMusicResId);
            mp.Start();

            // control audio levels

            am = (AudioManager)GetSystemService(Context.AudioService);
            am.SetStreamVolume(Stream.Music, 5, VolumeNotificationFlags.PlaySound);//Notification/VoiceCall//Alarm
            return StartCommandResult.Sticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
            binder = new MusicServiceBinder(this);
            return binder;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (mp != null)
            {
                mp.Stop();
                mp.Release();
                mp = null;
            }

        }
    }

    // לא שייך
    public class MusicServiceBinder : Binder
    {
        readonly MusicService service;

        public MusicServiceBinder(MusicService service)
        {
            this.service = service;
        }

        public MusicService GetFirstService()
        {
            return service;
        }
    }
}
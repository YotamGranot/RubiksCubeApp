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
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        Button startMusic, stopMusic, btnCubeHelpOn, btnCubeHelpOff, chooseMusic;
        EditText etTps;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings_screen_layout);
            // Create your application here
            startMusic = FindViewById<Button>(Resource.Id.btnSettingsStartMusic);
            stopMusic = FindViewById<Button>(Resource.Id.btnSettingsStopMusic);
            chooseMusic = FindViewById<Button>(Resource.Id.btnChooseMusic);
            btnCubeHelpOn = FindViewById<Button>(Resource.Id.btnSettingsHelpButtonsOn);
            btnCubeHelpOff = FindViewById<Button>(Resource.Id.btnSettingsHelpButtonsOff);
            etTps = FindViewById<EditText>(Resource.Id.etTps);
            startMusic.Click += StartMusic_Click;
            stopMusic.Click += StopMusic_Click;
            btnCubeHelpOff.Click += BtnCubeHelpOff_Click;
            btnCubeHelpOn.Click += BtnCubeHelpOn_Click;
            etTps.AfterTextChanged += EtTps_AfterTextChanged;
            chooseMusic.Click += ChooseMusic_Click;
            initialize();
        }

        private void ChooseMusic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ChooseMusicActivity));
            StartActivity(intent);
        }

        private void EtTps_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            try
            {
                Preferences.exampleSolveTps = Int32.Parse(etTps.Text);
            }
            catch (FormatException exeption)
            {
                Toast.MakeText(this, exeption.Message, ToastLength.Long);
            }
        }

        private void initialize()
        {
            //initialize the screen and buttons as current preferences
            if (!Preferences.musicState)
            {
                stopMusic.SetBackgroundResource(Resource.Drawable.rededgerect);
                stopMusic.BackgroundTintList = null;
            }
            else
            {
                startMusic.SetBackgroundResource(Resource.Drawable.coloredgerect);
                startMusic.BackgroundTintList = null;
            }
            if (Preferences.cubeHelpButtons)
            {
                btnCubeHelpOn.SetBackgroundResource(Resource.Drawable.coloredgerect);
                btnCubeHelpOn.BackgroundTintList = null;
            }
            else
            {
                btnCubeHelpOff.SetBackgroundResource(Resource.Drawable.rededgerect);
                btnCubeHelpOff.BackgroundTintList = null;
            }
            etTps.Text = Preferences.exampleSolveTps.ToString(); ;
        }
        private void BtnCubeHelpOn_Click(object sender, EventArgs e)
        {
            Preferences.cubeHelpButtons = true;
            btnCubeHelpOn.SetBackgroundResource(Resource.Drawable.coloredgerect);
            btnCubeHelpOn.BackgroundTintList = null;
            btnCubeHelpOff.BackgroundTintList = (Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#ff0000")));

        }

        private void BtnCubeHelpOff_Click(object sender, EventArgs e)
        {
            Preferences.cubeHelpButtons = false;
            btnCubeHelpOff.SetBackgroundResource(Resource.Drawable.rededgerect);
            btnCubeHelpOff.BackgroundTintList = null;
            btnCubeHelpOn.BackgroundTintList = (Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#00ff00")));
        }

        private void StopMusic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MusicService));
            StopService(intent);
            Preferences.musicState = false;
            stopMusic.SetBackgroundResource(Resource.Drawable.rededgerect);
            stopMusic.BackgroundTintList = null;
            startMusic.BackgroundTintList = (Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#00ff00")));
        }

        private void StartMusic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(MusicService));
            StartService(intent);
            Preferences.musicState = true;
            startMusic.SetBackgroundResource(Resource.Drawable.coloredgerect);
            startMusic.BackgroundTintList = null;
            stopMusic.BackgroundTintList = (Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.ParseColor("#ff0000")));
        }
    }
}
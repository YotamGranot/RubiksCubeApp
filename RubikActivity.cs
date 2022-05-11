using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace RubiksCubeApp
{
    [Activity(Label = "RubokActivity")]
    public class RubikActivity : Activity
    {
        RubiksCube cube;
        RelativeLayout relative;
        RubikView rubikView;
        Button btnSave, btnShuffle, btnSolve, btnShow;
        Button btnF, btnFI, btnB, btnBI, btnU, btnUI, btnD, btnDI, btnR, btnRI, btnL, btnLI;
        string shuffle = "";
        Dialog d;
        private EditText etFname;
        private EditText etLname;
        private ImageView dialogPic;
        private Button btnDialogAddPic;
        private Button btnDialogSave;
        private Bitmap bitmap;
        int time = int.MaxValue;
        TextView tvTimer;
        bool validTime = false; //did not use solve or show buttons
        public static Timer timer;
        int m = 0, s = 0, ms = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.cube_layout);

            relative = FindViewById<RelativeLayout>(Resource.Id.rltv);
            InitBtns();//initialize all buttons
            if (!Preferences.cubeHelpButtons)
            {
                DisableBtns();
            }
            else
            {
                EnableBtns();
            }


        }

        private void EnableBtns()
        {
            btnB.Enabled = true;
            btnB.Alpha = 255;
            btnD.Enabled = true;
            btnD.Alpha = 255;
            btnU.Enabled = true;
            btnU.Alpha = 255;
            btnR.Enabled = true;
            btnR.Alpha = 255;
            btnF.Enabled = true;
            btnF.Alpha = 255;
            btnL.Enabled = true;
            btnL.Alpha = 255;
            btnRI.Enabled = true;
            btnRI.Alpha = 255;
            btnLI.Enabled = true;
            btnLI.Alpha = 255;
            btnBI.Enabled = true;
            btnBI.Alpha = 255;
            btnFI.Enabled = true;
            btnFI.Alpha = 255;
            btnDI.Enabled = true;
            btnDI.Alpha = 255;
            btnUI.Enabled = true;
            btnUI.Alpha = 255;
        }

        private void DisableBtns()
        {
            btnB.Enabled = false;
            btnB.Alpha = 0;
            btnD.Enabled = false;
            btnD.Alpha = 0;
            btnU.Enabled = false;
            btnU.Alpha = 0;
            btnR.Enabled = false;
            btnR.Alpha = 0;
            btnF.Enabled = false;
            btnF.Alpha = 0;
            btnL.Enabled = false;
            btnL.Alpha = 0;
            btnRI.Enabled = false;
            btnRI.Alpha = 0;
            btnLI.Enabled = false;
            btnLI.Alpha = 0;
            btnBI.Enabled = false;
            btnBI.Alpha = 0;
            btnFI.Enabled = false;
            btnFI.Alpha = 0;
            btnDI.Enabled = false;
            btnDI.Alpha = 0;
            btnUI.Enabled = false;
            btnUI.Alpha = 0;
        }

        private void InitBtns()
        {
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnShuffle = FindViewById<Button>(Resource.Id.btnShuffle);
            btnSolve = FindViewById<Button>(Resource.Id.btnSolve);
            btnShow = FindViewById<Button>(Resource.Id.btnShow);
            btnF = FindViewById<Button>(Resource.Id.btnFront);
            btnFI = FindViewById<Button>(Resource.Id.btnFrontI);
            btnB = FindViewById<Button>(Resource.Id.btnBack);
            btnBI = FindViewById<Button>(Resource.Id.btnBackI);
            btnU = FindViewById<Button>(Resource.Id.btnUp);
            btnUI = FindViewById<Button>(Resource.Id.btnUpI);
            btnD = FindViewById<Button>(Resource.Id.btnDown);
            btnDI = FindViewById<Button>(Resource.Id.btnDownI);
            btnR = FindViewById<Button>(Resource.Id.btnRight);
            btnRI = FindViewById<Button>(Resource.Id.btnRightI);
            btnL = FindViewById<Button>(Resource.Id.btnLeft);
            btnLI = FindViewById<Button>(Resource.Id.btnLeftI);
            tvTimer = FindViewById<TextView>(Resource.Id.tvTimer);
            btnSave.Click += BtnSave_Click;
            btnShuffle.Click += BtnShuffle_Click;
            btnSolve.Click += BtnSolve_Click;
            btnShow.Click += BtnShow_Click;
            btnF.Click += BtnF_Click;
            btnFI.Click += BtnFI_Click;
            btnB.Click += BtnB_Click;
            btnBI.Click += BtnBI_Click;
            btnU.Click += BtnU_Click;
            btnUI.Click += BtnUI_Click;
            btnD.Click += BtnD_Click;
            btnDI.Click += BtnDI_Click;
            btnR.Click += BtnR_Click;
            btnRI.Click += BtnRI_Click;
            btnL.Click += BtnL_Click;
            btnLI.Click += BtnLI_Click;
            timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += Timer_Elapsed;


        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //each time the timer elapses increase, increase the time counters
        {
            ms += 10;
            if (ms == 1000)
            {
                s++;
                ms = 0;
            }
            if (s == 60)
            {
                m++;
                s = 0;
            }
            RunOnUiThread(() => { tvTimer.Text = String.Format("{0}:{1}:{2}", m, s, (ms / 10)); });
        }

        private void BtnLI_Click(object sender, EventArgs e)
        {
            cube.moveLeftInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnL_Click(object sender, EventArgs e)
        {
            cube.moveLeft(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnRI_Click(object sender, EventArgs e)
        {
            cube.moveRightInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnR_Click(object sender, EventArgs e)
        {
            cube.moveRight(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnDI_Click(object sender, EventArgs e)
        {
            cube.moveDownInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnD_Click(object sender, EventArgs e)
        {
            cube.moveDown(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnUI_Click(object sender, EventArgs e)
        {
            cube.moveUpInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnU_Click(object sender, EventArgs e)
        {
            cube.moveUp(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnBI_Click(object sender, EventArgs e)
        {
            cube.moveBackInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnB_Click(object sender, EventArgs e)
        {
            cube.moveBack(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnFI_Click(object sender, EventArgs e)
        {
            cube.moveFrontInverted(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnF_Click(object sender, EventArgs e)
        {
            cube.moveFront(cube.cube);
            rubikView.draw();
            if (cube.isSolved())
            {
                solved();
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            //show example solve
            Queue<string> q = new Queue<string>();
            cube.solve(q);
            rubikView.draw();
            validTime = false;
            timer.Stop();
            resetTimer();
            DisplayUpdateHandler displayUpdate = new DisplayUpdateHandler(this, cube, rubikView);
            CubeDisplayUpdate cubeDisplayUpdate = new CubeDisplayUpdate(cube, rubikView, displayUpdate, q);
            cubeDisplayUpdate.Start();
        }




        private void BtnSolve_Click(object sender, EventArgs e)
        {
            cube.initializeCube();
            validTime = false;
            timer.Stop();
            rubikView.draw();
            resetTimer();
        }
        private void resetTimer()
        {
            timer.Dispose();
            timer = new Timer();
            timer.Interval = 10;
            timer.Elapsed += Timer_Elapsed;
            m = 0;
            s = 0;
            ms = 0;
        }
        private void BtnShuffle_Click(object sender, EventArgs e)
        {
            shuffle = "";
            shuffle = rubikView.shuffle(shuffle);
            if (true) ;
            rubikView.draw();
            validTime = true;
            timer.Stop();
            resetTimer();
            timer.Start();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Finish();
        }
        public void solved()
        {
            if (validTime)
            {
                timer.Stop();
                time = 60 * m + s;
                d = new Dialog(this);
                d.SetContentView(Resource.Layout.insert_record_layout);
                d.SetTitle("Insert Record");
                d.SetCancelable(true);
                etFname = d.FindViewById<EditText>(Resource.Id.etFname);
                etLname = d.FindViewById<EditText>(Resource.Id.etLname);
                dialogPic = d.FindViewById<ImageView>(Resource.Id.ivImage);
                btnDialogAddPic = d.FindViewById<Button>(Resource.Id.btnDialogAddPic);
                btnDialogSave = d.FindViewById<Button>(Resource.Id.btnDialogSave);
                TextView tv = d.FindViewById<TextView>(Resource.Id.tvDialogTime);
                tv.Text = time.ToString();
                bitmap = Android.Graphics.BitmapFactory.DecodeResource(Application.Context.Resources, Resource.Drawable.cube);
                btnDialogAddPic.Click += BtnDialogAddPic_Click;
                btnDialogSave.Click += BtnDialogSave_Click;
                d.Show();
                d.Window.SetLayout(800, 1500);
            }
        }

        private void BtnDialogAddPic_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Android.Provider.MediaStore.ActionImageCapture);

            StartActivityForResult(intent, 0);
        }

        private void BtnDialogSave_Click(object sender, EventArgs e)
        {

            string fname = etFname.Text;
            string lname = etLname.Text;
            SolveRecord t = null;
            var db = new SQLiteConnection(DBHelper.Path());


            t = new SolveRecord(fname, lname, time, shuffle, DBHelper.BitmapToBase64(bitmap));
            db.Insert(t);
            RecordsActivity.solveRecords.Add(t);
            d.Dismiss();
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0)//coming from camera
            {
                if (resultCode == Result.Ok)
                {
                    bitmap = (Android.Graphics.Bitmap)data.Extras.Get("data");
                    dialogPic.SetImageBitmap(bitmap);
                }

            }
        }
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            if (hasFocus)
            {
                if (rubikView == null)
                {
                    int w = relative.Width;
                    int h = relative.Height;
                    cube = new RubiksCube(w, h);
                    rubikView = new RubikView(this, cube);
                    relative.AddView(rubikView);// connect to PaintView
                }
            }
        }

    }
}
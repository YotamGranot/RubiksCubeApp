using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using SQLite;
using System.Threading.Tasks;

namespace RubiksCubeApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, Android.Views.View.IOnClickListener
    {
        Button btnCube, btnData, btnSettings, btnHTP;
        const int RequestLocationId = 0;
        public static Android.Content.ISharedPreferences sp;
        readonly string[] Permission =//ask for permissions
            {
                Manifest.Permission.ReadPhoneState,
             };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TryToGetPermissions();
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.menu_layout);
            // Set our view from the "main" layout resource
            var db = new SQLiteConnection(DBHelper.Path());
            db.CreateTable<SolveRecord>(); //create db if not exist
            sp = this.GetSharedPreferences("preferences", Android.Content.FileCreationMode.Private);
            Preferences.downloadFromSP(sp);
            if (Preferences.musicState)
            {
                Intent intent = new Intent(this, typeof(MusicService));
                StartService(intent);
            }
            RecordsActivity.solveRecords = RecordsActivity.getAllRecords();
            RecordsActivity.dbAdapter = new DBAdapter(this, RecordsActivity.solveRecords);
            btnCube = FindViewById<Button>(Resource.Id.btnCube);
            btnData = FindViewById<Button>(Resource.Id.btnData);
            btnSettings = FindViewById<Button>(Resource.Id.btnSettings);
            btnHTP = FindViewById<Button>(Resource.Id.btnHowTo);
            btnCube.SetOnClickListener(this);
            btnData.SetOnClickListener(this);
            btnSettings.SetOnClickListener(this);
            btnHTP.SetOnClickListener(this);

        }


        public void OnClick(View v) //user can choose what screen he wants
        {
            if (btnCube == v)
            {
                Intent intent = new Intent(this, typeof(RubikActivity));
                StartActivity(intent);
            }
            else if (btnData == v)
            {
                Intent intent = new Intent(this, typeof(RecordsActivity));
                StartActivity(intent);
            }
            else if (btnSettings == v)
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                StartActivity(intent);
            }
            else if (btnHTP == v)
            {
                Intent intent = new Intent(this, typeof(HTPActivity));
                StartActivity(intent);
            }
        }
        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }


        }
        
        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.ReadPhoneState;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(Permission, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(Permission, RequestLocationId);

        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            Toast.MakeText(this, "Special permissions granted", ToastLength.Short).Show();

                        }
                        else
                        {
                            //Permission Denied 
                            Toast.MakeText(this, "Special permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }
}
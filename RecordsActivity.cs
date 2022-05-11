using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeApp
{
    [Activity(Label = "RecordsActivity")]
    public class RecordsActivity : Activity
    {
        public static List<SolveRecord> solveRecords { get; set; }
        public static DBAdapter dbAdapter;
        ListView lv;
        //Button btnGo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.record_layout);
            var db = new SQLiteConnection(DBHelper.Path());
            db.CreateTable<SolveRecord>();

            solveRecords = getAllRecords();
            dbAdapter = new DBAdapter(this, solveRecords);
            lv = FindViewById<ListView>(Resource.Id.lv);
            lv.Adapter = dbAdapter;

        }
        protected override void OnResume()
        {
            base.OnResume();
            dbAdapter.NotifyDataSetChanged();
        }

        public static List<SolveRecord> getAllRecords() // send sql query to get all of the solves from db ordered by the solve time.
        {
            var db = new SQLiteConnection(DBHelper.Path());
            string strsql = string.Format("SELECT * FROM Solves ORDER BY solveTime ASC");
            var records = db.Query<SolveRecord>(strsql);
            solveRecords = new List<SolveRecord>();
            if (records.Count > 0)
            {
                foreach (var item in records)
                {
                    solveRecords.Add(item);

                }
            }
            return solveRecords;
        }
    }
}
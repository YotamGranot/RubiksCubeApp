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
using SQLite;

namespace RubiksCubeApp
{
    [Table("Solves")]

    public class SolveRecord
    {

        [PrimaryKey, AutoIncrement, Column("_id")]
        public int id { get; set; }
        public string solverFname { get; set; }
        public string solverLname { get; set; }
        public double solveTime { get; set; }
        public string shuffle { get; set; }
        public string solverPic { get; set; }
        public SolveRecord()
        {
        }
        public SolveRecord(string solverFname, string solverLname, int solveTime, string shuffle, string solverPic)
        {
            setSolveRecord(solverFname, solverLname, solveTime, shuffle, solverPic);
        }
        public void setSolveRecord(string solverFname, string solverLname, int solveTime, string shuffle, string solverPic)
        {
            this.solverFname = solverFname;
            this.solverLname = solverLname;
            this.solveTime = solveTime;
            this.shuffle = shuffle;
            this.solverPic = solverPic;
        }

    }
}
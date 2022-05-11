using Android.App;
using Android.Content;
using Android.Graphics;
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
    public class Solver : RubiksCube
    {
        public Solver()
            : base(true)
        {

        }
        public void solve(Color[,] cube, Queue<String> queue)
        {

            BottomSolver bs = new BottomSolver();
            bs.solveBottom(cube, queue);
            SecondLSolver ssl = new SecondLSolver();
            ssl.solveSecond(cube, queue);
            TopSolver ts = new TopSolver();
            ts.solveTop(cube, queue);
        }

    }
}
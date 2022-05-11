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
using System.Threading;

namespace RubiksCubeApp
{
    public class CubeDisplayUpdate
    {
        RubiksCube cube;
        Dictionary<string, Action<Color[,]>> d;
        RubikView rubikView;
        Handler handler;
        Queue<string> q;
        public CubeDisplayUpdate(RubiksCube cube, RubikView rubikView, Handler handler, Queue<string> q)
        {
            this.cube = cube;
            this.rubikView = rubikView;
            setUpDict();// set up dictionery to translate the letter rep the cube move to the correct action
            this.handler = handler;
            this.q = q;
        }
        public void Start()
        {

            ThreadStart threadStart = new ThreadStart(Run);

            Thread t = new Thread(threadStart);

            t.Start();

        }

        void Run()
        {

            foreach (string s in q) // for each string in queue invoke the correct action and sleep according to the tps the user chose
            {
                d[s].Invoke(cube.cube);
                Thread.Sleep((int)(1000.0 / ((double)(Preferences.exampleSolveTps))));

                Message msg = new Message();


                handler.SendMessage(msg);

            }

        }

        private void setUpDict()
        {
            d = new Dictionary<string, Action<Color[,]>>();
            d.Add("r", cube.moveRight);
            d.Add("r'", cube.moveRightInverted);
            d.Add("l", cube.moveLeft);
            d.Add("l'", cube.moveLeftInverted);
            d.Add("f", cube.moveFront);
            d.Add("f'", cube.moveFrontInverted);
            d.Add("m", cube.moveMiddle);
            d.Add("m'", cube.moveMiddleInverted);
            d.Add("b", cube.moveBack);
            d.Add("b'", cube.moveBackInverted);
            d.Add("x", cube.moveX);
            d.Add("x'", cube.moveXInverted);
            d.Add("u", cube.moveUp);
            d.Add("u'", cube.moveUpInverted);
            d.Add("d", cube.moveDown);
            d.Add("d'", cube.moveDownInverted);
            d.Add("y", cube.moveY);
            d.Add("y'", cube.moveYInverted);
            d.Add("z", cube.moveZ);
            d.Add("z'", cube.moveZInverted);
            d.Add("s", cube.moveS);
            d.Add("s'", cube.moveSInverted);
        }
    }

}

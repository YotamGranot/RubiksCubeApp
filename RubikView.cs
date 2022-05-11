using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeApp
{
    public class RubikView : View
    {
        RubiksCube rubiksCube;
        bool touchDown = false, touchUp = false;
        float xDown = 0, yDown = 0, xUp = 0, yUp = 0;

        public RubikView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public RubikView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }
        public RubikView(Context context, RubiksCube rubiksCube)
           : base(context)
        {
            this.rubiksCube = rubiksCube;
            Initialize();
        }

        private void Initialize()
        {
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (MotionEventActions.Down == e.Action)
            {
                xDown = (int)e.GetX();
                yDown = (int)e.GetY();
                touchDown = true;
            }
            if (MotionEventActions.Up == e.Action)
            {
                xUp = (int)e.GetX();
                yUp = (int)e.GetY();
                touchUp = true;
            }
            if (touchUp & touchDown)
            {
                processTouch(xDown, yDown, xUp, yUp);
                touchDown = false;
                touchUp = false;
            }
            Invalidate();
            return true;
        }

        private void processTouch(float xDown, float yDown, float xUp, float yUp)
        {
            (int, int) Downloc = findLocation(xDown, yDown); //finds on what piece user pointed
            if (Downloc.Equals((-1, -1)))
            { //didnt touch a part
                return;
            }
            (int, int) Uploc = findLocation(xUp, yUp);
            if (Uploc.Equals((-1, -1)))
            { //didnt touch a part
                return;
            }
            if (!(Uploc.Item1 == Downloc.Item1)) //rotate cube
            {
                //to make a move you must start and end on same side
                // to rotate the cube you slide your finger from center to center.
                //move X
                if (Downloc.Item1 == 0 && Uploc.Item1 == 1)
                {
                    rubiksCube.moveX(rubiksCube.cube);
                    rubiksCube.moveX(rubiksCube.cube);
                }
                //move X Inverted
                else if (Downloc.Item1 == 1 && Uploc.Item1 == 0)
                {
                    rubiksCube.moveXInverted(rubiksCube.cube);
                }
                //move Y 
                else if (Downloc.Item1 == 1 && Uploc.Item1 == 2)
                {
                    rubiksCube.moveY(rubiksCube.cube);
                }
                //move Y Inverted
                else if (Downloc.Item1 == 2 && Uploc.Item1 == 1)
                {
                    rubiksCube.moveYInverted(rubiksCube.cube);
                }
                //move Z 
                else if (Downloc.Item1 == 0 && Uploc.Item1 == 2)
                {
                    rubiksCube.moveZ(rubiksCube.cube);
                }
                //move Z Inverted
                else if (Downloc.Item1 == 2 && Uploc.Item1 == 0)
                {
                    rubiksCube.moveZInverted(rubiksCube.cube);
                }
                return;
            }

            if (Uploc.Item1 == 0)
            {
                //move Front
                if ((Downloc.Item2 % 3 == 0 && Uploc.Item2 % 3 == 0))
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveFront(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveFrontInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //move Back
                if ((Downloc.Item2 % 3 == 2 && Uploc.Item2 % 3 == 2))
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveBack(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveBackInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //move S
                if ((Downloc.Item2 % 3 == 1 && Uploc.Item2 % 3 == 1))
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveS(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveSInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //move Left
                if ((Downloc.Item2 / 3 == 0 && Uploc.Item2 / 3 == 0))
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveLeftInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveLeft(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                // move right
                if ((Downloc.Item2 / 3 == 2 && Uploc.Item2 / 3 == 2))
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveRightInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveRight(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //move middle
                if ((Downloc.Item2 / 3 == 1 && Uploc.Item2 / 3 == 1))
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveMiddleInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveMiddle(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                return;
            }
            if (Uploc.Item1 == 1)
            {
                //left
                if (Downloc.Item2 % 3 == 0 && Uploc.Item2 % 3 == 0) //left
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveLeft(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveLeftInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //middle
                if (Downloc.Item2 % 3 == 1 && Uploc.Item2 % 3 == 1) //middle
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveMiddle(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveMiddleInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //right
                if (Downloc.Item2 % 3 == 2 && Uploc.Item2 % 3 == 2) //right
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveRight(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveRightInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //up
                if (Downloc.Item2 / 3 == 0 && Uploc.Item2 / 3 == 0) //Up
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveUp(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveUpInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //down
                if (Downloc.Item2 / 3 == 2 && Uploc.Item2 / 3 == 2) //Down
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveDown(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveDownInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //E
                if (Downloc.Item2 / 3 == 1 && Uploc.Item2 / 3 == 1) //E
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveE(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveEInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                return;
            }
            if (Uploc.Item1 == 2)
            {
                //front
                if (Downloc.Item2 % 3 == 0 && Uploc.Item2 % 3 == 0) //front
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveFront(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveFrontInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //S
                if (Downloc.Item2 % 3 == 1 && Uploc.Item2 % 3 == 1) //S
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveS(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveSInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //Back
                if (Downloc.Item2 % 3 == 2 && Uploc.Item2 % 3 == 2) //Back
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveBack(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveBackInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //up
                if (Downloc.Item2 / 3 == 0 && Uploc.Item2 / 3 == 0) //Up
                {
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveUp(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveUpInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //down
                if (Downloc.Item2 / 3 == 2 && Uploc.Item2 / 3 == 2) //Down
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveDown(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveDownInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }
                //E
                if (Downloc.Item2 / 3 == 1 && Uploc.Item2 / 3 == 1) //E
                {
                    if (Uploc.Item2 > Downloc.Item2)
                    {
                        rubiksCube.moveE(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                    if (Uploc.Item2 < Downloc.Item2)
                    {
                        rubiksCube.moveEInverted(rubiksCube.cube);
                        checkSolve();
                        return;
                    }
                }

                return;
            }
        }

        internal string shuffle(string shuffle)
        {
            Random random = new Random();
            int move;
            for (int i = 0; i < 30; i++)
            {
                move = random.Next(0, 18);

                switch (move)
                {
                    case 0:
                        rubiksCube.moveBack(rubiksCube.cube);
                        shuffle += "B,";

                        break;
                    case 1:
                        rubiksCube.moveBackInverted(rubiksCube.cube);
                        shuffle += "B',";

                        break;
                    case 2:
                        rubiksCube.moveFront(rubiksCube.cube);
                        shuffle += "F,";

                        break;
                    case 3:
                        rubiksCube.moveFrontInverted(rubiksCube.cube);
                        shuffle += ("F',");

                        break;
                    case 4:
                        rubiksCube.moveRight(rubiksCube.cube);
                        shuffle += ("R,");

                        break;
                    case 5:
                        rubiksCube.moveRightInverted(rubiksCube.cube);
                        shuffle += ("R',");

                        break;
                    case 6:
                        rubiksCube.moveLeft(rubiksCube.cube);
                        shuffle += ("L,");

                        break;
                    case 7:
                        rubiksCube.moveLeftInverted(rubiksCube.cube);
                        shuffle += ("L',");

                        break;
                    case 8:
                        rubiksCube.moveUp(rubiksCube.cube);
                        shuffle += "U,";

                        break;
                    case 9:
                        rubiksCube.moveUpInverted(rubiksCube.cube);
                        shuffle += "U',";
                        break;
                    case 10:
                        rubiksCube.moveDown(rubiksCube.cube);
                        shuffle += "D,";

                        break;
                    case 11:
                        rubiksCube.moveDownInverted(rubiksCube.cube);
                        shuffle += ("D',");

                        break;
                    case 12:
                        rubiksCube.moveS(rubiksCube.cube);
                        shuffle += ("S,");
                        break;
                    case 13:
                        rubiksCube.moveSInverted(rubiksCube.cube);
                        shuffle += "S',";

                        break;
                    case 14:
                        rubiksCube.moveE(rubiksCube.cube);
                        shuffle += "E,";

                        break;
                    case 15:
                        rubiksCube.moveEInverted(rubiksCube.cube);
                        shuffle += ("E',");

                        break;
                    case 16:
                        rubiksCube.moveMiddle(rubiksCube.cube);
                        shuffle += "M,";

                        break;
                    case 17:
                        rubiksCube.moveMiddleInverted(rubiksCube.cube);
                        shuffle += "M',";

                        break;
                }

            }
            return shuffle.Remove(shuffle.Length - 1);
        }

        private (int, int) findLocation(float xDown, float yDown)
        {
            return rubiksCube.findLocation(xDown, yDown);
        }

        public void draw()
        {
            Invalidate();
        }
        protected override void OnDraw(Canvas canvas)
        {

            base.OnDraw(canvas);

            Paint p = new Paint();
            p.Color = Color.Black;
            p.StrokeWidth = 30;

            rubiksCube.draw(canvas, p);



        }
        private void checkSolve()
        {
            if (rubiksCube.isSolved())
            {
                RubikActivity activity = (RubikActivity)Context;
                activity.solved();
            }
        }

    }
}
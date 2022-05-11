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
using Android.Graphics;

namespace RubiksCubeApp
{
    public class BottomSolver : Solver
    {
        public void solveBottom(Color[,] cube, Queue<string> queue)
        {
            orientCube(cube, queue); //first step solving bottom layer is orienting the cube, white face up, orange front.
            orientCross(cube, queue);// second step is to solve the white cross pieces.
            orientCube(cube, queue);
            solveFirstLayer(cube, queue); // third stage is to solve the 4 corner pieces.
        }
        private void solveFirstLayer(Color[,] cube, Queue<string> queue)
        {
            moveZ(cube); //place the white face down.
            queue.Enqueue("z");
            moveZ(cube);
            queue.Enqueue("z");
            int cornerCounter = 0;
            (int, int) whiteCornerLocation = findWhiteCorner(cube); //find the next white corner we solve.
            while (whiteCornerLocation != (-1, -1)) //while the corner is not -1,-1 there is an unsolved corner.
            {
                solveWhiteCorner(whiteCornerLocation, cube, queue); // solve the corner.
                cornerCounter++;
                orientCube(cube, queue);
                moveZ(cube);
                queue.Enqueue("z");
                moveZ(cube);
                queue.Enqueue("z");
                Console.WriteLine("Z2");
                whiteCornerLocation = findWhiteCorner(cube); //find another corner.
            }

        }

        private (int, int) findWhiteCorner(Color[,] cube)
        {
            //find unsolved white corner.
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j += 2)
                {
                    if (j == 4)
                    {
                        continue;
                    }
                    if (cube[i, j].Equals(Color.White))
                    {
                        if (!isWhiteCornerSolved(i, j, cube))
                        {
                            return (i, j);
                        }
                    }
                }
            }
            return (-1, -1);

        }

        private void solveWhiteCorner((int, int) whiteCornerLocation, Color[,] cube, Queue<string> queue)
        {
            int i = whiteCornerLocation.Item1; //what side is the white corner.
            int j = whiteCornerLocation.Item2; // what is the location of the corner on the side.
            if (!cube[i, j].Equals(Color.White))
            {
                throw new ArgumentException();
            }
            try
            {
                if (i == 5)
                //if the not solved white corner is on the white side we need to get it out to one of the sides
                //and put it back in the correct location.
                {
                    int c = 0;
                    while (j != 2) //rotate the cube until the corner is in the front right.
                    {
                        c++;
                        moveY(cube);
                        queue.Enqueue("y");

                        if (j == 8)
                        {
                            j = 2;
                        }
                        else if (j == 2)
                        {
                            j = 0;
                        }
                        else if (j == 0)
                        {
                            j = 6;
                        }
                        else if (j == 6)
                        {
                            j = 8;
                        }
                    }
                    RURPUP(cube, queue); // extrarct the corner to the side.
                    int c2 = c;
                    while (c > 0)
                    {
                        moveYInverted(cube);
                        queue.Enqueue("y'");
                        c--;
                    }
                    c2 = mod(5 - c2, 4);
                    c2 = c2 == 0 ? 4 : c2;
                    solveWhiteCornerFromSide(c2, 2, cube, queue); //solve the corner from the side.
                    return;
                }
                if (i == 0)
                //if the corner is on th top we will move it to he side
                {
                    int c = 0;
                    while (j != 6) //bring corner to front right.
                    {

                        c++;
                        moveY(cube);
                        queue.Enqueue("y");
                        if (j == 8)
                        {
                            j = 2;
                        }
                        else if (j == 2)
                        {
                            j = 0;
                        }
                        else if (j == 0)
                        {
                            j = 6;
                        }
                        else if (j == 6)
                        {
                            j = 8;
                        }
                    }
                    RURPUP(cube, queue);
                    RURPUP(cube, queue);//move corner to the side.
                    int c2 = c;
                    while (c > 0)
                    {
                        moveYInverted(cube);
                        queue.Enqueue("y'");
                        c--;
                    }
                    c2 = mod(6 - c2, 4);
                    c2 = c2 == 0 ? 4 : c2;
                    solveWhiteCornerFromSide(c2, 0, cube, queue);//solve the corner to the correct spot.
                    return;
                }
                if (j / 3 == 2)// if the corner is on one of the sides but on one of the pieces down we want to move it up.
                {
                    int c = 0;
                    while (i != 1) //move corner to front
                    {
                        c++;
                        moveY(cube);
                        queue.Enqueue("y");
                        i++;
                        i = mod(i, 4);
                        if (i == 0)
                        {
                            i = 4;
                        }
                    }
                    bool rightSide = false;
                    if (j == 8)// move the corner up
                    {
                        RURPUP(cube, queue);
                        RURPUP(cube, queue);
                        RURPUP(cube, queue);
                        rightSide = true;
                    }
                    else if (j == 6)
                    {
                        LPUPLU(cube, queue);
                        LPUPLU(cube, queue);
                        LPUPLU(cube, queue);
                        rightSide = false;
                    }
                    i = mod(5 - i, 4);
                    i = i == 0 ? 4 : i;

                    while (c > 0)
                    {
                        moveYInverted(cube);
                        queue.Enqueue("y'");
                        c--;

                    }
                    if (rightSide)
                    {
                        int p = whiteCornerLocation.Item1;
                        p++;
                        p = mod(p, 4);
                        p = p == 0 ? 4 : p;

                        solveWhiteCornerFromSide(p, 0, cube, queue);
                        return;
                    }
                    else
                    {
                        int p = whiteCornerLocation.Item1;
                        p--;
                        p = mod(p, 4);
                        p = p == 0 ? 4 : p;

                        solveWhiteCornerFromSide(p, 2, cube, queue);
                        return;

                    }
                }
                //white corner is already in a good spot to be solved from.
                solveWhiteCornerFromSide(i, j, cube, queue);
                return;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void solveWhiteCornerFromSide(int i, int j, Color[,] cube, Queue<string> queue)
        {
            if (!cube[i, j].Equals(Color.White))
            {
                throw new ArgumentException(String.Format("i={0}, j={1}", i, j));
            }
            //we want to look on the color on the same piece next to the side,
            //match it to his center and insert it to the correct spot.
            if (j == 2) //if the corner is on the left of the side.
            {

                j = 0;
                i++;
                i = mod(i, 4);
                if (i == 0)
                {
                    i = 4;
                }
                int c = 0;
                while (!cube[i, j].Equals(cube[i, 4])) //move corner to the side it belongs to.
                {
                    c++;
                    moveUp(cube);
                    queue.Enqueue("u");
                    i--;
                    i = mod(i, 4);
                    if (i == 0)
                    {
                        i = 4;
                    }
                }
                while (i != 1) //rotate the cube until the corner is in the front.
                {
                    moveYInverted(cube);
                    queue.Enqueue("y'");

                    i--;
                    i = mod(i, 4);
                    if (i == 0)
                    {
                        i = 4;
                    }
                }
                LPUPLU(cube, queue);//insert the corner to the correct spot
                return;
            }
            else if (j == 0) //if the corner is on the right of the side.
            {
                j = 2;
                i--;
                i = mod(i, 4);
                if (i == 0)
                {
                    i = 4;
                }
                int c = 0;
                while (!cube[i, j].Equals(cube[i, 4]))//move corner to the side it belongs to.
                {
                    c++;
                    moveUp(cube);
                    queue.Enqueue("u");
                    i--;
                    i = mod(i, 4);
                    if (i == 0)
                    {
                        i = 4;
                    }
                }
                while (i != 1)//rotate the cube until the corner is in the front.
                {
                    moveYInverted(cube);
                    queue.Enqueue("y'");

                    i--;
                    i = mod(i, 4);
                    if (i == 0)
                    {
                        i = 4;
                    }
                }
                RURPUP(cube, queue);//insert the corner to the correct spot
                return;
            }


        }

        private bool isWhiteCornerSolved(int i, int j, Color[,] cube)
        {
            if (i != 5)
            {
                return false;
            }
            if (j / 3 == 0)
            {
                return cube[1, j + 6] == Color.Orange;
            }
            return cube[3, 14 - j] == Color.Red;

        }

        private void orientCross(Color[,] cube, Queue<string> queue)
        {
            if (isCrossOriented(cube))//if the cros happend to be oriented do nothing
            {
                return;
            }
            (int, int) whiteEdgeLocation = findWhiteEdge(cube); // find wrong white edge i.e not on the yellow face.
            while (!(whiteEdgeLocation.Equals((-1, -1))))
            {
                putWhiteOnBottom(whiteEdgeLocation, cube, queue);// move the white edge to the yellow side
                whiteEdgeLocation = findWhiteEdge(cube);
            }
            for (int i = 1; i <= 4; i++) // move all 4 white edges from the yellow side to the white side and correct place.
            {
                int wLoc = 0;
                switch (i)
                {
                    case 1:
                        wLoc = 1;
                        break;
                    case 2:
                        wLoc = 5;

                        break;
                    case 3:
                        wLoc = 7;

                        break;
                    case 4:
                        wLoc = 3;

                        break;
                }
                while (!(cube[i, 7].Equals(cube[i, 4]) && cube[5, wLoc].Equals(Color.White)))
                //move the yellow face until the white edge piece is under the correct position.
                {
                    moveDown(cube);
                    queue.Enqueue("d");

                }
                switch (i)//move the white piece to correct spot.
                {
                    case 1:
                        moveFront(cube);
                        queue.Enqueue("f");
                        moveFront(cube);
                        queue.Enqueue("f");
                        break;
                    case 2:
                        moveRight(cube);
                        moveRight(cube);
                        queue.Enqueue("r");
                        queue.Enqueue("r");

                        break;
                    case 3:
                        moveBack(cube);
                        queue.Enqueue("b");
                        queue.Enqueue("b");
                        moveBack(cube);
                        break;
                    case 4:
                        moveLeft(cube);
                        moveLeft(cube);
                        queue.Enqueue("l");
                        queue.Enqueue("l");
                        break;
                }

            }

        }

        private void putWhiteOnBottom((int, int) loc, Color[,] cube, Queue<string> queue)
        {
            if (loc.Item1 == 5)//already on bottom
            {
                return;
            }
            else
            {
                if (loc.Item1 <= 4 && loc.Item1 >= 1)
                {
                    putWhiteFromSideToBottom(loc, cube, queue);
                    return;
                }
                if (loc.Item1 == 0)
                {
                    putWhiteFromUpToBottom(loc, cube, queue);
                    return;

                }
            }
        }

        private void putWhiteFromUpToBottom((int, int) loc, Color[,] cube, Queue<string> queue)
        {
            while (cube[5, 1].Equals(Color.White))
            //clear the position from white pieces
            {
                moveDown(cube);
                queue.Enqueue("d");
            }
            //move the edge to the front and than down.
            if (loc.Item2 == 1)
            {
                moveUpInverted(cube);
                queue.Enqueue("u'");
                moveFront(cube);
                queue.Enqueue("f");
                moveFront(cube);
                queue.Enqueue("f");
                return;
            }
            else if (loc.Item2 == 5)
            {
                moveUp(cube);
                moveUp(cube);
                queue.Enqueue("u");
                queue.Enqueue("u");
                moveFront(cube);
                moveFront(cube);
                queue.Enqueue("f");
                queue.Enqueue("f");
                return;
            }
            else if (loc.Item2 == 7)
            {
                moveUp(cube);
                queue.Enqueue("u");
                moveFront(cube);
                moveFront(cube);
                queue.Enqueue("f");
                queue.Enqueue("f");
                return;
            }
            else if (loc.Item2 == 3)
            {
                moveFront(cube);
                moveFront(cube);
                queue.Enqueue("f");
                queue.Enqueue("f");
                return;
            }
        }

        private void putWhiteFromSideToBottom((int, int) loc, Color[,] cube, Queue<string> queue)
        {
            while (loc.Item1 != 1)
            //move to the front
            {
                moveY(cube);
                queue.Enqueue("y");
                loc.Item1 = (loc.Item1 + 1) % 4;
            }
            if (loc.Item2 == 1)//if on the top of the side
            {

                while (cube[5, 1].Equals(Color.White))
                {
                    moveDown(cube);
                    queue.Enqueue("d");
                }
                moveFront(cube);
                queue.Enqueue("f");
                while (cube[5, 5].Equals(Color.White))
                {
                    moveDown(cube);
                    queue.Enqueue("d");
                };
                moveRightInverted(cube);
                queue.Enqueue("r'");
                return;
            }
            if (loc.Item2 == 7)//if on bottom of the side
            {
                while (cube[0, 3].Equals(Color.White))
                {
                    moveUp(cube);
                    queue.Enqueue("u");
                }
                moveFrontInverted(cube);
                queue.Enqueue("f'");
                while (cube[5, 5].Equals(Color.White))
                {
                    moveDown(cube);
                    queue.Enqueue("d");
                };
                moveRightInverted(cube);
                queue.Enqueue("r'");
                return;
            }

            if (loc.Item2 == 5)//if on right of the side
            {
                while (cube[5, 5].Equals(Color.White))
                {
                    moveDown(cube);
                    queue.Enqueue("d");
                };
                moveRightInverted(cube);
                queue.Enqueue("r'");
                return;
            }
            if (loc.Item2 == 3)//if on left of the side
            {
                while (cube[5, 3].Equals(Color.White))
                {
                    moveDown(cube);
                    queue.Enqueue("d");
                };
                moveLeft(cube);
                queue.Enqueue("l");
                return;
            }


        }

        private (int, int) findWhiteEdge(Color[,] cube)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 1; j < 9; j += 2)
                {
                    if (cube[i, j].Equals(Color.White))
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        private bool isCrossOriented(Color[,] cube)
        {
            return (cube[0, 1].Equals(Color.White) && cube[0, 3].Equals(Color.White) && cube[0, 5].Equals(Color.White) && cube[0, 7].Equals(Color.White) && cube[0, 0].Equals(Color.White) && cube[1, 1].Equals(Color.Orange) && cube[2, 1].Equals(Color.Green) && cube[3, 1].Equals(Color.Red) && cube[4, 1].Equals(Color.Blue));
        }
    }
}
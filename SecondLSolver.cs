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
    class SecondLSolver : Solver
    {
        public void solveSecond(Color[,] cube, Queue<string> queue)
        {
            ((int, int), (int, int)) colorEdgeLocation = findColorEdge(cube); //find next unsolved colored edge
            int c = 0;
            while (colorEdgeLocation != ((-1, -1), (-1, -1)))
            {
                if (colorEdgeLocation.Item1.Item1 != 0) //if the edge is in the second layer move it to the top.
                {
                    colorEdgeLocation = extractEdgeFromSecondLayer(colorEdgeLocation, cube, queue);
                }
                solveColorEdge(colorEdgeLocation, cube, queue);//solve edge
                orientCube(cube, queue); //orient cube
                moveZ(cube); //move white to bottom
                moveZ(cube);
                queue.Enqueue("z");
                queue.Enqueue("z");
                colorEdgeLocation = findColorEdge(cube); //find next not solved edge.
                c++;
            }
        }

        private ((int, int), (int, int)) extractEdgeFromSecondLayer(((int, int), (int, int)) colorEdgeLocation, Color[,] cube, Queue<string> q)
        {
            int c = 0;
            int c2 = 0;
            int side1i = colorEdgeLocation.Item1.Item1;
            int side1j = colorEdgeLocation.Item1.Item2;
            int side2i = colorEdgeLocation.Item2.Item1;
            int side2j = colorEdgeLocation.Item2.Item2;
            while (side1i != 1) //rotate the cube until the piece we are solving is facing the front
            {
                moveY(cube);
                q.Enqueue("y");
                side1i = mod(side1i + 1, 4) == 0 ? 4 : mod(side1i + 1, 4); //both sides index got bigger by 1 while 4->1
                side2i = mod(side2i + 1, 4) == 0 ? 4 : mod(side2i + 1, 4);
                c++;
            }
            if (side1j == 3) //if the edge is on the left of the side
            {
                while (!(cube[2, 1] == Color.Yellow || cube[0, 7] == Color.Yellow))
                //rotate the up side until there is a yellow to insert instead of the edge
                {
                    moveUp(cube);
                    q.Enqueue("u");
                }
                LPUPLU(cube, q); //take the edge out
                moveY(cube);
                q.Enqueue("y");
                RURPUP(cube, q); //insert another piece in
                moveYInverted(cube);
                q.Enqueue("y'");
                side1i = 0;
                side1j = 1;
                side2i = 4;
                side2j = 1;
            }
            if (side1j == 5)  //if the edge is on the right of the side
            {
                while (cube[4, 1] == Color.White || cube[0, 1] == Color.White)
                //rotate the up side until there is a yellow to insert instead of the edge
                {
                    moveUp(cube);
                    q.Enqueue("u");
                }
                RURPUP(cube, q);//take the edge out
                moveYInverted(cube);
                q.Enqueue("y'");
                LPUPLU(cube, q);//insert another piece in
                moveY(cube);
                q.Enqueue("y");
                side1i = 0;
                side1j = 7;
                side2i = 2;
                side2j = 1;
            }
            c2 = 0;
            while (c > 0)
            {
                c2++;
                c--;
                moveYInverted(cube);
                q.Enqueue("y'");
                side2i--;
                side2i = mod(side2i, 4);
                side2i = side2i == 0 ? 4 : side2i;
                if (side1j == 7)
                {
                    side1j = 3;
                }
                else if (side1j == 3)
                {
                    side1j = 1;
                }
                else if (side1j == 1)
                {
                    side1j = 5;
                }
                else if (side1j == 5)
                {
                    side1j = 7;
                }
                if (c2 >= 4)
                {
                    break;
                }
            }
            return ((side1i, side1j), (side2i, side2j));
        }

        private void solveColorEdge(((int, int), (int, int)) colorEdgeLocation, Color[,] cube, Queue<string> q)
        {
            int topi = colorEdgeLocation.Item1.Item1;
            int topj = colorEdgeLocation.Item1.Item2;
            int sidei = colorEdgeLocation.Item2.Item1;
            int sidej = colorEdgeLocation.Item2.Item2;
            int c2 = 0;

            while (cube[sidei, sidej] != cube[sidei, 4])
            {
                c2++;
                moveUp(cube);
                q.Enqueue("u");
                sidei--;
                sidei = mod(sidei, 4);
                sidei = sidei == 0 ? 4 : sidei;
                if (topj == 7)
                {
                    topj = 3;
                }
                else if (topj == 3)
                {
                    topj = 1;
                }
                else if (topj == 1)
                {
                    topj = 5;
                }
                else if (topj == 5)
                {
                    topj = 7;
                }
                if (c2 >= 4)
                {
                    //break;
                }
            }
            c2 = 0;
            while (sidei != 1)
            {
                c2++;
                moveYInverted(cube);
                q.Enqueue("y'");
                sidei--;
                sidei = mod(sidei, 4);
                sidei = sidei == 0 ? 4 : sidei;
                if (topj == 7)
                {
                    topj = 3;
                }
                else if (topj == 3)
                {
                    topj = 1;
                }
                else if (topj == 1)
                {
                    topj = 5;
                }
                else if (topj == 5)
                {
                    topj = 7;
                }
                if (c2 >= 4)
                {
                    break;
                }
            }
            if (cube[0, topj] == cube[mod(sidei + 1, 4) == 0 ? 4 : mod(sidei + 1, 4), 4])
            {
                moveUp(cube);
                q.Enqueue("u");
                RURPUP(cube, q);
                moveYInverted(cube);
                q.Enqueue("y'");
                LPUPLU(cube, q);
            }
            else if (cube[0, topj] == cube[mod(sidei - 1, 4) == 0 ? 4 : mod(sidei - 1, 4), 4])
            {
                moveUpInverted(cube);
                q.Enqueue("u'");
                LPUPLU(cube, q);
                moveY(cube);
                q.Enqueue("y");
                RURPUP(cube, q);
            }
            return;


        }

        private ((int, int), (int, int)) findColorEdge(Color[,] cube)
        {
            int c = 0;
            int side = 4;
            for (int j = 1; j < 9; j += 2)
            {

                if (j == 1)
                {
                    side = 4;
                }
                else if (j == 3)
                {
                    side = 1;
                }
                else if (j == 5)
                {
                    side = 3;
                }
                else if (j == 7)
                {
                    side = 2;
                }
                if (!cube[0, j].Equals(Color.Yellow) && !cube[side, 1].Equals(Color.Yellow))
                {
                    c++;
                    //continue;
                    return ((0, j), (side, 1));
                }
            }
            side = 0;
            for (int i = 1; i < 5; i += 2)
            {

                for (int j = 3; j < 7; j += 2)
                {
                    side = j == 3 ? i - 1 : i + 1;
                    side = mod(side, 4);
                    side = side == 0 ? 4 : side;
                    if (!cube[i, j].Equals(Color.Yellow) && !cube[side, 8 - j].Equals(Color.Yellow))
                    {
                        if (!cube[i, j].Equals(cube[i, 4]) || !cube[side, 8 - j].Equals(cube[side, 4]))
                        {
                            c++;
                            //continue;
                            return ((i, j), (side, 8 - j));
                        }

                    }
                }
            }
            int what = c;
            Console.WriteLine(what);
            return ((-1, -1), (-1, -1));
        }
    }
}
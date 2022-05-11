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
    class TopSolver : Solver
    {
        public void solveTop(Color[,] cube, Queue<string> q)
        {
            Console.WriteLine("Solve Top Cross");
            solveTopCross(cube, q);
            Console.WriteLine("Finish Solve Top Cross");
            Console.WriteLine("match Top Cross");
            matchTopCross(cube, q);
            Console.WriteLine("Finish match Top Cross");
            Console.WriteLine("match Top Corners");
            matchTopCorners(cube, q);
            Console.WriteLine("Finish match Top Corners");
            Console.WriteLine("Solve last 4 corners");
            SolveLast4(cube, q);
            Console.WriteLine("Finish solve last 4 corners");
            while (!isLayerSolved(1, cube))
            {
                moveDown(cube);
                q.Enqueue("d");
            }
        }
        private void SolveLast4(Color[,] cube, Queue<string> q)
        {
            moveZ(cube);
            moveZ(cube);
            q.Enqueue("z");
            q.Enqueue("z");
            while (!isLayerSolved(5, cube))
            {
                while (cube[5, 2] != Color.Yellow)
                {
                    RURPUP(cube, q);
                    if (isLayerSolved(5, cube))
                    {
                        return;
                    }
                }
                while (cube[5, 2] == Color.Yellow)
                {
                    moveDown(cube);
                    q.Enqueue("d");
                    if (isLayerSolved(5, cube))
                    {
                        return;
                    }
                }
            }
            return;
        }

        private bool isLayerSolved(int l, Color[,] cube)
        {
            for (int i = 0; i < 9; i++)
            {
                if (cube[l, i] != cube[l, 4])
                {
                    return false;
                }
            }
            return true;
        }

        private void matchTopCorners(Color[,] cube, Queue<string> q)
        {
            (int, int) numberAndLoc = countOrientCorners(cube, q);
            Console.WriteLine(numberAndLoc);
            if (numberAndLoc.Item1 == 4)
            {
                return;
            }
            else if (numberAndLoc.Item1 == 0)
            {
                matchingCornerAlg(cube, q);
                numberAndLoc = countOrientCorners(cube, q);
                if (numberAndLoc.Item1 == 4)
                {
                    return;
                }
            }
            do
            {
                for (int i = 0; i < numberAndLoc.Item2; i++)
                {
                    moveUp(cube);
                    q.Enqueue("u");
                }
                matchingCornerAlg(cube, q);
            } while (!((numberAndLoc = countOrientCorners(cube, q)).Item1 == 4));



        }

        private void matchingCornerAlg(Color[,] cube, Queue<string> q)
        {
            moveUp(cube);
            moveRight(cube);
            moveUpInverted(cube);
            moveLeftInverted(cube);
            moveUp(cube);
            moveRightInverted(cube);
            moveUpInverted(cube);
            moveLeft(cube);
            q.Enqueue("u");
            q.Enqueue("r");
            q.Enqueue("u'");
            q.Enqueue("l'");
            q.Enqueue("u");
            q.Enqueue("r'");
            q.Enqueue("u'");
            q.Enqueue("l");
        }

        private (int, int) countOrientCorners(Color[,] cube, Queue<string> q)
        {
            int c = 0;
            int loc = -1;
            for (int i = 0; i < 4; i++)
            {
                if (cornerInLoc(cube))
                {
                    c++;
                    loc = i;
                }
                moveUp(cube);
                q.Enqueue("u");
            }
            return (c, loc);
        }

        private bool cornerInLoc(Color[,] cube)
        {
            List<Color> colorPool = new List<Color>();
            colorPool.Add(cube[0, 4]);
            colorPool.Add(cube[1, 1]);
            colorPool.Add(cube[2, 1]);
            if ((colorPool.Remove(cube[0, 6]) && colorPool.Remove(cube[2, 0]) && colorPool.Remove(cube[1, 2])))
            {
                return true;
            }
            return false;
        }

        private void matchTopCross(Color[,] cube, Queue<string> q)
        {
            int oriented = countOrientCross(cube, q);
            Console.WriteLine(oriented);
            if (oriented == 4)
            {
                return;
            }
            setL(cube, q);
            crossOrientAlg(cube, q);
            while (!(cube[2, 1] == cube[2, 4] && cube[3, 1] == cube[3, 4] && cube[1, 1] == cube[1, 4] && cube[4, 1] == cube[4, 4]))
            {
                moveUp(cube);
                q.Enqueue("u");
            }
            //oriented  = countOrientCross();
            Console.WriteLine(oriented);
            return;
        }

        private void setL(Color[,] cube, Queue<string> q)
        {
            Dictionary<Color, Color> acrros = new Dictionary<Color, Color>();
            acrros.Add(Color.Red, Color.Orange);
            acrros.Add(Color.Orange, Color.Red);
            acrros.Add(Color.Blue, Color.Green);
            acrros.Add(Color.Green, Color.Blue);
            Dictionary<Color, Color> right = new Dictionary<Color, Color>();
            right.Add(Color.Red, Color.Green);
            right.Add(Color.Orange, Color.Blue);
            right.Add(Color.Blue, Color.Red);
            right.Add(Color.Green, Color.Orange);
            acrros.TryGetValue(cube[1, 1], out Color val);
            if (val == cube[3, 1])
            {
                crossOrientAlg(cube, q);
            }
            int next = 2;
            int i = 0;
            for (i = 1; i <= 4; i++)
            {
                next = i + 1;
                next = mod(next, 4);
                next = next == 0 ? 4 : next;
                right.TryGetValue(cube[i, 1], out val);
                if (val == cube[next, 1])
                {
                    break;
                }
            }

            while (i != 2)
            {
                moveUpInverted(cube);
                q.Enqueue("u'");
                i++;
                i = mod(i, 4);
                i = i == 0 ? 4 : i;
            }
            //moveUpInverted();


        }

        private void crossOrientAlg(Color[,] cube, Queue<string> q)
        {
            moveRight(cube);
            moveUp(cube);
            moveRightInverted(cube);
            moveUp(cube);
            moveRight(cube);
            moveUp(cube);
            moveUp(cube);
            moveRightInverted(cube);
            q.Enqueue("r");
            q.Enqueue("u");
            q.Enqueue("r'");
            q.Enqueue("u");
            q.Enqueue("r");
            q.Enqueue("u");
            q.Enqueue("u");
            q.Enqueue("r'");


        }

        private int countOrientCross(Color[,] cube, Queue<string> q)
        {
            Color[] order = { Color.Orange, Color.Blue, Color.Red, Color.Green };
            int start = 1;
            for (int i = 1; i <= 4; i++)
            {
                if (cube[i, 1] == Color.Orange)
                {
                    start = i;
                }
            }
            int countOriented = 0;
            for (int i = 0; i < 4; i++, start = mod(start + 1, 4) == 0 ? 4 : mod(start + 1, 4))
            {
                if (cube[start, 1] == order[i])
                {
                    countOriented++;
                }
            }
            if (countOriented == 4)
            {
                while (cube[1, 1] != Color.Orange)
                {
                    moveUp(cube);
                    q.Enqueue("u");
                }
                return 4;
            }
            return 2;
        }

        private void solveTopCross(Color[,] cube, Queue<string> q)
        {
            if (topHasCross(cube))
            {
                return;
            }
            if (topHasLine(cube, q))
            {
                topCrossLineAlg(cube, q);
                return;
            }
            if (topHasLShape(cube, q))
            {
                topCrossLShapeAlg(cube, q);
                return;
            }
            topCrossDotAlg(cube, q);
            return;
        }

        private bool topHasLShape(Color[,] cube, Queue<string> q)
        {
            if (cube[0, 1] == Color.Yellow && cube[0, 3] == Color.Yellow)
            {
                moveUp(cube);
                q.Enqueue("u");
                return true;
            }
            if (cube[0, 3] == Color.Yellow && cube[0, 7] == Color.Yellow)
            {
                moveUp(cube);
                moveUp(cube);
                q.Enqueue("u");
                q.Enqueue("u");
                return true;
            }
            if (cube[0, 7] == Color.Yellow && cube[0, 5] == Color.Yellow)
            {
                moveUpInverted(cube);
                q.Enqueue("u'");
                return true;
            }
            if (cube[0, 1] == Color.Yellow && cube[0, 5] == Color.Yellow)
            {
                return true;
            }
            return false;
        }

        private bool topHasLine(Color[,] cube, Queue<string> q)
        {
            if (cube[0, 1] == Color.Yellow && cube[0, 7] == Color.Yellow)
            {
                return true;
            }
            if (cube[0, 3] == Color.Yellow && cube[0, 5] == Color.Yellow)
            {
                moveUp(cube);
                q.Enqueue("u");
                return true;
            }
            return false;
        }

        private bool topHasCross(Color[,] cube)
        {
            for (int i = 1; i < 9; i += 2)
            {
                if (cube[0, i] != Color.Yellow)
                {
                    return false;
                }
            }
            return true;
        }

        private void topCrossDotAlg(Color[,] cube, Queue<string> q)
        {
            // F R U R' U' F' f R U R' U' f'
            moveFront(cube);
            q.Enqueue("f");
            RURPUP(cube, q);
            moveFrontInverted(cube);
            q.Enqueue("f'");
            moveFront(cube);
            q.Enqueue("f");
            moveS(cube);
            q.Enqueue("s");
            RURPUP(cube, q);
            moveFrontInverted(cube);
            q.Enqueue("f'");
            moveSInverted(cube);
            q.Enqueue("s'");


        }

        private void topCrossLShapeAlg(Color[,] cube, Queue<string> q)
        {
            moveFront(cube);
            moveUp(cube);
            moveRight(cube);
            moveUpInverted(cube);
            moveRightInverted(cube);
            moveFrontInverted(cube);
            q.Enqueue("f");
            q.Enqueue("u");
            q.Enqueue("r");
            q.Enqueue("u'");
            q.Enqueue("r'");
            q.Enqueue("f'");

        }

        private void topCrossLineAlg(Color[,] cube, Queue<string> q)
        {
            moveFront(cube);
            q.Enqueue("f");
            RURPUP(cube, q);
            moveFrontInverted(cube);
            q.Enqueue("f'");
        }
    }
}
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
    /* Map
     * White Blue Orange:
     * White [0,0] Blue [4,2] Orange[1,0]
     * White Blue:
     * White [0,1] Blue [4,1]
     * White Blue Red:
     * White [0,2] Blue[ [4,0] Red [3,2]
     * White Orange:
     * White [0,3] Orange [1,1]
     * White Center:
     * [0,4]
     * White Red:
     * White [0,5] Red [3,1]
     * White Orange Green:
     * White [0,6] Orange [1,2] Green [2,0]
     * White Green:
     * White [0,7] Green [2,1]
     * White Green Red:
     * White [0,8] Green [2,2] Red [3,0]
     * Orange Blue:
     * Orange [1,3] Blue [4,5]
     * Orange Center:
     * [1,4]
     * Orange Green:
     * Orange [1,5] Green [2,3]
     * Green Center:
     * [2,4]
     * Green Red:
     * Green [2,5] Red [3,3]
     * Red Center:
     * [3,4]
     * Red Blue:
     * Red [3,5] Blue [4,3]
     * Blue center:
     * [4,4]
     * Blue Orange:
     * Blue [4,5] Orange [1,3] 
     * Yellow Orange Blue:
     * Yellow [5,0] Orange [1,6] Blue [4,8]
     * Yellow Orange:
     * Yellow [5,1] Orange [1,7]
     * Yellow Orange Green:
     * Yellow [5,2] Orange [1,8] Green [2,6]
     * Yellow Blue:
     * Yellow [5,3] Blue[4,7]
     * Yellow center:
     * [5,4]
     * Yellow Green:
     * Yellow [5,5] Green [2,7]
     * Yellow Blue Red:
     * Yellow [5,6] Blue [4,6] Red [3,8]
     * Yellow Red:
     * Yellow [5,7] Red [3,7] 
     * Yellow Red Green:
     * Yellow [5,8] Red [3,6] Green [2,8]
     */
    public class RubiksCube
    {
        public Color[,] cube = new Color[6, 9];
        private RubikPart[,] visibleParts = new RubikPart[3, 9];
        private List<(float, float)> points = new List<(float, float)>();

        int w, h;

        private List<(float, float)> insidePointsUp = new List<(float, float)>();
        private List<(float, float)> insidePointsLeft = new List<(float, float)>();
        private List<(float, float)> insidePointsRight = new List<(float, float)>();




        public RubiksCube(int w, int h)
        {
            this.w = w;
            this.h = h;
            initializeCube();

        }
        public RubiksCube(bool solve)
        {

        }
        public void initializeCube()
        {


            for (int j = 0; j < 9; j++)
            {
                cube[0, j] = Color.White;
            }
            for (int j = 0; j < 9; j++)
            {
                cube[1, j] = Color.Orange;
            }
            for (int j = 0; j < 9; j++)
            {
                cube[2, j] = Color.Green;
            }
            for (int j = 0; j < 9; j++)
            {
                cube[3, j] = Color.Red;
            }
            for (int j = 0; j < 9; j++)
            {
                cube[4, j] = Color.Blue;
            }
            for (int j = 0; j < 9; j++)
            {
                cube[5, j] = Color.Yellow;
            }
            //get Pts of the cubes.
            //redmi 1080 x 2168

            GetPs((100, 750), 500);
            GetInsideUpPs((100, 750), 500);
            GetInsideLeftPs((100, 750), 500);
            GetInsideRightPs((100, 750), 500);



        }
        public void draw(Canvas canvas, Paint p)
        {
            //draw the cube
            updateVisibleParts();
            p.Color = Color.Black;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    visibleParts[i, j].draw(canvas);
                }
            }
        }
        public void solve(Queue<string> queue)
        {
            Solver solver = new Solver();
            solver.solve((Color[,])cube.Clone(), queue);
            return;
        }

        public bool isSolved()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!(this.cube[i, j] == this.cube[i, 4]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected void RURPUP(Color[,] cube, Queue<string> q)
        {
            //right trigger - useful moves
            moveRight(cube);
            moveUp(cube);
            moveRightInverted(cube);
            moveUpInverted(cube);
            q.Enqueue("r");
            q.Enqueue("u");
            q.Enqueue("r'");
            q.Enqueue("u'");
        }
        protected void LPUPLU(Color[,] cube, Queue<string> q)
        {
            //right trigger - useful moves
            moveLeftInverted(cube);
            moveUpInverted(cube);
            moveLeft(cube);
            moveUp(cube);
            q.Enqueue("l'");
            q.Enqueue("u'");
            q.Enqueue("l");
            q.Enqueue("u");

        }
        protected void orientCube(Color[,] cube, Queue<string> queue)
        {
            if (cube[0, 4].Equals(Color.Orange) || cube[0, 4].Equals(Color.Red))
            {
                queue.Enqueue("x");
                moveX(cube);
            }
            while (!cube[1, 4].Equals(Color.Orange))
            {
                queue.Enqueue("y");
                moveY(cube);
            }
            while (!cube[0, 4].Equals(Color.White))
            {
                queue.Enqueue("z");
                moveZ(cube);
            }
        }
        public void GetPs((float, float) t, float size)
        {
            float x = t.Item1, y = t.Item2, mul = (float)0.5 * size;
            points.Add((x, y));//A
            points.Add((x + mul * (float)Math.Sqrt(3), y + mul));//B
            points.Add((x, y + size));//C
            points.Add((x + mul * (float)Math.Sqrt(3), y - mul));//D
            points.Add((x + size * (float)Math.Sqrt(3), y));//E
            points.Add((x + mul * (float)Math.Sqrt(3), y + 3 * mul));//F
            points.Add((x + size * (float)Math.Sqrt(3), y + size));//G


        }

        public void GetInsideUpPs((float, float) t, float size)
        {
            float x = t.Item1, y = t.Item2, mul = (float)0.5 * size;
            insidePointsUp.Add((x, y));//A
            insidePointsUp.Add((x + mul * (float)Math.Sqrt(3), y + mul));//B
            insidePointsUp.Add((x + mul * (float)Math.Sqrt(3), y - mul));//D
            insidePointsUp.Add((x + size * (float)Math.Sqrt(3), y));//E
            insidePointsUp.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * 0.5 * size)));//AB1
            insidePointsUp.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * 0.5 * size)));//AB2
            insidePointsUp.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y - (float)((1.0 / 3) * 0.5 * size)));//AD1
            insidePointsUp.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y - (float)((2.0 / 3) * 0.5 * size)));//AD2
            insidePointsUp.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * 0.5 * size)));//BE1
            insidePointsUp.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * 0.5 * size)));//BE2
            insidePointsUp.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y - (float)((2.0 / 3) * 0.5 * size)));//DE1
            insidePointsUp.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y - (float)((1.0 / 3) * 0.5 * size)));//DE2
            insidePointsUp.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y)); // AD1xDE1 center
            insidePointsUp.Add((x + mul * (float)Math.Sqrt(3), y - (float)((1.0 / 3) * 0.5 * size))); // AD2xDE1 center
            insidePointsUp.Add((x + mul * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * 0.5 * size))); // AD1xDE2 center
            insidePointsUp.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y)); // AD2xDE2 center
            visibleParts[0, 0] = new RubikPart(insidePointsUp[0], insidePointsUp[6], insidePointsUp[4], insidePointsUp[12], cube[0, 0]);
            visibleParts[0, 1] = new RubikPart(insidePointsUp[6], insidePointsUp[7], insidePointsUp[12], insidePointsUp[13], cube[0, 1]);
            visibleParts[0, 2] = new RubikPart(insidePointsUp[7], insidePointsUp[2], insidePointsUp[13], insidePointsUp[10], cube[0, 2]);
            visibleParts[0, 3] = new RubikPart(insidePointsUp[4], insidePointsUp[12], insidePointsUp[5], insidePointsUp[14], cube[0, 3]);
            visibleParts[0, 4] = new RubikPart(insidePointsUp[12], insidePointsUp[13], insidePointsUp[14], insidePointsUp[15], cube[0, 4]);
            visibleParts[0, 5] = new RubikPart(insidePointsUp[13], insidePointsUp[10], insidePointsUp[15], insidePointsUp[11], cube[0, 5]);
            visibleParts[0, 6] = new RubikPart(insidePointsUp[5], insidePointsUp[14], insidePointsUp[1], insidePointsUp[8], cube[0, 6]);//
            visibleParts[0, 7] = new RubikPart(insidePointsUp[14], insidePointsUp[15], insidePointsUp[8], insidePointsUp[9], cube[0, 7]);
            visibleParts[0, 8] = new RubikPart(insidePointsUp[15], insidePointsUp[11], insidePointsUp[9], insidePointsUp[3], cube[0, 8]);


            //visibleParts.Add(new RubikPart(insidePointsUp[0], insidePointsUp[4]))
        }

        public void GetInsideLeftPs((float, float) t, float size)
        {
            float x = t.Item1, y = t.Item2, mul = (float)0.5 * size;
            insidePointsLeft.Add((x, y));//A
            insidePointsLeft.Add((x + mul * (float)Math.Sqrt(3), y + mul));//B
            insidePointsLeft.Add((x, y + size));//C
            insidePointsLeft.Add((x + mul * (float)Math.Sqrt(3), y + 3 * mul)); // F
            insidePointsLeft.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * 0.5 * size)));//AB1
            insidePointsLeft.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * 0.5 * size)));//AB2
            insidePointsLeft.Add((x, y + (float)((1.0 / 3) * size)));//AC1
            insidePointsLeft.Add((x, y + (float)((2.0 / 3) * size)));//AC2
            insidePointsLeft.Add((x + mul * (float)Math.Sqrt(3), y + mul + (float)((1.0 / 3) * size)));//BF1
            insidePointsLeft.Add((x + mul * (float)Math.Sqrt(3), y + mul + (float)((2.0 / 3) * size)));//BF2
            insidePointsLeft.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size + (float)((1.0 / 3) * 0.5 * size)));//CF1
            insidePointsLeft.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size + (float)((2.0 / 3) * 0.5 * size)));//CF2
            insidePointsLeft.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + mul)); //AC1xAB1 center
            insidePointsLeft.Add((x + size * (float)(1.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + mul + (float)((1.0 / 3) * size))); //AC2xAB1 center
            insidePointsLeft.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * size))); //AC1xAB2 center
            insidePointsLeft.Add((x + size * (float)(2.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size)); //AC2xAB2 center
            visibleParts[1, 0] = new RubikPart(insidePointsLeft[0], insidePointsLeft[6], insidePointsLeft[4], insidePointsLeft[12], cube[1, 0]);
            visibleParts[1, 3] = new RubikPart(insidePointsLeft[6], insidePointsLeft[7], insidePointsLeft[12], insidePointsLeft[13], cube[1, 3]);
            visibleParts[1, 2] = new RubikPart(insidePointsLeft[5], insidePointsLeft[14], insidePointsLeft[1], insidePointsLeft[8], cube[1, 2]);//
            visibleParts[1, 1] = new RubikPart(insidePointsLeft[4], insidePointsLeft[12], insidePointsLeft[5], insidePointsLeft[14], cube[1, 1]);
            visibleParts[1, 4] = new RubikPart(insidePointsLeft[12], insidePointsLeft[13], insidePointsLeft[14], insidePointsLeft[15], cube[1, 4]);
            visibleParts[1, 5] = new RubikPart(insidePointsLeft[14], insidePointsLeft[15], insidePointsLeft[8], insidePointsLeft[9], cube[1, 5]);
            visibleParts[1, 6] = new RubikPart(insidePointsLeft[7], insidePointsLeft[2], insidePointsLeft[13], insidePointsLeft[10], cube[1, 6]);
            visibleParts[1, 7] = new RubikPart(insidePointsLeft[13], insidePointsLeft[10], insidePointsLeft[15], insidePointsLeft[11], cube[1, 7]);
            visibleParts[1, 8] = new RubikPart(insidePointsLeft[15], insidePointsLeft[11], insidePointsLeft[9], insidePointsLeft[3], cube[1, 8]);


        }

        public void GetInsideRightPs((float, float) t, float size)
        {
            float x = t.Item1, y = t.Item2, mul = (float)0.5 * size;
            insidePointsRight.Add((x + mul * (float)Math.Sqrt(3), y + mul));//B
            insidePointsRight.Add((x + size * (float)Math.Sqrt(3), y));//E
            insidePointsRight.Add((x + mul * (float)Math.Sqrt(3), y + 3 * mul)); // F
            insidePointsRight.Add((x + size * (float)Math.Sqrt(3), y + size));//G
            insidePointsRight.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * 0.5 * size)));//BE1
            insidePointsRight.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * 0.5 * size)));//BE2
            insidePointsRight.Add((x + mul * (float)Math.Sqrt(3), y + mul + (float)((1.0 / 3) * size)));//BF1
            insidePointsRight.Add((x + mul * (float)Math.Sqrt(3), y + mul + (float)((2.0 / 3) * size)));//BF2
            insidePointsRight.Add((x + size * (float)Math.Sqrt(3), y + (float)((1.0 / 3) * size)));//EG1
            insidePointsRight.Add((x + size * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * size)));//EG2
            insidePointsRight.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size + (float)((2.0 / 3) * 0.5 * size)));//FG1
            insidePointsRight.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size + (float)((1.0 / 3) * 0.5 * size)));//FG2
            insidePointsRight.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + (float)((2.0 / 3) * size))); // BF1xBE1 center
            insidePointsRight.Add((x + size * (float)(4.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + size)); // BF2xBE1 center
            insidePointsRight.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + mul)); // BF1xBE2 center
            insidePointsRight.Add((x + size * (float)(5.0 / 3) * (float)0.5 * (float)Math.Sqrt(3), y + mul + (float)((1.0 / 3) * size))); // BF2xBE2 center
            visibleParts[2, 0] = new RubikPart(insidePointsRight[0], insidePointsRight[6], insidePointsRight[4], insidePointsRight[12], cube[2, 0]);
            visibleParts[2, 5] = new RubikPart(insidePointsRight[14], insidePointsRight[15], insidePointsRight[8], insidePointsRight[9], cube[2, 5]);
            visibleParts[2, 1] = new RubikPart(insidePointsRight[4], insidePointsRight[12], insidePointsRight[5], insidePointsRight[14], cube[2, 1]);
            visibleParts[2, 3] = new RubikPart(insidePointsRight[6], insidePointsRight[7], insidePointsRight[12], insidePointsRight[13], cube[2, 3]);
            visibleParts[2, 4] = new RubikPart(insidePointsRight[12], insidePointsRight[13], insidePointsRight[14], insidePointsRight[15], cube[2, 4]);
            visibleParts[2, 7] = new RubikPart(insidePointsRight[13], insidePointsRight[10], insidePointsRight[15], insidePointsRight[11], cube[2, 7]);
            visibleParts[2, 2] = new RubikPart(insidePointsRight[5], insidePointsRight[14], insidePointsRight[1], insidePointsRight[8], cube[2, 2]);//
            visibleParts[2, 6] = new RubikPart(insidePointsRight[7], insidePointsRight[2], insidePointsRight[13], insidePointsRight[10], cube[2, 6]);
            visibleParts[2, 8] = new RubikPart(insidePointsRight[15], insidePointsRight[11], insidePointsRight[9], insidePointsRight[3], cube[2, 8]);

        }

        public void moveRight(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            /*Right rotation moves:
             * [0,6] to [3,0] to [5,8] to [1,8] to [0,6]
             * [1,2] to [0,8] to [3,6] to [5,2] to [1,2]
             * [2,0] to [2,2] to [2,8] to [2,6] to [2,2]
             * [0,7] to [3,3] to [5,5] to [1,5] to [0,7]
             * [2,1] to [2,5] to [2,7] to [2,3] to [2,1]
             */
            //first cycle
            cube[0, 6] = cubeCopy[1, 8];
            cube[3, 0] = cubeCopy[0, 6];
            cube[5, 8] = cubeCopy[3, 0];
            cube[1, 8] = cubeCopy[5, 8];
            //second cycle
            cube[1, 2] = cubeCopy[5, 2];
            cube[0, 8] = cubeCopy[1, 2];
            cube[3, 6] = cubeCopy[0, 8];
            cube[5, 2] = cubeCopy[3, 6];
            //third cycle
            cube[2, 0] = cubeCopy[2, 6];
            cube[2, 2] = cubeCopy[2, 0];
            cube[2, 8] = cubeCopy[2, 2];
            cube[2, 6] = cubeCopy[2, 8];
            //forth cycle
            cube[0, 7] = cubeCopy[1, 5];
            cube[3, 3] = cubeCopy[0, 7];
            cube[5, 5] = cubeCopy[3, 3];
            cube[1, 5] = cubeCopy[5, 5];
            //fifth cycle 
            cube[2, 1] = cubeCopy[2, 3];
            cube[2, 5] = cubeCopy[2, 1];
            cube[2, 7] = cubeCopy[2, 5];
            cube[2, 3] = cubeCopy[2, 7];

        }
        public void moveRightInverted(Color[,] cube)
        {
            moveRight(cube);
            moveRight(cube);
            moveRight(cube);
        }
        public void moveUp(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            /*Right rotation moves:
             * [1,0] to [4,0] to [3,0] to [2,0] to [1,0]
             * [4,2] to [3,2] to [2,2] to [1,2] to [4,2]
             * [0,0] to [0,2] to [0,8] to [0,6] to [0,2]
             * [1,1] to [4,1] to [3,1] to [2,1] to [1,1]
             * [0,1] to [0,5] to [0,7] to [0,3] to [0,1]
             */
            //first cycle
            cube[1, 0] = cubeCopy[2, 0];
            cube[4, 0] = cubeCopy[1, 0];
            cube[3, 0] = cubeCopy[4, 0];
            cube[2, 0] = cubeCopy[3, 0];
            //second cycle
            cube[4, 2] = cubeCopy[1, 2];
            cube[3, 2] = cubeCopy[4, 2];
            cube[2, 2] = cubeCopy[3, 2];
            cube[1, 2] = cubeCopy[2, 2];
            //third cycle
            cube[0, 0] = cubeCopy[0, 6];
            cube[0, 2] = cubeCopy[0, 0];
            cube[0, 8] = cubeCopy[0, 2];
            cube[0, 6] = cubeCopy[0, 8];
            //forth cycle
            cube[1, 1] = cubeCopy[2, 1];
            cube[4, 1] = cubeCopy[1, 1];
            cube[3, 1] = cubeCopy[4, 1];
            cube[2, 1] = cubeCopy[3, 1];
            //fifth cycle 
            cube[0, 1] = cubeCopy[0, 3];
            cube[0, 5] = cubeCopy[0, 1];
            cube[0, 7] = cubeCopy[0, 5];
            cube[0, 3] = cubeCopy[0, 7];

        }
        public void moveUpInverted(Color[,] cube)
        {
            moveUp(cube);
            moveUp(cube);
            moveUp(cube);
        }
        public void moveFront(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            /*Right rotation moves:
             * [0,0] to [2,0] to [5,2] to [1,8] to [0,0]
             * [1,2] to [0,8] to [3,6] to [5,2] to [1,2]
             * [1,0] to [1,2] to [1,8] to [1,6] to [1,2]
             * [0,3] to [2,3] to [5,2] to [4,5] to [0,2]
             * [1,1] to [1,5] to [1,7] to [1,3] to [1,1]
             */
            //first cycle
            cube[0, 0] = cubeCopy[4, 8];
            cube[2, 0] = cubeCopy[0, 0];
            cube[5, 2] = cubeCopy[2, 0];
            cube[4, 8] = cubeCopy[5, 2];
            //second cycle
            cube[4, 2] = cubeCopy[5, 0];
            cube[0, 6] = cubeCopy[4, 2];
            cube[2, 6] = cubeCopy[0, 6];
            cube[5, 0] = cubeCopy[2, 6];
            //third cycle
            cube[1, 0] = cubeCopy[1, 6];
            cube[1, 2] = cubeCopy[1, 0];
            cube[1, 8] = cubeCopy[1, 2];
            cube[1, 6] = cubeCopy[1, 8];
            //forth cycle
            cube[0, 3] = cubeCopy[4, 5];
            cube[2, 3] = cubeCopy[0, 3];
            cube[5, 1] = cubeCopy[2, 3];
            cube[4, 5] = cubeCopy[5, 1];
            //fifth cycle 
            cube[1, 1] = cubeCopy[1, 3];
            cube[1, 5] = cubeCopy[1, 1];
            cube[1, 7] = cubeCopy[1, 5];
            cube[1, 3] = cubeCopy[1, 7];

        }
        public void moveFrontInverted(Color[,] cube)
        {
            moveFront(cube);
            moveFront(cube);
            moveFront(cube);
        }
        public void moveY(Color[,] cube)
        {
            moveE(cube);
            moveUpInverted(cube);
            moveDown(cube);

        }
        public void moveYInverted(Color[,] cube)
        {
            moveE(cube);
            moveE(cube);
            moveE(cube);
            moveUp(cube);
            moveDownInverted(cube);

        }
        public void moveMiddleInverted(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            cube[0, 3] = cubeCopy[1, 7];
            cube[0, 4] = cubeCopy[1, 4];
            cube[0, 5] = cubeCopy[1, 1];

            cube[1, 7] = cubeCopy[5, 7];
            cube[1, 4] = cubeCopy[5, 4];
            cube[1, 1] = cubeCopy[5, 1];

            cube[5, 1] = cubeCopy[3, 7];
            cube[5, 4] = cubeCopy[3, 4];
            cube[5, 7] = cubeCopy[3, 1];

            cube[3, 1] = cubeCopy[0, 3];
            cube[3, 4] = cubeCopy[0, 4];
            cube[3, 7] = cubeCopy[0, 5];

        }
        public void moveMiddle(Color[,] cube)
        {
            moveMiddleInverted(cube);
            moveMiddleInverted(cube);
            moveMiddleInverted(cube);
        }
        public void moveLeft(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            cube[4, 2] = cubeCopy[4, 0];
            cube[4, 8] = cubeCopy[4, 2];
            cube[4, 6] = cubeCopy[4, 8];
            cube[4, 0] = cubeCopy[4, 6];
            cube[4, 1] = cubeCopy[4, 3];
            cube[4, 5] = cubeCopy[4, 1];
            cube[4, 7] = cubeCopy[4, 5];
            cube[4, 3] = cubeCopy[4, 7];

            cube[5, 0] = cubeCopy[1, 0];
            cube[3, 8] = cubeCopy[5, 0];
            cube[0, 2] = cubeCopy[3, 8];
            cube[1, 0] = cubeCopy[0, 2];

            cube[5, 3] = cubeCopy[1, 3];
            cube[3, 5] = cubeCopy[5, 3];
            cube[0, 1] = cubeCopy[3, 5];
            cube[1, 3] = cubeCopy[0, 1];

            cube[5, 6] = cubeCopy[1, 6];
            cube[3, 2] = cubeCopy[5, 6];
            cube[0, 0] = cubeCopy[3, 2];
            cube[1, 6] = cubeCopy[0, 0];

        }
        public void moveLeftInverted(Color[,] cube)
        {
            moveLeft(cube);
            moveLeft(cube);
            moveLeft(cube);
        }
        public void moveX(Color[,] cube)
        {
            moveRightInverted(cube);
            moveMiddle(cube);
            moveLeft(cube);
        }
        public void moveXInverted(Color[,] cube)
        {
            moveLeftInverted(cube);
            moveMiddleInverted(cube);
            moveRight(cube);
        }
        public void moveDownInverted(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();

            cube[1, 6] = cubeCopy[2, 6];
            cube[4, 6] = cubeCopy[1, 6];
            cube[3, 6] = cubeCopy[4, 6];
            cube[2, 6] = cubeCopy[3, 6];
            //second cycle
            cube[1, 8] = cubeCopy[2, 8];
            cube[4, 8] = cubeCopy[1, 8];
            cube[3, 8] = cubeCopy[4, 8];
            cube[2, 8] = cubeCopy[3, 8];
            //third cycle
            cube[5, 6] = cubeCopy[5, 0];
            cube[5, 8] = cubeCopy[5, 6];
            cube[5, 2] = cubeCopy[5, 8];
            cube[5, 0] = cubeCopy[5, 2];
            //forth cycle
            cube[1, 7] = cubeCopy[2, 7];
            cube[4, 7] = cubeCopy[1, 7];
            cube[3, 7] = cubeCopy[4, 7];
            cube[2, 7] = cubeCopy[3, 7];
            //fifth cycle 
            cube[5, 3] = cubeCopy[5, 1];
            cube[5, 7] = cubeCopy[5, 3];
            cube[5, 5] = cubeCopy[5, 7];
            cube[5, 1] = cubeCopy[5, 5];

        }
        public void moveDown(Color[,] cube)
        {
            moveDownInverted(cube);
            moveDownInverted(cube);
            moveDownInverted(cube);
        }
        public void moveE(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();

            cube[1, 3] = cubeCopy[4, 3];
            cube[2, 3] = cubeCopy[1, 3];
            cube[3, 3] = cubeCopy[2, 3];
            cube[4, 3] = cubeCopy[3, 3];
            cube[1, 4] = cubeCopy[4, 4];
            cube[2, 4] = cubeCopy[1, 4];
            cube[3, 4] = cubeCopy[2, 4];
            cube[4, 4] = cubeCopy[3, 4];
            cube[1, 5] = cubeCopy[4, 5];
            cube[2, 5] = cubeCopy[1, 5];
            cube[3, 5] = cubeCopy[2, 5];
            cube[4, 5] = cubeCopy[3, 5];
        }
        public void moveEInverted(Color[,] cube)
        {
            moveE(cube);
            moveE(cube);
            moveE(cube);
        }
        public void moveS(Color[,] cube)
        {
            Color[,] cubeCopy = (Color[,])cube.Clone();
            cube[0, 1] = cubeCopy[4, 7];
            cube[0, 4] = cubeCopy[4, 4];
            cube[0, 7] = cubeCopy[4, 1];

            cube[2, 7] = cubeCopy[0, 7];
            cube[2, 4] = cubeCopy[0, 4];
            cube[2, 1] = cubeCopy[0, 1];

            cube[5, 5] = cubeCopy[2, 1];
            cube[5, 4] = cubeCopy[2, 4];
            cube[5, 3] = cubeCopy[2, 7];

            cube[4, 7] = cubeCopy[5, 5];
            cube[4, 4] = cubeCopy[5, 4];
            cube[4, 1] = cubeCopy[5, 3];

        }
        public void moveSInverted(Color[,] cube)
        {
            moveS(cube);
            moveS(cube);
            moveS(cube);
        }
        public void moveZ(Color[,] cube)
        {
            moveX(cube);
            moveY(cube);
            moveXInverted(cube);
        }
        public void moveZInverted(Color[,] cube)
        {
            moveX(cube);
            moveYInverted(cube);
            moveXInverted(cube);
        }
        public void moveBack(Color[,] cube)
        {
            moveX(cube);
            moveUp(cube);
            moveXInverted(cube);
        }
        public void moveBackInverted(Color[,] cube)
        {
            moveX(cube);
            moveUpInverted(cube);
            moveXInverted(cube);
        }

        private void updateVisibleParts()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    visibleParts[i, j].setColor(cube[i, j]);
                }
            }
        }
        internal (int, int) findLocation(float xDown, float yDown)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 9; j++)
                {


                    if (visibleParts[i, j].isIn(xDown, yDown))
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }
        protected int mod(int x, int m)
        {
            return (x % m + m) % m;
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;

namespace ProjectXWindows.Core
{
    class Helper
    {
        public static int OddEvenChoice(int number)
        {
            if (number % 2 == 0)
                return 1;
            return 0;
        }

        public static int RealRandom(int min, int max)
        {
            Random random = new Random(Convert.ToInt32(Regex.Replace(System.Guid.NewGuid().ToString(), "[^0-9]", "").Substring(0, 1)));
            return random.Next(min, max);
        }

        public static void OutputDouble(double[,] data)
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    Console.Write(data[i,j]+",");
                }
                Console.WriteLine();
            }
        }

        public static double[,] MatrixMultiply(double[,] input, double[,] identity)
        {
            double[,] output = new double[input.GetLength(0), input.GetLength(1)];
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < identity.GetLength(1); j++)
                {
                    output[i, j] = 0.0;
                    for (int k = 0; k < input.GetLength(1); k++)
                    {
                        output[i, j] = output[i, j] + input[i, k] * identity[k, j];
                    }
                }
            }
            return output;
        }

        public static double[,] MatrixTransform(double[,] input, double scaleX, double scaleY, float angle, double transX, double transY)
        {
            double rotCos = Math.Round(Math.Cos((double)angle), 10);
            double rotSin = Math.Round(Math.Sin((double)angle), 10);

            double[,] identity = new double[3, 3] { { scaleX * rotCos, scaleX * -rotSin, 0.0 },
                                                    { rotSin, scaleY * rotCos, 0.0 }, 
                                                    { transX, transY, 1.0 } };

            return MatrixMultiply(input, identity);
        }

        public static double[,] RectangleToMatrix(Rectangle rect)
        {
            double[,] points = new double[4, 3] {	{rect.X, rect.Y, 1.0}, 
											        {rect.X, rect.Y+rect.Height, 1.0},
											        {rect.X+rect.Width, rect.Y+rect.Height, 1.0},
											        {rect.X+rect.Width, rect.Y, 1.0}};
            return points;
        }

        public static double[] StripArrayFromMulti(double[,] the_array, int index)
        {
            double[] output = new double[the_array.GetLength(0)];
            for (int i = 0; i < the_array.GetLength(0); i++)
            {
                output[i] = the_array[i, index];
            }
            return output;
        }

        public static double GetMinMaxArray(double[] data, bool max)
        {
            double result = data[0];
            foreach (double item in data)
            {
                if (max)
                    result = Math.Max(result, item);
                else
                {
                    result = Math.Min(result, item);
                }
            }
            return result;
        }

        public static Rectangle MatrixToMaxRectangle(double[,] points)
        {
            double minX = GetMinMaxArray(StripArrayFromMulti(points, 0), false);
            double minY = GetMinMaxArray(StripArrayFromMulti(points, 1), false);
            double new_width = GetMinMaxArray(StripArrayFromMulti(points, 0), true) - minX;
            double new_height = GetMinMaxArray(StripArrayFromMulti(points, 1), true) - minY;

            Rectangle rect = new Rectangle((int)minX, (int)minY, (int)new_width, (int)new_height);

            return rect;
        }

        public static Vector2 LineIntersection(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B)
        {
            Vector2 intersectionPoint = new Vector2(-1, -1);

            float Ax = line1A.X;
            float Ay = line1A.Y;
            float Bx = line1B.X;
            float By = line1B.Y;
            float Cx = line2A.X;
            float Cy = line2A.Y;
            float Dx = line2B.X;
            float Dy = line2B.Y;

            float distAB, theCos, theSin, newX, ABpos;
            //  Fail if either line segment is zero-length.
            if (Ax == Bx && Ay == By || Cx == Dx && Cy == Dy) return intersectionPoint;
            //  Fail if the segments share an end-point.
            if (Ax==Cx && Ay==Cy || Bx==Cx && By==Cy || Ax==Dx && Ay==Dy || Bx==Dx && By==Dy) return intersectionPoint;

            //  (1) Translate the system so that point A is on the origin.
            Bx-=Ax; 
            By-=Ay;
            Cx-=Ax; 
            Cy-=Ay;
            Dx-=Ax; 
            Dy-=Ay;

            //  Discover the length of segment A-B.
            distAB=(float)Math.Sqrt(Bx*Bx+By*By);

            //  (2) Rotate the system so that point B is on the positive X axis.
            theCos=Bx/distAB;
            theSin=By/distAB;
            newX=Cx*theCos+Cy*theSin;
            Cy=Cy*theCos-Cx*theSin; 
            Cx=newX;
            newX=Dx*theCos+Dy*theSin;
            Dy=Dy*theCos-Dx*theSin; 
            Dx=newX;

            //  Fail if segment C-D doesn't cross line A-B.
            if (Cy<0 && Dy<0 || Cy>=0 && Dy>=0) return intersectionPoint;

            //  (3) Discover the position of the intersection point along line A-B.
            ABpos=Dx+(Cx-Dx)*Dy/(Dy-Cy);

            //  Fail if segment C-D crosses line A-B outside of segment A-B.
            if (ABpos<0 || ABpos>distAB) return intersectionPoint;

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            intersectionPoint.X = Ax + ABpos * theCos;
            intersectionPoint.Y = Ay + ABpos * theSin;
            
            return intersectionPoint; 
        }

        public static Vector2[,] RectangleToLines(Rectangle rect)
        {
            Vector2[,] rectLines = new Vector2[4,2];
            //top
            rectLines[0, 0].X = rect.X;
            rectLines[0, 0].Y = rect.Y;
            rectLines[0, 1].X = rect.X+rect.Width;
            rectLines[0, 1].Y = rect.Y;
            //right
            rectLines[1, 0].X = rect.X+rect.Width;
            rectLines[1, 0].Y = rect.Y;
            rectLines[1, 1].X = rect.X + rect.Width;
            rectLines[1, 1].Y = rect.Y+rect.Height;
            //bottom
            rectLines[2, 0].X = rect.X;
            rectLines[2, 0].Y = rect.Y+rect.Height;
            rectLines[2, 1].X = rect.X + rect.Width;
            rectLines[2, 1].Y = rect.Y+rect.Height;
            //left
            rectLines[3, 0].X = rect.X;
            rectLines[3, 0].Y = rect.Y;
            rectLines[3, 1].X = rect.X;
            rectLines[3, 1].Y = rect.Y+rect.Height;

            return rectLines;
        }

    }
}

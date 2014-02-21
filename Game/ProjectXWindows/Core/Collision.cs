using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace ProjectXWindows.Core
{
    class Collision
    {
        public static Vector2[] LastSideTouched(Rectangle rect, Vector2 spriteStart, Vector2 spriteEnd)
        {
            Vector2[] sideTouched = new Vector2[4] { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
            Vector2[,] rectLines = Helper.RectangleToLines(rect);

            //check for each line of the rectangle if the sprite path has intersected
            for (int s = 0; s < rectLines.GetLength(0); s++)
            {
                //get intersection point of current lines
                Vector2 intersectionPoint = Helper.LineIntersection(spriteStart, spriteEnd, rectLines[s, 0], rectLines[s, 1]);
                //check to see if intersection can exist
                if (intersectionPoint.X > -1 && intersectionPoint.Y > -1)
                {
                    //0=top 1=right 2=bottom 3=left
                    //check to see if start point is in right place for entry intersection to exist

                    //if the intersection point is top
                    if (s == 0)
                    {
                        //if the start point Y is lower than the top line then top intersection cannot be entry
                        if (spriteStart.Y > rect.Y)
                            sideTouched[0] = new Vector2(-1, -1);
                        else
                            sideTouched[0] = intersectionPoint;
                    }

                    //if the intersection point is bottom
                    if (s == 2)
                    {
                        //if the start point Y is higher than the bottom line then bottom intersection cannot be entry
                        if (spriteStart.Y < (rect.Y+rect.Height))
                            sideTouched[2] = new Vector2(-1, -1);
                        else
                            sideTouched[2] = intersectionPoint;
                    }

                    //if the intersection point is right
                    if (s == 1)
                    {
                        //if the start point X is lower than the right line then right intersection cannot be entry
                        if (spriteStart.X < (rect.X + rect.Width))
                            sideTouched[1] = new Vector2(-1, -1);
                        else
                            sideTouched[1] = intersectionPoint;
                    }

                    //if the intersection point is left
                    if (s == 3)
                    {
                        //if the start point X is higher than the left line then left intersection cannot be entry
                        if (spriteStart.X > rect.X)
                            sideTouched[3] = new Vector2(-1, -1);
                        else
                            sideTouched[3] = intersectionPoint;
                    }
                }
            }
            return sideTouched;
        }

        public static Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            // Calculate half sizes.
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // Calculate centers.
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // Calculate current and minimum-non-intersecting distances between centers.
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            // If we are not intersecting at all, return (0, 0).
            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            // Calculate and return intersection depths.
            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Gets the position of the center of the bottom edge of the rectangle.
        /// </summary>
        public static Vector2 GetBottomCenter(Rectangle rect)
        {
            return new Vector2(rect.X + rect.Width / 2.0f, rect.Bottom);
        }

        public static Vector2 TexturesCollide(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);

            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0)
                            {
                                if (tex2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

    }
}

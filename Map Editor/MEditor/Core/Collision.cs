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
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect
            return (a.Right > b.Left && a.Left < b.Right &&
                    a.Bottom > b.Top && a.Top < b.Bottom);
        }

        public static bool Touches(Rectangle a, Rectangle b)
        {
            // check if two Rectangles intersect or touch sides
            return (a.Right >= b.Left && a.Left <= b.Right &&
                    a.Bottom >= b.Top && a.Top <= b.Bottom);
        }

        public static Rectangle[] GetBoundingSides(Rectangle rect, int depth)
        {
            int boundWidth = (int)rect.Width / depth;
            int boundHeight = (int)rect.Height / depth;

            Rectangle[] returns = new Rectangle[8];

            returns[0] = new Rectangle(rect.X, rect.Y, boundWidth, boundHeight);
            returns[1] = new Rectangle(rect.X + boundWidth, rect.Y, rect.Width - (boundWidth*2), boundHeight);
            returns[2] = new Rectangle(rect.Width - boundWidth, rect.Y, boundWidth, boundHeight);

            returns[7] = new Rectangle(rect.X, rect.Y + boundHeight, boundWidth, rect.Height - (boundHeight * 2));
            //returns[4] = new Rectangle(rect.X + boundWidth, rect.Y + boundHeight, rect.Width - (boundWidth * 2), rect.Height - (boundHeight * 2));
            returns[3] = new Rectangle(rect.Width - boundWidth, rect.Y + boundHeight, boundWidth, rect.Height - (boundHeight * 2));

            returns[6] = new Rectangle(rect.X, rect.Height-boundHeight, boundWidth, boundHeight);
            returns[5] = new Rectangle(rect.X + boundWidth, rect.Height - boundHeight, rect.Width - (boundWidth * 2), boundHeight);
            returns[4] = new Rectangle(rect.Width - boundWidth, rect.Height - boundHeight, boundWidth, boundHeight);

            return returns;
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
    }
}

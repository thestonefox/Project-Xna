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
using ProjectXWindows.Core;

namespace ProjectXWindows.Sprite_Elements
{
    class Map
    {
        public Tile[][] tiles;
        public Rectangle drawArea;
        public Point tileSize;        
        public Point tileOffset;
        private Point drawOffset;
        private Point tempOffset;        
        public Point scrollSpeed;
        private Point scrollTimer;        

        public Map(Rectangle gameArea)
        {
            drawArea = gameArea;
            tileOffset = new Point(0,0);
            drawOffset = Point.Zero;
            scrollTimer = new Point(0, 0);
            scrollSpeed = new Point(0, 0);
        }

        public void ScrollMap(Point newOffset)
        {
            if (newOffset.X != 0 && newOffset.X + tileOffset.X <= MaxOffset('X', 0) && newOffset.X + tileOffset.X >= 0 && scrollTimer.X == 0)
            {
                scrollTimer.X = (int)tileSize.X / scrollSpeed.X;
                tempOffset.X = newOffset.X;
            }

            if (newOffset.Y != 0 && newOffset.Y + tileOffset.Y <= MaxOffset('Y', 0) && newOffset.Y + tileOffset.Y >= 0 && scrollTimer.Y == 0)
            {
                scrollTimer.Y = (int)tileSize.Y / scrollSpeed.Y;
                tempOffset.Y = newOffset.Y;
            }
        }

        public Point SpriteOverTile(Sprite sprite)
        {
            //check to see what tile the sprite currently sits over
            Point answer = new Point((int)(((sprite.position.X - drawArea.X) / tileSize.X)-1)+tileOffset.X,(int)(((sprite.position.Y - drawArea.Y) / tileSize.Y)-1)+tileOffset.Y);
            //If the answer will be out of the array bounds, then limit it
            if (answer.X-tileOffset.X < 0) answer.X = 0+tileOffset.X;
            if (answer.Y-tileOffset.Y < 0) answer.Y = 0 + tileOffset.Y;
            if (answer.X >= (int)(drawArea.Width / tileSize.X) + tileOffset.X) answer.X = ((int)(drawArea.Width / tileSize.X)+tileOffset.X)-1;
            if (answer.Y >= (int)(drawArea.Height / tileSize.Y) + tileOffset.Y) answer.Y = ((int)(drawArea.Height / tileSize.Y) + tileOffset.Y) - 1;
            return answer;
        }

        public List<Tile> GetSurroundingTiles(Point current, int distance)
        {
            List<Tile> tempTiles = new List<Tile>();
            for (int x = current.X - distance; x <= current.X + distance; x++)            
            {
                for (int y = current.Y - distance; y <= current.Y + distance; y++)
                {
                    //if the current y/x tile exists then add it the the surrounding tiles list
                    if (x >= 0 && y >= 0 && x < tiles.Length && y < tiles[x].Length)
                        tempTiles.Add(tiles[x][y]);
                }
            }
            return tempTiles;
        }

        public Tile GetTile(int x, int y)
        {
            return tiles[x][y];
        }

        private void SmoothScroll()
        {
            if (scrollTimer.X >= scrollSpeed.X)
            {
                if(tempOffset.X>0)
                    drawOffset.X -= scrollSpeed.X;
                else if (tempOffset.X < 0)
                    drawOffset.X += scrollSpeed.X;
                scrollTimer.X -= 1;
            }
            else if (scrollTimer.X > 0)
            {
                scrollTimer.X = 0;                
                tileOffset.X += tempOffset.X;
                drawOffset.X = 0;
                tempOffset.X = 0;
            }

            if (scrollTimer.Y >= scrollSpeed.Y)
            {
                if (tempOffset.Y > 0)
                    drawOffset.Y -= scrollSpeed.Y;
                else if (tempOffset.Y < 0)
                    drawOffset.Y += scrollSpeed.Y;
                scrollTimer.Y -= 1;
            }
            else if (scrollTimer.Y > 0)
            {
                scrollTimer.Y = 0;
                tileOffset.Y += tempOffset.Y;
                drawOffset.Y = 0;
                tempOffset.Y = 0;
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            SmoothScroll();

            int xLoop = (int)(drawArea.Width / tileSize.X);
            if (xLoop >= tiles.Length) xLoop = tiles.Length;

            for (int x = 0; x < xLoop; x++)
            {
                int yLoop = (int)(drawArea.Height / tileSize.Y);
                if (yLoop >= tiles[x].Length) yLoop = tiles[x].Length;

                for (int y = 0; y < yLoop; y++)
                {
                    float xpos = ((x * tileSize.X) + drawOffset.X) + drawArea.X;
                    float ypos = ((y * tileSize.Y) + drawOffset.Y) + drawArea.Y;                  

                    if (tileOffset.X < 0) tileOffset.X = 0;
                    if (tileOffset.Y < 0) tileOffset.Y = 0;
                    if ((tileOffset.Y + y) >= tiles[x].Length) tileOffset.Y = MaxOffset('Y', x);
                    if ((tileOffset.X + x) >= tiles.Length) tileOffset.X = MaxOffset('X', 0);

                    tiles[x + tileOffset.X][y + tileOffset.Y].Prepare(new Vector2(xpos, ypos), false);
                    tiles[x + tileOffset.X][y + tileOffset.Y].Render(spriteBatch);
                }
            }
        }

        private int MaxOffset(char coodType, int arraypos)
        {
            if (coodType == 'X') return ((tiles.Length) - (int)(drawArea.Width / tileSize.X));
            if (coodType == 'Y') return ((tiles[arraypos].Length) - (int)(drawArea.Height / tileSize.Y));
            return 0;
        }        
    }
}

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
    class Sprite : ICloneable
    {
        public String guid;
        public Texture2D texture;

        public bool alive;
        public float depth;

        public Vector2 position;
        public Vector2 center;
        public Vector2 sizeModifier;
        public float width;
        public float height;
        public float rotation;
        public float facing;
        public float scale;

        public Color colour;
        public float alpha;

        public Color[,][,] pixelData;
        public Point currentFrame;

        public int dieTimerStart;
        public int dieTimerCurrent;

        private Texture2D debugTexture; 

        public Sprite(int timer, Texture2D _texture)
        {
            dieTimerStart = timer;
            Initialise(_texture, _texture.Width, _texture.Height);
        }

        public Sprite(int timer, Texture2D _texture, float _width, float _height)
        {
            dieTimerStart = timer;
            Initialise(_texture, _width, _height);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public bool Compare(Sprite sprite)
        {
            if (guid.Equals(sprite.guid)) return true;
            return false;
        }

        public void Dispose()
        {
            texture.Dispose();
        }

        private void Initialise(Texture2D _texture, float _width, float _height)
        {
            debugTexture = new Texture2D(Game1.Graphics().GraphicsDevice, 1, 1, 1, TextureUsage.None, SurfaceFormat.Color);
            Color[] myPixel = new Color[] { Color.CornflowerBlue };
            debugTexture.SetData<Color>(myPixel);

            guid = System.Guid.NewGuid().ToString();
            texture = _texture;
            alive = false;
            width = _width;
            height = _height;
            position = Vector2.Zero;
            colour = Color.White;
            alpha = 1.0f;
            rotation = 0.0f;
            facing = rotation;
            scale = 1.0f;
            depth = 0.0f;
            sizeModifier = Vector2.Zero;
            currentFrame = new Point(0, 0);
            dieTimerCurrent = 0;
            center = new Vector2(width / 2, height / 2);
            rotation=MathHelper.ToRadians(0);            
        }

        public void Prepare(Vector2 _position)
        {
            position = _position;
            alive = true;
        }

        public void Prepare(Vector2 _position, bool _setAlive)
        {
            //Ensure the position is top left and not the center of the object
            position = _position;
            if(_setAlive)
                alive = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CheckAlive();
            if (alive)
                spriteBatch.Draw(texture, new Rectangle(0,0, (int)width, (int)height), colour);                
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset, SpriteEffects spriteEffects)
        {
            CheckAlive();
            if (alive)
            {
                currentFrame = new Point(xOffset, yOffset);
                Rectangle source = new Rectangle(xOffset * (int)width, yOffset * (int)height, (int)width, (int)height);
                spriteBatch.Draw(texture, position, source, new Color(colour, alpha), rotation, center, scale, spriteEffects, depth);

                //if (this.GetType().Name.Equals("Player"))
                //    spriteBatch.Draw(debugTexture, GetBounds(), Color.Black);
            }
        }

        public Rectangle GetBounds()
        {
            Rectangle bounds = new Rectangle((int)position.X - (int)center.X, (int)position.Y - (int)center.Y, (int)width, (int)height);
            double[,] rectMat = Helper.RectangleToMatrix(bounds);
            rectMat = Helper.MatrixTransform(rectMat, 1.0, 1.0, 0, (int)-position.X, -(int)position.Y);
            rectMat = Helper.MatrixTransform(rectMat, 1.0, 1.0, rotation, 0.0, 0.0);
            rectMat = Helper.MatrixTransform(rectMat, 1.0, 1.0, 0, (int)position.X, (int)position.Y);
            bounds = Helper.MatrixToMaxRectangle(rectMat);

            int newWidth = bounds.Width + (int)sizeModifier.X;
            int newHeight = bounds.Height + (int)sizeModifier.Y;
            int newX = bounds.X - (int)(sizeModifier.X / 2);
            int newY = bounds.Y - (int)(sizeModifier.Y / 2);

            Rectangle final = new Rectangle(newX, newY, newWidth, newHeight);
            return final;
        }

        private void CheckAlive()
        {
            dieTimerCurrent -= 1;
            if (dieTimerCurrent < 0) dieTimerCurrent = 0;

            if (dieTimerCurrent == 1)
            {
                alive = false;
            }
        }

        public void SetColorArray()
        {
            int totalFrames = texture.Width / ((int)width);
            int totalLines = texture.Height / ((int)height);

            pixelData = new Color[totalFrames, totalLines][,];
            for (int row = 0; row < totalLines; row++)
            {
                for (int col = 0; col < totalFrames; col++)
                {
                    Color[] colors1D = new Color[(int)width * (int)height];
                    texture.GetData(0, new Rectangle(col * (int)width, row * (int)height, (int)width, (int)height), colors1D, 0, colors1D.Length);

                    Color[,] colors2D = new Color[(int)width, (int)height];
                    for (int x = 0; x < (int)width; x++)
                        for (int y = 0; y < (int)height; y++)
                            colors2D[x, y] = colors1D[x + y * (int)width];

                    pixelData[col, row] = colors2D;
                }
            }
        }
    }
}

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
        public float width;
        public float height;
        public Vector2 position;
        public Vector2 center;
        public Color colour;
        public float alpha;
        public float rotation;
        public float facing;
        public float scale;
        public float depth;

        public int dieTimerStart;
        public int dieTimerCurrent;

        public Sprite(int timer, Texture2D _texture)
        {
            dieTimerStart = timer;
            Initialise(_texture, texture.Width, texture.Height);
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

        private void Initialise(Texture2D _texture, float _width, float _height)
        {
            dieTimerCurrent = 0;
            guid = System.Guid.NewGuid().ToString();
            texture = _texture;
            alive = false;
            width = _width;
            height = _height;
            position = Vector2.Zero;
            center = new Vector2(width / 2, height / 2);
            colour = Color.White;
            alpha = 1.0f;
            rotation = 0.0f;
            facing = rotation;
            scale = 1.0f;
            depth = 0.0f;
        }

        public void Prepare(Vector2 _position)
        {
            //Ensure the position is top left and not the center of the object
            position = _position+center;
            alive = true;
        }

        public void Prepare(Vector2 _position, bool _setAlive)
        {
            //Ensure the position is top left and not the center of the object
            position = _position + center;
            if(_setAlive)
               alive = true;
        }

        public Rectangle GetBounds()
        {
            //Ensure the bounds are top, left, width, height and not the center of the object
            Rectangle bounds = new Rectangle((int)(position.X-center.X), (int)(position.Y-center.Y), (int)width, (int)height);
            return bounds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            CheckAlive();
            if (alive)
                spriteBatch.Draw(texture, GetBounds(), colour);                
        }

        public void Draw(SpriteBatch spriteBatch, int xOffset, int yOffset, SpriteEffects spriteEffects)
        {
            CheckAlive();
            if (alive)
            {
                Rectangle source = new Rectangle((int)(xOffset*width), (int)(yOffset*height), (int)width, (int)height);
                spriteBatch.Draw(texture, position, source, colour, rotation, center, scale, spriteEffects, depth);           
            }
        }

        private void CheckAlive()
        {
            if (dieTimerCurrent == 1) alive = false;
            dieTimerCurrent -= 1;
            if (dieTimerCurrent < 0) dieTimerCurrent = 0;
        }
    }
}

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
    class CollidableObject : AnimatedSprite
    {
        public bool collidable;
        public int power;
        public int lastSideTouched;
        private int initialEnergy;
        public int currentEnergy;
        public int worth;
        public bool destroyable;

        public CollidableObject(bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, bool isLooping)
            : base(timer, textureSheet, width, height, spriteLine, frameRate, isLooping)
        {
            destroyable = _destroyable;
            collidable = _collidable;
            power = _power;
            initialEnergy = energy;
            currentEnergy = energy;
            worth = points;
        }

        public bool HitTest(CollidableObject sprite)
        {
            bool collided = false;
            if (collidable && sprite.collidable && alive && sprite.alive && !sprite.guid.Equals(guid) && Collision.Touches(sprite.GetBounds(), GetBounds()))
            {   
                Vector2 intersectionDepth = Collision.GetIntersectionDepth(GetBounds(), sprite.GetBounds());
                if (intersectionDepth != Vector2.Zero)
                {
                    float absDepthX = Math.Abs(intersectionDepth.X);
                    float absDepthY = Math.Abs(intersectionDepth.Y);

                    if (absDepthY < absDepthX)
                    {
                        sprite.lastSideTouched = 0;
                        position = new Vector2(position.X, position.Y + intersectionDepth.Y);
                        collided = true;
                    }
                    else
                    {
                        sprite.lastSideTouched = 1;
                        position = new Vector2(position.X + intersectionDepth.X, position.Y);
                        collided = true;
                    }
                }
                else
                    sprite.lastSideTouched = -1;
            }

            //if collision has happened then remove energy
            if (collided)
            {
                if(currentEnergy>0)
                    currentEnergy -= sprite.power;
                if(sprite.currentEnergy>0)
                    sprite.currentEnergy -= power;
            }
            return collided;
        }

        public int CheckEnergy()
        {
            int points = 0;
            if (destroyable && currentEnergy <= 0 && dieTimerCurrent <= 0)
            {                
                currentEnergy = 0;
                dieTimerCurrent = dieTimerStart;
                collidable = false;
                points = worth;
            }
            return points;
        }
    }
}

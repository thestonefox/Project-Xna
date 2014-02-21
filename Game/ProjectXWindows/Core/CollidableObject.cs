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
        public int initialEnergy;
        public int currentEnergy;
        public int initialEnergySaver;
        public int energySaver;
        public bool energySaverOn;
        public int worth;
        public bool destroyable;
        public bool hascollided;

        public Vector2[] lastSideTouched;

        public CollidableObject(bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(timer, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            hascollided = false;
            destroyable = _destroyable;
            collidable = _collidable;
            power = _power;
            initialEnergy = energy;
            currentEnergy = energy;
            energySaver = 0;
            initialEnergySaver = energySaver;
            energySaverOn = false;
            worth = points;
            Vector2[] lastSideTouched = new Vector2[4] { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
        }

        public int GetSingleLastSideTouched()
        {
            int lastSide = 0;
            foreach (Vector2 lst in lastSideTouched)
            {
                if (lst.X > -1)
                    return lastSide;
                lastSide += 1;
            }
            return -1;
        }

        public void setEnergy(CollidableObject sprite)
        {
            if(!energySaverOn)
                currentEnergy -= sprite.power;
            if (!sprite.energySaverOn)
                sprite.currentEnergy -= power;
        }

        public bool BoundingCollision(CollidableObject sprite)
        {
            if (collidable && sprite.collidable && alive && sprite.alive && !sprite.guid.Equals(guid))
            {
                if (GetBounds().Intersects(sprite.GetBounds()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool TextureCollision(CollidableObject sprite)
        {
            Matrix spriteAMatrix = Matrix.CreateTranslation(-center.X, -center.Y, 0) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position.X, position.Y, 0);
            Matrix spriteBMatrix = Matrix.CreateTranslation(-sprite.center.X, -sprite.center.Y, 0) * Matrix.CreateRotationZ(sprite.rotation) * Matrix.CreateScale(sprite.scale) * Matrix.CreateTranslation(sprite.position.X, sprite.position.Y, 0);
            Vector2 collisionCheck = Collision.TexturesCollide(pixelData[currentFrame.X, currentFrame.Y], spriteAMatrix, sprite.pixelData[sprite.currentFrame.X, sprite.currentFrame.Y], spriteBMatrix);
            if (collisionCheck.X > -1 && collisionCheck.Y > -1)
            {
                return true;
            }


            return false;
        }

        public void ResolveBounding(CollidableObject sprite)
        {
            Vector2 intersectionDepth = Collision.GetIntersectionDepth(GetBounds(), sprite.GetBounds());
            if (intersectionDepth != Vector2.Zero)
            {
                float absDepthX = Math.Abs(intersectionDepth.X);
                float absDepthY = Math.Abs(intersectionDepth.Y);
                if (absDepthY < absDepthX)
                    position = new Vector2(position.X, position.Y + (intersectionDepth.Y * 1.05f));
                else
                    position = new Vector2(position.X + (intersectionDepth.X * 1.05f), position.Y);
            }
        }

        public void ResolveIntersecting(CollidableObject sprite)
        {
            foreach (Vector2 intersectingPoint in sprite.lastSideTouched)
            {
                if (intersectingPoint.X > -1 && intersectingPoint.Y > -1)
                {
                    float newX = intersectingPoint.X - position.X;
                    float newY = intersectingPoint.Y - position.Y;
                    position += new Vector2(newX, newY);
                    if (newX > 0)
                        position.X += (float)(center.X * 1.01);
                    else
                        position.X -= (float)(center.X * 1.01);

                    if (newY > 0)
                        position.Y += (float)(center.Y * 1.01);
                    else
                        position.Y -= (float)(center.Y * 1.01);
                }
            }
        }

        public int CheckEnergy()
        {
            return CheckEnergy(16);
        }

        public int CheckEnergy(int totalFrameCount)
        {
            int points = 0;
            if (destroyable && currentEnergy <= 0 && dieTimerCurrent <= 0)
            {
                currentEnergy = 0;
                dieTimerCurrent = dieTimerStart;
                collidable = false;
                points = worth;
                //change sprite line to explosion sprite
                SetSpriteLine(0, totalFrameCount);
                //stop the looping as explosions dont loop
                isLooping = false;
            }
            return points;
        }
    }
}

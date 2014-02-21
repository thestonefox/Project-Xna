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
using ProjectXWindows.Sprite_Elements;

namespace ProjectXWindows.Game_Elements.Projectiles
{
    class Laser : Projectile
    {
        private float rotateway;
        private int maxCollision;
        public Laser(int projectileNumber, Weapon firingWeapon, Vessel origin, float gravity, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(firingWeapon, origin, gravity, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            maxCollision = 0;
            rotateway = 45.0f;
            if (Helper.OddEvenChoice(projectileNumber) == 1)
                rotateway = -45.0f;

            rotation = origin.rotation;
            SetTrajectory(rotation);
        }

        public override void SetTrajectory(float rotate)
        {
            acceleration = 3.0f;
            trajectory = new Vector4[1];

            rotate = rotate - MathHelper.ToRadians(rotateway);

            Vector2 startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, 999999.0f, rotate);
        }

        public override int CheckCollision(CollidableObject sprite)
        {
            int score = 0;

            if (!originiator.Compare(sprite) && collisionTimer == 0.0f && Collision(sprite))
            {
                maxCollision += 1;
                //if the sprite is not destroyable then bounce the bullet
                if (!sprite.destroyable)
                {                   
                    reflection.X = 1;
                    reflection.Y = 1;

                    if (lastSideCollided == 1 || lastSideCollided == 3)
                    {
                        reflection.X = 1;
                        reflection.Y = 0;
                    }
                    if (lastSideCollided == 0 || lastSideCollided == 2)
                    {
                        reflection.X = 0;
                        reflection.Y = 1;
                    }

                    lastSideCollided = -1;
                    if (reflection.X == 1 && reflection.Y == 1) alive = false;
                }
                else
                {
                    score = GetDamage(sprite, false);
                    maxCollision = 0;
                }
            }
            if (maxCollision == 8)
            {
                maxCollision = 0;
                alive = false;
            }
            return score;
        }
    }
}

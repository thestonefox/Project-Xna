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
    abstract class Projectile : WaypointMovableObject
    {
        public Vessel originiator;
        public Weapon firingWeapon;
        public bool collided;
        public int lastSideCollided;
        public Vector2 safePosition;
        public int totalLife;
        public Projectile(Weapon _firingWeapon, Vessel origin, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            firingWeapon = _firingWeapon;
            originiator = origin;
            position = origin.position;
            safePosition = position;
            collided = false;
            lastSideCollided = -1;
            totalLife = 0;
        }

        protected bool Collision(CollidableObject sprite)
        {
            bool collide = BoundingCollision(sprite);
            collided = collide;
            return collide;
        }

        protected int GetDamage(CollidableObject sprite, bool aliveStatus)
        {
            alive = aliveStatus;
            setEnergy(sprite);
            return sprite.CheckEnergy();
        }

        public virtual int CheckCollision(CollidableObject sprite)
        {
            int score = 0;
            if (!originiator.Compare(sprite) && collisionTimer == 0 && Collision(sprite))
            {
                score = GetDamage(sprite, false);
            }
            //added to stop bullets living for ages and looking silly
            totalLife += 1;
            if (totalLife > 1000)
            {
                totalLife = 0;
                alive = false;
            }
            return score;
        }
    }
}

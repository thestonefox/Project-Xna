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
    class Missile : Projectile
    {
        private float rotateway;
        public Missile(int projectileNumber, Weapon firingWeapon, Vessel origin, float gravity, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(firingWeapon, origin, gravity, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            rotateway = -90.0f;
            if (Helper.OddEvenChoice(projectileNumber) == 1 && projectileNumber>0)
                rotateway = 90.0f;
            rotation = origin.rotation;
            SetTrajectory(rotation);
        }

        public override void SetTrajectory(float rotate)
        {
            acceleration = 2.5f;
            trajectory = new Vector4[2];
            rotate = rotate - MathHelper.ToRadians(rotateway);
            Vector2 startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, 20.0f, rotate);
            
            rotate = rotate + MathHelper.ToRadians(rotateway);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[1] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);
        }

    }
}

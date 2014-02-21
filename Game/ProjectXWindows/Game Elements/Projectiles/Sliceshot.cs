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
    class Sliceshot : Projectile
    {
        public Sliceshot(int projectileNumber, Weapon firingWeapon, Vessel origin, float gravity, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(firingWeapon, origin, gravity, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            rotation = origin.rotation;
            SetTrajectory(rotation);
        }

        public override void SetTrajectory(float rotate)
        {
            acceleration = 3.0f;
            float sliceDistance = 10.0f;
            float wobbleTime = 2.0f;
            trajectory = new Vector4[12];
            Vector2 startDirection;

            rotate = rotate + MathHelper.ToRadians(0);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[1] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[2] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[3] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[4] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[5] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[6] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[7] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[8] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[9] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[10] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[11] = new Vector4(startDirection.X, startDirection.Y, 0.0f, rotate);
        }
    }
}

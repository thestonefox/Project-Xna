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

namespace ProjectXWindows.Game_Elements.Enemies
{
    class Skipper : Enemy
    {
        public Skipper(float _acceleration, float initial_direction, Point[] _respawn, bool _levelEnder, float fireRate, Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(_respawn, _levelEnder, fireRate, projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            acceleration = _acceleration;
            facing = MathHelper.ToRadians(initial_direction);
            SetTrajectory(facing);
        }

        public override void SetTrajectory(float rotate)
        {            
            float sliceDistance = 16.0f;
            float wobbleTime = 8.0f;
            trajectory = new Vector4[8];
            Vector2 startDirection;

            rotate = rotate + MathHelper.ToRadians(0);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[1] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate + MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[2] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[3] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[4] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[5] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance+10.5f);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[6] = new Vector4(startDirection.X, startDirection.Y, wobbleTime, rotate);

            rotate = rotate - MathHelper.ToRadians(sliceDistance);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[7] = new Vector4(startDirection.X, startDirection.Y, 0.0f, rotate);
        }
}
}

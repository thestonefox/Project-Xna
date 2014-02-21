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
    class SkipperStatic : Enemy
    {
        private float travelTime;
        private float jumpHeight;

        public SkipperStatic(float _jumpHeight, float _travelTime, float _acceleration, float initial_direction, Point[] _respawn, bool _levelEnder, float fireRate, Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(_respawn, _levelEnder, fireRate, projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            travelTime = _travelTime;
            jumpHeight = _jumpHeight;
            acceleration = _acceleration;
            facing = MathHelper.ToRadians(initial_direction);
            SetTrajectory(facing);
        }

        public override void SetTrajectory(float rotate)
        {
            trajectory = new Vector4[18];
            Vector2 startDirection;

            float rotDirection = 0.0f;

            rotDirection = rotate + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[1] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[2] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight*3);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[3] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);


            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[3] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[4] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[5] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);


            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight * 3);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[6] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);
            rotDirection = rotDirection - MathHelper.ToRadians(180);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[7] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);



            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[8] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[9] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[10] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight * 3);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[11] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);


            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[12] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[13] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(jumpHeight);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[14] = new Vector4(startDirection.X, startDirection.Y, travelTime, rotate);

            rotDirection = rotDirection - MathHelper.ToRadians(jumpHeight * 3);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[15] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);
            rotDirection = rotDirection + MathHelper.ToRadians(180);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[16] = new Vector4(startDirection.X, startDirection.Y, 1.0f, rotate);

            rotDirection = rotDirection + MathHelper.ToRadians(0);
            startDirection = new Vector2((float)Math.Sin(rotDirection), (float)-Math.Cos(rotDirection) * acceleration);
            trajectory[17] = new Vector4(startDirection.X, startDirection.Y, 0.0f, rotate);
        }
    }
}

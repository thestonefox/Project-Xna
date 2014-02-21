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
    class Dart : Enemy
    {
        public Dart(float _acceleration, float initial_direction, float changeFacing, Point[] _respawn, bool _levelEnder, float fireRate, Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(_respawn, _levelEnder, fireRate, projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            facingAmend = MathHelper.ToRadians(changeFacing); 
            acceleration = _acceleration;
            float direction = MathHelper.ToRadians(initial_direction);
            facing = direction + facingAmend;
            SetTrajectory(direction);            
        }

        public override void SetTrajectory(float rotate)
        {
            trajectory = new Vector4[1];
            Vector2 startDirection;

            rotate = rotate + MathHelper.ToRadians(0);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, 99999.0f, rotate);
        }
    }
}

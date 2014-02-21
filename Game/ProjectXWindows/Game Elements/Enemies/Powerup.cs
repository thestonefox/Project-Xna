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
    class Powerup : Enemy
    {
        private int powerType;

        public Powerup(float _acceleration, float initial_direction, Point[] _respawn, bool _levelEnder, float fireRate, Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(_respawn, _levelEnder, fireRate, projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            powerType = spriteLine;
            acceleration = _acceleration;
            facing=MathHelper.ToRadians(initial_direction);
            SetTrajectory(facing);
        }

        public override void SetTrajectory(float rotate)
        {
            trajectory = new Vector4[1];
            Vector2 startDirection;

            rotate = rotate + MathHelper.ToRadians(0);
            startDirection = new Vector2((float)Math.Sin(rotate), (float)-Math.Cos(rotate)) * acceleration;
            trajectory[0] = new Vector4(startDirection.X, startDirection.Y, 99999.0f, rotate);
        }

        public override void CollisionEffects(Vessel player)
        {
            switch (powerType)
            {
                //extra health
                case 1: player.currentEnergy += 250;
                        break;
                //extra shield
                case 2: player.energySaver += 250;
                        break;
                //kill health
                case 3: player.currentEnergy -= 100;
                        break;
                //kill shield
                case 4: player.energySaver -= 100;
                        break;
                //add bullet
                case 5: player.ChangeWeapon(0, 16, 8, 1);
                        break;
                //add missiles
                case 6: player.ChangeWeapon(1, 6, 10, 2);
                        break;
                //add sliceshot
                case 7: player.ChangeWeapon(0, 5, 15, 3);
                        break;
                //add laser
                case 8: player.ChangeWeapon(1, 6, 2, 4);
                        break;
                //extra health
                default: player.currentEnergy += 250;
                        if (player.currentEnergy > player.initialEnergy)
                            player.currentEnergy = player.initialEnergy;
                        break;
            }
        }
    }
}

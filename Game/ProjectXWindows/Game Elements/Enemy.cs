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
using ProjectXWindows.Sprite_Elements;

namespace ProjectXWindows.Game_Elements
{
    class Enemy : Vessel
    {
        private float fireTimer;
        private float currentFireTimer;
        public bool levelEnder;
        public Point[] respawn;
        public bool spawned;

        public Enemy(Point[] _respawn, bool _levelEnder, float fireRate, Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            spawned = false;
            respawn = _respawn;
            fireTimer = fireRate;
            currentFireTimer = 0;
            GenerateWeapons();
            levelEnder = _levelEnder;
        }

        private void FireWeapon(ProjectileManager pm)
        {
            if (maxWeapons > 0)
            {
                currentFireTimer -= 1;
                if (currentFireTimer <= 0 && alive && currentEnergy>0)
                {
                    currentFireTimer = fireTimer;
                    foreach (Weapon w in weapons)
                    {
                        w.FireProjectile(pm);
                    }
                }
            }
        }

        public void Update(Map map, Player[] players, List<Enemy> enemies, ProjectileManager pm)
        {
            CalculateNextVector();
            MoveObjects(map, players, enemies);
            FireWeapon(pm);
        }

        public virtual void CollisionEffects(Vessel player) { }
    }
}

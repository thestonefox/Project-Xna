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
using ProjectXWindows.Game_Elements;
using ProjectXWindows.Game_Elements.Projectiles;

namespace ProjectXWindows.Sprite_Elements
{
    class Weapon
    {
        public Projectile[] projectiles;
        public int maxProjectiles;
        public int fireRate;
        public int ammoType;
        private Vessel originator;
        public int fireTimer;
        private Texture2D textureSheet;

        public Weapon(Texture2D projectileTextures, int _maxProjectiles, int _fireRate, int _ammoType, Vessel origin)
        {
            maxProjectiles = _maxProjectiles;
            fireRate = _fireRate;
            originator = origin;
            ammoType = _ammoType;
            fireTimer = 0;
            textureSheet = projectileTextures;
            GenerateProjectiles();
        }

        public void GenerateProjectiles()
        {
            projectiles = new Projectile[maxProjectiles];
            for (int i = 0; i < projectiles.Length; i++)
            {
                switch (ammoType)
                {
                    case 0: projectiles[i] = new Bullet(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 5.0f), true, true, 30, 0, 0, 0, textureSheet, 16, 16, 0, 0.05f, 1, false);
                            break;
                    case 1: projectiles[i] = new Bullet(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 4.0f), true, true, 5, 0, 0, 0, textureSheet, 16, 16, 1, 0.05f, 4, false);
                            break;
                    case 2: projectiles[i] = new Missile(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 2.0f), true, true, 15, 0, 0, 0, textureSheet, 16, 16, 2, 0.05f, 4, true);
                            break;
                    case 3: projectiles[i] = new Sliceshot(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 5.0f), true, true, 10, 0, 0, 0, textureSheet, 16, 16, 4, 0.1f, 4, true);
                            break;
                    case 4: projectiles[i] = new Laser(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 5.0f), true, true, 20, 0, 0, 0, textureSheet, 16, 16, 3, 0.1f, 4, true);
                            break;
                    case 5: projectiles[i] = new Bomb(i, this, originator, 0.0f, 0.0f, new Vector2(0.0f, 2.0f), true, true, 100, 0, 0, 0, textureSheet, 16, 16, 5, 0.05f, 4, false);
                            break;
                }
            }
        }

        public void Update()
        {
            //decrease fire time to prevent daft rapid fire
            fireTimer -= 1;
        }

        public void FireProjectile(ProjectileManager pm)
        {            
            if (fireTimer <= 0)
            {                
                //loop through projectiles until an alive projectile is found
                for (int p = 0; p < projectiles.Length; p++ )
                {
                    //set the found projectile to be live and initiate for moving
                    if (!projectiles[p].alive)
                    {
                        projectiles[p].Prepare(originator.position - (projectiles[p].center), 0);
                        projectiles[p].SetTrajectory(originator.facing);
                        projectiles[p].safePosition = projectiles[p].position;
                        projectiles[p].collided = false;
                        projectiles[p].totalLife = 0;
                        projectiles[p].ResetAnimation();
                        projectiles[p].lastSideCollided = -1;
                        pm.liveProjectiles.Add(projectiles[p]);
                        p = projectiles.Length;
                    }
                }
                fireTimer = fireRate;
            }
        }
    }
}

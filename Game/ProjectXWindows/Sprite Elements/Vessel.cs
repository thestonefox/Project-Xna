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

namespace ProjectXWindows.Sprite_Elements
{
    class Vessel : WaypointMovableObject
    {
        public int maxWeapons;
        public int score;
        public Vector2 weaponMount;
        public Weapon[] weapons;
        private Texture2D projectileTexture;
        public int rumbleTimer;
        public int explosionCount;

        public Vessel(Texture2D projectileTextures, int _maxWeapons, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            maxWeapons = _maxWeapons;
            weapons = new Weapon[maxWeapons];
            projectileTexture = projectileTextures;
            weaponMount = Vector2.Zero;
            explosionCount = 16;
            SetColorArray();
        }

        protected void GenerateWeapons()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i] = new Weapon(projectileTexture, 5, 20, 0, this);
            }
        }

        public void ChangeWeapon(int weaponIndex, int maxProjectiles, int fireRate, int ammoType)
        {
            weapons[weaponIndex].maxProjectiles = maxProjectiles;
            weapons[weaponIndex].fireRate = fireRate;
            weapons[weaponIndex].ammoType = ammoType;
            weapons[weaponIndex].GenerateProjectiles();
        }

        public void FireWeapon(int weaponIndex, ProjectileManager pm)
        {
            weapons[weaponIndex].FireProjectile(pm);
        }

        private int GetBoundaryCheck(Point dimensions)
        {
            int bwidth = (int)Math.Ceiling((decimal)(width / dimensions.X));
            int bheight = (int)Math.Ceiling((decimal)(height / dimensions.Y));
            if (bwidth > bheight)
                return bwidth;
            return bheight;
        }

        protected int MoveObjects(Map map, Player[] players, List<Enemy> enemies)
        {
            int score = 0;            
            foreach (Weapon weapon in weapons)
            {
                weapon.Update();
            }
            Vector2 collisionCheck = new Vector2(-1, -1);

            //check if vessel has hit any bounding tiles
            Tile[,] tileSet = map.GetSurroundingTiles(map.SpriteOverTile(this), GetBoundaryCheck(map.tileSize)+1);
            for(int x=0; x<tileSet.GetLength(0); x++)
            {
                for (int y = 0; y < tileSet.GetLength(1); y++)
                {
                    if (tileSet[x, y] != null)
                    {
                        Tile tile = tileSet[x, y];
                        //had the vessel hit the collidable tile?
                        if (BoundingCollision(tile))
                        {
                                ResolveBounding(tile);
                                //bounce off tile
                                acceleration = ReflectVector((acceleration));
                                setEnergy(tile);
                                CheckEnergy(explosionCount);
                                score += tile.CheckEnergy();
                                //If you've died on a tile then stop the movement
                                if (currentEnergy == 0) acceleration = 0.0f;

                                if (this.GetType().Name.Equals("Player"))
                                    rumbleTimer = 40;
                        }
                    }
                }
            }

            //If type is player then
            //check if vessel has hit any alive enemies
            if (this.GetType().Name.Equals("Player"))
            {
                foreach (Enemy enemy in enemies)
                {
                    if (BoundingCollision(enemy))
                    {
                        if (TextureCollision(enemy))
                        {
                            rumbleTimer = 20;
                            setEnergy(enemy);
                            CheckEnergy();
                            score += enemy.CheckEnergy();
                            //check to see if the collision has any effects on the player, like powerup
                            enemy.CollisionEffects(this);
                        }
                    }
                }
            }
            UpdatePosition();
            return score;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(gameTime, spriteBatch, SpriteEffects.None);
        }
    }
}

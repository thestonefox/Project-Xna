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

namespace ProjectXWindows.Game_Elements
{
    class ProjectileManager
    {
        public List<Projectile> liveProjectiles;

        public ProjectileManager()
        {
            liveProjectiles = new List<Projectile>();
        }

        private int CheckProjectileCollisions(CollidableObject sprite, Projectile projectile)
        {
            int score = 0;
            if (projectile.alive)
                score += projectile.CheckCollision(sprite);
            return score;
        }

        public int FindLastSideTouched(Map map, Projectile projectile, Point currentPosition, int currentFinalSide, ref Vector2 intersectingPoint)
        {
            Point[] directions = new Point[] { new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0) };
            int finalSide = currentFinalSide;
            Vector2 newPosition = projectile.position + (projectile.velocity * 2);
            //if the new position is not the same as the last position and still in the tileMatrix
            if (currentPosition.X >= 0 && currentPosition.Y >= 0 && currentPosition.X < map.tiles.Length && currentPosition.Y < map.tiles[0].Length)
            {
                Tile tile = map.GetTile(currentPosition);
                //if the tile is a collidable tile then check
                if (tile.collidable)
                {
                    //check to see if this tile has a last side touched                                                
                    tile.lastSideTouched = Core.Collision.LastSideTouched(tile.GetBounds(), projectile.safePosition, newPosition);
                    int actualSide = tile.GetSingleLastSideTouched();
                    if (actualSide > -1)
                    {
                        //reset projectile position to intersection point of the tile
                        intersectingPoint = tile.lastSideTouched[actualSide];
                        finalSide = FindLastSideTouched(map, projectile, new Point(currentPosition.X + directions[actualSide].X, currentPosition.Y + directions[actualSide].Y), actualSide, ref intersectingPoint);
                    }
                    tile.lastSideTouched = new Vector2[4] { new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1), new Vector2(-1, -1) };
                }
            }
            return finalSide;
        }

        public int Update(Map map, Player[] players, List<Enemy> enemies)
        {
            List<Projectile> deleted = new List<Projectile>();
            int score = 0;
            //loop through projectiles but then i need to access the checkcollision routine, how?!
            //calculate the current vector of the projectile
            foreach (Projectile projectile in liveProjectiles)
            {
                if (projectile.alive)
                {
                    Vessel orignalFirer = (Vessel)projectile.originiator;
                    //Check to see if the projectile has hit any bounding tiles
                    Tile[,] tileSet = map.GetSurroundingTiles(map.SpriteOverTile(projectile), 2);

                    //check each tile in set to see if it has collided
                    Vector2 resetPoint = new Vector2(-1,-1);
                    for (int x = 0; x < tileSet.GetLength(0); x++)
                    {
                        for (int y = 0; y < tileSet.GetLength(1); y++)
                        {
                            if (tileSet[x, y] != null)
                            {
                                Tile tile = tileSet[x, y];
                                if (tile.collidable && projectile.BoundingCollision(tile))
                                {
                                    projectile.lastSideCollided = FindLastSideTouched(map, projectile, tile.coordinates, -1, ref resetPoint);                  
                                    if (projectile.lastSideCollided > -1)
                                    {
                                        orignalFirer.score += CheckProjectileCollisions(tile, projectile);
                                        if (projectile.collided)
                                        {
                                            projectile.collided = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if(resetPoint.X>-1)
                    {
                        projectile.position = resetPoint - projectile.velocity;
                        projectile.safePosition = resetPoint - projectile.velocity;
                    }
                    
                    //Check to see if the projectile has hit any enemies
                    //Only check enemy hit if the projectile has come from a player
                    if (projectile.originiator.GetType().Name.Equals("Player"))
                    {
                        //check projectiles for hitting enemies
                        foreach (Enemy enemy in enemies)
                        {
                            orignalFirer.score += CheckProjectileCollisions(enemy, projectile);
                        }
                    }

                    //Check to see if the projectile has hit any players
                    foreach (Player player in players)
                    {
                        //can players shoot each other?
                        // if no then use  ! projectile.originiator.GetType().Name.Equals("Player")
                        // if yes then use  player != orignalFirer
                        if (! projectile.originiator.GetType().Name.Equals("Player"))
                        {
                            int checkScore = CheckProjectileCollisions(player, projectile);
                            orignalFirer.score += checkScore;
                        }
                    }

                    //expand draw area to check to see if enemy has left area
                    map.drawArea.Inflate(map.tileSize.X, map.tileSize.Y);
                    if (!map.drawArea.Contains(projectile.GetBounds()))
                        projectile.alive = false;
                    //return map to original size
                    map.drawArea.Inflate(-map.tileSize.X, -map.tileSize.Y);

                    projectile.CalculateNextVector();
                    projectile.UpdatePosition();
                }

                if (!projectile.alive)
                    deleted.Add(projectile);
            }

            //remove dead projectiles
            foreach (Projectile projectile in deleted)
                liveProjectiles.Remove(projectile);

            return score;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Projectile projectile in liveProjectiles)
            {
                if (projectile.alive)
                {
                    projectile.Draw(gameTime, spriteBatch, SpriteEffects.None);
                }
            }
        }

    }
}

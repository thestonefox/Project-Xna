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
using ProjectXWindows.Game_Elements.Enemies;

namespace ProjectXWindows.Game_Elements
{
    class EnemySpawner
    {
        public List<Enemy> liveEnemies;
        private Texture2D[] textures;
        private int lastPowerup;

        public EnemySpawner(Texture2D[] _textures)
        {
            lastPowerup = 0;
            liveEnemies = new List<Enemy>();
            textures = _textures;
        }

        public int AddLevelEnders(int type)
        {
/*
            int returnCount = 0;
            switch (type)
            {
                //Shaker Enemy Type 1 (Boss Type)
                case 23: returnCount += 1;
                        break;
            }
            return returnCount;
*/
            if (type >= 23) return 1;
            else return 0;
        }

        public void SpawnEnemy(int type, Vector2 position)
        {
            Enemy tmpEnemy = null;
            switch (type)
            {
                //None Enemy Powerup
                case 0:
                        int thisPowerup = Core.Helper.RealRandom(0, 12);
                        if (thisPowerup == lastPowerup)
                            thisPowerup += 1;
                        if (thisPowerup > 11) thisPowerup = 2;
                        if (thisPowerup == 9) thisPowerup = 1;
                        if (thisPowerup >= 10) thisPowerup = 6;
                        lastPowerup = thisPowerup;
                        tmpEnemy = new Powerup(1.5f, 270.0f, new Point[0], false, 100.0f, textures[0], 0, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 0, 100, 15, 0, textures[1], 32, 32, thisPowerup, 0.15f, 6, true);
                        break;
                //Curving Enemy Type 1 (SineType)
                case 1: tmpEnemy = new Swooper(15.0f, 10.0f, 3f, 270.0f, new Point[0], false, 50.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 50, 1, 30, 100, textures[2], 32, 32, 1, 0.05f, 8, true);
                        tmpEnemy.ChangeWeapon(0, 2, 20, 1);     
                        break;
                //Squid Swim Enemy Type 1 (Swimmer)
                case 2: tmpEnemy = new Squid(2.5f, 270.0f, new Point[0], false, 20.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 100, 10, 30, 100, textures[3], 64, 64, 1, 0.09f, 10, true);
                        tmpEnemy.ChangeWeapon(0, 1, 20, 3);  
                        break;
                //Dart Type 1 (StraightFlyer)
                case 3: tmpEnemy = new Dart(2.25f, 270.0f, 0.0f, new Point[0], false, 300.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 50, 1, 30, 50, textures[4], 48, 48, 1, 0.07f, 16, true);
                        tmpEnemy.ChangeWeapon(0, 1, 300, 1);  
                        break;
                //Skipper Type 1 (Floor Skipper)
                case 4: tmpEnemy = new Skipper(2.0f, 270.0f, new Point[0], false, 80.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 250, 1, 30, 200, textures[5], 28, 28, 1, 0.04f, 13, true);
                        tmpEnemy.ChangeWeapon(0, 4, 10, 4);    
                        break;
                //Dart Type 2 (Faller)
                case 5: tmpEnemy = new Dart(1.5f, 250.0f, 0.0f, new Point[] { new Point(0, 1) }, false, 100.0f, textures[0], 0, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 250, 1, 30, 100, textures[6], 32, 32, 1, 0.06f, 16, true);
                        break;
                //Curving Enemy Type 2 (Snake Up)
                case 6: tmpEnemy = new Swooper(20.0f, 5.0f, 4.0f, 290.0f, new Point[0], false, 30.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 100, 1, 30, 100, textures[7], 32, 32, 1, 0.05f, 9, true);
                        tmpEnemy.ChangeWeapon(0, 3, 10, 1); 
                        break;
                //Swirler Enemy Type 1
                case 7: tmpEnemy = new Swirler(15.0f, 15.0f, 1.2f, 1.0f, new Point[0], false, 10.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-3.0f, 3.0f), true, true, 100, 30, 30, 300, textures[8], 48, 48, 1, 0.04f, 16, true);
                        tmpEnemy.ChangeWeapon(0, 5, 5, 0);
                        break;
                //Dart Type 2 (BombDropper)
                case 8: tmpEnemy = new Dart(1.5f, 270.0f, 0.0f,new Point[] { new Point(5, 1) }, false, 30.0f, textures[0], 2, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 150, 40, 30, 50, textures[9], 48, 96, 1, 0.07f, 6, true);
                        tmpEnemy.ChangeWeapon(0, 3, 30, 0);
                        tmpEnemy.ChangeWeapon(1, 1, 30, 5);
                        break;
                //Fixed Type 1 (Missile SAM)
                case 9: tmpEnemy = new Dart(1.4f, 270.0f, 20.0f, new Point[0], false, 25.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.4f, 1.4f), true, true, 50, 1, 30, 50, textures[10], 30, 30, 1, 0.07f, 1, false);
                        tmpEnemy.ChangeWeapon(0, 2, 30, 2);
                        break;
                //Up Down (Fireball)
                case 10: tmpEnemy = new UpDown(1.5f, 0.0f, new Point[] {new Point(10, 1)}, false, 100.0f, textures[0], 0, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 450, 100, 30, 100, textures[11], 28, 28, 1, 0.06f, 7, true);
                        break;

                //Curving Enemy Type 2 (SineType 2)
                case 11: tmpEnemy = new Swooper(15.0f, 10.0f, 3f, 270.0f, new Point[0], false, 50.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 50, 1, 30, 100, textures[2], 32, 32, 1, 0.05f, 8, true);
                        tmpEnemy.ChangeWeapon(0, 2, 20, 0);
                        break;
                //Squid Swim Enemy Type 2 (Swimmer 2)
                case 12: tmpEnemy = new Squid(2.5f, 270.0f, new Point[0], false, 20.0f, textures[0], 2, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 100, 10, 30, 100, textures[3], 64, 64, 1, 0.09f, 10, true);
                        tmpEnemy.ChangeWeapon(0, 1, 20, 3);
                        tmpEnemy.ChangeWeapon(1, 4, 40, 2);
                        break;
                //Dart Type 2 (StraightFlyer 2)
                case 13: tmpEnemy = new Dart(2.25f, 270.0f, 0.0f, new Point[0], false, 300.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 50, 1, 30, 50, textures[4], 48, 48, 1, 0.07f, 16, true);
                        tmpEnemy.ChangeWeapon(0, 1, 300, 0);
                        break;
                //Curving Enemy Type 3 (Snake Down)
                case 14: tmpEnemy = new Swooper(-15.0f, 5.0f, 4.0f, 250.0f, new Point[0], false, 30.0f, textures[0], 1, 1.1f, 0.01f, new Vector2(-1.5f, 1.5f), true, true, 100, 1, 30, 100, textures[7], 32, 32, 1, 0.05f, 9, true);
                        tmpEnemy.ChangeWeapon(0, 3, 10, 1);
                        break;

                //Shaker Enemy Type 1 (Boss Type)
                case 23: tmpEnemy = new Shaker(1.0f, 270.0f, new Point[0], true, 10.0f, textures[0], 2, 1.1f, 0.01f, new Vector2(-3.0f, 3.0f), true, true, 50, 500, 150, 1500, textures[12], 224, 384, 1, 0.1f, 8, true);
                        tmpEnemy.ChangeWeapon(0, 10, 2, 4);
                        tmpEnemy.ChangeWeapon(1, 10, 6, 3);
                        tmpEnemy.explosionCount = 9;
                        break;

                //Skipper Static (Boss Type)
                case 24: tmpEnemy = new SkipperStatic(10.0f, 30.0f, 2.0f, 270.0f, new Point[0], true, 10.0f, textures[0], 2, 1.1f, 0.01f, new Vector2(-3.0f, 3.0f), true, true, 50, 700, 150, 1500, textures[12], 224, 376, 1, 0.1f, 8, true);
                        tmpEnemy.ChangeWeapon(0, 16, 2, 2);
                        tmpEnemy.ChangeWeapon(1, 10, 6, 4);
                        tmpEnemy.explosionCount = 9;
                        break;

                //Up Down Static (Boss Type)
                case 25: tmpEnemy = new UpDownStatic(70.0f, 2.0f, 270.0f, new Point[0], true, 100.0f, textures[0], 0, 1.1f, 0.01f, new Vector2(-4.0f, 4.0f), true, true, 50, 1000, 150, 2000, textures[12], 224, 384, 1, 0.06f, 7, true);
                        tmpEnemy.explosionCount = 9;   
                        break;
            }
            if (tmpEnemy != null && ( (type == 0 && tmpEnemy.GetSpriteLine()!=0) || (type!=0) ) )
            {
                tmpEnemy.Prepare(position);
                liveEnemies.Add(tmpEnemy);
            }
        }

        public int UpdateEnemies(Map map, Player[] players, List<Enemy> enemies, ProjectileManager pm)
        {
            int returnVal=0;
            List<Enemy> deletedIndex = new List<Enemy>();
            List<Enemy> addIndex = new List<Enemy>();
            foreach (Enemy enemy in liveEnemies)
            {
                //expand draw area to check to see if enemy has left area
                map.drawArea.Inflate(map.tileSize.X+(int)enemy.width, map.tileSize.Y+(int)enemy.height);
                if (!map.drawArea.Contains(enemy.GetBounds()))
                    enemy.alive = false;
                //return map to original size
                map.drawArea.Inflate(-(map.tileSize.X + (int)enemy.width), -(map.tileSize.Y + (int)enemy.height));

                enemy.Update(map, players, enemies, pm);

                if (enemy.currentEnergy == 0 && !enemy.spawned)
                {
                    enemy.spawned = true;
                    addIndex.Add(enemy);
                }

                //if the enemy is not alive then remove it from the list
                if (!enemy.alive)
                    deletedIndex.Add(enemy);
            }

            foreach (Enemy addEnemy in addIndex)
            {
                //respawn another enemy
                foreach (Point respawns in addEnemy.respawn)
                {
                    for (int i = 0; i < respawns.Y; i++)
                        SpawnEnemy(respawns.X, addEnemy.position);
                }
            }

            //Remove enemies tagged for deletion
            foreach (Enemy deleteEnemy in deletedIndex)
            {
                if (deleteEnemy.levelEnder)
                    returnVal += 1;
                liveEnemies.Remove(deleteEnemy);
            }
            return returnVal;
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in liveEnemies)
                enemy.Render(gameTime, spriteBatch);
        }
    }
}

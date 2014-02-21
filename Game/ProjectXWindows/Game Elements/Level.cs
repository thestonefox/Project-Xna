using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
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
using ProjectXWindows.Game_Elements.Enemies;

namespace ProjectXWindows.Game_Elements
{
    class Level
    {
        public Player[] players;
        private PlayerHud[] playerHuds;
        public int[] playerLives;
        public int[] playerScores;
        public Weapon[][] playerWeapons;
        private Map map;
        private EnemySpawner enemies;
        private ProjectileManager projectiles;

        private Rectangle gameArea;
        private ContentManager content;
        private XmlDocument levelsXml;
        public int levelIndex;
        public int totalPlayers;
        private int endOfLevel;

        public int gameState;

        public Level(ContentManager _content, Rectangle _gameArea, int _levelIndex)
        {
            gameArea = _gameArea;
            content = _content;
            levelsXml = new XmlDocument();            
            levelIndex = _levelIndex;
            endOfLevel = 0;
            gameState = 0;
        }

        public void Prepare(int _totalPlayers, int[] _playerLives, int[] _playerScores, Weapon[][] _playerWeapons)
        {
            totalPlayers = _totalPlayers;
            playerLives = _playerLives;
            playerScores = _playerScores;
            playerWeapons = _playerWeapons;
            NextLevel();
        }

        public void Update()
        {
            //check to see if the game is complete or you're dead
            CheckGameOver();

            if (gameState == 2)
                return;

            //check to see if the level has ended
            if (endOfLevel <= 0)
            {
                gameState = 1;
                //check to see if the next level exists
                if (!File.Exists(Path.Combine(StorageContainer.TitleLocation, "Content\\Maps\\level_" + (levelIndex+1) + ".xml")))
                {
                    WinGame();
                    return;
                }
            }

            projectiles.Update(map, players, enemies.liveEnemies);
            for (int p = 0; p < players.Length; p++)
            {
                if(players[p].alive)
                    players[p].Update(map, players, enemies.liveEnemies, projectiles);
            }
            endOfLevel -= enemies.UpdateEnemies(map, players, enemies.liveEnemies, projectiles);
            map.ScrollMap(new Point(1, 0));
        }

        public void Render(GameTime gameTime, SpriteBatch spriteBatch)
        {
            map.Render(spriteBatch, enemies);
            projectiles.Render(gameTime, spriteBatch);
            enemies.Render(gameTime, spriteBatch);
            for (int p = 0; p < players.Length; p++)
            {
                if (players[p].alive)
                    players[p].Render(gameTime, spriteBatch);
                playerHuds[p].Render(spriteBatch, "PLAYER "+(p+1));
            }
        }

        private void CheckGameOver()
        {
            int remainingLives = 0;
            for (int p = 0; p < players.Length; p++)
            {
                //store current data for player    
                playerLives[p] = players[p].lives;
                playerScores[p] = players[p].score;
                playerWeapons[p] = players[p].weapons;

                //check lives remaining
                remainingLives += players[p].lives;
            }
            //if lives for all players is 0 then game over screen
            if (remainingLives <= 0)
                gameState = 2;
        }

        private void WinGame()
        {
            gameState = 3;
        }

        private void NextLevel()
        {
            gameState = 0;
            LoadLevel(levelIndex, totalPlayers);
            if (gameState == 3)
                return;
        }

        private void LoadLevel(int levelID, int totalPlayers)
        {
            if (!File.Exists(Path.Combine(StorageContainer.TitleLocation, "Content\\Maps\\level_" + levelID + ".xml")))
            {
                WinGame();
                return;
            }
            levelsXml.Load(Path.Combine(StorageContainer.TitleLocation, "Content\\Maps\\level_" + levelID + ".xml"));           
            map = new Map(gameArea);
            XmlNode levelData = levelsXml.DocumentElement.SelectSingleNode("/levels/level[@id='" + levelID + "']");
            //If there is no level data then the game must have been completed
            if (levelData == null)
            {
                WinGame();
                return;
            }
            //Load Map Data
            XmlNode mapData = levelData.SelectSingleNode("map");
            Texture2D tileTextures = content.Load<Texture2D>(levelData.SelectSingleNode("map/@texture_path").InnerText);

            XmlNodeList enemyTexturesNodes = levelData.SelectNodes("enemyTextures/texture");
            Texture2D[] enemyTextures = new Texture2D[enemyTexturesNodes.Count];

            for (int t = 0; t < enemyTexturesNodes.Count; t++)
                enemyTextures[t] = content.Load<Texture2D>(enemyTexturesNodes[t].SelectSingleNode("@path").InnerText);

            enemies = new EnemySpawner(enemyTextures);
            projectiles = new ProjectileManager();

            Point scrollSpeed = new Point(Convert.ToInt32(levelData.SelectSingleNode("map/@scroll_x").InnerText), Convert.ToInt32(levelData.SelectSingleNode("map/@scroll_y").InnerText));
            XmlNodeList tileColumns = mapData.SelectNodes("column");

            map.tiles = new Tile[tileColumns.Count][];
            int tileWidth = 0;
            int tileHeight = 0;
            for (int x = 0; x < tileColumns.Count; x++)
            {
                XmlNodeList tileRows = tileColumns[x].SelectNodes("tile");
                map.tiles[x] = new Tile[tileRows.Count];
                for (int y = 0; y < tileRows.Count; y++)
                {                    
                    XmlNode tileCell = tileRows[y];
                    //get all graphic types
                    XmlNodeList graphics = tileCell.SelectNodes("graphics/graphic");
                    Vector4[] types = new Vector4[graphics.Count];
                    for (int g = 0; g < graphics.Count; g++)
                    {
                        XmlNode graphic = graphics[g];
                        types[g] = new Vector4(Convert.ToInt32(graphic.SelectSingleNode("@sprite_x").InnerText), Convert.ToInt32(graphic.SelectSingleNode("@sprite_y").InnerText), (float)Convert.ToDouble(graphic.SelectSingleNode("@rotation").InnerText), Convert.ToInt32(graphic.SelectSingleNode("@flip").InnerText));
                    }

                    int power = Convert.ToInt32(tileCell.SelectSingleNode("@power").InnerText);
                    int energy = Convert.ToInt32(tileCell.SelectSingleNode("@energy").InnerText);
                    int timer = 2;
                    int worth = Convert.ToInt32(tileCell.SelectSingleNode("@worth").InnerText);
                    bool destroyable = System.Convert.ToBoolean(tileCell.SelectSingleNode("@destroyable").InnerText);
                    bool collidable = System.Convert.ToBoolean(tileCell.SelectSingleNode("@collidable").InnerText);

                    tileWidth = Convert.ToInt32(tileCell.SelectSingleNode("dimensions/@width").InnerText);
                    tileHeight = Convert.ToInt32(tileCell.SelectSingleNode("dimensions/@height").InnerText);

                    //digest enemy spawn points
                    XmlNodeList enemiesToSpawn = tileCell.SelectNodes("enemies/enemy");
                    List<int> enemySpawns = new List<int>();
                    for (int e = 0; e < enemiesToSpawn.Count; e++)
                    {
                        XmlNode enemy = enemiesToSpawn[e];
                        int enemyToAdd=Convert.ToInt32(enemy.SelectSingleNode("@type").InnerText);
                        enemySpawns.Add(enemyToAdd);
                        endOfLevel += enemies.AddLevelEnders(enemyToAdd);
                    }

                    map.tiles[x][y] = new Tile(enemySpawns, 0.0f, new Point(x, y), types, 
                                            destroyable, collidable, power, energy, timer, worth, tileTextures,
                                            tileWidth, tileHeight);
                    map.tiles[x][y].alive = true;                    
                }
            }
            map.scrollSpeed = scrollSpeed;
            map.tileSize = new Point(tileWidth, tileHeight);

            //Load Player Data
            XmlNodeList playerNodes = levelData.SelectNodes("players/player");

            players = new Player[totalPlayers];
            playerHuds = new PlayerHud[totalPlayers];
            SpriteFont playerhudFont = content.Load<SpriteFont>("Fonts/hudfont");
            Texture2D pixelDot = content.Load<Texture2D>("Sprites/pixel");

            PlayerIndex[] playerIndexes = new PlayerIndex[] { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };
            Color[] playerColors = new Color[] { Color.Red, Color.Green };
            for (int p = 0; p < totalPlayers; p++)
            {                
                XmlNodeList weapons = playerNodes[p].SelectNodes("weapons/weapon");
                Texture2D projectileTexture = content.Load<Texture2D>(playerNodes[p].SelectSingleNode("@projectile_path").InnerText);
                Texture2D playerTexture = content.Load<Texture2D>(playerNodes[p].SelectSingleNode("@texture_path").InnerText);
                float pWeight = (float) Convert.ToDouble(playerNodes[p].SelectSingleNode("@weight").InnerText);
                int pPower = Convert.ToInt32(playerNodes[p].SelectSingleNode("@power").InnerText);
                //energy and shield is reset to full every map load
                int pEnergy = Convert.ToInt32(playerNodes[p].SelectSingleNode("@energy").InnerText);
                int pShield = Convert.ToInt32(playerNodes[p].SelectSingleNode("@shield").InnerText);
                int pWorth = Convert.ToInt32(playerNodes[p].SelectSingleNode("@worth").InnerText);

                //lives are set to current
                int pLives = Convert.ToInt32(playerNodes[p].SelectSingleNode("@lives").InnerText);
                if (playerLives[p] > -1)
                    pLives = playerLives[p];

                int pWidth = Convert.ToInt32(playerNodes[p].SelectSingleNode("dimensions/@width").InnerText);
                int pHeight = Convert.ToInt32(playerNodes[p].SelectSingleNode("dimensions/@height").InnerText);
                int pTileX = Convert.ToInt32(playerNodes[p].SelectSingleNode("tile_start/@x").InnerText);
                int pTileY = Convert.ToInt32(playerNodes[p].SelectSingleNode("tile_start/@y").InnerText);

                float xpos = ((pTileX * tileWidth)) + gameArea.X;
                float ypos = ((pTileY * tileHeight)) + gameArea.Y;   

                players[p] = new Player(new Vector2(xpos, ypos), pShield, playerIndexes[p], projectileTexture, weapons.Count, pLives, pWeight, 0.01f, new Vector2(-4.0f, 5.0f), true, true, pPower, pEnergy, 18, pWorth, playerTexture, pWidth, pHeight, 1, 0.025f, 1, false);
                //set the player score to the current level score held by the player
                players[p].score = playerScores[p];
                players[p].Prepare(players[p].startPosition);

                //set player huds 
                playerHuds[p] = new PlayerHud(pixelDot, playerhudFont, players[p], playerColors[p], new Vector2(60 + (600 * p), gameArea.Top + 60), 0.4f);

                //set weapons
                //If there are no set weapons then use default weapons
                if (playerWeapons[p]==null)
                {
                    playerWeapons[p] = new Weapon[weapons.Count];
                    for (int w = 0; w < weapons.Count; w++)
                    {
                        int wProjectiles = Convert.ToInt32(weapons[w].SelectSingleNode("@max_projectiles").InnerText);
                        int wRate = Convert.ToInt32(weapons[w].SelectSingleNode("@fire_rate").InnerText);
                        int wType = Convert.ToInt32(weapons[w].SelectSingleNode("@projectile_type").InnerText);
                        players[p].ChangeWeapon(w, wProjectiles, wRate, wType);
                    }
                }
                else
                {
                    for (int w = 0; w < playerWeapons[p].Length; w++)
                    {
                        Weapon tmpWep = playerWeapons[p][w];
                        players[p].ChangeWeapon(w, tmpWep.maxProjectiles, tmpWep.fireRate, tmpWep.ammoType);
                    }
                }               
            }            
        }
    }
}

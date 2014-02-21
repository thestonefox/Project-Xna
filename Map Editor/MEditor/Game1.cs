using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
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

namespace MEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private Map map;
        private Rectangle gameArea;
        private Point mapArea;
        private Point tileSize;
        private Vector2 mousePos;
        private MovableObject pointer;
        private Point currentClickTile;
        private int currentTileLevel;
        private int maxTileLevel;
        private int keyTimer;
        private Vector4 tileType;

        private Vector4 enemyType;

        Texture2D tileTextures;
        Texture2D enemyTextures;
        ScreenText writer;
        private Tile previewTile;
        private Tile previewEnemey;
        
        private int keytimermax;

        private int current_power;
        private int current_energy;
        private int current_worth;
        private bool current_destroyable;
        private bool current_collidable;
        private int current_flipfactor;
        private int current_rotation;
        private int current_scrollspeed;

        private int placement_type;

        private Point totalMapSize;

        private Helper myHelper; 

        public Game1()
        {
            keyTimer = 0;
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 800;
            //this.graphics.ToggleFullScreen();
            this.graphics.ApplyChanges();

            Content.RootDirectory = "Content";            
            tileSize = new Point(32, 32);

            totalMapSize=new Point(1280, 736);

            gameArea = new Rectangle(0 - tileSize.X, 0 - tileSize.Y, totalMapSize.X + (tileSize.X * 2), totalMapSize.Y);
            mapArea = new Point(500, 23);
            mousePos = new Vector2(0, 0);
            currentClickTile = new Point(0, 0);
            currentTileLevel = 0;
            maxTileLevel = 3;
            tileType = new Vector4(0, 0, 0, 0);
            enemyType = new Vector4(0, 0, 0, 0);
            myHelper = new Helper();
            placement_type = 0;

            keytimermax = 10;

            current_power = 0;
            current_energy = 0;
            current_worth = 0;
            current_destroyable = false;
            current_collidable = false;

            current_flipfactor = 0;
            current_rotation = 0;

            current_scrollspeed=2;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        public void LoadMap(String mapname, int levelID)
        {
            map = new Map(gameArea);
            XmlDocument levelsXml = new XmlDocument();
            levelsXml.Load(Path.Combine(StorageContainer.TitleLocation, mapname));
            XmlNode levelData = levelsXml.DocumentElement.SelectSingleNode("/levels/level[@id='" + levelID + "']");

            //Load Map Data
            XmlNode mapData = levelData.SelectSingleNode("map");
            Texture2D tileTextures = Content.Load<Texture2D>(levelData.SelectSingleNode("map/@texture_path").InnerText);
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
                    Vector4[] types = new Vector4[maxTileLevel];
                    for (int g = 0; g < types.Length; g++)
                    {
                        if (g < graphics.Count)
                        {
                            XmlNode graphic = graphics[g];
                            types[g] = new Vector4(Convert.ToInt32(graphic.SelectSingleNode("@sprite_x").InnerText), Convert.ToInt32(graphic.SelectSingleNode("@sprite_y").InnerText), (float)Convert.ToDouble(graphic.SelectSingleNode("@rotation").InnerText), Convert.ToInt32(graphic.SelectSingleNode("@flip").InnerText));
                        }
                        else
                        {
                            types[g] = new Vector4(0, 0, 0, 0);
                        }
                    }

                    int power = Convert.ToInt32(tileCell.SelectSingleNode("@power").InnerText);
                    int energy = Convert.ToInt32(tileCell.SelectSingleNode("@energy").InnerText);
                    int timer = 0;
                    int worth = Convert.ToInt32(tileCell.SelectSingleNode("@worth").InnerText);
                    bool destroyable = System.Convert.ToBoolean(tileCell.SelectSingleNode("@destroyable").InnerText);
                    bool collidable = System.Convert.ToBoolean(tileCell.SelectSingleNode("@collidable").InnerText);

                    tileWidth = Convert.ToInt32(tileCell.SelectSingleNode("dimensions/@width").InnerText);
                    tileHeight = Convert.ToInt32(tileCell.SelectSingleNode("dimensions/@height").InnerText);

                    //digest enemy spawn points
                    XmlNodeList enemies = tileCell.SelectNodes("enemies/enemy");
                    List<int> enemySpawns = new List<int>();
                    for (int e = 0; e < enemies.Count; e++)
                    {
                        XmlNode enemy = enemies[e];
                        enemySpawns.Add(Convert.ToInt32(enemy.SelectSingleNode("@type").InnerText)+1);
                    }


                    map.tiles[x][y] = new Tile(enemySpawns, 0.0f, new Point(x, y), types, 
                                            destroyable, collidable, power, energy, timer, worth, tileTextures,
                                            tileWidth, tileHeight);
                    map.tiles[x][y].alive = true;
                    map.tiles[x][y].SetContent(Content);
                    /*
                    if (collidable)
                        map.tiles[x][y].colour = Color.Red;
                    */
                }
            }
            map.scrollSpeed = scrollSpeed;
            map.tileSize = new Point(tileWidth, tileHeight);
        }

        private void CreateMap()
        {
            map = new Map(gameArea);
            map.tiles = new Tile[mapArea.X][];

            for (int x = 0; x < mapArea.X; x++)
            {
                map.tiles[x] = new Tile[mapArea.Y];
                for (int y = 0; y < mapArea.Y; y++)
                {
                    Vector4[] types = new Vector4[maxTileLevel];
                    for (int g = 0; g < maxTileLevel; g++)
                    {
                        Vector4 defaultPoint = new Vector4(0, 0, 0, 0);
                        if (g == 0)
                        {
                            Random r = new Random(Convert.ToInt32(Regex.Replace(System.Guid.NewGuid().ToString(), "[^0-9]", "").Substring(0, 1)));
                            defaultPoint = new Vector4(r.Next(6,10), 7, 0, 0);
                        }
                        types[g] = defaultPoint;
                    }

                    int power = 0;
                    int energy = 0;
                    int timer = 0;
                    int worth = 0;
                    bool destroyable = false;
                    bool collidable = false;

                    map.tiles[x][y] = new Tile(new List<int>(), 0.0f, new Point(x, y), types, 
                                            destroyable, collidable, power, energy, timer, worth, tileTextures,
                                            tileSize.X, tileSize.Y);
                    map.tiles[x][y].alive = true;
                    map.tiles[x][y].SetContent(Content);
                }
            }
            map.scrollSpeed = new Point(current_scrollspeed, current_scrollspeed);
            map.tileSize = new Point(tileSize.X, tileSize.Y);
        }

        public void NewMap()
        {
            map = null;
            CreateMap();
        }

        public void SaveMap()
        {
            String outputXML = "<map texture_path=\"Sprites\\Map\\level1\" scroll_x=\"2\" scroll_y=\"2\">" + System.Environment.NewLine;

            //loop through columns
            for (int x = 0; x < map.tiles.Length; x++)
            {
                outputXML += "<column x=\"" + x + "\">" + System.Environment.NewLine;
                for (int y = 0; y < map.tiles[x].Length; y++)
                {
                    outputXML += map.tiles[x][y].TileToXML();
                }
                outputXML += "</column>" + System.Environment.NewLine;
            }
            outputXML += "</map>" + System.Environment.NewLine;

            StreamWriter sw = new StreamWriter(Path.Combine(StorageContainer.TitleLocation, "outputlevel.xml"));
            sw.WriteLine(outputXML);
            sw.Close();            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here            
            tileTextures = Content.Load<Texture2D>("Sprites\\map\\level1");
            enemyTextures = Content.Load<Texture2D>("enemy");

            Texture2D pointerTexture = Content.Load<Texture2D>("pointer");

            pointer = new MovableObject(0.0f, 0.0f, new Vector2(0, 0), false, false, 0, 0, 0, 0, pointerTexture, 32, 32, 0, 0, false);
            pointer.Prepare(mousePos);

            //if cannot load a map file then create a new one
            CreateMap();

            previewTile = new Tile(null, 0.0f, new Point(0, 0), new Vector4[] { new Vector4(0, 0, 0, 0) }, false, false, 90, 0, 0, 0, tileTextures, tileSize.X, tileSize.Y);
            previewTile.Prepare(new Vector2(230.0f, gameArea.Height-5.0f));

            previewEnemey = new Tile(null, 0.0f, new Point(0, 0), new Vector4[] { new Vector4(0, 0, 0,0 ) }, false, false, 90, 0, 0, 0, enemyTextures, tileSize.X, tileSize.Y);
            previewEnemey.Prepare(new Vector2(230.0f, gameArea.Height + 30.0f));

            writer = new ScreenText(Content.Load<SpriteFont>("GameHud"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            map.scrollSpeed = new Point(current_scrollspeed, current_scrollspeed);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Right))
                map.ScrollMap(new Point(1,0));
            if (newState.IsKeyDown(Keys.Left))
                map.ScrollMap(new Point(-1, 0));

            if (newState.IsKeyDown(Keys.Delete) && keyTimer == 0)
            {
                this.Exit();
            }

            if (newState.IsKeyDown(Keys.Enter) && keyTimer == 0)
            {
                this.graphics.ToggleFullScreen();
                this.graphics.ApplyChanges();
            }

            if (newState.IsKeyDown(Keys.D0) && keyTimer==0)
            {
                keyTimer = keytimermax;
                currentTileLevel += 1;
                if (currentTileLevel >= maxTileLevel)
                    currentTileLevel = maxTileLevel - 1;
            }

            if (newState.IsKeyDown(Keys.D9) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                currentTileLevel -= 1;
                if (currentTileLevel <0)
                    currentTileLevel = 0;
            }

            if (newState.IsKeyDown(Keys.A) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (tileType.X - 1 >= 0)
                    tileType = new Vector4(tileType.X - 1, tileType.Y, myHelper.ChangeRotationByDegrees(current_rotation), current_flipfactor);
            }

            if (newState.IsKeyDown(Keys.D) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (tileType.X + 1 <= tileTextures.Width / tileSize.X)
                    tileType = new Vector4(tileType.X + 1, tileType.Y, myHelper.ChangeRotationByDegrees(current_rotation), current_flipfactor);
            }

            if (newState.IsKeyDown(Keys.W) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (tileType.Y - 1 >= 0)
                    tileType = new Vector4(tileType.X, tileType.Y - 1, myHelper.ChangeRotationByDegrees(current_rotation), current_flipfactor);
            }

            if (newState.IsKeyDown(Keys.S) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (tileType.Y + 1 <= tileTextures.Height / tileSize.Y)
                    tileType = new Vector4(tileType.X, tileType.Y + 1, myHelper.ChangeRotationByDegrees(current_rotation), current_flipfactor);
            }




            if (newState.IsKeyDown(Keys.F) && keyTimer == 0)
            {                
                keyTimer = keytimermax;
                if (enemyType.X - 1 >= 0)
                    enemyType = new Vector4(enemyType.X - 1, enemyType.Y, 0, 0);
            }

            if (newState.IsKeyDown(Keys.H) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (enemyType.X + 1 <= enemyTextures.Width / tileSize.X)
                    enemyType = new Vector4(enemyType.X + 1, enemyType.Y, 0, 0);
            }

            if (newState.IsKeyDown(Keys.T) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (enemyType.Y - 1 >= 0)
                    enemyType = new Vector4(enemyType.X, enemyType.Y - 1, 0, 0);
            }

            if (newState.IsKeyDown(Keys.G) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (enemyType.Y + 1 <= enemyTextures.Height / tileSize.Y)
                    enemyType = new Vector4(enemyType.X, enemyType.Y + 1, 0, 0);
            }


            //increase the current map area
            if (newState.IsKeyDown(Keys.NumPad8) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (mapArea.Y - 1 >= 0)
                    mapArea = new Point(mapArea.X, mapArea.Y - 1);
            }

            if (newState.IsKeyDown(Keys.NumPad2) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (mapArea.Y + 1 <= 1024)
                    mapArea = new Point(mapArea.X, mapArea.Y + 1);
            }

            if (newState.IsKeyDown(Keys.NumPad4) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (mapArea.X - 1 >= 0)
                    mapArea = new Point(mapArea.X - 1, mapArea.Y);
            }

            if (newState.IsKeyDown(Keys.NumPad6) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (mapArea.X + 1 <= 2048)
                    mapArea = new Point(mapArea.X + 1, mapArea.Y);
            }




            if (newState.IsKeyDown(Keys.F1) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (current_collidable == true)
                {
                    current_collidable = false;
                    current_power = 0;
                }
                else
                {
                    current_collidable = true;
                    current_power = 100;
                }
            }

            if (newState.IsKeyDown(Keys.F2) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                if (current_destroyable == true)
                    current_destroyable = false;
                else
                    current_destroyable = true;
            }

            if (newState.IsKeyDown(Keys.F3) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_power -= 1;
                if (current_power <= 0)
                    current_power = 0;
            }

            if (newState.IsKeyDown(Keys.F4) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_power += 1;
                if (current_power >= 200)
                    current_power = 200;
            }

            if (newState.IsKeyDown(Keys.F5) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_energy -= 1;
                if (current_energy <= 0)
                    current_energy = 0;
            }

            if (newState.IsKeyDown(Keys.F6) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_energy += 1;
                if (current_energy >= 200)
                    current_energy = 200;
            }

            if (newState.IsKeyDown(Keys.F7) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_worth -= 10;
                if (current_worth <= 0)
                    current_worth = 0;
            }

            if (newState.IsKeyDown(Keys.F8) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_worth += 10;
                if (current_worth >= 1000)
                    current_worth = 1000;
            }

            if (newState.IsKeyDown(Keys.F9) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_flipfactor += 1;
                if (current_flipfactor >= 4)
                    current_flipfactor = 0;
            }

            if (newState.IsKeyDown(Keys.F10) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_rotation += 90;
                if (current_rotation >= 360)
                    current_rotation = 0;
            }

            if (newState.IsKeyDown(Keys.D1) && keyTimer == 0)
            {
                NewMap();
            }

            if (newState.IsKeyDown(Keys.D2) && keyTimer == 0)
            {
                LoadMap("loadlevel.xml", 1);
            }

            if (newState.IsKeyDown(Keys.D3) && keyTimer == 0)
            {
                SaveMap();
            }

            if (newState.IsKeyDown(Keys.OemComma) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                placement_type -= 1;
                if (placement_type <0)
                    placement_type = 0;
            }

            if (newState.IsKeyDown(Keys.OemPeriod) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                placement_type += 1;
                if (placement_type > 2)
                    placement_type = 2;
            }


            if (newState.IsKeyDown(Keys.RightShift) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_scrollspeed += 1;
                if (current_scrollspeed > 20)
                    current_scrollspeed = 20;
            }

            if (newState.IsKeyDown(Keys.RightControl) && keyTimer == 0)
            {
                keyTimer = keytimermax;
                current_scrollspeed -= 1;
                if (current_scrollspeed < 2)
                    current_scrollspeed = 2;
            }

            // TODO: Add your update logic here
            keyTimer -= 1;
            if (keyTimer < 0) keyTimer = 0;
            UpdateMouse();
            
            previewTile.tileTypes[0] = tileType;

            previewTile.tileTypes[0].Z = myHelper.ChangeRotationByDegrees(current_rotation);
            previewTile.tileTypes[0].W = current_flipfactor;

            previewEnemey.tileTypes[0] = enemyType;

            base.Update(gameTime);
        }

        protected void UpdateMouse()
        {
            MouseState current_mouse = Mouse.GetState();

            // The mouse x and y positions are returned relative to the
            // upper-left corner of the game window.
            mousePos.X = current_mouse.X;
            mousePos.Y = current_mouse.Y;
            pointer.Prepare(mousePos-pointer.center);

            if (current_mouse.LeftButton == ButtonState.Pressed)
            {                
                currentClickTile = map.SpriteOverTile(pointer);

                if (currentClickTile.X < map.tiles.Length && currentClickTile.Y < map.tiles[0].Length)
                {
                    //tile
                    if (placement_type == 0)
                    {
                        Tile tempTile = new Tile(map.tiles[currentClickTile.X][currentClickTile.Y].enemySpawns, 0.0f, currentClickTile, map.tiles[currentClickTile.X][currentClickTile.Y].tileTypes, 
                                                    current_destroyable, current_collidable, current_power, current_energy, 0, current_worth, tileTextures,
                                                    tileSize.X, tileSize.Y);
                        map.tiles[currentClickTile.X][currentClickTile.Y] = tempTile;

                        map.tiles[currentClickTile.X][currentClickTile.Y].alive = true;
                        map.tiles[currentClickTile.X][currentClickTile.Y].SetContent(Content);
                        map.tiles[currentClickTile.X][currentClickTile.Y].tileTypes[currentTileLevel] = previewTile.tileTypes[0];
                    }

                    //enemy
                    if (placement_type == 1 && keyTimer == 0)
                    {
                        keyTimer = keytimermax*2;
                        if (enemyType.X == 0 && enemyType.Y == 0)
                        {
                            if (map.tiles[currentClickTile.X][currentClickTile.Y].enemySpawns.Count > 0)
                                map.tiles[currentClickTile.X][currentClickTile.Y].enemySpawns.RemoveAt(map.tiles[currentClickTile.X][currentClickTile.Y].enemySpawns.Count - 1);
                        }
                        else
                        {
                            int total_cols = (enemyTextures.Width / tileSize.X);
                            int enemyID = (total_cols * (int)enemyType.Y) + ((int)enemyType.X);
                            map.tiles[currentClickTile.X][currentClickTile.Y].enemySpawns.Add(enemyID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            map.Render(spriteBatch);            

            previewTile.Render(spriteBatch);
            previewEnemey.Render(spriteBatch);

            Color textCol = Color.White;
            writer.WriteText(spriteBatch, "Map Level [9|0]: " + currentTileLevel, new Vector2(120.0f, gameArea.Height - 10.0f), textCol);
            writer.WriteText(spriteBatch, "Power [F3|F4]: " + current_power, new Vector2(120.0f, gameArea.Height + 10.0f), textCol);
            writer.WriteText(spriteBatch, "Energy [F5|F6]: " + current_energy, new Vector2(120.0f, gameArea.Height + 30.0f), textCol);
            writer.WriteText(spriteBatch, "Worth [F7|F8]: " + current_worth, new Vector2(120.0f, gameArea.Height + 50.0f), textCol);

            writer.WriteText(spriteBatch, "Collidable [F1]: " + current_collidable, new Vector2(400.0f, gameArea.Height - 10.0f), textCol);
            writer.WriteText(spriteBatch, "Destroyable [F2]: " + current_destroyable, new Vector2(400.0f, gameArea.Height + 10.0f), textCol);
            writer.WriteText(spriteBatch, "Flip Factor [F9]: " + current_flipfactor, new Vector2(400.0f, gameArea.Height + 30.0f), textCol);
            writer.WriteText(spriteBatch, "Rotation [F10]: " + current_rotation, new Vector2(400.0f, gameArea.Height + 50.0f), textCol);

            writer.WriteText(spriteBatch, "Placement Type [,|.]: " + placement_type, new Vector2(640.0f, gameArea.Height - 10.0f), textCol);
            writer.WriteText(spriteBatch, "Map Size [NUM]: " + mapArea , new Vector2(640.0f, gameArea.Height + 10.0f), textCol)  ;
            writer.WriteText(spriteBatch, "Current Loc: " + map.tileOffset, new Vector2(640.0f, gameArea.Height + 30.0f), textCol);
            writer.WriteText(spriteBatch, "1: New | 2: Load | 3: Save", new Vector2(640.0f, gameArea.Height + 50.0f), textCol);            
            writer.WriteText(spriteBatch, "Scroll Speed [R:Shift|R:Ctrl]: "+ current_scrollspeed, new Vector2(940.0f, gameArea.Height - 10.0f), textCol);

            pointer.Draw(spriteBatch, 0, 0, SpriteEffects.None);
            writer.WriteText(spriteBatch, "X", pointer.position-pointer.center, Color.Red);

            spriteBatch.End();            

            base.Draw(gameTime);
        }
    }
}

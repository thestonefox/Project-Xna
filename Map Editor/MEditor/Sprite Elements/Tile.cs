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

namespace ProjectXWindows.Sprite_Elements
{
    class Tile : CollidableObject
    {
        public float dragFactor;
        public Point coordinates;
        public Vector4[] tileTypes;
        public List<int> enemySpawns;
        ContentManager Content;
        Texture2D enemyTextures;

        public void SetContent(ContentManager content)
        {
            Content = content;
            enemyTextures = Content.Load<Texture2D>("enemy");
        }

        public Tile(List<int> _enemySpawns, float drag, Point _coordinates, Vector4[] types, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height)
            : base(_destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, 0, 0, false)
        {
            dragFactor = drag;
            //x=x y=y z=rotation w=flip
            tileTypes = types;
            coordinates = _coordinates;
            enemySpawns = _enemySpawns;
        }

        public String TileToXML()
        {


            String outputXML = "<tile y=\"" + coordinates.Y + "\" collidable=\"" + collidable + "\" destroyable=\"" + destroyable + "\" worth=\"" + worth + "\" energy=\"" + currentEnergy + "\" power=\"" + power + "\">" + System.Environment.NewLine;
            outputXML += "<dimensions width=\"" + width + "\" height=\"" + height + "\"/>" + System.Environment.NewLine;
            outputXML += "<graphics>"+ System.Environment.NewLine;

            for (int g = 0; g < tileTypes.Length; g++)
            {
                if (tileTypes[g].X == 0 && tileTypes[g].Y == 0)
                {
                    outputXML += "";
                }
                else
                {
                    outputXML += "<graphic depth=\"" + g + "\" sprite_x=\"" + tileTypes[g].X + "\" sprite_y=\"" + tileTypes[g].Y + "\" rotation=\"" + tileTypes[g].Z + "\" flip=\"" + tileTypes[g].W + "\" />" + System.Environment.NewLine;
                }
            }
            outputXML += "</graphics>" + System.Environment.NewLine;


            if(enemySpawns.Count>0)
                outputXML += "<enemies>" + System.Environment.NewLine;
            foreach(int enemy in enemySpawns)
            {
            outputXML += "<enemy type=\""+(enemy-1)+"\"/>" + System.Environment.NewLine;                      
            }
            if (enemySpawns.Count > 0)
                outputXML += "</enemies>" + System.Environment.NewLine;

            outputXML += "</tile>" + System.Environment.NewLine;
            return outputXML;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (Vector4 tileType in tileTypes)
            {
                SpriteEffects spriteEffects = SpriteEffects.None;
                switch ((int)tileType.W)
                {
                    case 1: spriteEffects = SpriteEffects.FlipHorizontally;
                        break;
                    case 2: spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                        break;
                    case 3: spriteEffects = SpriteEffects.FlipVertically;
                        break;
                }
                rotation = tileType.Z;
                Draw(spriteBatch, (int)tileType.X, (int)tileType.Y, spriteEffects);
            }

            if (enemySpawns != null)
            {
                //render enemies
                foreach (int enemySpawn in enemySpawns)
                {
                    //id = (total rows * (current line - 1)) + current  row
                    //int enemyID = (total_rows * enemyType.Y) + (enemyType.X);

                    int total_cols = (int)(enemyTextures.Width / width);

                    int currow = (int)Math.Floor((decimal)enemySpawn / total_cols);
                    int curcol = enemySpawn - (total_cols * currow);
                    
                    //make a new sprite
                    Sprite enemyTemp = new Sprite(0, enemyTextures, 32, 32);
                    enemyTemp.Prepare(position-center);
                    enemyTemp.alive = true;
                    enemyTemp.Draw(spriteBatch, curcol, currow, SpriteEffects.None);                    
                }
            }
        }
    }
}

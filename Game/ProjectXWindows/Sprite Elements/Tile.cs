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

        public Tile(List<int> _enemySpawns, float drag, Point _coordinates, Vector4[] types, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height)
            : base(_destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, 0, 0, 0, false)
        {
            dragFactor = drag;
            tileTypes = types;
            coordinates = _coordinates;
            enemySpawns = _enemySpawns;
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
        }
    }
}

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
using ProjectXWindows.Core;

namespace ProjectXWindows.Game_Elements
{
    class PlayerHud
    {
        private Player player;
        private Color color;
        private Vector2 position;
        private Sprite bar;
        private SpriteFont font;
        private TextWriter tw;
        
        public PlayerHud(Texture2D _texture, SpriteFont _font, Player _player, Color _color, Vector2 _position, float _transparency)
        {
            player = _player;
            color = new Color(_color, _transparency);
            position = _position;
            font = _font;
            bar = new Sprite(10, _texture);
            bar.Prepare(Vector2.Zero);
            bar.alpha = _transparency;

            tw = new TextWriter(font);
        }

        public void Render(SpriteBatch spriteBatch, String title)
        {
            tw.WriteText(spriteBatch, title, position, color, 0);
            tw.WriteText(spriteBatch, "LIVES REMAINING: "+player.lives, position+new Vector2(0, (font.MeasureString("A").Y * 1.1f) ), color, 0);
            tw.WriteText(spriteBatch, "SCORE: " + player.score.ToString().PadLeft(12, '0'), position + new Vector2(font.MeasureString("LIVES REMAINING: " + player.lives + "        ").X, (font.MeasureString("A").Y * 1.1f)), color, 0);

            //energy bar
            DrawBar(spriteBatch, position + new Vector2(0, (font.MeasureString("A").Y * 2.1f)), Color.Red, 18, 240, player.initialEnergy, player.currentEnergy);
            //shield bar
            DrawBar(spriteBatch, position + new Vector2(268, (font.MeasureString("A").Y * 2.1f)), Color.Blue, 18, 240, player.initialEnergySaver, player.energySaver);
        }
        
        private void DrawBar(SpriteBatch spriteBatch, Vector2 pos, Color col, int height, int maxWidth, int initialValue, int currentValue)
        {
            bar.height = height;
            bar.position = pos;

            bar.width = maxWidth;
            bar.colour = Color.Black;
            bar.Draw(spriteBatch, 0, 0, SpriteEffects.None);

            bar.width = (float)((float)maxWidth / (float)initialValue) * (float)currentValue;
            bar.colour = col;
            bar.Draw(spriteBatch, 0, 0, SpriteEffects.None);
        }


    }
}

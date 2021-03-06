﻿using System;
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

namespace ProjectXWindows.Core
{
    class ScreenText
    {
        SpriteFont font;

        public ScreenText(SpriteFont loadedFont)
        {
            font = loadedFont;
        }

        public void WriteText(SpriteBatch sb, String text, Vector2 position, Color color)
        {
            Vector2 FontOrigin = font.MeasureString(text) / 2;
            sb.DrawString(font, text, position, color, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}

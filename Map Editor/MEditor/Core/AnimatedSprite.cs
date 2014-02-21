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

namespace ProjectXWindows.Core
{
    class AnimatedSprite : Sprite
    {
        public float frameRate;
        public bool isLooping;
        private int spriteLine;

        private int frameIndex;
        private float elapsedTime;

        public AnimatedSprite(int timer, Texture2D textureSheet, float width, float height, int _spriteLine, float _frameRate, bool _isLooping)
            : base(timer, textureSheet, width, height)
        {
            frameRate = _frameRate;
            isLooping = _isLooping;
            SetSpriteLine(_spriteLine);
        }

        public int FrameCount()
        {
            return (int)(texture.Width / width);
        }

        public void SetSpriteLine(int _spriteLine)
        {
            if (spriteLine == _spriteLine)
                return;
            spriteLine= _spriteLine;
            frameIndex = 0;
            elapsedTime = 0.0f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frameRate > 0)
            {
                while (elapsedTime > frameRate)
                {
                    elapsedTime -= frameRate;
                    if (isLooping)
                        frameIndex = (frameIndex + 1) % FrameCount();
                    else
                        frameIndex = Math.Min(frameIndex + 1, FrameCount() - 1);
                }
            }
            else
            {
                frameIndex = 0;
            }
            Draw(spriteBatch, frameIndex, spriteLine, spriteEffects);
        }
    }
}

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
        private int frameCount;

        public AnimatedSprite(int timer, Texture2D textureSheet, float width, float height, int _spriteLine, float _frameRate, int _frameCount, bool _isLooping)
            : base(timer, textureSheet, width, height)
        {
            frameRate = _frameRate;
            frameCount = 0;
            isLooping = _isLooping;
            SetSpriteLine(_spriteLine, _frameCount);
        }

        public int FrameCount()
        {
            if (frameCount > 0) return frameCount;
            return (int)(texture.Width / width);
        }

        public void ResetAnimation()
        {
            frameIndex = 0;
            elapsedTime = 0.0f;         
        }

        public void SetSpriteLine(int _spriteLine, int _frameCount)
        {
            frameCount = _frameCount;
            if (spriteLine == _spriteLine)
                return;
            spriteLine= _spriteLine;
            ResetAnimation();
        }

        public int GetSpriteLine()
        {
            return spriteLine;
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

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
    class MovableObject : CollidableObject
    {
        public Vector2 velocity;
        protected Vector2 reflection;
        public float weight;
        public float acceleration;
        private float deceleration;
        private Vector2 accelerationLimits;

        public MovableObject(float _weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, bool isLooping)
            : base(_destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, isLooping)
        {
            weight = _weight;
            velocity = Vector2.Zero;
            reflection = Vector2.Zero;
            acceleration = 0;
            deceleration = _deceleration;
            accelerationLimits = _accelerationLimits;
        }

        public float ReflectVector(float vector)
        {
            return vector - vector - vector;
        }

        public void UpdatePosition()
        {
            acceleration = MathHelper.Clamp(acceleration, accelerationLimits.X, accelerationLimits.Y);

            float xPos = velocity.X;
            if (reflection.X == 1)
            {
                xPos = ReflectVector(velocity.X);
                reflection.X = 0;
            }

            float yPos = velocity.Y;
            if (reflection.Y == 1)
            {
                yPos = ReflectVector(velocity.Y);
                reflection.Y = 0;
            }
            velocity = new Vector2(xPos, yPos);
            position += velocity * acceleration;
        }

        public void Decelerate()
        {
            if (acceleration > 0) acceleration -= deceleration;
            else if (acceleration < 0) acceleration += deceleration;
            if (acceleration > -0.2f && acceleration < 0.2f) acceleration = 0;
        }

        public void Decelerate(ref float direction)
        {
            if (direction > 0) direction -= deceleration;
            else if (direction < 0) direction += deceleration;
            if (direction > -0.2f && direction < 0.2f) direction = 0;
        }
    }
}

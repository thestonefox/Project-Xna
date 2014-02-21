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
    class WaypointMovableObject : MovableObject
    {
        protected Vector4[] trajectory;
        protected int trajectoryIndex;
        private float trajectoryTimer;
        protected float collisionTimer;
        protected float lifeTimer;
        public float facingAmend;

        public WaypointMovableObject(float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount,  isLooping)
        {
            facingAmend = 0.0f;
            trajectory = new Vector4[0];
            trajectoryIndex = 0;
            trajectoryTimer = 0.0f;            
            collisionTimer = 0.0f;
            lifeTimer = 0.0f;
        }

        public void Prepare(Vector2 newPosition, int resetTrajectory)
        {
            Prepare(newPosition);
            reflection = Vector2.Zero;
            trajectoryIndex = resetTrajectory;
            trajectoryTimer = 0.0f;
            collisionTimer = 0.0f;
            lifeTimer = 0.0f;
        }

        public virtual void SetTrajectory(float rotate) { }

        public void CalculateNextVector()
        {
            //This works its way through the current trajectory and sets the movement vector accordingly
            if (trajectoryTimer <= 0.0f)
            {
                rotation=trajectory[trajectoryIndex].W;
                facing = rotation + facingAmend;
                trajectoryTimer = trajectory[trajectoryIndex].Z;
                velocity = new Vector2(trajectory[trajectoryIndex].X, trajectory[trajectoryIndex].Y);
                trajectoryIndex += 1;
                if (trajectoryIndex >= trajectory.Length)
                {
                    if (trajectoryTimer > 0)
                        trajectoryIndex = trajectory.Length - 1;
                    else
                        trajectoryIndex = 0;
                }
            }
            trajectoryTimer -= 1.0f;
            collisionTimer -= 1.0f;
            if (collisionTimer < 0) collisionTimer = 0.0f;
            lifeTimer -= 1.0f;
            if (lifeTimer == 1) alive = false;
            if (lifeTimer < 0) lifeTimer = 0.0f;
        }
    }
}

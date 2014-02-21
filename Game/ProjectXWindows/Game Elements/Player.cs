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
    class Player : Vessel
    {
        public int lives;
        private InputState input;
        private PlayerIndex playerIndex;
        public Vector2 startPosition;
        private PlayerIndex actualPlayerIndex;
        private int safetyTimer;

        public Player(Vector2 _startPosition, int shieldPower, PlayerIndex _playerIndex, Texture2D projectileTextures, int _maxWeapons, int _lives, float weight, float _deceleration, Vector2 _accelerationLimits, bool _destroyable, bool _collidable, int _power, int energy, int timer, int points, Texture2D textureSheet, float width, float height, int spriteLine, float frameRate, int _frameCount, bool isLooping)
            : base(projectileTextures, _maxWeapons, weight, _deceleration, _accelerationLimits, _destroyable, _collidable, _power, energy, timer, points, textureSheet, width, height, spriteLine, frameRate, _frameCount, isLooping)
        {
            startPosition = _startPosition;
            playerIndex = _playerIndex;
            lives = _lives;
            score = 0;
            energySaver = shieldPower;
            initialEnergySaver = energySaver;
            rotation = MathHelper.ToRadians(90);
            facing = MathHelper.ToRadians(90);
            GenerateWeapons();
            input = new InputState();
            actualPlayerIndex = new PlayerIndex();
            safetyTimer = 0;
            rumbleTimer = 0;
            sizeModifier = new Vector2(0, -25);
        }

        private void CaptureMovement(ProjectileManager pm)
        {
            input.Update();
            //check movement            
            //check fire is pressed for specific weapon
            if (input.IsNewKeyHeld(Keys.Space, playerIndex, out actualPlayerIndex) || 
                input.IsNewButtonHeld(Buttons.A, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.RightTrigger, playerIndex, out actualPlayerIndex))
            {                
                weapons[0].FireProjectile(pm);
            }

            if (input.IsNewKeyHeld(Keys.LeftShift, playerIndex, out actualPlayerIndex) || 
                input.IsNewButtonHeld(Buttons.B, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.LeftTrigger, playerIndex, out actualPlayerIndex))
            {                
                weapons[1].FireProjectile(pm);
            }

            bool defaultLine = false;
            int energyAdder = 0;

            if ((input.IsNewKeyHeld(Keys.LeftControl, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.X, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.LeftShoulder, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.RightShoulder, playerIndex, out actualPlayerIndex) ||
                safetyTimer > 0) && energySaver>0)
            {
                safetyTimer -= 1;
                if (safetyTimer < 0) safetyTimer = 0;
                if(safetyTimer==0)
                    energySaver -= 2;
                if (energySaver < 0) energySaver = 0;
                energyAdder = 4;
                defaultLine = true;
                energySaverOn = true;
            }
            else
            {
                defaultLine = false;
                energySaverOn = false;
            }

            //check for movement keys
            if (input.IsNewKeyHeld(Keys.A, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.DPadLeft, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.LeftThumbstickLeft, playerIndex, out actualPlayerIndex))
            {
                velocity.X = -0.5f;
                acceleration += 2.0f;
                defaultLine = false;
            }
            else if (input.IsNewKeyHeld(Keys.D, playerIndex, out actualPlayerIndex) ||
                     input.IsNewButtonHeld(Buttons.DPadRight, playerIndex, out actualPlayerIndex) ||
                     input.IsNewButtonHeld(Buttons.LeftThumbstickRight, playerIndex, out actualPlayerIndex))
            {
                velocity.X = 0.5f;
                acceleration += 2.0f;
                defaultLine = false;
            }
            else
            {
                Decelerate(ref velocity.X);
                defaultLine = false;
            }

            if (input.IsNewKeyHeld(Keys.W, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.DPadUp, playerIndex, out actualPlayerIndex) ||
                input.IsNewButtonHeld(Buttons.LeftThumbstickUp, playerIndex, out actualPlayerIndex))
            {
                velocity.Y = -0.5f;
                acceleration += 1.0f;
                sizeModifier = new Vector2(0, 0);
                defaultLine = true;
                SetSpriteLine(3 + energyAdder, 5);
            }
            else if (input.IsNewKeyHeld(Keys.S, playerIndex, out actualPlayerIndex) ||
                     input.IsNewButtonHeld(Buttons.DPadDown, playerIndex, out actualPlayerIndex) ||
                     input.IsNewButtonHeld(Buttons.LeftThumbstickDown, playerIndex, out actualPlayerIndex))
            {
                velocity.Y = 0.5f;
                acceleration += 1.0f;
                sizeModifier = new Vector2(0, 0);
                defaultLine = true;
                SetSpriteLine(2 + energyAdder, 5);
            }
            else
            {
                Decelerate(ref velocity.Y);
                sizeModifier = new Vector2(0, -25);
                if (defaultLine == false)
                {
                SetSpriteLine(1 + energyAdder, 1);
                }
            }
        }

        public void Update(Map map, Player[] players, List<Enemy> enemies, ProjectileManager pm)
        {
            if (currentEnergy > 0)
                CaptureMovement(pm);

            bool newLife = true;
            //if energy goes too high then give some more life
            if (energySaver > initialEnergySaver)
            {
                energySaver = initialEnergySaver;
                currentEnergy += 50;
                newLife = false;
            }
            //if health goes too high then get new life
            if (currentEnergy > initialEnergy)
            {
                bool iWantToGiveNewLife = false;
                if (newLife && iWantToGiveNewLife)
                    lives += 1;
                currentEnergy = initialEnergy;
            }

            if (dieTimerCurrent==2)
            {
                //decrease lives and reset energy
                lives -= 1;
                dieTimerCurrent = 0;
                //if there are still more lives then reset player position
                if (lives > 0)
                {
                    currentEnergy = initialEnergy;
                    energySaver = initialEnergySaver;
                    safetyTimer = 100;
                    //reset all weapon bays
                    ChangeWeapon(0, 4, 15, 1);
                    for (int w = 1; w < weapons.Length; w++)
                    {
                        ChangeWeapon(w, 0, 10, 1);
                    }
                    Prepare(startPosition);
                }
                else
                {
                    alive = false;
                }
            }

            if (safetyTimer > 0)
            {
                alpha = 0.6f;
                collidable = false;
            }
            else
            {
                alpha = 1.0f;
                collidable = true;
            }
            score += MoveObjects(map, players, enemies);

            if (rumbleTimer > 0)
            {
                rumbleTimer -= 1;
                if(energySaverOn)
                    input.DoRumble(playerIndex, out actualPlayerIndex, 0.15f, 0.15f);
                else
                    input.DoRumble(playerIndex, out actualPlayerIndex, 0.3f, 0.5f);
            }
            else
            {
                rumbleTimer = 0;
                input.DoRumble(playerIndex, out actualPlayerIndex, 0.0f, 0.0f);
            }

            if (position.X < map.drawArea.Left + (GetBounds().Width/2)) position.X = map.drawArea.Left + (GetBounds().Width/2);
            if (position.X > map.drawArea.Right - (GetBounds().Width/2)) position.X = map.drawArea.Right - (GetBounds().Width/2);
            if (position.Y < map.drawArea.Top + (GetBounds().Height )) position.Y = map.drawArea.Top + (GetBounds().Height );
            if (position.Y > map.drawArea.Bottom - (GetBounds().Height )) position.Y = map.drawArea.Bottom - (GetBounds().Height );
            Decelerate();
        }
    }
}

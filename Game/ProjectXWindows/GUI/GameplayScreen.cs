using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectXWindows.Core;
using ProjectXWindows.Sprite_Elements;
using ProjectXWindows.Game_Elements;
using ProjectXWindows.Game_Elements.Enemies;


namespace ProjectXWindows
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields
        ContentManager content;
        Level level;
        int levelNumber;

        private int totalPlayers;
        private int[] playerLives;
        private int[] playerScores;
        private Weapon[][] playerWeapons;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(int _levelNumber, int _totalPlayers, int[] _playerLives, int[] _playerScores, Weapon[][] _playerWeapons)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
            levelNumber = _levelNumber;

            totalPlayers = _totalPlayers;
            playerLives = _playerLives;
            playerScores = _playerScores;
            playerWeapons = _playerWeapons;
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            level = new Level(content, new Rectangle(0, -16, 1280+64, 736), levelNumber);
            level.Prepare(totalPlayers, playerLives, playerScores, playerWeapons);
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            //if the pause button hasnt been pressed and the game is in progress
            if (IsActive && level.gameState == 0)
                level.Update();
            //level complete
            if (level.gameState == 1)
            {
                GameplayScreen nextLevel = new GameplayScreen(level.levelIndex + 1, level.totalPlayers, level.playerLives, level.playerScores, level.playerWeapons);
                LoadingScreen.Load(ScreenManager, true, ControllingPlayer, nextLevel);
            }
            //if the game has ended because the player(s) have died
            if(level.gameState==2)
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new GameOverScreen(level.players));
            //If the win condition is met
            if (level.gameState == 3)
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new BackgroundScreen(), new GameWinScreen(level.players));
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            PlayerIndex[] ControllingPlayers = new PlayerIndex[] { PlayerIndex.One, PlayerIndex.Two, PlayerIndex.Three, PlayerIndex.Four };

            // Look up inputs for the active player profile.
            //int playerIndex = (int)ControllingPlayer.Value;

            for (int playerIndex = 1; playerIndex <= totalPlayers; playerIndex++)
            {
                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
                GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

                // The game pauses either if the user presses the pause button, or if
                // they unplug the active gamepad. This requires us to keep track of
                // whether a gamepad was ever plugged in, because we don't want to pause
                // on PC if they are playing with a keyboard and have no gamepad at all!
                bool gamePadDisconnected = !gamePadState.IsConnected &&
                                           input.GamePadWasConnected[playerIndex];

                if (input.IsPauseGame(ControllingPlayers[playerIndex-1]) || gamePadDisconnected)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayers[playerIndex - 1]);
                }
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            if(level.gameState==0)
                level.Render(gameTime, spriteBatch);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }


        #endregion
    }
}

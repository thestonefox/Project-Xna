#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace ProjectXWindows
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("Main Menu")
        {
            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("1 Player Game");
            MenuEntry playGameMenuEntry2 = new MenuEntry("2 Player Game");
            MenuEntry exitMenuEntry = new MenuEntry("Quit To Dashboard");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            playGameMenuEntry2.Selected += PlayGameMenuEntrySelected2;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(playGameMenuEntry2);
            MenuEntries.Add(exitMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //set up defaults for level session information
            int totalPlayers = 1;
            RunGame(e, totalPlayers);
        }

        void PlayGameMenuEntrySelected2(object sender, PlayerIndexEventArgs e)
        {
            //set up defaults for level session information
            int totalPlayers = 2;
            RunGame(e, totalPlayers);
        }

        void RunGame(PlayerIndexEventArgs e, int totalPlayers)
        {
            int[] playerLives = new int[totalPlayers];
            for (int i = 0; i < totalPlayers; i++)
                playerLives[i] = -1;
            int[] playerScores = new int[totalPlayers];
            for (int i = 0; i < totalPlayers; i++)
                playerScores[i] = 0;
            Sprite_Elements.Weapon[][] playerWeapons = new Sprite_Elements.Weapon[totalPlayers][];

            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(3, totalPlayers, playerLives, playerScores, playerWeapons));
        }


        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are You Sure You Want To Quit?";
            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);
            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;
            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion
    }
}

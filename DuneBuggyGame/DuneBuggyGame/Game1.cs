using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DuneBuggyGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region Variables
        MenuOptions _OptionsMenu;
        Menu _GameMenu;
        Play _Play;
        Track1 _Track1;
        MenuCredits _CreditsMenu;
        Graphics _Graphics;
        SpriteBatch spriteBatch;
        GameTime gamet;
        
        /// <summary>
        /// Find with and height of game resolution to fit background
        /// </summary>
        int width, height;
        GameMode gameMode = GameMode.Menu;
        #endregion

        #region Constructor

        public Game1()
        {
            _Graphics = new Graphics(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            width = GraphicsDevice.DisplayMode.Width;
            height = GraphicsDevice.DisplayMode.Height;

            Sound.StartMusic();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _GameMenu = new Menu(this.Content);
            _Play = new Play(this.Content);
            _Track1 = new Track1(this.Content, _Graphics);
            _OptionsMenu = new MenuOptions(this.Content);
            _CreditsMenu = new MenuCredits(this.Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        #endregion

        #region Update
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gameMode == GameMode.Menu)
                gameMode = _GameMenu.Update(this);
            else if (gameMode == GameMode.Play)
            {
                Sound.StopMusic();
                gameMode = _Track1.Update(gameTime, this);
            }
            else if (gameMode == GameMode.Options)
                gameMode = _OptionsMenu.UpdateOptions(this);
            else if (gameMode == GameMode.Credits)
                gameMode = _CreditsMenu.UpdateCredits(this);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }//Update(GameTime gameTime)
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _Graphics.GDM.GraphicsDevice.Clear(Color.CornflowerBlue);
            gamet = gameTime;
            gameModeToDraw(gameMode);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// This is called to draw specific GameModes
        /// </summary>
        /// <param name="mode">GameMode Enumeration, provides the mode to display on the screen</param>
        private void gameModeToDraw(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Menu:
                    _GameMenu.Draw(spriteBatch, width, height);
                    break;
                case GameMode.Play:    
                    _Track1.Draw(gamet);
                    break;
                case GameMode.Options:
                    _OptionsMenu.DrawOptions(spriteBatch);
                    break;
                case GameMode.Credits:
                    _CreditsMenu.DrawCredits(spriteBatch);
                    break;
            }
        }
        #endregion
    }//Game1 : Microsoft.Xna.Framework.Game
}

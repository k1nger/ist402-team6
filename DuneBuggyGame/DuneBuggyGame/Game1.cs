using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        //Test for 3D model
        bool TEST = false;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Model myModel;
        // Set the position of the model in world space, and set the rotation.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 2500.0f);

        //Aspect Ratio to use for the projection matrix
        float aspectRatio = 640.0f / 480.0f;


        /// <summary>
        /// Find with and height of game resolution to fit background
        /// </summary>
        int width, height;

        //Menu Textures
        Texture2D mainMenuBG, menuHeader,
            menuCredits, menuExit, menuOptions,
            menuPlay, hudExample;

        GameMode gameMode = GameMode.Menu;
        MenuTab menuTab = MenuTab.Play;
        GamePadState gamePad;
        KeyboardState keyboard;
        bool remUpPressed = false,
             remDownPressed = false;
        #endregion

        #region Constructor

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            mainMenuBG = Content.Load<Texture2D>(@"Textures\dunebuggy");
            menuCredits = Content.Load<Texture2D>(@"Textures\MenuCredits");
            menuExit = Content.Load<Texture2D>(@"Textures\MenuExit");
            menuOptions = Content.Load<Texture2D>(@"Textures\MenuOptions");
            menuPlay = Content.Load<Texture2D>(@"Textures\MenuPlay");
            menuHeader = Content.Load<Texture2D>(@"Textures\MenuHeader");
            hudExample = Content.Load<Texture2D>(@"Textures\hudcopy");
            myModel = Content.Load<Model>(@"Models\Mars");

            aspectRatio = (float)graphics.GraphicsDevice.Viewport.Width /
    (float)graphics.GraphicsDevice.Viewport.Height;

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
            GetInputStates();

            if (gameMode == GameMode.Play && keyboard.IsKeyDown(Keys.Z))
                TEST = true;

            if (gameMode == GameMode.Play && keyboard.IsKeyDown(Keys.Right))
                modelRotation += 0.05f;

            if (gameMode != GameMode.Menu)
                Sound.StopMusic();

            if (gameMode == GameMode.Menu)
            {
                Sound.StartMusic();

                if ((menuTab == MenuTab.Play && keyboard.IsKeyDown(Keys.Down) && remDownPressed == false)
                    || (gamePad.DPad.Down == ButtonState.Pressed && remDownPressed == false))
                {
                    menuTab = MenuTab.Options;
                    Sound.Play(Sound.Sounds.MenuMove);
                }
                else if ((menuTab == MenuTab.Options && keyboard.IsKeyDown(Keys.Down) && remDownPressed == false)
                    || (gamePad.DPad.Down == ButtonState.Pressed && remDownPressed == false))
                {
                    menuTab = MenuTab.Credits;
                    Sound.Play(Sound.Sounds.MenuMove);
                }
                else if ((menuTab == MenuTab.Credits && keyboard.IsKeyDown(Keys.Down) && remDownPressed == false)
                    || (gamePad.DPad.Down == ButtonState.Pressed && remDownPressed == false))
                {
                    menuTab = MenuTab.Exit;
                    Sound.Play(Sound.Sounds.MenuMove);
                }
                else if ((menuTab == MenuTab.Exit && keyboard.IsKeyDown(Keys.Up) && remUpPressed == false)
                    || (gamePad.DPad.Up == ButtonState.Pressed && remUpPressed == false))
                {
                    menuTab = MenuTab.Credits;
                    Sound.Play(Sound.Sounds.MenuMove);
                }
                else if ((menuTab == MenuTab.Credits && keyboard.IsKeyDown(Keys.Up) && remUpPressed == false)
                    || (gamePad.DPad.Up == ButtonState.Pressed && remUpPressed == false))
                {
                    menuTab = MenuTab.Options;
                    Sound.Play(Sound.Sounds.MenuMove);
                }
                else if ((menuTab == MenuTab.Options && keyboard.IsKeyDown(Keys.Up) && remUpPressed == false)
                    || (gamePad.DPad.Up == ButtonState.Pressed && remUpPressed == false))
                {
                    menuTab = MenuTab.Play;
                    Sound.Play(Sound.Sounds.MenuMove);
                }

                if (menuTab == MenuTab.Exit && keyboard.IsKeyDown(Keys.Enter))
                {
                    Sound.Play(Sound.Sounds.MenuSelect);
                    this.Exit();
                }
                if (menuTab == MenuTab.Play && keyboard.IsKeyDown(Keys.Enter))
                {
                    Sound.Play(Sound.Sounds.MenuSelect);
                    gameMode = GameMode.Play;
                }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }//Update(GameTime gameTime)

        private void GetInputStates()
        {
            // Remember last keyboard and gamepad states for the menu
            remUpPressed =
                gamePad.DPad.Up == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y > 0.5f ||
                keyboard.IsKeyDown(Keys.Up);
            remDownPressed =
                gamePad.DPad.Down == ButtonState.Pressed ||
                gamePad.ThumbSticks.Left.Y < -0.5f ||
                keyboard.IsKeyDown(Keys.Down);

            // Get current gamepad and keyboard states
            gamePad = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();
        }//GetInputStates()
        #endregion

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            gameModeToDraw(gameMode);


            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        

        /// <summary>
        /// This is called to draw specific GameModes
        /// </summary>
        /// <param name="mode">GameMode Object, provides the mode to display on the screen</param>
        private void gameModeToDraw(GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Menu:
                    DrawMenu();
                    break;
                case GameMode.Play:
                    if (TEST == false)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(hudExample, new Rectangle(0, 0, 800, 600), Color.White);
                        spriteBatch.End();
                    }
                    else if (TEST == true)
                    {
                        // Copy any parent transforms.
                        Matrix[] transforms = new Matrix[myModel.Bones.Count];
                        myModel.CopyAbsoluteBoneTransformsTo(transforms);

                        // Draw the model. A model can have multiple meshes, so loop.
                        foreach (ModelMesh mesh in myModel.Meshes)
                        {
                            // This is where the mesh orientation is set, as well as our camera and projection.
                            foreach (BasicEffect effect in mesh.Effects)
                            {
                                effect.EnableDefaultLighting();
                                effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(modelRotation)
                                    * Matrix.CreateTranslation(modelPosition);
                                effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                                effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                                    aspectRatio, 1.0f, 10000.0f);
                            }
                            // Draw the mesh, using the effects set above.
                            mesh.Draw();
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws, and controls the visible aspects of the menu
        /// </summary>
        public void DrawMenu()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuBG, new Rectangle(-75, 0, width, height), Color.White);
            spriteBatch.Draw(menuHeader, new Rectangle(175, 0, 500, 250), Color.White);
            spriteBatch.End();

            if (menuTab == MenuTab.Play)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.Crimson);
                spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                spriteBatch.End();
            }
            else if (menuTab == MenuTab.Options)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.Crimson);
                spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                spriteBatch.End();
            }
            else if (menuTab == MenuTab.Credits)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.Crimson);
                spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                spriteBatch.End();
            }
            else if (menuTab == MenuTab.Exit)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.Crimson);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.Crimson);
                spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                spriteBatch.End();
            }
        }
        #endregion
    }//Game1 : Microsoft.Xna.Framework.Game
}

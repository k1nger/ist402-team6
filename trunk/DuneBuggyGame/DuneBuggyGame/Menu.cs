using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DuneBuggyGame
{
    class Menu
    {
        #region Variables
        //Menu Textures
        Texture2D mainMenuBG;
        Texture2D menuHeader;
        Texture2D menuCredits;
        Texture2D menuExit;
        Texture2D menuOptions;
        Texture2D menuPlay;

        private int _SelectedMenuItem;
        private const int _TotalMenuItems = 3;// This number is zero-based, so there are three menu items in total.

        GameInput _PreviousInputState;
        #endregion

        #region Constructor
        public Menu(ContentManager contentMngr)
        {
            mainMenuBG = contentMngr.Load<Texture2D>(@"Textures\Menu\dunebuggy");
            menuCredits = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuCredits");
            menuExit = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuExit");
            menuOptions = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuOptions");
            menuPlay = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuPlay");
            menuHeader = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuHeader");

            _PreviousInputState = GameInput.GetState();
        }//Menu
        #endregion

        #region Update
        public GameMode Update(Game1 game)
        {
            GameInput currentInputState = GameInput.GetState();

            // This should cause the selected menu item to "scroll" up or down
            // depending on what D-Pad button is pressed.
            if (currentInputState.DownButton == ButtonState.Pressed && _PreviousInputState.DownButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuMove);
                if (_SelectedMenuItem == _TotalMenuItems)
                    _SelectedMenuItem = 0;
                else
                    _SelectedMenuItem++;
            }
            else if (currentInputState.UpButton == ButtonState.Pressed && _PreviousInputState.UpButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuMove);
                if (_SelectedMenuItem == 0)
                    _SelectedMenuItem = _TotalMenuItems;
                else
                    _SelectedMenuItem--;
            }

            if (currentInputState.SelectButton == ButtonState.Pressed && _PreviousInputState.SelectButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuSelect);
                switch (_SelectedMenuItem)
                {
                    case 0://Play
                        return GameMode.Play;
                    case 1://Options
                        return GameMode.Options;
                    case 2://Credits
                        return GameMode.Credits;
                    case 3://Exit
                        game.Exit();
                        break;
                }
            }
        
            // Set the previous state to the current input state at
            // the end so that the next time this method is called,
            // the state can be compared
            _PreviousInputState = currentInputState;
            return GameMode.Menu;
        }//Update()
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch, int width, int height)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuBG, new Rectangle(-75, 0, width, height), Color.White);
            spriteBatch.Draw(menuHeader, new Rectangle(175, 0, 500, 250), Color.White);
            switch (_SelectedMenuItem)
            {
                case 0://Play
                    spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.Crimson);
                    spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                    spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                    spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                    break;
                case 1://Options
                    spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                    spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.Crimson);
                    spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                    spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                    break;
                case 2://Credits
                    spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                    spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                    spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.Crimson);
                    spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.White);
                    break;
                case 3://Exit
                    spriteBatch.Draw(menuPlay, new Rectangle(5, 200, 350, 85), Color.White);
                    spriteBatch.Draw(menuOptions, new Rectangle(10, 275, 350, 85), Color.White);
                    spriteBatch.Draw(menuCredits, new Rectangle(10, 350, 350, 85), Color.White);
                    spriteBatch.Draw(menuExit, new Rectangle(5, 425, 350, 85), Color.Crimson);
                    break;
            }
            spriteBatch.End();
        }//Draw()
        #endregion
    }
}

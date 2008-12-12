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
    class MenuOptions
    {
        Texture2D optionsSoundsOn, optionsSoundsOff, mainMenuBG, menuHeader;
        private int _SelectedOptionItem;
        private const int _TotalOptionItems = 1;//zero-based number.

        GameInput _PreviousInputState;

        public MenuOptions(ContentManager contentMngr)
        {
            mainMenuBG = contentMngr.Load<Texture2D>(@"Textures\Menu\dunebuggy");
            menuHeader = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuHeader");
            optionsSoundsOff = contentMngr.Load<Texture2D>(@"Textures\Menu\SoundsOff");
            optionsSoundsOn = contentMngr.Load<Texture2D>(@"Textures\Menu\SoundsOn");
            _PreviousInputState = GameInput.GetState();
        }
        
        public GameMode UpdateOptions(Game1 game)
        {
            GameInput currentInputState = GameInput.GetState();

            if (currentInputState.DownButton == ButtonState.Pressed && _PreviousInputState.DownButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuMove);
                if (_SelectedOptionItem == _TotalOptionItems)
                    _SelectedOptionItem = 0;
                else
                    _SelectedOptionItem++;
            }
            else if (currentInputState.UpButton == ButtonState.Pressed && _PreviousInputState.UpButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuMove);
                if (_SelectedOptionItem == 0)
                    _SelectedOptionItem = _TotalOptionItems;
                else
                    _SelectedOptionItem--;
            }
            if (currentInputState.SelectButton == ButtonState.Pressed && _PreviousInputState.SelectButton == ButtonState.Released)
            {
                Sound.Play(Sound.Sounds.MenuSelect);
                switch (_SelectedOptionItem)
                {
                    case 0://Off
                        Sound.StartMusic();
                        break;
                    case 1://On
                        Sound.StopMusic();
                        break;
                }
            }
            else if (currentInputState.ExitButton == ButtonState.Pressed && _PreviousInputState.ExitButton == ButtonState.Released)
                return GameMode.Menu;
            
            // Set the previous state to the current input state at
            // the end so that the next time this method is called,
            // the state can be compared
            _PreviousInputState = currentInputState;
            return GameMode.Options;
        }

        public void DrawOptions(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuBG, new Rectangle(-75, 0, 1024, 768), Color.White);
            spriteBatch.Draw(menuHeader, new Rectangle(175, 0, 500, 250), Color.White);
            switch (_SelectedOptionItem)
            {
                case 0:
                    spriteBatch.Draw(optionsSoundsOn, new Rectangle(10, 250, 350, 85), Color.Red);
                    spriteBatch.Draw(optionsSoundsOff, new Rectangle(10, 300, 350, 85), Color.White);                    
                    break;
                case 1:
                    spriteBatch.Draw(optionsSoundsOn, new Rectangle(10, 250, 350, 85), Color.White); 
                    spriteBatch.Draw(optionsSoundsOff, new Rectangle(10, 300, 350, 85), Color.Red);
                    break;
            }
            spriteBatch.End();
        }
    }
}

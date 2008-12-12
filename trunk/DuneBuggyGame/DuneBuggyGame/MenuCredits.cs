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
    class MenuCredits
    {
        SpriteFont BoldFont, RegularFont;
        Texture2D mainMenuBG, menuHeader;
        GameInput _PreviousInputState;
        

        public MenuCredits(ContentManager contentMngr)
        {
            mainMenuBG = contentMngr.Load<Texture2D>(@"Textures\Menu\dunebuggy");
            menuHeader = contentMngr.Load<Texture2D>(@"Textures\Menu\MenuHeader");
            BoldFont = contentMngr.Load<SpriteFont>("Bold Courier New");
            RegularFont = contentMngr.Load<SpriteFont>("Courier New");
            _PreviousInputState = GameInput.GetState();
        }

        public GameMode UpdateCredits(Game1 game)
        {
            GameInput currentInputState = GameInput.GetState();

            if (currentInputState.ExitButton == ButtonState.Pressed && _PreviousInputState.ExitButton == ButtonState.Released)
                return GameMode.Menu;
            return GameMode.Credits;
        }

        public void DrawCredits(SpriteBatch spriteBatch)
        {
            Vector2 FontOrigin = BoldFont.MeasureString("test") / 2;
            spriteBatch.Begin();
            spriteBatch.Draw(mainMenuBG, new Rectangle(-75, 0, 1024, 768), Color.White);
            spriteBatch.Draw(menuHeader, new Rectangle(175, 0, 500, 250), Color.White);
            spriteBatch.DrawString(BoldFont, "Team USA", new Vector2(50, 190), Color.Black,
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(BoldFont, "_______", new Vector2(40, 195), Color.Black,
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Ryan King", new Vector2(75, 250), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "John Haslam", new Vector2(75, 275), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Edward Morgan", new Vector2(75, 300), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Colin Snyder", new Vector2(75, 325), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(BoldFont, "Team Germany", new Vector2(300, 190), Color.Black,
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(BoldFont, "_________", new Vector2(300, 195), Color.Black,
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Christoph Mueller", new Vector2(325, 250), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Jan Stulier", new Vector2(325, 275), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Katharina Rosshirt", new Vector2(325, 300), Color.Black,
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(BoldFont, "Acknowledgements", new Vector2(112, 375), new Color(147, 64, 56),
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(BoldFont, "_____________", new Vector2(112, 380), new Color(147, 64, 56),
                0, FontOrigin, 0.5f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "Dan Herbert", new Vector2(206, 440), new Color(147, 64, 56),
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);
            spriteBatch.DrawString(RegularFont, "John Lee", new Vector2(206, 465), new Color(147, 64, 56),
                0, FontOrigin, 0.75f, SpriteEffects.None, 0);

            spriteBatch.End();
        }
    }
}

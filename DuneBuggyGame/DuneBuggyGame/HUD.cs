using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace DuneBuggyGame
{
    class HUD
    {
        #region Variables
        Texture2D lapTimes;
        Texture2D position;
        SpriteFont SpriteFont;
        Texture2D GaugeSprite;
        Texture2D MeterFillSprite;
        Texture2D NeedleSprite;

        //Gauge Constants.

        //These Origins tell SpriteBatch what is the center of rotation for the sprites.
        Vector2 GaugeOrigin = new Vector2(100, 100);
        Vector2 NeedleOrigin = new Vector2(2, 90);
        Vector2 MeterOrigin = new Vector2(100, 100);

        //These two Colors control the colors of the meters.
        Vector4 TopMeterColor = new Vector4(0, 255, 0, 255);
        Vector4 BottomMeterColor = new Vector4(255, 255, 0, 255);
        //How far the numbers are below the center of the gauge.
        float NumberOffset = 60;

        //Scale of the Gauge.
        float Scale = 1.0f;

        public Vector2 gaugePosition;

        //Values set externally based on object the gauge is monitoring
        int speed;
        float throttle;
        float afterburner;

        //This value is not set externally, but determined by the gauge based on the current speed and range of the gauge.
        float rotation;

        Effect FillEffect;

        GameInput _PreviousInputState;
        #endregion

        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="contentMngr">Content Manager that this game is using</param>
        public HUD(ContentManager contentMngr)
        {
            gaugePosition = new Vector2(700, 500);
            speed = 0;
            throttle = 1f;
            afterburner = 1f;

            //I made a SpriteFont from a texture using.  You could easily replace this with an XML-defined SpriteFont.
            SpriteFont = contentMngr.Load<SpriteFont>(@"Textures\HUD\dungeon28");

            //Load the Sprites.
            GaugeSprite = contentMngr.Load<Texture2D>(@"Textures\HUD\gauge");
            NeedleSprite = contentMngr.Load<Texture2D>(@"Textures\HUD\needle");
            MeterFillSprite = contentMngr.Load<Texture2D>(@"Textures\HUD\meterfill");
            lapTimes = contentMngr.Load<Texture2D>(@"Textures\HUD\laptimes");
            position = contentMngr.Load<Texture2D>(@"Textures\HUD\position");


            //Load the Effect used to render the Meters on top and bottom.
            FillEffect = contentMngr.Load<Effect>(@"Textures\HUD\MeterEffect");

            _PreviousInputState = GameInput.GetState();
        }

        #endregion

        #region Update
        /// <summary>
        /// Updates the HUD
        /// </summary>
        /// <param name="game">Game Object that this game is using</param>
        public void Update(Game1 game)
        {
            GameInput currentInputState = GameInput.GetState();

            if (currentInputState.UpButton == ButtonState.Pressed)
            {
                if (speed <= 300)
                    speed += 5;
                else if (speed > 300)
                    speed += 1;
            }
            else
                speed -= 1;

            speed = (int)(MathHelper.Clamp((float)(speed), 0f, 500));
            rotation = MathHelper.ToRadians(270) / 500 * speed - MathHelper.ToRadians(135);
        }
        #endregion 

        #region Draw
        /// <summary>
        /// Draws the HUD Textures
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawGauge(spriteBatch);
            DrawText(spriteBatch);

            //The meters are drawn separately because they have different values.
            DrawMeterTop(spriteBatch);
            DrawMeterBottom(spriteBatch);

            DrawNeedle(spriteBatch);

            DrawPosition(spriteBatch);
            DrawLapTimes(spriteBatch);
            
        }

        /// <summary>
        /// Draws the Lap texture on the screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawLapTimes(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(lapTimes, new Rectangle(535, 20, 232, 158), Color.CornflowerBlue);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the Position texture onto the screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawPosition(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(position, new Rectangle(25, 20, 207, 96), Color.CornflowerBlue);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the gauge onto the screen
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawGauge(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Draw(GaugeSprite, gaugePosition,
                null, Color.White, 0f, new Vector2(100, 100), Scale, SpriteEffects.None, 1f);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws and rotates needle
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawNeedle(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Draw(NeedleSprite, gaugePosition,
                null, Color.White, rotation, NeedleOrigin, Scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        //The meters are drawn separately because they have different values.
        /// <summary>
        /// Draws the top meter to control an aspect like boost or nitrous
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawMeterTop(SpriteBatch spriteBatch)
        {
            FillEffect.Parameters["meterValue"].SetValue(throttle);
            FillEffect.Parameters["Color"].SetValue(TopMeterColor);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            FillEffect.Begin();
            FillEffect.CurrentTechnique.Passes[0].Begin();
            spriteBatch.Draw(MeterFillSprite, gaugePosition, null, Color.White, 0, MeterOrigin, 1f, SpriteEffects.None, 1f);
            FillEffect.CurrentTechnique.Passes[0].End();
            FillEffect.End();
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the bottom meter to control an aspect like boost or nitrous
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawMeterBottom(SpriteBatch spriteBatch)
        {
            FillEffect.Parameters["meterValue"].SetValue(afterburner);
            FillEffect.Parameters["Color"].SetValue(BottomMeterColor);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            FillEffect.Begin();
            FillEffect.CurrentTechnique.Passes[0].Begin();
            spriteBatch.Draw(MeterFillSprite, gaugePosition, null, Color.White, MathHelper.Pi, MeterOrigin, 1f, SpriteEffects.FlipHorizontally, 1);
            FillEffect.CurrentTechnique.Passes[0].End();
            FillEffect.End();
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the text of speed, lap number, and position
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to begin and end drawing</param>
        private void DrawText(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred,
                SaveStateMode.SaveState);
            string text = "" + speed;
            //Draw String twice for shadow
            spriteBatch.DrawString(SpriteFont, text, new Vector2(gaugePosition.X + 2, gaugePosition.Y + NumberOffset + 2),
                            Color.Black, 0f,
                            new Vector2(SpriteFont.MeasureString(text).X / 2, SpriteFont.MeasureString(text).Y / 2),
                            Scale, SpriteEffects.None, 0f);
            spriteBatch.DrawString(SpriteFont, text, new Vector2(gaugePosition.X, gaugePosition.Y + NumberOffset),
                            Color.White, 0f,
                            new Vector2(SpriteFont.MeasureString(text).X / 2, SpriteFont.MeasureString(text).Y / 2),
                            Scale, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        #endregion
    }
}

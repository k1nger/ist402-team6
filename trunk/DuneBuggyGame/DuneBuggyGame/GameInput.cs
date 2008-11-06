using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DuneBuggyGame
{
    class GameInput
    {
        bool remUpPressed, remDownPressed;

        public static GameInput GetState(PlayerIndex playerIndex)
        {
            GameInput i = new GameInput();
            i._PlayerIndex = playerIndex;
            i._GamePadState = GamePad.GetState(playerIndex);
            i._KeyboardState = Keyboard.GetState(playerIndex);
            return i;
        }

        public static GameInput GetState()
        {
            return GetState(PlayerIndex.One);
        }

        public void ResetState()
        {
            _KeyboardState = new KeyboardState();
            _GamePadState = new GamePadState();
        }

        private KeyboardState _KeyboardState;
        private GamePadState _GamePadState;
        private PlayerIndex _PlayerIndex = PlayerIndex.One;

        public void GetInputStates()
        {
            // Remember last keyboard and gamepad states for the menu
            remUpPressed =
                 _GamePadState.DPad.Up == ButtonState.Pressed ||
                 _GamePadState.ThumbSticks.Left.Y > 0.5f ||
                 _KeyboardState.IsKeyDown(Keys.Up);
            remDownPressed =
                _GamePadState.DPad.Down == ButtonState.Pressed ||
                _GamePadState.ThumbSticks.Left.Y < -0.5f ||
                _KeyboardState.IsKeyDown(Keys.Down);

            // Get current gamepad and keyboard states
            GameInput.GetState();
        }//GetInputStates()

        public ButtonState UpButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.W) || _KeyboardState.IsKeyDown(Keys.Up) || _GamePadState.DPad.Up == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState DownButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.S) || _KeyboardState.IsKeyDown(Keys.Down) || _GamePadState.DPad.Down == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState LeftButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.A) || _KeyboardState.IsKeyDown(Keys.Left) || _GamePadState.DPad.Left == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState RightButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.D) || _KeyboardState.IsKeyDown(Keys.Right) || _GamePadState.DPad.Right == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState StartButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.Enter) || _GamePadState.Buttons.Start == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState SelectButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.Enter) || _GamePadState.Buttons.Start == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState ExitButton
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.Escape) || _GamePadState.Buttons.Back == ButtonState.Pressed)
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public ButtonState Test
        {
            get
            {
                if (_KeyboardState.IsKeyDown(Keys.Z))
                    return ButtonState.Pressed;
                return ButtonState.Released;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is GameInput)
            {
                GameInput o = obj as GameInput;
                return this._PlayerIndex == o._PlayerIndex
                        && this.DownButton == o.DownButton
                        && this.ExitButton == o.ExitButton
                        && this.LeftButton == o.LeftButton
                        && this.RightButton == o.RightButton
                        && this.StartButton == o.StartButton
                        && this.UpButton == o.UpButton;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

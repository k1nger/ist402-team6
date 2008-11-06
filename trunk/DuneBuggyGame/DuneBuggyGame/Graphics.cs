using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuneBuggyGame
{
    class Graphics
    {
        GraphicsDeviceManager graphics;

        public Graphics(Game1 game)
        {
            graphics = new GraphicsDeviceManager(game);
        }

        public GraphicsDeviceManager GDM
        {
            get
            {
                return graphics;
            }
        }
    }
}

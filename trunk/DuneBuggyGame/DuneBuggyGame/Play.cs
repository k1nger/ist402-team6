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
    class Play
    {
        #region Variables
        bool TEST = false;
        GraphicsDeviceManager GDM;
        GameInput _PreviousInputState;
        Graphics _graphics;

        Texture2D hudExample;

        Model myModel;
        // Set the position of the model in world space, and set the rotation.
        Vector3 modelPosition = Vector3.Zero;
        float modelRotation = 0.0f;

        // Set the position of the camera in world space, for our view matrix.
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 2500.0f);

        //Aspect Ratio to use for the projection matrix
        float aspectRatio = 640.0f / 480.0f;
        #endregion

        #region Constructor
        public void Graphics()
        {
            GDM = _graphics.GDM;
            aspectRatio = (float)GDM.GraphicsDevice.Viewport.Width /
    (float)GDM.GraphicsDevice.Viewport.Height;
        }

        public Play(ContentManager contentMngr)
        {
            hudExample = contentMngr.Load<Texture2D>(@"Textures\hud copy");

            myModel = contentMngr.Load<Model>(@"Models\Mars");

            _PreviousInputState = GameInput.GetState();
        }
        #endregion

        #region Update
        public GameMode Update(Game1 game)
        {
            GameInput currentInputState = GameInput.GetState();
            if (currentInputState.Test == ButtonState.Pressed)
                TEST = true;

            if (currentInputState.RightButton == ButtonState.Pressed)
                modelRotation += 0.05f;
            
            _PreviousInputState = currentInputState;
            return GameMode.Play;
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (TEST == false)
                spriteBatch.Draw(hudExample, new Rectangle(0, 0, 800, 600), Color.White);
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
            spriteBatch.End();
        }
        #endregion

    }
}

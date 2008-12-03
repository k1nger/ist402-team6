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
    class Track1
    {
        GraphicsDeviceManager graphics;
        GraphicsDevice device;
        SpriteBatch spriteBatch;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        Vector3 campos;
        HUD hud;

        Model buggyModel;
        Model landscape;

        Vector3 buggyPosition = new Vector3(8, 1, -3);
        Quaternion buggyRotation = Quaternion.Identity;
        Quaternion cameraRotation = Quaternion.Identity;

        float gameSpeed = 1.0f;

        float curAcc = 0.0f;
        float buggySpeed = 0.0f;
        const float MaxAccSpeed = 2;
        const float MaxAccBreak = -0.5f;
        const float MaxSpeed = 20;

        SpriteFont textFont;

        public Track1()
        {

        }

        public Track1(ContentManager contentMgr, Graphics graphic)
        {
            graphics = graphic.GDM;
            device = graphics.GraphicsDevice;
            spriteBatch = new SpriteBatch(device);

            hud = new HUD(contentMgr);
            landscape = contentMgr.Load<Model>("Models\\plane");
            buggyModel = contentMgr.Load<Model>("Models\\buggy");
            textFont = contentMgr.Load<SpriteFont>("Courier New");
            curAcc = 0;
            SetUpCamera();
        }

      


        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(20, 13, -5), new Vector3(8, 0, -7), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.2f, 500.0f);
        }

        public GameMode Update(GameTime gameTime, Game1 game)
        {
            ProcessKeyboard(gameTime);
            float moveSpeed = gameTime.ElapsedGameTime.Milliseconds / 500.0f * gameSpeed;
            moveSpeed *= (MaxSpeed * curAcc);
            MoveForward(ref buggyPosition, buggyRotation, moveSpeed);
            hud.Update(game);

            buggySpeed = moveSpeed;

            UpdateCamera();
            
            return GameMode.Play;
        }

        private void UpdateCamera()
        {

            cameraRotation = Quaternion.Lerp(cameraRotation, buggyRotation, 0.1f);

            campos = new Vector3(0f, 0.5f, 3.0f);
            campos = Vector3.Transform(campos, Matrix.CreateFromQuaternion(cameraRotation));
            campos += buggyPosition;

            Vector3 camup = new Vector3(0, 1, 0);
            camup = Vector3.Transform(camup, Matrix.CreateFromQuaternion(cameraRotation));

            viewMatrix = Matrix.CreateLookAt(campos, buggyPosition, camup);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.1f, 500.0f);
        }

        private void ProcessKeyboard(GameTime gameTime)
        {
            float leftRightRot = 0;

            float turningSpeed = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            turningSpeed *= 1.6f * gameSpeed;
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Right))
                leftRightRot -= turningSpeed;
            if (keys.IsKeyDown(Keys.Left))
                leftRightRot += turningSpeed;

            float upDownRot = 0;

            if (keys.IsKeyDown(Keys.W))
                upDownRot += turningSpeed;
            else if (keys.IsKeyDown(Keys.S))
                upDownRot -= turningSpeed;

            if (keys.IsKeyDown(Keys.Up))
            {
                if (curAcc < MaxAccSpeed)
                    curAcc += 0.1f;
            }
            else if (keys.IsKeyDown(Keys.Down))
            {
                if (curAcc > MaxAccBreak)
                    curAcc -= 0.1f;
            }
            else
            {
                if (curAcc > 0)
                    curAcc -= 0.1f;
                if (curAcc < 0)
                    curAcc += 0.1f;
                if ((curAcc > -0.1f) && (curAcc < 0.1f))
                    curAcc = 0;
            }              
                

            Quaternion additionalRot = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), leftRightRot) * Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), upDownRot);
            buggyRotation *= additionalRot;

        }

        private void MoveForward(ref Vector3 position, Quaternion rotationQuat, float speed)
        {
            Vector3 addVector = Vector3.Transform(new Vector3(0, 0, -1), rotationQuat);
            position += addVector * speed;
        }


        public void Draw(GameTime gameTime)
        {
            device.Clear(Color.CornflowerBlue);
            device.RenderState.CullMode = CullMode.None;           

            DrawLandscape(landscape,new Vector3(0,0,0));
            DrawModel(buggyModel);

            Vector2 FontOrigin = textFont.MeasureString("test") / 2;
            spriteBatch.Begin();
            spriteBatch.DrawString(
                textFont,
                "CameraPos: " + campos.X + " " + campos.Y + " " + campos.Z,
                new Vector2(20, 20), Color.LightGreen, 0, FontOrigin, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(
                textFont,
                "BuggyPos: " + buggyPosition.X + " " + buggyPosition.Y + " " + buggyPosition.Z,
                new Vector2(20, 40), Color.LightGreen, 0, FontOrigin, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(
                textFont,
                "BuggyAcc: " + curAcc,
                new Vector2(20, 60), Color.LightGreen, 0, FontOrigin, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(
                textFont,
                "BuggySpeed: " + buggySpeed,
                new Vector2(20, 80), Color.LightGreen, 0, FontOrigin, 0.5f, SpriteEffects.None, 0.5f);
            spriteBatch.End();
            hud.Draw(spriteBatch);
            
        }

        private void DrawModel(Model model)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    
                    effect.World = Matrix.CreateScale(0.005f, 0.005f, 0.005f) *
                                   Matrix.CreateRotationX(-MathHelper.Pi / 2) *
                                   Matrix.CreateFromQuaternion(buggyRotation) *
                                   Matrix.CreateTranslation(buggyPosition);


                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
                mesh.Draw();
            }
        }

        void DrawLandscape(Model model, Vector3 position)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Matrix.CreateTranslation(position) * 
                                   Matrix.CreateScale(new Vector3(50, 50, 50)) *
                                   Matrix.CreateRotationX(MathHelper.Pi / 2);
                    effect.Projection = projectionMatrix;
                    effect.View = viewMatrix;
                }
                mesh.Draw();
            }
        }
    }
}

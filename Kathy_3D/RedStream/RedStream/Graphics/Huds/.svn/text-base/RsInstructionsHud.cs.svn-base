using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RedStream
{
    public class RsInstructionsHud : RsIHud
    {
        private SpriteFont font;
        public Texture2D BackgroundTexture;
        public SmoothFloatComponent SmoothAlpha;

        private Texture2D mainMenuTexture;
        private Texture2D blackBackgroundTexture;
        private Rectangle viewportRect;

        public RsInstructionsHud()
        {
            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.01f, 0);
            BackgroundTexture = RedStream.Content.GetTexture("gyroMenu");
            font = RedStream.Content.GetFont("Courier New");
            //main menu background
            blackBackgroundTexture = RedStream.Content.GetTexture("blackBackground");
            //set a viewport variable
            viewportRect = new Rectangle(0, 0,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 curPos;
            RedStream.Graphics.Clear(Color.Black);
            RedStream.Graphics.DrawFullscreenQuad(BackgroundTexture, new Vector4(1, 1, 1, .10f));
            batch.Begin();
#if !XBOX
            curPos = new Vector2(50, 125);
            batch.DrawString(font, "Instructions:", curPos, Color.Red); curPos.Y += 25;
            batch.DrawString(font, "Your planet is under attack!\nBuild towers on your defensive gyroscope platform\nSelect ring with Up/Down keys\nRotate ring with Left/Right keys\nSpin rings with A/D keys\nChange Master Ring with Space (Also resets orientation)", curPos, Color.Red); curPos.Y += 165;
            batch.DrawString(font, "Your planet's sheilds and health are at the top of your screen\nYour shields will regenerate over time if they aren't hit\nIf health becomes empty, you die a horrible fiery death.\nDon't do that, Survive!", curPos, Color.Red); curPos.Y += 25;
            batch.DrawString(font, "Press Enter to return!", new Vector2(250,30), Color.Red);
#else
            RsSprites.SpriteScale = 1f;
            curPos = new Vector2(125, 25);
            batch.DrawString(font, "Instructions:", curPos, Color.Red); curPos.Y += 25;
            batch.DrawString(font, "Your planet is under attack!\nBuild towers on your defensive gyroscope platform", curPos, Color.Red); curPos.Y += 70;
            batch.DrawString(font, "Select ring with ", curPos, Color.Red);
            RsSprites.drawLeftTrigger(new Vector2(curPos.X + 170, curPos.Y - 15), batch); 
            RsSprites.drawRightTrigger(new Vector2(curPos.X + 170 + 78, curPos.Y - 15), batch);
            curPos.Y += 65;
            
            batch.DrawString(font, "Rotate ring with       to aim", curPos, Color.Red);
            RsSprites.drawLeftThumbStick(new Vector2(curPos.X + 185, curPos.Y - 15), batch);
            curPos.Y += 55;
            batch.DrawString(font, "Rotate camera with         Zoom with", curPos, Color.Red);
            RsSprites.drawRightThumbStick(new Vector2(curPos.X + 200, curPos.Y - 15), batch);
            RsSprites.drawLeftBumper(new Vector2(curPos.X + 405, curPos.Y - 5), batch);
            RsSprites.drawRightBumper(new Vector2(curPos.X + 405 + 75, curPos.Y - 5), batch);
            curPos.Y += 55;
            batch.DrawString(font, "Your planet's sheilds are at the top of your screen\nIf it becomes empty, you die a horrible fiery death.\nDon't do that, Survive!", curPos, Color.Red); curPos.Y += 85;
            batch.DrawString(font, "Press       to return!", new Vector2(250,curPos.Y), Color.Red);
            RsSprites.drawButtonA(new Vector2(250 + 65, curPos.Y - 10), batch);
#endif
            batch.End();
        }
        public void ProcessInput()
        {
#if XBOX
            
            if (RsInput.Tapped(Buttons.Start) || RsInput.Tapped(Buttons.A))
                RsStateManager.Pop();
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
#else

            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Pop();
            if (RsInput.Down(RsInput.KeyBindings.ExitKey))
                RedStream.Game.Exit();   
#endif
        }
    }
}

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
    public class RsMainMenuState : RsState
    {
        //RsIHud Hud = new RsMainMenuHud();
        private SpriteFont font, largeFont;

        private Texture2D mainMenuTexture;
        private Texture2D titleTexture;
        private Texture2D mainSelectionTexture;
        private Rectangle viewportRect;
        
        public override void Draw()
        {
            //RedStream.Graphics.Render();
            SpriteBatch batch = RedStream.Graphics.batch;
            Vector2 curPos = new Vector2();
            batch.Begin();
            batch.DrawString(font, "FPS: " + RedStream.Game.frameRate, curPos, Color.Red); curPos.Y += 15;
            batch.Draw(mainMenuTexture, viewportRect, Color.White);
            batch.Draw(titleTexture, new Vector2(175, 10), Color.White);
#if !XBOX
            batch.Draw(mainSelectionTexture, new Vector2(15, 300), Color.White);
#else
            curPos.X = 10;
            curPos.Y = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height / 2 + 50;

            RsSprites.SpriteScale = 1f;
            batch.DrawString(largeFont, "Press    to play!", curPos, Color.OrangeRed);
            RsSprites.drawButtonStart(new Vector2(curPos.X + 78, curPos.Y - 0), batch);
            curPos.Y += 45;
            batch.DrawString(largeFont, "Press    for instructions", curPos, Color.OrangeRed);
            RsSprites.drawButtonY(new Vector2(curPos.X + 88, curPos.Y - 5), batch);
            curPos.Y += 45;
            batch.DrawString(largeFont, "Press    for options", curPos, Color.OrangeRed);
            RsSprites.drawButtonX(new Vector2(curPos.X + 88, curPos.Y - 0), batch);
            curPos.Y += 45;
            batch.DrawString(largeFont, "Press    to leave :(", curPos, Color.OrangeRed);
            RsSprites.drawButtonBack(new Vector2(curPos.X + 77, curPos.Y - 0), batch);

#endif
            batch.End();
            //Hud.Draw(RedStream.Graphics.batch);
        }
        public override void Input(GameTime time)
        {
#if XBOX
            if (RsInput.Tapped(Buttons.Start))
                RsStateManager.Push(new RsIntroState());
            if (RsInput.Tapped(Buttons.Y))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
            if (RsInput.Tapped(Buttons.X))
                RsStateManager.Push(new RsOptionsState());
#else
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsIntroState());
            if (RsInput.Tapped(Keys.Space))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(RsInput.KeyBindings.ExitKey))
                RedStream.Game.Exit();
            if (RsInput.Down(Keys.O))
                RsStateManager.Push(new RsOptionsState());
#endif
            base.Input(time);
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
        }
        public override void EnterState()
        {
            RsGameInfo.LoadContent();
            font = RedStream.Content.GetFont("Courier New");
            largeFont = RedStream.Content.GetFont("Courier New Large");

            //main menu background
            mainMenuTexture = RedStream.Content.GetTexture("GyroMenu");

            //Title
            titleTexture = RedStream.Content.GetTexture("Title");

            // obv
            mainSelectionTexture = RedStream.Content.GetTexture("MainSelection");

            //set a viewport variable
            viewportRect = new Rectangle(0, 0,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
            base.EnterState();
        }
    }
}

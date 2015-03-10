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
    public class RsGameplayHud : RsIHud
    {
        private SpriteFont font;
        private Texture2D gameplayButtonsTexture;
        private Texture2D healthBarTexture;
        private Texture2D innerHealthBarTexture;
        private Rectangle viewportRect;

        public RsGameplayHud()
        {
            font = RedStream.Content.GetFont("Courier New");
            gameplayButtonsTexture = RedStream.Content.GetTexture("MenuFrameSample");
            healthBarTexture = RedStream.Content.GetTexture("MHealthBar");
            innerHealthBarTexture = RedStream.Content.GetTexture("MHealthBar");
            
            //set a viewport variable
            viewportRect = new Rectangle(0, 0,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
        
        }
        public void Draw(SpriteBatch batch)
        {
            Vector2 curPos = new Vector2();
            batch.Begin();
            batch.DrawString(font, "FPS: " + RedStream.Game.frameRate, curPos, Color.Red); curPos.Y += 35;
            batch.DrawString(font, "Wave: " + RsGameInfo.iWave, curPos, Color.Red); curPos.Y += 15; //**Must reflect actual wave -DONE
            batch.DrawString(font, "Enemies remaining: " + RsGameInfo.ShipsLeft, curPos, Color.Red); curPos.Y += 15; //**Must reflect actual enemies -DONE
            batch.DrawString(font, "Money: " + RsGameInfo.Money, curPos, Color.Red); curPos.Y += 15;
            if (RsGameInfo.Gyroscope != null)
            {
                batch.DrawString(font, "Selected Gyro: " + RsGameInfo.Gyroscope.GetSelectedRingName(), curPos, Color.Gold); curPos.Y += 15;
                batch.DrawString(font, "Master Ring: " + RsGameInfo.Gyroscope.GetModeName(), curPos, Color.Gold); curPos.Y += 15;
            }
#if !XBOX
            batch.DrawString(font, "Press P to pause\nPress H for help", new Vector2(2, viewportRect.Height-70), Color.Red); curPos.Y += 15;
#else
            RsSprites.SpriteScale = 0.5f;
            batch.DrawString(font, "Press   to pause", new Vector2(2, viewportRect.Height - 65), Color.Red);
            RsSprites.drawButtonStart(new Vector2(57, viewportRect.Height - 60), batch);
            curPos.Y += 15;
            batch.DrawString(font, "\nPress   for help", new Vector2(2, viewportRect.Height - 65), Color.Red);
            RsSprites.drawButtonY(new Vector2(61, viewportRect.Height - 40), batch);
            curPos.Y += 15;
#endif
            //batch.Draw(gameplayButtonsTexture, new Vector2(viewportRect.Width-gameplayButtonsTexture.Width, viewportRect.Height-gameplayButtonsTexture.Height), Color.White);
            batch.End();
        }
        public void ProcessInput()
        {
            RedStream.Input.ProcessInput();
        }
    }
}

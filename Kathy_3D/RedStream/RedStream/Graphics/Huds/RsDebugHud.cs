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
    public class RsDebugHud : RsIHud 
    {
        private SpriteFont font;
        public RsDebugHud()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        public void Draw(SpriteBatch batch)
        {
            Vector2 curPos = new Vector2();
            batch.Begin();
            batch.DrawString(font, "FPS: " + RedStream.Game.frameRate, curPos, Color.Red); curPos.Y += 15;
            batch.DrawString(font, "Objects Drawn: " + RedStream.Graphics.ObjectsDrawn, curPos, Color.Red); curPos.Y += 15;
            batch.DrawString(font, "Resources: " + RedStream.Content.GetNumResources(), curPos, Color.Red); curPos.Y += 15;
            batch.DrawString(font, "Components: " + RedStream.Game.Components.Count, curPos, Color.Red); curPos.Y += 15;
            batch.DrawString(font, "Controls:", curPos, Color.White); curPos.Y += 15;
            batch.DrawString(font, "`/1: Toggle Wireframe mode", curPos, Color.White); curPos.Y += 15;
            batch.DrawString(font, "2-9: Switch rendering Targets", curPos, Color.White); curPos.Y += 15;
            batch.DrawString(font, "Toggle the Master Gyro: Space", curPos, Color.White); curPos.Y += 15;
            batch.DrawString(font, "Left/Right: Rotate Selected Gyro", curPos, Color.White); curPos.Y += 15;
            batch.DrawString(font, "Up/Down: Change Selected Gyro", curPos, Color.White); curPos.Y += 15;
            if (RsGameInfo.Gyroscope != null)
            {
                batch.DrawString(font, "Selected Gyro: " + RsGameInfo.Gyroscope.GetSelectedRingName(), curPos, Color.Gold); curPos.Y += 15;
                batch.DrawString(font, "Gyroscope Master Ring: " + RsGameInfo.Gyroscope.GetModeName(), curPos, Color.Gold); curPos.Y += 15;
                //batch.Draw(RedStream.Content.GetTexture("pixel"), RsState.Project(RedStream.Scene.Gyroscope.Pos), Color.White);
            }
            batch.End();
        }
        public void ProcessInput()
        {
            RedStream.Input.ProcessInput();
        }
    }
}

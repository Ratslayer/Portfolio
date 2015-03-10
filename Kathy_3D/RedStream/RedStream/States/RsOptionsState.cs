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
    public class RsOptionsState : RsMainRenderState
    {
        public RsOptionsState()
            : base()
        {
            font = RedStream.Content.GetFont("Courier New");
        }
        private SpriteFont font;
        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = RedStream.Graphics.batch;
            batch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            Vector2 pos = new Vector2(20);
            batch.DrawString(font, "Music Volume: " + (int)(RsGameInfo.MusicVolume * 100), pos, Color.White);

            pos.Y = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height - 40;
#if !XBOX
            batch.DrawString(font, "Press enter to return", pos, Color.White);
#else
            batch.DrawString(font, "Press   to return", pos, Color.White);
            RsSprites.SpriteScale = 0.5f;
            RsSprites.drawButtonB(new Vector2(pos.X + 60, pos.Y), batch);
#endif
            batch.End();
        }
        public override void Input(GameTime time)
        {
#if XBOX
            if (RsInput.Tapped(Buttons.Start) || RsInput.Tapped(Buttons.B) || RsInput.Tapped(Buttons.X))
                RsStateManager.Pop();

            if (RsInput.Down(Buttons.LeftThumbstickLeft) || RsInput.Tapped(Buttons.DPadLeft))
                RsGameInfo.MusicVolume -= 0.01f;
            if (RsInput.Down(Buttons.LeftThumbstickRight) || RsInput.Tapped(Buttons.DPadRight))
                RsGameInfo.MusicVolume += 0.01f;
#else
            if (RsInput.Down(Keys.Enter))
                RsStateManager.Pop();

            if (RsInput.Down(Keys.Left))
                RsGameInfo.MusicVolume-=0.01f;
            if (RsInput.Down(Keys.Right))
                RsGameInfo.MusicVolume+=0.01f;
#endif
            base.Input(time);
        }
        public override void Update(GameTime time)
        {
            RsGameInfo.MusicVolume = MathHelper.Clamp(RsGameInfo.MusicVolume, 0, 1);
            MediaPlayer.Volume = RsGameInfo.MusicVolume;

            base.Update(time);
        }
        public override void EnterState()
        {
            base.EnterState();
        }
    }
}

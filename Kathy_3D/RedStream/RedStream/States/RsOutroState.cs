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
    public class RsOutroState : RsState
    {
        public RsOutroState()
        {
            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.01f, 0);
            BackgroundTexture = RedStream.Content.GetTexture("gyroMenu");
            Font = RedStream.Content.GetFont("Courier New");
            BigFont = RedStream.Content.GetFont("Pericles");
            TopLimit = 100;
            BottomLimit = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height;
        }
        public Texture2D BackgroundTexture;
        public SpriteFont Font, BigFont;
        public SmoothFloatComponent SmoothAlpha;
        public bool ApplyPP = false;
        public Vector2 TextPos, CurTextPos;
        public float TextScrollSpeed, DefaultTextScrollingSpeed=20, TopLimit, BottomLimit;
        public override void EnterState()
        {
            TextPos = new Vector2(30, BottomLimit);
            CurTextPos = TextPos;
            ApplyPP = false;
            base.EnterState();
        }
        public override void Draw()
        {
            CurTextPos = TextPos;
            SpriteBatch batch = RedStream.Graphics.batch;
            RedStream.Graphics.Clear(Color.Black);
            if(ApplyPP)
                RedStream.Graphics.BeginPostProcess();
            RedStream.Graphics.DrawFullscreenQuad(BackgroundTexture, new Vector4(1, 1, 1, SmoothAlpha.F));
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null);
            batch.DrawString(BigFont, "Congratulations!", new Vector2(10, 20), Color.White);
            DrawText("With the first waves of the Agnotaritans attack on Fraxus swatted,");
            DrawText("the remnants of humanity are given a little breathing room.  ");
            DrawText("");
            DrawText("Make no mistake, the Agnotaritans will be back and stronger than ");
            DrawText("ever, but the people of Fraxus will attempt to do the one thing ");
            DrawText("they've been doing since this all started:");
            DrawText("");
            DrawText("                           Survive.");
            DrawText("");
            DrawText("");
            DrawText("And that's exactly what they've fought for and earned today,");
            DrawText("one more day alive.");
            DrawText("");
            DrawText("");
            DrawText("TO BE CONTINUED...");
            batch.End();
            if(ApplyPP)
                RedStream.Graphics.EndPostProcess();
        }
        public void DrawText(string text)
        {
            CurTextPos.Y += 30;
            float textCoords = MathHelper.Clamp(CurTextPos.Y - TopLimit, 0, BottomLimit - TopLimit) / (BottomLimit - TopLimit);
            RedStream.Graphics.batch.DrawString(Font, text, CurTextPos, new Color(new Vector4(1, 1, 1, (float)Math.Sin(Math.PI*textCoords))));
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
            TextPos.Y -= TextScrollSpeed * (float)time.ElapsedGameTime.TotalSeconds;
            if(CurTextPos.Y < TopLimit)
                RsStateManager.Push(new RsMainMenuState());

        }
        public override void Input(Microsoft.Xna.Framework.GameTime time)
        {
#if XBOX

            if (RsInput.Down(Buttons.A))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Buttons.LeftShoulder))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
            if (RsInput.Tapped(Buttons.Start))
                RsStateManager.Push(new RsMainMenuState());
#else

            if (RsInput.Down(Keys.Space))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Keys.LeftShift))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsMainMenuState());
#endif
            base.Input(time);
        }
    }
}

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
    public class RsIntroState : RsState
    {
        public RsIntroState()
        {
            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.01f, 0);
            BackgroundTexture = RedStream.Content.GetTexture("gyroMenu");
            Font = RedStream.Content.GetFont("Courier New");
            BigFont = RedStream.Content.GetFont("PericlesSmaller");
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
            batch.DrawString(BigFont, "Gyros: Defense Evolved", new Vector2(20, 20), Color.White);
            DrawText("When an alien race, the Agnotaritans, pushes humanity to the brink ");
            DrawText("of extinction, one lone planet survives. Equiped with high tech ");
            DrawText("customizable Gyroscope ring and home to some of the best scientific");
            DrawText("and technical minds in the galaxy, the planet Fraxus is all that ");
            DrawText("stands between humanity and their extinction.");
            DrawText("");
            DrawText("");
            DrawText("Earth and other colonies we not prepared for the attacks and fell ");
            DrawText("quickly, Fraxus will not go without a fight. The first defensive ");
            DrawText("towers come online as the first waves of Agnotaritan ships approach. ");
            DrawText("");
            DrawText("");
            DrawText("The planet must hold off this wave and each wave in turn, in order ");
            DrawText("to produce increasing deadly and effective defensive structures,");
            DrawText("with the slim hope of surviving another day.");
            DrawText("");
            DrawText("");
            DrawText("You are Commander Bigguns, in charge of the planets defenses. ");
            DrawText("Good luck, ");
            DrawText("you're going to need it!");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("Press Enter to begin construction!");
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
                RsStateManager.Push(new RsSelectionState());

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
                RsStateManager.Push(new RsSelectionState());
#else

            if (RsInput.Down(Keys.Space))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Keys.LeftShift))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsSelectionState());
#endif
            base.Input(time);
        }
    }
}

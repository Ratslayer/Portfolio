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
namespace RedStream.Kathy
{
    public class EndState : RsState
    {
        public EndState()
        {
            BackgroundTexture = RedStream.Content.GetTexture("Skull");
            Font = RedStream.Content.GetFont("Courier New");
            BigFont = RedStream.Content.GetFont("Pericles");
        }
        public Texture2D BackgroundTexture;
        public SpriteFont Font, BigFont;
        public SmoothFloatComponent SmoothAlpha, SmoothAlphaBackground;
        public bool ApplyPP = false, Alive=true;
        public Vector2 TextPos;
        public int iText = 0, numLines=8;
        public override void EnterState()
        {
            TextPos = new Vector2(30, 100);
            ApplyPP = false;
            Alive = true;
            RedStream.Game.Components.Clear();
            SmoothAlpha = new SmoothFloatComponent(0, numLines, 0.09f, 0);
            base.EnterState();
        }
        public override void Draw()
        {
            iText = 0;
            SpriteBatch batch = RedStream.Graphics.batch;
            RedStream.Graphics.Clear(Color.Black);
            if(ApplyPP)
                RedStream.Graphics.BeginPostProcess();
            RedStream.Graphics.DrawFullscreenQuad(BackgroundTexture, new Vector4(1, 1, 1, (numLines-SmoothAlpha.F)/(float)numLines));
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null);
            batch.DrawString(BigFont, "Congratulations", new Vector2(10, 20), Color.White);
            DrawText("You have escaped the Purgatory...");
            DrawText("And cleared the path for Kathy's soul...");
            DrawText("Maybe she will finally find peace...");
            DrawText("Or maybe the horror is not over yet...");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("It's never over...");
            batch.End();
            if(ApplyPP)
                RedStream.Graphics.EndPostProcess();
        }
        public void DrawText(string text)
        {
            RedStream.Graphics.batch.DrawString(Font, text, TextPos+new Vector2(0,30*iText), new Color(new Vector4(1, 1, 1, SmoothAlpha.F-iText)));
            iText++;
        }
        public override void Update(GameTime time)
        {
            base.Update(time);
        }
        public override void Input(Microsoft.Xna.Framework.GameTime time)
        {
            if (RsInput.Down(Keys.Space))
            {
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Push(new MainMenuState());
            }
            else if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            base.Input(time);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RedStream
{
    class RsGameOverState : RsState
    {
        public Texture2D background1;
        public SpriteFont font, bigFont;
        public float angle;
        public SmoothFloatComponent SmoothAlpha;
        public Color GameOverColor, BaseColor;
        public string gameOverString = "GAME OVER";
        public Vector2 stringLength, titlePos, scorePos;

        public Vector2 TextPos, CurTextPos;
        public float TextScrollSpeed, DefaultTextScrollingSpeed = 20, TopLimit, BottomLimit;

        public RsGameOverState()
        {
            background1 = RedStream.Content.GetTexture("worldsplode");
            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.01f, 0);
            font = RedStream.Content.GetFont("Courier New");
            bigFont = RedStream.Content.GetFont("Game Over");
            stringLength = bigFont.MeasureString(gameOverString);
            titlePos = new Vector2(RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width/2, 10);
            titlePos.X -= stringLength.X/2;
            GameOverColor = BaseColor = new Color(0.905f, 0.404f, 0);
            TopLimit = 100;
            BottomLimit = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height;
            scorePos = new Vector2(RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width - 200, RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height / 2);
        }
        public override void EnterState()
        {
            RsGameInfo.Gyroscope.Reset();
            TextPos = new Vector2(30, BottomLimit);
            CurTextPos = TextPos;
            angle = 0;
            base.EnterState();
        }
        public override void Draw()
        {
            CurTextPos = TextPos;
            SpriteBatch batch = RedStream.Graphics.batch;
            RedStream.Graphics.Clear(Color.Black);
            RedStream.Graphics.DrawFullscreenQuad(background1, new Vector4(1, 1, 1, SmoothAlpha.F));

            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null);
            batch.DrawString(bigFont, gameOverString, titlePos, GameOverColor);

            batch.DrawString(font, "End Score: " + RsGameInfo.Score, scorePos, Color.White);

            DrawText("You have failed...");
            DrawText("   ");
            DrawText("   ");
            DrawText("   ");
            DrawText("Everyone hates you...");
            DrawText("   ");
            DrawText("   ");
            DrawText("   ");
            DrawText("Congratulations you died...");
            batch.End();
        }

        public void DrawText(string text)
        {
            CurTextPos.Y += 30;
            float textCoords = MathHelper.Clamp(CurTextPos.Y - TopLimit, 0, BottomLimit - TopLimit) / (BottomLimit - TopLimit);
            RedStream.Graphics.batch.DrawString(font, text, CurTextPos, new Color(new Vector4(0.905f, 0.404f, 0, (float)Math.Sin(Math.PI * textCoords))));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            TextPos.Y -= TextScrollSpeed * (float)time.ElapsedGameTime.TotalSeconds;
            angle += (float)time.ElapsedGameTime.Milliseconds / 1000f;
            double value = Math.Abs(Math.Sin(angle));
            GameOverColor = new Color((int)(BaseColor.R * value), (int)(BaseColor.G * value), (int)(BaseColor.B * value));
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
            {
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Push(new RsMainMenuState());
            }
#else

            if (RsInput.Down(Keys.Space))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Keys.LeftShift))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Tapped(Keys.Enter))
            {
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Pop();
                RsStateManager.Push(new RsMainMenuState());
            }
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            
#endif
            base.Input(time);
        }
    }
}

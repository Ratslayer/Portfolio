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
    public class MainMenuState : RsState
    {
        public MainMenuState()
        {
            GameInfo.Level = 1;
            SmoothAlpha = new SmoothFloatComponent(0, 1, 0.01f, 0);
            BackgroundTexture = RedStream.Content.GetTexture("Skull");
            Font = RedStream.Content.GetFont("Courier New");
            BigFont = RedStream.Content.GetFont("Pericles");
            TopLimit = 100;
            BottomLimit = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height;
        }
        public Texture2D BackgroundTexture;
        public SpriteFont Font, BigFont;
        public SmoothFloatComponent SmoothAlpha;
        public bool ApplyPP = false, Alive=true, Begin=false;
        public Vector2 TextPos, CurTextPos;
        public float TextScrollSpeed, DefaultTextScrollingSpeed=20, TopLimit, BottomLimit;
        public override void EnterState()
        {
            TextPos = new Vector2(30, BottomLimit);
            CurTextPos = TextPos;
            ApplyPP = false;
            Alive = true;
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
            batch.DrawString(BigFont, "Kathy 2: Redemption", new Vector2(10, 20), Color.White);
            DrawText("And so it happened...");
            DrawText("That Kathy...");
            DrawText("Having lived a life of sin...");
            DrawText("Sleeping with creepy strangers and stealing others' ice creams...");
            DrawText("Has passed away...");
            DrawText("");
            DrawText("");
            DrawText("But her soul is not beyond redemption");
            DrawText("For she may still repent...");
            DrawText("And her spirit may still find peace...");
            DrawText("As it is not over yet...");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("It's never over...");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("Guide Kathy to the red orb through "+GameInfo.nLevels+" stages of Purgatory...");
            DrawText("The light shall guide your path, but you have only 1 minute to do so...");
            DrawText("");
            DrawText("Beware of pits as they are often home to creatures known as Dwellers.");
            DrawText("Dwellers hate light and they try to attack you...");
            DrawText("...should your light come close to their pit.");
            DrawText("");
            DrawText("However, they are also afraid of the light and will run away...");
            DrawText("...should they be lit outside of their home.");
            DrawText("");
            DrawText("They can only attack Kathy's last known location...");
            DrawText("...which is the last location where Kathy was lit.");
            DrawText("So you can trick them if you step into the dark...");
            DrawText("");
            DrawText("If you fall into a pit, you will be stunned for 3 seconds...");
            DrawText("...and all the Dwellers will gather to feast on your helpless body.");
            DrawText("");
            DrawText("At time of emergency, you can choose to blow up your light...");
            DrawText("...which will kill all surrounding Dwellers...");
            DrawText("...but also dim out your light...");
            DrawText("...rendering you helpless for 5 seconds...");
            DrawText("You might as well fall into a pit...");
            DrawText("");
            DrawText("");
            DrawText("");
            DrawText("Controls:");
            DrawText("Arrows       |    Right Analog Stick   |    Move the Light");
            DrawText("WASD         |    Left Analog Stick    |    Move Kathy");
            DrawText("QE           |    L1/R1                |    Rotate the camera");
            DrawText("Space        |    R2                   |    Jump");
            DrawText("Left Control |    L2                   |    Blow up the light");
            DrawText("Left Shift   |    Y                    |    Toggle camera mode");
            DrawText("");
            DrawText("");
            DrawText("It's never over...");
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
                Begin=true;
            if (Begin && Alive)
            {
                ApplyPP = true;
                Alive = false;
                GameInfo.BeginLevel();
            }
        }
        public override void Input(Microsoft.Xna.Framework.GameTime time)
        {
//#if XBOX

            if (RsInput.Down(Buttons.A))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Buttons.LeftShoulder))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
            if (RsInput.Down(Buttons.Start))
                Begin = true;
//#else

            if (RsInput.Down(Keys.Space))
                TextScrollSpeed = 200;
            else if (RsInput.Down(Keys.LeftShift))
                TextScrollSpeed = -200;
            else TextScrollSpeed = DefaultTextScrollingSpeed;
            if (RsInput.Down(Keys.Escape))
                RedStream.Game.Exit();
            if (RsInput.Down(Keys.Enter))
                Begin = true;
//#endif
            base.Input(time);
        }
    }
}

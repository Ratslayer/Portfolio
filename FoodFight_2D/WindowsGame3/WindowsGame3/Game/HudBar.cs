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
using System.Collections;
namespace WindowsGame3
{
    public class HudBar
    {
        private static SpriteBatch batch;
        private static float displayedHealth=0, displayedTime=0;
        public const float maxBarLength = 1013f;
        public const float maxBarHeight = 272f;
        public static void Draw(SpriteBatch spriteBatch)
        {
            batch = spriteBatch;
            ComputeLengths();
            Vector4 color = GetHealthColor();
            DrawHud(color);            
        }
        private static Vector4 GetHealthColor()
        {
            Vector4 healthColor = new Vector4(1 - displayedHealth / maxBarLength, displayedHealth / maxBarLength, 0, 1);
            float offset = 1 - Math.Max(healthColor.X, healthColor.Y);
            healthColor += new Vector4(offset, offset, 0, 0);
            return healthColor;
        }
        private static void ComputeLengths()
        {
            float healthLength = 0, timeLength = 0;
            if (Level.Player.Health > 0.0)
                healthLength = maxBarLength * Level.Player.Health / Player.MaxHealth;
            if (Level.door.timeLeft > 0.0)
                timeLength = maxBarLength * Level.door.timeLeft / Door.MaxTime;
            if (healthLength != displayedHealth)
            {
                displayedHealth += maxBarLength / 60.0f * Math.Sign(healthLength - displayedHealth);
                if (Math.Abs(displayedHealth - healthLength) < 10)
                    displayedHealth = healthLength;
            }
            if (timeLength != displayedTime)
            {
                displayedTime += maxBarLength / 60.0f;
                if (displayedTime > timeLength)
                    displayedTime = timeLength;
            }
            displayedHealth = MathHelper.Clamp(displayedHealth, 0f, maxBarLength);
            displayedTime = MathHelper.Clamp(displayedTime, 0f, maxBarLength);
        }
        private static void DrawHud(Vector4 healthColor)
        {
            batch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            //HUD borders
            Draw(new Vector2(0, 0), new Vector2(1440, 70), new Vector4(1, 1, 1, 1));
            Draw(new Vector2(1, 1), new Vector2(1438, 68), new Vector4(0, 0, 0, 1));
            Draw(new Vector2(251, 2), new Vector2(1017, 66), new Vector4(1, 1, 1, 1));
            //Health Bar outline
            Draw(new Vector2(252, 3), new Vector2(1015, 44), new Vector4(0, 0, 0, 1));
            //Health Bar
            Draw(new Vector2(253, 4), new Vector2(displayedHealth, 42), healthColor);
            //Time Bar outline
            Draw(new Vector2(252, 49), new Vector2(1015, 20), new Vector4(0, 0, 0, 1));
            //Time Bar
            for (int i = 0; i < displayedTime; i++)
                Draw(new Vector2(253 + i, 50), new Vector2(1, 18), new Vector4(0, i / maxBarLength, .7f + i * .3f / maxBarLength, 1));
            //Displayed numbers
            Draw("Score: " + Level.Player.Points, new Vector2(2, 2), new Vector4(.3f, .3f, 1, 1));
            Draw("Level: " + Program.game.level.level, new Vector2(1265, 2), new Vector4(.8f, .8f, 0, 1));
            batch.End();
        }
        private static void Draw(Vector2 pos, Vector2 size, Vector4 color)
        {
            batch.Draw(GameContent.pixel, pos, null, new Color(color), 0, new Vector2(), size, SpriteEffects.None, 0.5f);
        }
        private static void Draw(string text, Vector2 pos, Vector4 color)
        {
            batch.DrawString(GameContent.MediumFont, text, pos, new Color(color));
        }
    }
}

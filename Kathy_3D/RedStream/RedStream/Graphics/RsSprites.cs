using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RedStream
{
    class RsSprites
    {
        private static Texture2D buttonSpriteSheet;
        private static Rectangle [] buttonSourceRects = {   new Rectangle(40, 10, 70, 60), // 0: Left Thumb Stick
                                                            new Rectangle(160,10, 70, 60), // 1: Right Thumb Stick
                                                            new Rectangle(10, 78, 62, 62), // 2: D-Pad Right
                                                            new Rectangle(70, 78, 62, 62), // 3: D-Pad Left
                                                            new Rectangle(130, 78, 62, 62), // 4: D-Pad Down
                                                            new Rectangle(196, 78, 62, 62), // 5: D-Pad Up
                                                            new Rectangle(10, 152, 50, 50), // 6: A Button
                                                            new Rectangle(62, 152, 50, 50), // 7: B Button
                                                            new Rectangle(116, 152, 50, 50), // 8: X Button
                                                            new Rectangle(168, 152, 50, 50), // 9: Y Button
                                                            new Rectangle(260, 32, 66, 34), // 10: Left Bumper
                                                            new Rectangle(332, 32, 66, 34), // 11: Right Bumper
                                                            new Rectangle(262, 74, 68, 58), // 12: Left Trigger
                                                            new Rectangle(332, 74, 68, 58), // 13: Right Trigger
                                                            new Rectangle(250, 150, 68, 46), // 14: Back Button
                                                            new Rectangle(320, 150, 68, 46),}; // 15: Start Button
        private static float spriteScale = 1f;
        public static float SpriteScale { get { return spriteScale; } set { spriteScale = value; } }

        public static void init()
        {
            buttonSpriteSheet = RedStream.Content.GetTexture("button_layout_3");
        }

        public static void drawLeftThumbStick(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[0], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawRightThumbStick(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[1], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawDPadRight(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[2], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawDPadLeft(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[3], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawDPadDown(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[4], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawDPadUp(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[5], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonA(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[6], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonB(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[7], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonX(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[8], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonY(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[9], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawLeftBumper(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[10], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawRightBumper(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[11], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawLeftTrigger(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[12], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawRightTrigger(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[13], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonBack(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[14], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }
        public static void drawButtonStart(Vector2 position, SpriteBatch batch) { batch.Draw(buttonSpriteSheet, position, buttonSourceRects[15], Color.White, 0f, Vector2.Zero, spriteScale, SpriteEffects.None, 0); }

    }
}

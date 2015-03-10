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
namespace WindowsGame3
{
    /// <summary>
    /// State that supports dashing text and images. Used by any information state.
    /// </summary>
    public class TextState : DeferredRenderState
    {
        /// <summary>
        /// Class that represents any sprite/text that is drawn on the screen.
        /// </summary>
        public class Entry
        {
            public string Text;         //Text that is associated with the object
            public Vector2 DesiredPos;  //The final position of the object
            public Vector2 Pos;         //Current position of the object
            public Texture2D texture;   //Texture associated with the object
            public float scale;         //Scale of the texture
            public Vector4 color;       //Color of texture or text
            public SpriteFont font;     //Font used to draw the text
        };
        /// <summary>
        /// Internal variable that helps align text.
        /// </summary>
        protected Vector2 lastPos;
        /// <summary>
        /// Speed at which entries move.
        /// </summary>
        public float EntrySpeed = 5000.0f;
        public TextState(GameState renderState)
            : base(renderState)
        {
            Entries = new List<Entry>();
        }
        /// <summary>
        /// List of entries that are on the screen right now.
        /// </summary>
        protected List<Entry> Entries;
        /// <summary>
        /// Helper function that adds a text entry to the screen.
        /// </summary>
        /// <param name="text">Text to be displayed</param>
        /// <param name="color">Text Color</param>
        /// <param name="font">Text Font</param>
        public void Add(string text, Vector4 color = new Vector4(), SpriteFont font=null)
        {
            Entry entry = new Entry();
            entry.Pos = lastPos + new Vector2(1440, 0);
            entry.DesiredPos = lastPos;
            entry.Text = text;
            //get color
            if (color.Length() == 0.0f)
                entry.color = new Vector4(1, .5f, .5f, 1);
            else entry.color = color;
            //get font
            if (font == null)
                entry.font = GameContent.Font;
            else entry.font = font;
            Entries.Add(entry);
            //offset the position
            lastPos.Y += entry.font.LineSpacing;
        }
        public override void Update(float gameTime)
        {
            //move each entry closer to the desired position
            foreach (Entry entry in Entries)
            {
                if (entry.DesiredPos != entry.Pos)
                {
                    Vector2 dir = entry.DesiredPos - entry.Pos;
                    dir.Normalize();
                    entry.Pos += dir * gameTime * EntrySpeed;
                    if ((entry.Pos - entry.DesiredPos).Length() < 50)
                        entry.Pos = entry.DesiredPos;
                    break;
                }
            }
            base.Update(gameTime);
        }
        public override void Draw()
        {
            RenderManager rman = Program.game.rMan;
            base.Draw();
            //draw text
            rman.BeginDraw(null);
            foreach (Entry entry in Entries)
            {
                if (entry.texture == null)
                    rman.batch.DrawString(entry.font, entry.Text, entry.Pos, new Color(entry.color));
            }
            rman.batch.End();
            //draw sprites
            rman.BeginDraw(rman.colorEffect);
            foreach (Entry entry in Entries)
            {
                if (entry.texture != null)
                    rman.batch.Draw(entry.texture, entry.Pos, null, new Color(entry.color), 0, new Vector2(), entry.scale, SpriteEffects.None, 0);
            }
            rman.batch.End();
        }
    }
}

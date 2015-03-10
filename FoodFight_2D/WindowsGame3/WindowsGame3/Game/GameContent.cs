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
    public class GameContent
    {
        public static Texture2D Player, test, projectile, shoop, background, enemy, door, pixel;
        public static Texture2D[] Fireballs;
        public static SpriteFont Font, BigFont, MediumFont;
        public static void LoadContent()
        {
            ContentManager con=Program.game.Content;
            Player=con.Load<Texture2D>("kathy");
            test = con.Load<Texture2D>("GameThumbnail");
            projectile = con.Load<Texture2D>("fire_particle");
            shoop = con.Load<Texture2D>("sdw");
            Font = con.Load<SpriteFont>("SpriteFont1");
            BigFont = con.Load<SpriteFont>("SpriteFont2");
            MediumFont = con.Load<SpriteFont>("SpriteFont3");
            LoadBackground("rainbow");
            Fireballs = new Texture2D[4];
            Fireballs[0] = con.Load<Texture2D>("Fireball_1");
            Fireballs[1] = con.Load<Texture2D>("fireball_2");
            Fireballs[2] = con.Load<Texture2D>("fireball_3");
            Fireballs[3] = con.Load<Texture2D>("beachball");
            enemy = con.Load<Texture2D>("Pedobear");
            door = con.Load<Texture2D>("Door");
            pixel = con.Load<Texture2D>("pixel");
        }
        public static void LoadBackground(string name)
        {
            background = Program.game.Content.Load<Texture2D>(name);
        }
    }
}

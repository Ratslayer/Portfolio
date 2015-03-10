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
    /// <summary>
    /// The level class which contains the Door and Player objects, as well as spawns pits whenver is reset.
    /// </summary>
    public class Level
    {
        public Level(Game1 game)
        {
            this.game = game;
            door = new Door();
            door.pos = new Vector2(200, 450);
        }
        /// <summary>
        /// If set to true, then the level will be reset in the GameState.Update function.
        /// Needed for special effects associated with level change in the game.
        /// </summary>
        public bool bReset
        {
            get;
            private set;
        }
        /// <summary>
        /// Level number. Used for various number calculations, such as level duration and number of pits.
        /// </summary>
        public int level;
        /// <summary>
        /// End door object.
        /// </summary>
        public static Door door;
        /// <summary>
        /// Player object.
        /// </summary>
        public static Player Player;
        /// <summary>
        /// Default Player spawning position.
        /// </summary>
        public static Vector2 PlayerSpawnPosition = new Vector2(1200, 400);
        /// <summary>
        /// Game1 shortcut, as to clean the code up a little and reduce coupling.
        /// </summary>
        private Game1 game;
        /// <summary>
        /// Set level to the given number.
        /// </summary>
        /// <param name="nLevel">Level id</param>
        public void Reset(int nLevel = 1)
        {
            bReset = true;
            level = nLevel;
        }
        /// <summary>
        /// Helper function that gets a random center, as to have the bounds to be fully inside the screen.
        /// </summary>
        /// <param name="bounds">Size of bounds</param>
        /// <returns>The result center</returns>
        public Vector2 GetRandomCenteredPos(Vector2 bounds)
        {
            return GetRandomPos(bounds / 2);
        }
        /// <summary>
        /// Helper function that gets a random top left corner, as to have the bounds fully inside the screen.
        /// </summary>
        /// <param name="bounds">Size of bounds</param>
        /// <returns>The result corner</returns>
        public Vector2 GetRandomPos(Vector2 bounds)
        {
            Vector2 min = new Vector2(door.Bounds.X + door.pos.X, HudBar.maxBarHeight) + bounds;
            Vector2 max = new Vector2(PlayerSpawnPosition.X - Player.Bounds.X / 2, game.height) - bounds;
            Vector2 pos = Game1.GetRandom(min, max);
            return pos;
        }
        /// <summary>
        /// Level Initialize function.
        /// </summary>
        public void InitLevel()
        {
            bReset = false;
            door.Reset();
            game.Add(door);
            //recreate the player if it's the first level
            if (level == 1)
            {
                Player = new Player();

                Player.color = new Vector4(1, 0, 0, 1);
                Player.Bounds = new Vector2(64, 64);
                Player.angle = 0;
                Player.movSpeed = 300;

                Player.Points = 0;
                Player.RestoreHealth();
                Player.Arm(null);
            }
            game.ClearAll();
            Player.pos = PlayerSpawnPosition;
            game.Add(Player);
            //generate the pits on the screen
            for (int i = 0; i < (int)Math.Ceiling((double)level / 2.0); i++)
            {
                Pit pit = new Pit();
                pit.texture = GameContent.test;
                pit.desiredRadius = Game1.GetRandom(50, 200);
                pit.Radius = 0;
                pit.pos = GetRandomPos(new Vector2(pit.desiredRadius, pit.desiredRadius));
                pit.color = new Vector4(Game1.GetRandom(1, 0), Game1.GetRandom(1, 0), Game1.GetRandom(1, 0), 1);

                game.Add(pit);
            }
            //generate ammo
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = GetRandomPos(new Vector2(100, 100));
                AmmoFactory.UID.CreateRandomAmmoPack(pos);
            }
            game.AddNew();
        }
    }
}

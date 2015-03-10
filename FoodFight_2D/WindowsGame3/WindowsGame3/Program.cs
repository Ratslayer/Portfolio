using System;

namespace WindowsGame3
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Game1 game;
        static void Main(string[] args)
        {
            using (game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}


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
    /// State that is pushed if the game is beaten.
    /// </summary>
    public class EndGameState : PageTextState
    {
        public EndGameState(GameState renderState)
            : base(renderState)
        {
            CreatePage();
        }
        #region Page Creation Functions
        protected override void CreatePage()
        {
            Entries.Clear();
            switch (iPage)
            {
                case 0:
                    CreatePage0();
                    break;
                case 1:
                    CreatePage1();
                    break;
                default:
                    GameplayState gameplayState = new GameplayState();
                    Program.game.stateStack.Clear();
                    Program.game.PushState(new MainMenuState(new BackGroundState()));
                    break;
            }
        }
        private void CreatePage0()
        {
            lastPos = new Vector2(300, 200);
            Add("Congratulations, you have beaten this game with a score of "+Level.Player.Points+" points!");
            lastPos.Y += 30;
            Add("But surely, you are wondering what happened to Kathy, right?");
            lastPos.Y += 30;
            Add("Well, she escaped the bar, but she was under such shock that, soon afterwards, she ran away");
            Add("and entered a monastery, where she spent the rest of her life praying and preaching to");
            Add("god knows what, until she finally died... As a 92 year old virgin.");
            lastPos.Y += 30;
            Add("Space... Space? I'm in space!");
        }
        private void CreatePage1()
        {
            lastPos = new Vector2(300, 200);
            Add("So what's the moral of the story you might ask?");
            lastPos.Y += 30;
            Add("Well, to start off - girls, don't go to the bar if you're underage.");
            Add("No one's going to sell you anything anyway");
            lastPos.Y += 30;
            Add("Then, if you still happened to go there - watch your drink.");
            Add("As long as you're not fat you are always a target for various Me Gustas.");
            Add("And even if you are, well, people like different things.");
            lastPos.Y += 30;
            Add("But last, and the most important, ladies...");
            Add("Do not... Under any circumstances...");
            Add("Never... EVER...");
            Add("LEAVE THE KITCHEN...");
            lastPos.Y += 100;
            Add("Press SPAAAAAAAAACE to relive the horror once more...");
            Add("Press ESCAPE to wake up.");
        }
        #endregion
    }
}

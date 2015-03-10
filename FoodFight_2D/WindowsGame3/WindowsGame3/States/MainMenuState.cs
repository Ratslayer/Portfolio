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
    /// The state that displays the main menu.
    /// </summary>
    public class MainMenuState : PageTextState
    {
        public MainMenuState(GameState renderState)
            : base(renderState)
        {
        }
        public override void EnterState()
        {
            GameContent.LoadBackground("rainbow2");
            Program.game.rMan.bloomFactor = -.9f;
            CreatePage();
            base.EnterState();
        }
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
                case 2:
                    CreatePage2();
                    break;
                default:
                    GameplayState gameplayState = new GameplayState();
                    Program.game.PushState(gameplayState);
                    Program.game.PushState(new BubblingState(300, gameplayState));
                    Program.game.PushState(new TransitionState(1, 20, gameplayState));
                    Program.game.PushState(new TransitionState(15, 20, gameplayState));
                    Program.game.PushState(new SwapBackgroundState("rainbow", this));
                    Program.game.PushState(new TransitionState(-10, 5, this));
                    break;
            }
        }
        private void CreatePage0()
        {
            lastPos = new Vector2(100, 100);
            Add("Kathie's Grand Escape", new Vector4(.7f, 0, 0, 1), GameContent.BigFont);
            lastPos = new Vector2(300, 250);
            Add("One day Kathy hit 13 and decided that it was time to leave the kitchen and get laid");
            Add("So she got her sister's ID and went to a bar. However, the stupid bitch that she was,");
            Add("she didn't watch her glass and some Mexican slipped her a present.");
            lastPos.Y += 30;
            Add("All went OMGTEHFAK in Kathie's head. The bar melted into a rainbow disaster.", new Vector4(.3f, .3f, 1, 1));
            Add("Spacetime continuum did not seem to hold anymore.", new Vector4(.3f, .3f, 1, 1));
            Add("Kathy knew one thing - she had to get out of here before any of those pedophiles caught her!", new Vector4(.3f, .3f, 1, 1));
            lastPos.Y += 30;
            Add("Press SPAAAAAAAAAAAAAAACE to continue!", new Vector4(.3f, 1, .3f, 1));
        }
        private void CreatePage1()
        {
            lastPos = new Vector2(300, 100);
            Add("INSTRUCTIONS", new Vector4(.9f, .9f, 0, 1), GameContent.BigFont);
            lastPos = new Vector2(300, 250);
            Add("Kathy has to escape 10 levels of madness.");
            Add("To do so she must reach the door at the end of each level");
            lastPos.Y += 50;
            Add("Avoid spacetime warps (bubbles) and various creepy personel", new Vector4(.3f, .3f, 1, 1));
            Add("as they will deal damage to Kathy", new Vector4(.3f, .3f, 1, 1));
            Add("If Kathie's Health will fall below 0, you will be thrown back to level 1", new Vector4(.3f, .3f, 1, 1));
            Add("You can't leave until you win!!! Or press escape for that matter...", new Vector4(.3f, .3f, 1, 1));
            lastPos.Y += 50;
            Add("Mexicans and other masters of the trade will throw projectiles,");
            Add("in an attempt to try and knock Kathie out.");
            lastPos.Y += 50;
            Add("Kathy can pick up those projectiles and use them against her foes.");
            Add("In addition to restoring her health, they have special effects when thrown:");
            Add("Red fireball deals 5 damage and does not stop until it hits a wall.", new Vector4(1,0,0,1));
            Add("Green vortex deals 10 damage and bounces off enemies and walls.", new Vector4(0, 1, 0, 1));
            Add("Blue energon deals 7 damage and multiplies when hits enemies.", new Vector4(0, 0, 1, 1));
            lastPos.Y += 50;
            Add("All of the the projectiles follow Kathie's movement vector.");
            Add("If Kathy is standing still she will land a mine at her position.");
            Add("The mines have similar effects to the projectiles.");
            lastPos.Y += 30;
            Add("Dad, are you space?", new Vector4(.3f, 1, .3f, 1));
            //draw the images that help guide the player
            Entry entry = new Entry();
            entry.texture = GameContent.door;
            entry.DesiredPos = new Vector2(1100, 50);
            entry.Pos = entry.DesiredPos + new Vector2(0, 1000);
            entry.scale = 1;
            entry.color = new Vector4(1, 0, 0, 1);
            Entries.Add(entry);

            entry = new Entry();
            entry.texture = GameContent.enemy;
            entry.DesiredPos = new Vector2(1250, 150);
            entry.Pos = entry.DesiredPos + new Vector2(-1440,0);
            entry.scale = 1;
            entry.color = new Vector4(1f, .7f, .5f, 1);
            Entries.Add(entry);

            entry = new Entry();
            entry.texture = GameContent.Fireballs[0];
            entry.DesiredPos = new Vector2(1150, 500);
            entry.Pos = entry.DesiredPos + Game1.GetRotation(240)*1440;
            entry.scale = .25f;
            entry.color = new Vector4(1f, 0, 0, .99f);
            Entries.Add(entry);

            entry = new Entry();
            entry.texture = GameContent.Fireballs[1];
            entry.DesiredPos = new Vector2(1175, 560);
            entry.Pos = entry.DesiredPos - Game1.GetRotation(270) * 1440;
            entry.scale = .3f;
            entry.color = new Vector4(0, 1, 0, .99f);
            Entries.Add(entry);

            entry = new Entry();
            entry.texture = GameContent.Fireballs[2];
            entry.DesiredPos = new Vector2(1250, 500);
            entry.Pos = entry.DesiredPos + Game1.GetRotation(300) * 1440;
            entry.scale = .25f;
            entry.color = new Vector4(0, 0, 1, .99f);
            Entries.Add(entry);
        }
        private void CreatePage2()
        {
            lastPos = new Vector2(400, 100);
            Add("CONTROLS", new Vector4(.3f, .3f, 1, 1), GameContent.BigFont);
            lastPos.Y += 100;
            Add("Left/Right/Up/Down: Move Kathy");
            Add("SPAAAAAAAAAAAAAACE: Shoop da woop!");
            Add("TAB:                Pause the Game");
            Add("ESCAPE:             Abandon Kathy and leave the game (like a boss)");
            lastPos.Y += 100;
            Add("\"Yes son, I am... Now we can be family again!\"", new Vector4(.3f, 1, .3f, 1));
        }
    }
}

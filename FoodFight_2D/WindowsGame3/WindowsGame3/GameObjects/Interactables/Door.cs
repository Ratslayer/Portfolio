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
    /// The door class which must be reached at the end of the level. 
    /// It is closed for the duration of the timer.
    /// Interacting with a closed door more than breakThreshold times will keep the door closed until level reset.
    /// </summary>
    public class Door : Interactable
    {
        #region Attributes
        #region Static
        /// <summary>
        /// The factor that defines how much the door timer scales with each level.
        /// </summary>
        public const float SecondsPerLevel = 5f;
        /// <summary>
        /// The total time that the door is shut for the current level.
        /// </summary>
        public static float MaxTime
        {
            get
            {
                return Program.game.level.level * SecondsPerLevel;
            }
            private set { }
        }
        /// <summary>
        /// The door break threshold.
        /// </summary>
        private const int breakThreshold = 6;
        #endregion
        #region Public
        /// <summary>
        /// Time left before this Door is enterable.
        /// </summary>
        public float timeLeft;
        #endregion
        #region Private
        /// <summary>
        /// List of messages currently on the screen.
        /// </summary>
        private List<Message> msgs;
        /// <summary>
        /// The messages that will be displayed if the player enters a closed door.
        /// </summary>
        private List<string> closedDoorMsgs;
        /// <summary>
        /// Closed door messages fetched counter. Once it reaches the breakThreshold the door will jam.
        /// </summary>
        private int numMessagesFetched = 0;
        #endregion
        #endregion
        #region Constructors
        public Door()
            : base(GameContent.door)
        {
            timeLeft = 0;
            bAllowEnter = true;
            InitMsgs();
        }
        #endregion
        #region Functions
        #region Public
        /// <summary>
        /// Reset the door state.
        /// </summary>
        public void Reset()
        {
            timeLeft = MaxTime;
            bDisabled = true;
            color = new Vector4(0, 0, 0, 1);
            InitMsgs();
        }
        /// <summary>
        /// The function that draws the messages to the screen. Must be called by the state.
        /// </summary>
        /// <param name="batch">The SpriteBatch object that will draw the text.</param>
        public void DrawMsgs(SpriteBatch batch)
        {
            batch.Begin();
            foreach (Message msg in msgs)
            {
                SpriteFont font = GameContent.Font;
                Vector2 bounds = font.MeasureString(msg.text);
                Vector2 center = GetCenteredPos(msg.center, bounds, msg.angle);
                float rads = MathHelper.ToRadians(msg.angle);
                Vector4 color = new Vector4(1, 0, 0, msg.lifeTime / Message.initialLifeTime);
                batch.DrawString(font, msg.text, center, new Color(color), rads, new Vector2(), 1, SpriteEffects.None, 0.0f);
            }
            batch.End();
        }
        public override void Update(float time)
        {
            timeLeft -= time;
            if (bDisabled 
                && timeLeft <= 0 
                && numMessagesFetched < breakThreshold)
            {
                bDisabled = false;
                color = new Vector4(Game1.GetRandom(1, 0), Game1.GetRandom(1, 0), Game1.GetRandom(1, 0), 1);
                SpawnMsg("Here! Come in honey!");
            }
            UpdateMsgs(time);
            base.Update(time);
        }
        public override void Interact()
        {
            base.Interact();
            Player.Points += 100;
            Program.game.level.Reset(Program.game.level.level + 1);
        }
        public override void Enter()
        {
            base.Enter();
            SpawnMsg(GetMessage());
        }
        #endregion
        #region Private
        /// <summary>
        /// Creates a text message which radiates outwardly from the door.
        /// </summary>
        /// <param name="text">Text that message contains</param>
        private void SpawnMsg(string text)
        {
            float angle = Game1.GetRandom(22.5f, -22.5f);
            angle += 22.5f * Math.Sign(angle);
            Message msg = new Message();
            msg.text = text;
            msg.lifeTime = Message.initialLifeTime;
            msg.center = pos;
            msg.angle = angle;
            msgs.Add(msg);
        }
        /// <summary>
        /// Init the msg list and populate the closedDoorMsgs.
        /// Also resets the msg counter to 0.
        /// </summary>
        private void InitMsgs()
        {
            msgs = new List<Message>();
            closedDoorMsgs = new List<string>();
            closedDoorMsgs.Add("Wait a sec darling. Lemme open the door!");
            closedDoorMsgs.Add("It's stuck cutie. Lemme try and fix it.");
            closedDoorMsgs.Add("I'm working on it! Be patient.");
            closedDoorMsgs.Add("Hitting it won't help...");
            closedDoorMsgs.Add("Ok, this is getting annoying... Stop it!");
            closedDoorMsgs.Add("Stop this! You'll break it!");
            closedDoorMsgs.Add("Ok, that's it! You broke it!");
            closedDoorMsgs.Add("I hope you're happy now!");
            closedDoorMsgs.Add("Yeah, you keep banging that door!");
            closedDoorMsgs.Add("That'll fix it!");
            closedDoorMsgs.Add("I don't even feel sorry for you!");
            closedDoorMsgs.Add("You prolly like Twilight too.");
            closedDoorMsgs.Add("Have fun there!");
            numMessagesFetched = 0;
        }
        /// <summary>
        /// Get the message, based on the state of the Door.
        /// </summary>
        /// <returns>The message text that should be displayed.</returns>
        private string GetMessage()
        {
            string msg;
            if (!bDisabled)
                msg = "There you go sweetheat!";
            else
            {
                if (closedDoorMsgs.Count > 0)
                {
                    msg = closedDoorMsgs[0];
                    closedDoorMsgs.RemoveAt(0);
                    numMessagesFetched++;
                }
                else msg = "U MAD?";
            }
            return msg;
        }
        
        /// <summary>
        /// Internal Message class which is used to store the spatial data of a text message.
        /// </summary>
        private class Message
        {
            public static float speed = 50;
            public static float initialLifeTime = 2;

            public string text;
            public Vector2 center;
            public float angle, lifeTime;
        }
        /// <summary>
        /// Update msg positions.
        /// </summary>
        /// <param name="time">the time elapsed since alst frame</param>
        private void UpdateMsgs(float time)
        {
            List<Message> liveMsgs = new List<Message>();
            foreach (Message msg in msgs)
            {
                msg.lifeTime -= time;
                if (msg.lifeTime >= 0f)
                {
                    //move the msg perperndicularly to its orientation
                    float dirAngle = msg.angle - Math.Sign(msg.angle) * 90f;
                    float dirRads = MathHelper.ToRadians(dirAngle);
                    Vector2 dir = new Vector2((float)Math.Cos(dirRads), (float)Math.Sin(dirRads));
                    msg.center += dir * time * Message.speed;
                    liveMsgs.Add(msg);
                }
            }
            msgs = liveMsgs;
        }
        #endregion
        #endregion
    }
}

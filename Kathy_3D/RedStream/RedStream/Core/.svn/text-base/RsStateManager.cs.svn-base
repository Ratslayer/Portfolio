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
namespace RedStream
{
    public class RsStateManager
    {
        private static Stack<RsState> Stack = new Stack<RsState>();
        public static RsState Top()
        {
            return Stack.Peek();
        }
        public static void Push(RsState state)
        {
            Stack.Push(state);
        }
        public static RsState Pop()
        {
            if (Stack.Count > 0)
            {
                RsState state = Stack.Pop();
                state.ExitState();
                return state;
            }
            else return null;
        }
    }
}

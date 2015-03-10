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
    class RsContent
    {
        private class Pair
        {
            public Pair(string name, Object obj)
            {
                Name = name;
                Object = obj;
            }
            public string Name;
            public Object Object;
        }
        public RsContent()
        {
            Resources = new List<Pair>();
            Init();
        }
        private List<Pair> Resources;
        public int GetNumResources()
        {
            return Resources.Count;
        }
        private static AudioEngine Audio;
        private static WaveBank Wave;
        private static SoundBank Sound;
        public static void PlayCue(string cue)
        {
            Sound.PlayCue(cue);
        }
        private void Init()
        {
            Audio = new AudioEngine(@"Content\Sound\AudioEngine.xgs");
            Wave = new WaveBank(Audio, @"Content\Sound\Wave Bank.xwb");
            Sound = new SoundBank(Audio, @"Content\Sound\Sound Bank.xsb");
        }
        private T LoadContent<T>(string name)
        {
            if (!name.Contains("None"))
            {
                T content = RedStream.Game.Content.Load<T>(typeof(T).Name + "s\\" + name);
                Resources.Add(new Pair(name, content));
                return content;
            }
            return default(T);
        }
        private T GetContent<T>(string name)
        {
            if (name != "")
            {
                foreach (Pair pair in Resources)
                    if (pair.Object is T && pair.Name == name)
                        return (T)pair.Object;
                return LoadContent<T>(name);
            }
            else return default(T);
        }
        public Song GetSong(string name)
        {
            return GetContent<Song>(name);
        }
        public SpriteFont GetFont(string name)
        {
            return GetContent<SpriteFont>(name);
        }
        public Model GetModel(string name)
        {
            return GetContent<Model>(name);
        }
        public Texture2D GetTexture(string name)
        {
            return GetContent<Texture2D>(name);
        }
        public Effect GetEffect(string name)
        {
            return GetContent<Effect>(name);
        }
        public RsData GetObjectAttributes(string name)
        {
            return GetContent<RsData>(name);
        }
        /// <summary>
        /// Helper function that simplifies loading of GameObjects into Game.Components.
        /// Is called within 
        /// </summary>
        /// <param name="obj"></param>
        public static void AddComponent(RsGameObject obj)
        {
            RedStream.Game.LoadComponent(obj);
        }
        public static void RemoveComponent(RsGameObject obj)
        {
            RedStream.Game.DestroyComponent(obj);
        }  
    }
}

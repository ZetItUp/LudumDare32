using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace APEngine.SoundManager
{
    public enum MusicTrack
    {
        MenuThemeSong = 0
    };

    public static class MusicManager
    {
        private static SoundEffect menuThemeSong;
        public static SoundEffectInstance Instance;

        public static MusicTrack Track;

        private static float vol = 0f;
        public static float Volume
        {
            get { return vol; }
            set
            {
                vol = value;
                vol = (float)Math.Round(vol, 2);

                if (vol > 1f)
                    vol = 1f;
                else if (vol < 0f)
                    vol = 0f;

                if(Instance != null)
                {
                    Instance.Volume = vol;
                }
            }
        }

        public static float Pitch;
        public static float Pan;

        public static void LoadContent(ContentManager Content)
        {
            menuThemeSong = Content.Load<SoundEffect>("Data\\Music\\menusong");
        }

        private static SoundEffect GetEffectByTrack(MusicTrack track)
        {
            if (track == MusicTrack.MenuThemeSong)
                return menuThemeSong;

            return null;
        }

        public static void Play(MusicTrack track)
        {
            Track = track;

            Instance = GetEffectByTrack(track).CreateInstance();
            Instance.Play();

            Instance.Volume = Volume;
            Instance.Pitch = Pitch;
            Instance.Pan = Pan;
        }

        public static void Play(MusicTrack track, bool loop)
        {
            Track = track;

            Instance = GetEffectByTrack(track).CreateInstance();
            Instance.IsLooped = loop;
            Instance.Play();

            Instance.Volume = Volume;
            Instance.Pitch = Pitch;
            Instance.Pan = Pan;
        }

        public static void Pause()
        {
            Instance.Pause();
        }

        public static void Stop()
        {
            Instance.Stop();
        }
    }
}

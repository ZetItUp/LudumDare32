using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace APEngine.SoundManager
{
    public class SoundManager
    {
        // Menu Sounds
        public static SoundEffect MenuSelect01 { get; set; }
        public static SoundEffect MenuSelect02 { get; set; }

        // Enemy Sounds
        public static SoundEffect AlienHurt01 { get; set; }
        public static SoundEffect AlienJump { get; set; }
        public static SoundEffect AlienJump2 { get; set; }

        // Player Sounds
        public static SoundEffect Jump { get; set; }

        // Game Object Sounds
        public static SoundEffect ItemBoxBreak { get; set; }
        public static SoundEffect LifePickUp { get; set; }
        public static SoundEffect StarPickUp { get; set; }

        public float Volume { get; set; }
        public float Pitch { get; set; }
        public float Pan { get; set; }

        public SoundManager()
        {
            Volume = 0.1f;
            Pitch = 0.0f;
            Pan = 0.0f;
        }

        public static void LoadContent(ContentManager Content)
        {
            AlienHurt01 = Content.Load<SoundEffect>("Data\\Sounds\\AlienHurt01");
            AlienJump = Content.Load<SoundEffect>("Data\\Sounds\\AlienJump");
            AlienJump2 = Content.Load<SoundEffect>("Data\\Sounds\\AlienJump2");

            Jump = Content.Load<SoundEffect>("Data\\Sounds\\Jump");

            MenuSelect01 = Content.Load<SoundEffect>("Data\\Sounds\\MenuSelect");
            MenuSelect02 = Content.Load<SoundEffect>("Data\\Sounds\\MenuSelect2");

            ItemBoxBreak = Content.Load<SoundEffect>("Data\\Sounds\\BoxBreakTemp");
            LifePickUp = Content.Load<SoundEffect>("Data\\Sounds\\LifePickUpTemp");
            StarPickUp = Content.Load<SoundEffect>("Data\\Sounds\\StarPickUpTemp");
        }

        public void PlaySoundEffect(SoundEffectInstance instance)
        {
            if (instance.State == SoundState.Playing)
                return;

            instance.Volume = Volume;
            instance.Pitch = Pitch;
            instance.Pan = Pan;
            instance.Play();
        }
    }
}

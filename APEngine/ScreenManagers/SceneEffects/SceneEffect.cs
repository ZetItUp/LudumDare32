using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneEffects
{
    public class SceneEffect
    {
        public float Duration { get; set; }
        public bool HasPlayed { get; set; }
        public bool PlayThis { get; set; }
        public bool Visible { get; set; }

        float currTick = 0f;

        public SceneEffect()
        {
            Duration = 0f;
            HasPlayed = false;
            PlayThis = false;
            Visible = true;
        }

        public virtual void LoadContent(ContentManager Content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            if (HasPlayed || !PlayThis)
                return;

            if (Duration != 0)
            {
                currTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (currTick > Duration)
                {
                    HasPlayed = true;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (HasPlayed || !PlayThis || !Visible)
                return;
        }
    }
}

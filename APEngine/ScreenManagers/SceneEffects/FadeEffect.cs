using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.ScreenManagers.SceneEffects
{
    public enum FadeEffectType
        {
            In = 0,
            Out
        };

    public class FadeEffect : SceneEffect
    {
        private FadeEffectType type;
        public FadeEffectType Type 
        {
            get { return type; }
            set
            {
                type = value;

                if (value == FadeEffectType.In)
                {
                    Alpha = 1f;
                }
                else
                {
                    Alpha = 0f;
                }
            }
        }
        private Texture2D fadeTexture;

        public Color FadeColor { get; set; }
        public float Alpha { get; set; }
        private Rectangle Rect { get; set; }

        float currTick;

        public FadeEffect()
            : base()
        {
            Type = FadeEffectType.In;
            FadeColor = Color.Black;
            Alpha = 1f;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            Duration = 5000;
            Rect = new Rectangle(0, 0, Engine.Monitor.VirtualWidth, Engine.Monitor.VirtualHeight);
            fadeTexture = Engine.AtriceGraphics.CreateRectangle(Rect.Width, Rect.Height, FadeColor);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasPlayed || !PlayThis)
                return;

            currTick += (float)gameTime.ElapsedGameTime.Milliseconds;

            if(Type == FadeEffectType.In)
            {
                Alpha -= 1f * (float)gameTime.ElapsedGameTime.Milliseconds / Duration;
            }
            else
            {

                Alpha += 1f * (float)gameTime.ElapsedGameTime.Milliseconds / Duration;
            }

            Alpha = MathHelper.Clamp(Alpha, 0, 1);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (HasPlayed || !PlayThis || !Visible)
                return;

            spriteBatch.Draw(fadeTexture, Rect, Color.White * Alpha);
        }
    }
}

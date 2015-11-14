using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public enum WaterType
    {
        Standard = 0,
        Top
    };

    public class Water : GameObject
    {
        public WaterType WaterType { get; set; }
        private Graphics.Animation animation;

        public Water(Vector2 position)
            : base(position)
        {
            WaterType = WaterType.Standard;
            Rect = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            if (WaterType == WaterType.Standard)
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Water"), Position, 32, 32, 0, 0);
            else
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\WaterWave"), Position, 32, 32, 3, 150);

            animation.Depth = 1.0f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            animation.Draw(spriteBatch);
        }
    }
}

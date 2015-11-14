using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.GameObjects
{
    public class Mushroom : GameObject
    {
        Graphics.Animation animation;

        public Mushroom(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\Mushroom01"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 1, Engine.Randomizer.Next(250, 350));
            animation.Depth = 0.41f;
        }

        public override void Update(GameTime gameTime)
        {
            if (InProportion)
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(DEFAULT_WIDTH * ScaleXY), (int)(DEFAULT_HEIGHT * ScaleXY));
                animation.ScaleTo(ScaleXY);
            }
            else
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(DEFAULT_WIDTH * ScaleX), (int)(DEFAULT_HEIGHT * ScaleY));
                animation.SetScale(ScaleX, ScaleY);
            }
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

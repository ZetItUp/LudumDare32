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
    public enum BigFlowerColor
    {
        Yellow = 0
    };

    public class BigFlower : GameObject
    {
        Graphics.Animation animation;
        Graphics.Animation stalk;
        public BigFlowerColor FlowerColor { get; set; }

        public BigFlower(Vector2 position)
            : base(position)
        {
            Position = new Vector2(Position.X - 32, Position.Y - 28);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, 64, 64);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\BackgroundFlower" + FlowerColor.ToString()), Position, Rect.Width, Rect.Height, 0, 1);
            stalk = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\BackgroundFlower" + FlowerColor.ToString() + "Stalk"), Position, Rect.Width, Rect.Height, 0, 1);
        }

        public override void Update(GameTime gameTime)
        {
            if(InProportion)
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(64 * ScaleXY), (int)(64 * ScaleXY));
                animation.ScaleTo(ScaleXY);
                stalk.ScaleTo(ScaleXY);
            }
            else
            {
                Rect = new Rectangle((int)Position.X, (int)Position.Y, (int)(64 * ScaleX), (int)(64 * ScaleY));
                animation.SetScale(ScaleX, ScaleY);
                stalk.SetScale(ScaleX, ScaleY);
            }

            animation.Depth = Depth;
            animation.Position = Position;
            stalk.Depth = Depth - 0.1f;
            animation.Update(gameTime);
            stalk.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
            stalk.Draw(spriteBatch);
        }
    }
}

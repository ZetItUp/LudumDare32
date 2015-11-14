using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public enum WoodFencePiece
    {
        Center = 0,
        Left,
        Right
    };

    public class WoodenFence : GameObject
    {
        Graphics.Animation animation;
        public WoodFencePiece Piece { get; set; }

        public WoodenFence(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            Piece = WoodFencePiece.Center;
        }

        public override void LoadContent(ContentManager Content)
        {
            if(Piece == WoodFencePiece.Center)
            {
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\WoodFenceCenter"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 0);
            }
            else if(Piece == WoodFencePiece.Left)
            {
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\WoodFenceLeft"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 0);
            }
            else if(Piece == WoodFencePiece.Right)
            {
                animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\WoodFenceRight"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 0);
            }

            animation.Depth = 0.15f;
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

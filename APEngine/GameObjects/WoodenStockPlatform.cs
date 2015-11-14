using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.GameObjects
{
    public class WoodenStockPlatform : GameObject
    {
        Graphics.Animation woodenTop;
        Graphics.Animation woodenBottom;

        Graphics.Animation woodenBG;

        public bool HasBackground { get; set; }

        public WoodenStockPlatform(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y + 16, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            HasBackground = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            Texture2D woodTop = Content.Load<Texture2D>("Data\\GFX\\WoodStocksTop");
            Texture2D woodBottom = Content.Load<Texture2D>("Data\\GFX\\WoodStocksGround");
            woodenTop = new Graphics.Animation(woodTop, new Vector2(Position.X, Position.Y - 30), 32, woodTop.Height, 0, 0);
            woodenBottom = new Graphics.Animation(woodBottom, new Vector2(Position.X, Position.Y + 2), woodBottom.Width, woodBottom.Height, 0, 0);

            woodenTop.Visible = true;
            woodenBottom.Visible = true;

            woodenBottom.Depth = 0.73f;
            woodenTop.Depth = 0.331f;

            if(HasBackground)
            {
                Texture2D woodBG = Content.Load<Texture2D>("Data\\GFX\\WoodStocksBackground");
                woodenBG = new Graphics.Animation(woodBG, new Vector2(Position.X - 8, Position.Y), woodBG.Width, woodBG.Height, 0, 0);
                woodenBG.Visible = true;
                woodenBG.Depth = 0.1f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            woodenBottom.Update(gameTime);
            woodenTop.Update(gameTime);

            if(HasBackground)
            {
                woodenBG.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            woodenBottom.Draw(spriteBatch);
            woodenTop.Draw(spriteBatch);

            if(HasBackground)
            {
                woodenBG.Draw(spriteBatch);
            }
        }
    }
}


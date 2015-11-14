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
    public class WoodenStockBackground : GameObject
    {
        List<Graphics.Animation> backgrounds = new List<Graphics.Animation>();

        public WoodenStockBackground(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            DrawAtAllTimes = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            Texture2D woodBG = Content.Load<Texture2D>("Data\\GFX\\WoodStocksBackground");

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            int difference = (int)(gs.level.Height * 32 - Position.Y);
            float amount = difference / 32;

            amount++;

            for(int i = 0; i < amount; i++)
            {
                Graphics.Animation woodenBG;
                woodenBG = new Graphics.Animation(woodBG, new Vector2(Position.X - 8, Position.Y + i * 64 + 32), woodBG.Width, woodBG.Height, 0, 0);
                woodenBG.Visible = true;
                woodenBG.Depth = 0.1f;

                backgrounds.Add(woodenBG);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach(var b in backgrounds)
            {
                b.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach(var b in backgrounds)
            {
                b.Draw(spriteBatch);
            }
        }
    }
}


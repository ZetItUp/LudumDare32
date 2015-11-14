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
    public class ForestHouse : GameObject
    {
        Graphics.Animation house;

        public ForestHouse(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, 128, 128);
            DrawAtAllTimes = true;
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            Texture2D houseTexture = Content.Load<Texture2D>("Data\\GFX\\Forest\\ForestHouse");

            house = new Graphics.Animation(houseTexture, new Vector2(Position.X - 48, Position.Y), houseTexture.Width, houseTexture.Height, 0, 0);
            house.Visible = true;
            house.Depth = 0.35f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            house.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            house.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}


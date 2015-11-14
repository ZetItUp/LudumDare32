using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Map
{
    public enum CloudType
    {
        Bright = 0,
        Dark
    };

    public class Cloud
    {
        private Texture2D texture;
        public CloudType Type { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        public Cloud(Vector2 pos, CloudType type)
        {
            Type = type;
            Position = pos;
            Speed = (float)Engine.Randomizer.Next(1, 10) / 1000;
        }

        public void LoadContent(ContentManager content)
        {
            if (Type == CloudType.Bright)
                texture = content.Load<Texture2D>("Data\\GFX\\BrightCloud");
            else
                texture = content.Load<Texture2D>("Data\\GFX\\DarkCloud");
        }

        public void Update(GameTime gameTime)
        {
            Position += new Vector2(-Speed, 0f) * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Position.X < -texture.Width)
                Position = new Vector2(Engine.Monitor.VirtualWidth, Position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
    }
}

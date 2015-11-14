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
    public class CheckPointText : GameObject
    {
        Graphics.Animation animation;
        Vector2 originalPos;
        bool goUp;
        float velocity = 0.01f;

        ATimer durTimer;

        public CheckPointText(Vector2 position)
            : base(position)
        {
            goUp = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            durTimer = new ATimer();
            durTimer.Interval = 3000;
            durTimer.TimerType = ATimerType.MilliSeconds;

            Texture2D texture = Content.Load<Texture2D>("Data\\GFX\\CheckPointText");
            Position = new Vector2(Position.X + 16 - (texture.Width / 2), Position.Y - 32);
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(Position.X < 0 || Position.X + texture.Width > gs.level.Width)
            {
                float x = Position.X;
                float y = Position.Y;

                x = MathHelper.Clamp(x, 0, gs.level.Width * Map.Tile.WIDTH - texture.Width);
                y = MathHelper.Clamp(y, 0, gs.level.Height * Map.Tile.HEIGHT - texture.Height);

                Position = new Vector2(x, y);
            }

            originalPos = Position;
            animation = new Graphics.Animation(texture, Position, texture.Width, texture.Height, 0, 0);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            animation.Depth = 1.0f;
        }

        public override void Update(GameTime gameTime)
        {
            durTimer.Update(gameTime);

            if(durTimer.DidTick)
            {
                ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
                gs.RemoveGameObject(this);
            }

            if (goUp)
            {
                Position += new Vector2(0, -velocity * (float)gameTime.ElapsedGameTime.Milliseconds);

                if (Position.Y < originalPos.Y - 3f)
                    goUp = false;
            }
            else
            {
                Position += new Vector2(0, velocity * (float)gameTime.ElapsedGameTime.Milliseconds);

                if (Position.Y > originalPos.Y + 3f)
                    goUp = true;
            }

            animation.Position = Position;
            animation.Update(gameTime);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

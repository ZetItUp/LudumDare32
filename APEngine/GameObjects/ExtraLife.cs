using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class ExtraLife : GameObject
    {
        Graphics.Animation animation;
        Vector2 originalPos;
        bool goUp;
        float velocity = 0.0075f;

        public ExtraLife(Vector2 position)
            : base(position)
        {
            originalPos = position;
            goUp = false;
        }

        public override void LoadContent(ContentManager Content)
        {
            Texture2D texture = Content.Load<Texture2D>("Data\\GFX\\LifeIcon");
            Position = new Vector2(Position.X + 16 - (texture.Width / 2), Position.Y);
            animation = new Graphics.Animation(texture, Position, texture.Width, texture.Height, 0, 1);
            Rect = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            Depth = 0.60f;
        }

        public override object Clone()
        {
            originalPos = base.Position;

            return base.Clone();
        }

        public override void Update(GameTime gameTime)
        {
            animation.Update(gameTime);

            if (Animate)
            {
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
            }

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            if(gs.Player.CollisionRectangle.Intersects(Rect))
            {
                gs.Player.Lives++;
                gs.RemoveGameObject(this);
                gs.lifeCountTimer.Show();

                sfxInstance = SoundManager.SoundManager.LifePickUp.CreateInstance();
                gs.SoundManager.PlaySoundEffect(sfxInstance);
            }

            animation.Position = Position;
            animation.Depth = Depth;
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}

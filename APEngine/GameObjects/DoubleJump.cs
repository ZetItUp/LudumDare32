using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public class DoubleJump : GameObject
    {
        Graphics.Animation animation;

        public DoubleJump(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public override void LoadContent(ContentManager Content)
        {
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\DoubleJumpIcon"), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 1);
            animation.Depth = 0.53f;
        }

        public override void Update(GameTime gameTime)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);

            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            if(gs.Player.CollisionRectangle.Intersects(Rect))
            {
                gs.Player.Ability.HasDoubleJump = true;
                gs.RemoveGameObject(this);
                gs.ShowMessage("         Congratulations!\n \n  You have learned a new ability:\n \n \n         DOUBLE JUMP  \n \n \n \n   Press ACTION button to close.");

                sfxInstance = SoundManager.SoundManager.LifePickUp.CreateInstance();
                gs.SoundManager.PlaySoundEffect(sfxInstance);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

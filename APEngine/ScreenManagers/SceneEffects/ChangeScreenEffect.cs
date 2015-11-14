using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.ScreenManagers.SceneEffects
{
    public class ChangeScreenEffect : SceneEffect
    {
        public string Argument { get; set; }
        public string ScreenName { get; set; }

        public ChangeScreenEffect()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasPlayed || !PlayThis)
                return;

            if(ScreenName == "GameScreen")
            {
                GameScreen screen = new GameScreen();
                screen.LoadContent(Engine.ContentManager, Argument);

                Engine.ScreenManager.SetScreen(screen);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

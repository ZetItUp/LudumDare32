using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.ScreenManagers.SceneEffects
{
    public class HideAllEffect : SceneEffect
    {
        public HideAllEffect()
            : base()
        {

        }

        public override void LoadContent(ContentManager Content)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasPlayed || !PlayThis)
                return;

            List<SceneObjects.SceneObject> objects = SceneScreen.GetSceneObjects();
            foreach(SceneObjects.SceneObject so in objects)
            {
                so.Visible = false;
            }

            HasPlayed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}

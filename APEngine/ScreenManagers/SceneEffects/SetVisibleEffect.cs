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
    public class SetVisibleEffect : SceneEffect
    {
        public int ObjectID { get; set; }
        public int Visibility { get; set; }

        public SetVisibleEffect()
            : base()
        {
            ObjectID = -1;
        }

        public override void LoadContent(ContentManager Content)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (HasPlayed || !PlayThis)
                return;

            SceneScreen.GetSceneObject(ObjectID).Visible = ATools.IntToBool(Visibility);
            HasPlayed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}

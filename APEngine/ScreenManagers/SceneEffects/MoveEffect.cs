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
    public class MoveEffect : SceneEffect
    {
        public Vector2 Destination { get; set; }
        public float Speed { get; set; }
        public int ObjectID { get; set; }

        Vector2 dir = Vector2.Zero;

        public MoveEffect()
            : base()
        {
            ObjectID = -1;
            Speed = 1f;
            Destination = Vector2.Zero;
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

            SceneObjects.SceneObject obj =  SceneScreen.GetSceneObject(ObjectID);

            SceneScreen.MoveObject(ObjectID, Speed, Destination);
            HasPlayed = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

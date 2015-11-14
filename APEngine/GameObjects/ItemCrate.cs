using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.GameObjects
{
    public enum ItemCrateType
    {
        Standard = 0,
        Life,
        CheckPoint,
        Trash
    }

    public class ItemCrate : GameObject
    {
        public static Texture2D TrashTexture;
        Graphics.Animation animation;
        public ItemCrateType Type { get; set; }

        public ItemCrate(Vector2 position)
            : base(position)
        {
            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public void BreakBox()
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(gs.EditorMode == true)
                return;

            if(Type == ItemCrateType.Life)
            {
                ExtraLife life = new ExtraLife(this.Position);

                gs.AddGameObject(life);
            }
            else if(Type == ItemCrateType.CheckPoint)
            {
                CheckPointText text = new CheckPointText(this.Position);
                gs.level.PlayerSpawn = new Vector2(Position.X, Position.Y - 16);
                gs.level.CheckpointReached = true;

                gs.AddGameObject(text);
            }

            gs.brokenBoxes.Add(Position);

            Type = ItemCrateType.Trash;

            sfxInstance = SoundManager.SoundManager.ItemBoxBreak.CreateInstance();
            gs.SoundManager.PlaySoundEffect(sfxInstance);
        }

        public override void LoadContent(ContentManager Content)
        {
            string val = "";
            if (Type == ItemCrateType.Standard)
                val = "Box01";
            else if (Type == ItemCrateType.Life)
                val = "Box02";
            else if (Type == ItemCrateType.CheckPoint)
                val = "Box03";
            else if (Type == ItemCrateType.Trash)
                val = "BoxTrash";

            TrashTexture = Content.Load<Texture2D>("Data\\GFX\\BoxTrash");
            animation = new Graphics.Animation(Content.Load<Texture2D>("Data\\GFX\\" + val), Position, DEFAULT_WIDTH, DEFAULT_HEIGHT, 0, 0);
            animation.Depth = 0.43f;

            // TODO: If a crate has been destroyed before reaching a checkpoint,
            // do not spawn it again, but spawn a broken crate.
        }

        public override void Update(GameTime gameTime)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;
            if(!gs.level.GetTileAt(new Vector2(Position.X, Position.Y + Rect.Height)).Solid)
            {
                Position += new Vector2(Position.X, 0.001f * (float)gameTime.ElapsedGameTime.Milliseconds);
            }

            Rect = new Rectangle((int)Position.X, (int)Position.Y, DEFAULT_WIDTH, DEFAULT_HEIGHT);
            animation.Update(gameTime);

            if (Type == ItemCrateType.Trash)
                animation.Texture = TrashTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}

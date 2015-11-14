using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace APEngine.Map
{
    public enum FenceStyle
    {
        Door,
        Short,
        LeftEdge,
        RightEdge
    };

    public class Fence
    {
        public FenceStyle Style { get; set; }
        public Vector2 Position { get; set; }
        public float Depth { get; set; }
        private Texture2D texture;

        public Fence(FenceStyle style)
        {
            Style = style;
            Depth = 0.5f;
            LoadContent(ScreenManagers.ScreenManager.Content);
        }

        public void LoadContent(ContentManager Content)
        {
            string strStyle = "";
            if(Style == FenceStyle.Door)
            {
                strStyle = "Fence02";
            }
            else if(Style == FenceStyle.Short)
            {
                strStyle = "Fence01";
            }
            else if(Style == FenceStyle.LeftEdge)
            {
                strStyle = "Fence03";
            }
            else if(Style == FenceStyle.RightEdge)
            {
                strStyle = "Fence04";
            }

            texture = Content.Load<Texture2D>("Data\\GFX\\" + strStyle);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, Depth);
        }
    }
}

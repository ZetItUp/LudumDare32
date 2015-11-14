using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using AtriLib2;

namespace APEngine.Map
{
    public enum TileID
    {
        Empty = -1,
        None = 0,
        Platform,
        GrassPlatform,
        LeftGrassPlatform,
        RightGrassPlatform,
        Environment,
        WoodPlatform
    };

    public enum TileType
    {
         Empty = -1,
         DarkGround = 1000,
         DarkGroundBottom,
         DarkGroundBottomLeftCorner,
         DarkGroundBottomRightCorner,  
         DarkGroundGrass, 
         DarkGroundGrassEndLeft, 
         DarkGroundGrassEndRight,
         DarkGroundGrassLeft, 
         DarkGroundGrassRight, 
         DarkGroundLeftWall,
         DarkGroundRightWall, 
         DarkGroundRightWallBottomGrass,
         DarkGroundRightWallTopBottomGrass,
         DarkGroundRightWallTopGrass
    };

    public class Tile : ICloneable
    {
        public static Dictionary<string, Tile> Tiles { get; private set; }
        public static Tile GetTile(string tileName)
        {
            return Tiles[tileName].Clone() as Tile;
        }

        public float Depth = 0.6f;
        public static int WIDTH = 32;
        public static int HEIGHT = 32;
        public Texture2D Texture;

        public Vector2 Position { get; set; }
        public bool Solid { get; set; }
        public bool AllowedToDrawDebug { get; set; }
        public TileID ID { get; set; }
        public TileType TileType { get; set; }

        public String GetTileData
        {
            get
            {
                return "[T=\"TILE\", TileType=\"" + (int)TileType + 
                    "\", Solid=\"" + Convert.ToInt32(Solid) + "\", XPos=\"" + (int)Position.X + "\", YPos=\"" + 
                    (int)Position.Y + "\"]";
            }
        }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Tile.WIDTH, Tile.HEIGHT);
            }
        }

        public Tile()
        {
            Solid = true;
            TileType = Map.TileType.DarkGround;
            AllowedToDrawDebug = true;
        }

        public Tile(bool allowToDrawDebug)
        {
            Solid = true;
            TileType = Map.TileType.DarkGround;
            AllowedToDrawDebug = allowToDrawDebug;
        }
        
        static Tile()
        {
            Tiles = new Dictionary<string, Tile>();

            var t = new Tile(false);
            t.ID = TileID.None;
            t.Texture = null;
            t.TileType = TileType.Empty;
            t.Solid = false;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.ID = TileID.Platform;
            t.Texture = Graphics.Textures.DarkGround;
            t.TileType = TileType.DarkGround;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundBottom;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundBottom;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundBottomLeftCorner;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundBottomLeftCorner;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundBottomRightCorner;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundBottomRightCorner;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundGrass;
            t.ID = TileID.GrassPlatform;
            t.TileType = TileType.DarkGroundGrass;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundGrassEndLeft;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundGrassEndLeft;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundGrassEndRight;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundGrassEndRight;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundGrassLeft;
            t.ID = TileID.LeftGrassPlatform;
            t.TileType = TileType.DarkGroundGrassLeft;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundGrassRight;
            t.ID = TileID.RightGrassPlatform;
            t.TileType = TileType.DarkGroundGrassRight;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundLeftWall;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundLeftWall;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundRightWall;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundRightWall;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundRightWallBottomGrass;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundRightWallBottomGrass;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundRightWallTopBottomGrass;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundRightWallTopBottomGrass;
            Tiles.Add(t.TileType.ToString(), t);

            t = new Tile(false);
            t.Texture = Graphics.Textures.DarkGroundRightWallTopGrass;
            t.ID = TileID.Platform;
            t.TileType = TileType.DarkGroundRightWallTopGrass;
            Tiles.Add(t.TileType.ToString(), t);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            ScreenManagers.GameScreen gs = (ScreenManagers.GameScreen)ScreenManagers.ScreenManager.CurrentScreen;

            if(Texture != null)
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, Depth);
            else if (AllowedToDrawDebug)
            {
                
                if (gs.EditorMode)
                {
                    if (this.ID == TileID.None)
                    {
                        Engine.AtriceGraphics.DrawBorder(spriteBatch, 1, new Rectangle((int)Position.X, (int)Position.Y, WIDTH, HEIGHT), new Color(0, 0, 0, 120));
                    }
                    else
                    {
                        Engine.AtriceGraphics.DrawBorder(spriteBatch, 1, new Rectangle((int)Position.X, (int)Position.Y, WIDTH, HEIGHT), Color.Red);
                    }
                }
            }
        }
    }
}

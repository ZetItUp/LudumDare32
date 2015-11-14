using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace APEngine.Graphics
{
    public static class Textures
    {
        private static bool Initialized { get; set; }
        public static Texture2D DarkGround { get; private set; }
        public static Texture2D DarkGroundBottom { get; private set; }
        public static Texture2D DarkGroundBottomLeftCorner { get; private set; }
        public static Texture2D DarkGroundBottomRightCorner { get; private set; }
        public static Texture2D DarkGroundGrass { get; private set; }
        public static Texture2D DarkGroundGrassEndLeft { get; private set; }
        public static Texture2D DarkGroundGrassEndRight { get; private set; }
        public static Texture2D DarkGroundGrassLeft { get; private set; }
        public static Texture2D DarkGroundGrassRight { get; private set; }
        public static Texture2D DarkGroundLeftWall { get; private set; }
        public static Texture2D DarkGroundRightWall { get; private set; }
        public static Texture2D DarkGroundRightWallBottomGrass { get; private set; }
        public static Texture2D DarkGroundRightWallTopBottomGrass { get; private set; }
        public static Texture2D DarkGroundRightWallTopGrass { get; private set; }
        public static Texture2D FlameBowlBottom { get; private set; }
        public static Texture2D FlameBowlFireBurst { get; private set; }
        public static Texture2D FlameBowlFireIdle { get; private set; }
        public static Texture2D FlameBowlTop { get; private set; }
        public static Texture2D ForestHouse { get; private set; }

        public static void LoadContent(ContentManager Content)
        {
            if (Initialized)
                return;

            DarkGround = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGround");
            DarkGroundBottom = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundBottom");
            DarkGroundBottomLeftCorner = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundBottomLeftCorner");
            DarkGroundBottomRightCorner = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundBottomRightCorner");
            DarkGroundGrass = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundGrass");
            DarkGroundGrassEndLeft = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundGrassEndLeft");
            DarkGroundGrassEndRight = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundGrassEndRight");
            DarkGroundGrassLeft = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundGrassLeft");
            DarkGroundGrassRight = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundGrassRight");
            DarkGroundLeftWall = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundLeftWall");
            DarkGroundRightWall = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundRightWall");
            DarkGroundRightWallBottomGrass = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundRightWallBottomGrass");
            DarkGroundRightWallTopBottomGrass = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundRightWallTopBottomGrass");
            DarkGroundRightWallTopGrass = Content.Load<Texture2D>("Data\\GFX\\Forest\\DarkGroundRightWallTopGrass");
            FlameBowlBottom = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlBottom");
            FlameBowlFireBurst = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlFireBurst");
            FlameBowlFireIdle = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlFireIdle");
            FlameBowlTop = Content.Load<Texture2D>("Data\\GFX\\Forest\\FlameBowlTop");
            ForestHouse = Content.Load<Texture2D>("Data\\GFX\\Forest\\ForestHouse");

            Initialized = true;
        }
    }
}

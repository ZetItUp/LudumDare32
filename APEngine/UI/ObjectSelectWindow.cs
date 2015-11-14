using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using AtriLib2;
using gObject = APEngine.GameObjects.GameObject;

namespace APEngine.UI
{
    public class ObjectSelectWindow : Window
    {
        private UI.Panel wndPanel;

        public ObjectSelectWindow(Rectangle windowRectangle)
            : base(windowRectangle)
        {
            wndPanel = new Panel(new Rectangle(windowRectangle.X + 4, windowRectangle.Y + 4, windowRectangle.Width - 8, windowRectangle.Height - 8));
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            wndPanel.LoadContent(Content);
            wndPanel.DrawWindow = false;
        }

        public void FillWithObjects(Map.Theme Theme)
        {
            if(Theme == Map.Theme.JungleForest)
            {
                TileButton tButtonDarkGround = new TileButton(new Rectangle(15, 20, 32, 32));
                tButtonDarkGround.Tile = Map.Tile.Tiles["DarkGround"];
                tButtonDarkGround.Tile.Position = tButtonDarkGround.Position;
                tButtonDarkGround.Parent = wndPanel;
                tButtonDarkGround.LoadContent(Engine.ContentManager);
                tButtonDarkGround.Clicked += tButtonDarkGround_Clicked;
                wndPanel.AddWindow("btnDarkGround", tButtonDarkGround);

                TileButton tButtonDarkGroundBottom = new TileButton(new Rectangle(15 + 32 * 1 + 10, 20, 32, 32));
                tButtonDarkGroundBottom.Tile = Map.Tile.Tiles["DarkGroundBottom"];
                tButtonDarkGroundBottom.Parent = wndPanel;
                tButtonDarkGroundBottom.LoadContent(Engine.ContentManager);
                tButtonDarkGroundBottom.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundBottom", tButtonDarkGroundBottom);

                TileButton tButtonDarkGroundBottomLeftCorner = new TileButton(new Rectangle(15 + 32 * 2 + 10 * 2, 20, 32, 32));
                tButtonDarkGroundBottomLeftCorner.Tile = Map.Tile.Tiles["DarkGroundBottomLeftCorner"];
                tButtonDarkGroundBottomLeftCorner.Parent = wndPanel;
                tButtonDarkGroundBottomLeftCorner.LoadContent(Engine.ContentManager);
                tButtonDarkGroundBottomLeftCorner.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundBottomLeftCorner", tButtonDarkGroundBottomLeftCorner);

                TileButton tButtonDarkGroundBottomRightCorner = new TileButton(new Rectangle(15 + 32 * 3 + 10 * 3, 20, 32, 32));
                tButtonDarkGroundBottomRightCorner.Tile = Map.Tile.Tiles["DarkGroundBottomRightCorner"];
                tButtonDarkGroundBottomRightCorner.Parent = wndPanel;
                tButtonDarkGroundBottomRightCorner.LoadContent(Engine.ContentManager);
                tButtonDarkGroundBottomRightCorner.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundBottomRightCorner", tButtonDarkGroundBottomRightCorner);

                TileButton tButtonDarkGroundGrass = new TileButton(new Rectangle(15 + 32 * 4 + 10 * 4, 20, 32, 32));
                tButtonDarkGroundGrass.Tile = Map.Tile.Tiles["DarkGroundGrass"];
                tButtonDarkGroundGrass.Parent = wndPanel;
                tButtonDarkGroundGrass.LoadContent(Engine.ContentManager);
                tButtonDarkGroundGrass.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundGrass", tButtonDarkGroundGrass);

                TileButton tButtonDarkGroundGrassEndLeft = new TileButton(new Rectangle(15 + 32 * 5 + 10 * 5, 20, 32, 32));
                tButtonDarkGroundGrassEndLeft.Tile = Map.Tile.Tiles["DarkGroundGrassEndLeft"];
                tButtonDarkGroundGrassEndLeft.Parent = wndPanel;
                tButtonDarkGroundGrassEndLeft.LoadContent(Engine.ContentManager);
                tButtonDarkGroundGrassEndLeft.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundGrassEndLeft", tButtonDarkGroundGrassEndLeft);

                TileButton tButtonDarkGroundGrassEndRight = new TileButton(new Rectangle(15 + 32 * 6 + 10 * 6, 20, 32, 32));
                tButtonDarkGroundGrassEndRight.Tile = Map.Tile.Tiles["DarkGroundGrassEndRight"];
                tButtonDarkGroundGrassEndRight.Parent = wndPanel;
                tButtonDarkGroundGrassEndRight.LoadContent(Engine.ContentManager);
                tButtonDarkGroundGrassEndRight.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundGrassEndRight", tButtonDarkGroundGrassEndRight);

                TileButton tButtonDarkGroundGrassLeft = new TileButton(new Rectangle(15 + 32 * 7 + 10 * 7, 20, 32, 32));
                tButtonDarkGroundGrassLeft.Tile = Map.Tile.Tiles["DarkGroundGrassLeft"];
                tButtonDarkGroundGrassLeft.Parent = wndPanel;
                tButtonDarkGroundGrassLeft.LoadContent(Engine.ContentManager);
                tButtonDarkGroundGrassLeft.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundGrassLeft", tButtonDarkGroundGrassLeft);

                TileButton tButtonDarkGroundGrassRight = new TileButton(new Rectangle(15 + 32 * 8 + 10 * 8, 20, 32, 32));
                tButtonDarkGroundGrassRight.Tile = Map.Tile.Tiles["DarkGroundGrassRight"];
                tButtonDarkGroundGrassRight.Parent = wndPanel;
                tButtonDarkGroundGrassRight.LoadContent(Engine.ContentManager);
                tButtonDarkGroundGrassRight.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundGrassRight", tButtonDarkGroundGrassRight);

                TileButton tButtonDarkGroundLeftWall = new TileButton(new Rectangle(15 + 32 * 9 + 10 * 9, 20, 32, 32));
                tButtonDarkGroundLeftWall.Tile = Map.Tile.Tiles["DarkGroundLeftWall"];
                tButtonDarkGroundLeftWall.Parent = wndPanel;
                tButtonDarkGroundLeftWall.LoadContent(Engine.ContentManager);
                tButtonDarkGroundLeftWall.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundLeftWall", tButtonDarkGroundLeftWall);

                TileButton tButtonDarkGroundRightWall = new TileButton(new Rectangle(15 + 32 * 10 + 10 * 10, 20, 32, 32));
                tButtonDarkGroundRightWall.Tile = Map.Tile.Tiles["DarkGroundRightWall"];
                tButtonDarkGroundRightWall.Parent = wndPanel;
                tButtonDarkGroundRightWall.LoadContent(Engine.ContentManager);
                tButtonDarkGroundRightWall.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundRightWall", tButtonDarkGroundRightWall);

                TileButton tButtonDarkGroundRightWallBottomGrass = new TileButton(new Rectangle(15, 20 + 32 * 1 + 10 * 1, 32, 32));
                tButtonDarkGroundRightWallBottomGrass.Tile = Map.Tile.Tiles["DarkGroundRightWallBottomGrass"];
                tButtonDarkGroundRightWallBottomGrass.Parent = wndPanel;
                tButtonDarkGroundRightWallBottomGrass.LoadContent(Engine.ContentManager);
                tButtonDarkGroundRightWallBottomGrass.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundRightWallBottomGrass", tButtonDarkGroundRightWallBottomGrass);

                TileButton tButtonDarkGroundRightWallTopBottomGrass = new TileButton(new Rectangle(15 + 32 * 1 + 10 * 1, 20 + 32 * 1 + 10 * 1, 32, 32));
                tButtonDarkGroundRightWallTopBottomGrass.Tile = Map.Tile.Tiles["DarkGroundRightWallTopBottomGrass"];
                tButtonDarkGroundRightWallTopBottomGrass.Parent = wndPanel;
                tButtonDarkGroundRightWallTopBottomGrass.LoadContent(Engine.ContentManager);
                tButtonDarkGroundRightWallTopBottomGrass.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundRightWallTopBottomGrass", tButtonDarkGroundRightWallTopBottomGrass);

                TileButton tButtonDarkGroundRightWallTopGrass = new TileButton(new Rectangle(15 + 32 * 2 + 10 * 2, 20 + 32 * 1 + 10 * 1, 32, 32));
                tButtonDarkGroundRightWallTopGrass.Tile = Map.Tile.Tiles["DarkGroundRightWallTopGrass"];
                tButtonDarkGroundRightWallTopGrass.Parent = wndPanel;
                tButtonDarkGroundRightWallTopGrass.LoadContent(Engine.ContentManager);
                tButtonDarkGroundRightWallTopGrass.Clicked += tButtonDarkGroundBottom_Clicked;
                wndPanel.AddWindow("btnDarkGroundRightWallTopGrass", tButtonDarkGroundRightWallTopGrass);

                GameObjectButton gButtonStar = new GameObjectButton(new Rectangle(15 + 32 * 3 + 10 * 3, 20 + 32 * 1 + 10 * 1, 32, 32));
                var gobStar = gObject.GetNewGameObject<GameObjects.Star>(gButtonStar.Position);

                gButtonStar.GameObject = gobStar;
                gButtonStar.Parent = wndPanel;
                gButtonStar.LoadContent(Engine.ContentManager);
                gButtonStar.Clicked += gButtonStar_Clicked;
                wndPanel.AddWindow("gButtonStar", gButtonStar);

                GameObjectButton gButtonExtraLife = new GameObjectButton(new Rectangle(15 + 32 * 4 + 10 * 4, 20 + 32 * 1 + 10 * 1, 32, 32));
                var gobLife = gObject.GetNewGameObject<GameObjects.ExtraLife>(gButtonExtraLife.Position);

                gButtonExtraLife.GameObject = gobLife;
                gButtonExtraLife.Parent = wndPanel;
                gButtonExtraLife.LoadContent(Engine.ContentManager);
                gButtonExtraLife.Clicked += gButtonStar_Clicked;
                wndPanel.AddWindow("gButtonExtraLife", gButtonExtraLife);
            }
        }

        void gButtonStar_Clicked(object sender, EventArgs e)
        {
            ApplyObject(sender);
            RemoveMe();
        }

        void tButtonDarkGroundBottom_Clicked(object sender, EventArgs e)
        {
            ApplyTile(sender);
            RemoveMe();
        }

        void tButtonDarkGround_Clicked(object sender, EventArgs e)
        {
            ApplyTile(sender);
            RemoveMe();
        }

        private void ApplyObject(object sender)
        {
            var obj = sender as GameObjectButton;

            var selectedObject = ScreenManagers.ScreenManager.GameScreen.EditUI.SelectedGameObject;
            selectedObject.GameObject = obj.GameObject;
            ScreenManagers.ScreenManager.GameScreen.EditUI.MousePointer.MouseObject = Map.Editor.MouseObject.GameObject;
        }

        private void ApplyTile(object sender)
        {
            var tb = sender as TileButton;

            var selectTile = ScreenManagers.ScreenManager.GameScreen.EditUI.SelectedTile;
            selectTile.Tile.ID = tb.Tile.ID;
            selectTile.Tile.Texture = tb.Tile.Texture;
            selectTile.Tile.TileType = tb.Tile.TileType;

            ScreenManagers.ScreenManager.GameScreen.EditUI.MousePointer.MouseObject = Map.Editor.MouseObject.Tile;
        }

        private void RemoveMe()
        {
            ScreenManagers.ScreenManager.GameScreen.Player.AllowMovement = true;
            ScreenManagers.ScreenManager.GameScreen.EditUI.MousePointer.AllowWorldModification = true;
            DestroyMe = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (AInput.KeyPressed(GameSettings.TEXTURE_SELECT_KEY) && AllowClose)
            {
                RemoveMe();
            }

            if(AInput.KeyReleased(GameSettings.TEXTURE_SELECT_KEY))
            {
                AllowClose = true;
            }

            wndPanel.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (!Visible)
                return;

            wndPanel.Draw(spriteBatch);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using AtriLib2;

namespace APEngine.ScreenManagers
{
    public class MainMenu : Screen
    {
        Texture2D menuBackground;
        Texture2D menuStart;
        Texture2D menuStartSelect;
        Texture2D menuLoad;
        Texture2D menuLoadSelect;
        Texture2D menuExit;
        Texture2D menuExitSelect;
        Texture2D menuOptions;
        Texture2D menuOptionsSelect;
        Texture2D logo;
        Texture2D logo2;

        int selected = 0;
        int menuSelectHeight = 0;
        Vector2 MenuPos = Vector2.Zero;

        SoundEffectInstance sfxInstance;
        SoundManager.SoundManager soundManager;

        float scale = 1f;
        float currentTick = 0f;
        float interval = 10f;
        bool incr = true;

        float rotation = 0f;
        bool incRot = true;

        public MainMenu()
            : base()
        {
            if(SoundManager.MusicManager.Instance == null)
                SoundManager.MusicManager.Play(SoundManager.MusicTrack.MenuThemeSong, true);
        }

        public override void LoadContent(ContentManager Content)
        {
            if (SoundManager.MusicManager.Instance != null)
            {
                if (SoundManager.MusicManager.Instance.State == SoundState.Playing && SoundManager.MusicManager.Track != SoundManager.MusicTrack.MenuThemeSong)
                {
                    SoundManager.MusicManager.Stop();
                    SoundManager.MusicManager.Play(SoundManager.MusicTrack.MenuThemeSong, true);
                }
            }
            else
            {
                SoundManager.MusicManager.Play(SoundManager.MusicTrack.MenuThemeSong, true);
            }

            base.LoadContent(Content);

            menuBackground = Content.Load<Texture2D>("Data\\GFX\\MainMenuBackground");
            menuStart = Content.Load<Texture2D>("Data\\GFX\\MenuStartGame");
            menuStartSelect = Content.Load<Texture2D>("Data\\GFX\\MenuStartGameSelected");
            menuExit = Content.Load<Texture2D>("Data\\GFX\\MenuExitGame");
            menuExitSelect = Content.Load<Texture2D>("Data\\GFX\\MenuExitGameSelected");
            menuLoad = Content.Load<Texture2D>("Data\\GFX\\MenuLoadGame");
            menuLoadSelect = Content.Load<Texture2D>("Data\\GFX\\MenuLoadGameSelected");
            menuOptions = Content.Load<Texture2D>("Data\\GFX\\MenuOptions");
            menuOptionsSelect = Content.Load<Texture2D>("Data\\GFX\\MenuOptionsSelected");
            logo = Content.Load<Texture2D>("Data\\GFX\\Logo");
            logo2 = Content.Load<Texture2D>("Data\\GFX\\Logo2");

            menuSelectHeight = menuStart.Height;
            SoundManager.SoundManager.LoadContent(Content);
            soundManager = new SoundManager.SoundManager();
            soundManager.Volume = GameSettings.SFXVol;

            MenuPos = new Vector2((Engine.Monitor.VirtualWidth - menuStart.Width), (Engine.Monitor.VirtualHeight - menuStart.Height) / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentTick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(currentTick >= interval)
            {
                if(incRot)
                {
                    rotation += 0.005f;

                    if (rotation >= 0.1f)
                        incRot = false;
                }
                else
                {
                    rotation -= 0.005f;

                    if(rotation <= -0.1f)
                    {
                        incRot = true;
                    }
                }

                if(incr)
                {
                    scale += 0.001f;

                    if (scale >= 1.1f)
                        incr = false;
                }
                else
                {
                    scale -= 0.001f;

                    if (scale <= 0.9f)
                        incr = true;
                }

                currentTick -= interval;
            }

            if (AInput.KeyPressed(Keys.Down) || AInput.ButtonPressed(Buttons.DPadDown, PlayerIndex.One))
            {
                sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                soundManager.PlaySoundEffect(sfxInstance);
                selected++;

                if (selected > 3)
                    selected = 0;
            }
            else if(AInput.KeyPressed(Keys.Up) || AInput.ButtonPressed(Buttons.DPadUp, PlayerIndex.One))
            {
                sfxInstance = SoundManager.SoundManager.MenuSelect01.CreateInstance();
                soundManager.PlaySoundEffect(sfxInstance);
                selected--;

                if (selected < 0)
                    selected = 3;
            }

            if(AInput.KeyPressed(Keys.Enter) || AInput.ButtonPressed(Buttons.A, PlayerIndex.One))
            {
                sfxInstance = SoundManager.SoundManager.MenuSelect02.CreateInstance();
                soundManager.PlaySoundEffect(sfxInstance);

                if(selected == 0)
                {
                    Engine.ScreenManager.SetScreen(new GameScreen());
                    //Game1.screenManager.SetScreen(new SceneScreen(), "Scene01");
                }
                else if(selected == 1)
                {
                    // Load game
                }
                else if(selected == 2)
                {
                    Engine.ScreenManager.SetScreen(new MenuOptions());
                }
                else
                {
                    Engine.DoExit = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Engine.Monitor.GetTransformationMatrix());
            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 800, 600), Color.White);

            int x = 100 + logo2.Width - (int)(logo2.Width * scale);
            int y = (600 / 2) - ((int)(logo.Height * scale) / 2);
            spriteBatch.Draw(logo2, new Rectangle(x, y, (int)(logo.Width * scale), (int)(logo.Height * scale)), null, Color.White, 
                rotation, 
                new Vector2(x - logo.Width / 2.5f, y - 170), SpriteEffects.None, 1f);

            spriteBatch.Draw(logo, new Rectangle(50, 300 - (int)((logo.Height * 1.5f) / 2), (int)(logo.Width * 1.5f), (int)(logo.Height * 1.5f)), Color.White);

            if (selected == 0)
            {
                spriteBatch.Draw(menuStartSelect, MenuPos, Color.White);
                spriteBatch.Draw(menuLoad, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight), Color.White);
                spriteBatch.Draw(menuOptions, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuExit, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 3), Color.White);
            }
            else if (selected == 1)
            {
                spriteBatch.Draw(menuStart, MenuPos, Color.White);
                spriteBatch.Draw(menuLoadSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight), Color.White);
                spriteBatch.Draw(menuOptions, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuExit, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 3), Color.White);
            }
            else if (selected == 2)
            {
                spriteBatch.Draw(menuStart, MenuPos, Color.White);
                spriteBatch.Draw(menuLoad, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight), Color.White);
                spriteBatch.Draw(menuOptionsSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuExit, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 3), Color.White);
            }
            else if (selected == 3)
            {
                spriteBatch.Draw(menuStart, MenuPos, Color.White);
                spriteBatch.Draw(menuLoad, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight), Color.White);
                spriteBatch.Draw(menuOptions, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 2), Color.White);
                spriteBatch.Draw(menuExitSelect, new Vector2(MenuPos.X, MenuPos.Y + menuSelectHeight * 3), Color.White);
            }

            spriteBatch.End();
        }
    }
}

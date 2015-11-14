/*
 * Alien Prisoner
 * by: Andreas Lindström 
 * Nicknames: CorpselolEU or ZetItUp
 * 
 * Copyright (C) 2015, Andreas Lindström
 * 
 * This was only created for the Ludum Dare 32 compo.
 */
#region Using Statements
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using AtriLib2;
using APEngine;
#endregion

namespace LudumDare32
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public static string WindowMessage = "Alien Prisoner";

        private static GameWindow staticWindow;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;    

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Components.Add(new AInput(this));
            Components.Add(new AMouse(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Engine.CurrentGame = this;

            AMonitor monitor;
            monitor = new AMonitor(ref graphics);
            monitor.SetVirtualResolution(800, 600);
            monitor.SetResolution(Resolutions.R800x600);
            Engine.Monitor = monitor;
            Engine.KeyboardDispatcher = new KeyboardDispatcher(Window);

            staticWindow = Window;

            // Try and load the settings, if failed, save the current settings to the file
            if(GameSettings.LoadSettings())
            {
                GameSettings.ApplySettings();
            }
            else
            {
                GameSettings.SaveSettings(0.5f, 0.5f);
            }

            AGraphics gfx = new AGraphics();
            gfx.InitializeGraphics(monitor.gfxDev);
            Engine.AtriceGraphics = gfx;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Engine.ContentManager = Content;
            APEngine.SoundManager.MusicManager.LoadContent(Content);
            APEngine.UI.WindowGraphics.LoadContent(Content);

            Engine.ScreenManager = new APEngine.ScreenManagers.ScreenManager();
            Engine.ScreenManager.LoadContent(Content);
            Engine.ScreenManager.SetScreen(new APEngine.ScreenManagers.MainMenu());

            Engine.LoadCursors();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Engine.ScreenManager.Update(gameTime);

            if(Window != null)
                Window.Title = WindowMessage;

            if (Engine.DoExit)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        public static void ExitGame()
        {
            Engine.CurrentGame.Exit();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Engine.Monitor.BeginDraw();

            Engine.ScreenManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}

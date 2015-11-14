using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AtriLib2;

namespace APEngine.ScreenManagers
{
    public class UITimer
    {
        ATimer timer;
        ATimer startTimer;
        public float Alpha { get; private set; }

        public UITimer(float interval)
        {
            timer = new ATimer();
            timer.Interval = 20;
            timer.TimerType = ATimerType.MilliSeconds;

            startTimer = new ATimer();
            startTimer.Interval = interval;
            startTimer.TimerType = ATimerType.MilliSeconds;

            Alpha = 0f;
        }

        public void Show()
        {
            Alpha = 1f;

            timer.Stop();
            startTimer.Start();
        }

        public void Update(GameTime gameTime)
        {
            timer.Update(gameTime);
            startTimer.Update(gameTime);

            if(startTimer.DidTick)
            {
                startTimer.Stop();
                timer.Start();
            }

            if(timer.DidTick)
            {
                Alpha -= 0.025f;
            }

            if(Alpha <= 0f)
            {
                timer.Stop();
            }
        }
    }

    public class GameScreen : Screen
    {
        Texture2D backgroundSky;
        Texture2D backgroundFront;
        List<Map.Cloud> clouds;

        public Camera camera { get; set; }
        public Map.Level level;

        public List<Vector2> brokenBoxes = new List<Vector2>();

        public Entities.Player Player
        {
            get
            {
                foreach(Entities.Entity e in entities)
                {
                    if (e is Entities.Player)
                        return (Entities.Player)e;
                }

                return null;
            }
        }

        public List<Entities.Projectile> projectiles;

        private List<GameObjects.GameObject> mapGameObjects;
        public List<GameObjects.GameObject> gameObjects;
        public List<GameObjects.Water> waterObjects;
        public List<Entities.Entity> entities;

        public Entities.Gossip.GossipManager GossipManager;
        public SoundManager.SoundManager SoundManager;
        Texture2D healthText;
        Texture2D healthBar;
        Texture2D paused;
        Texture2D Congrats;
        Texture2D lifeCounter;
        Texture2D ammoCounter;
        Texture2D starPowerCounter;
        Texture2D uiBackgroundTexture;
        bool congrats = false;
        bool silentPause = false;

        UI.GameFont textBox;
        UI.GameFont ammo;
        UI.GameFont lives;
        UI.GameFont starPowerFont;

        Vector2 lifePos;
        Vector2 ammoPos;
        Vector2 starPowerPos;
        Vector2 uiBackgroundPos;

        public UITimer lifeCountTimer;
        public UITimer ammoTimer;
        public UITimer starPowerTimer;
        public UITimer uiBackgroundTimer;

        UI.InfoBox powerUpsWindow;

        public const float UIScale = 2f;
        bool showWelcomeMsg = false;

        public bool EditorMode { get; set; }
        public Map.Editor.EditorUI EditUI { get; set; }

        public GameScreen()
            : base()
        {
            camera = new Camera(Engine.Monitor.Viewport);
        }

        public void NewMap(int width, int height)
        {

        }

        public bool SaveMap()
        {
            level.SaveMap();

            return true;
        }

        public void AddProjectile(Entities.Projectile projectile)
        {
            projectiles.Add(projectile);
        }

        public void RemoveAllBackgroundObjects()
        {
            for(int i = 0; i < gameObjects.Count; i++)
            {
                if(gameObjects[i] is GameObjects.BigFlower)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
                else if(gameObjects[i] is GameObjects.Flower)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
                else if(gameObjects[i] is GameObjects.Grass)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
                else if(gameObjects[i] is GameObjects.GrassWall)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
                else if(gameObjects[i] is GameObjects.Mushroom)
                {
                    gameObjects.RemoveAt(i);
                    i--;
                }
            }
        }

        public List<GameObjects.GameObject> GetAllNoneBackgroundObjects()
        {
            List<GameObjects.GameObject> retList = new List<GameObjects.GameObject>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i] is GameObjects.BigFlower)
                {
                    continue;
                }
                else if (gameObjects[i] is GameObjects.Flower)
                {
                    continue;
                }
                else if (gameObjects[i] is GameObjects.Grass)
                {
                    continue;
                }
                else if (gameObjects[i] is GameObjects.GrassWall)
                {
                    continue;
                }
                else if (gameObjects[i] is GameObjects.Mushroom)
                {
                    continue;
                }
                else
                {
                    retList.Add(gameObjects[i]);
                }
            }

            return retList;
        }

        public void AddEntity(Entities.Entity e)
        {
            e.LoadContent(ScreenManager.Content);
            entities.Add(e);
        }

        public void AddGameObject(GameObjects.GameObject gameObject)
        {
            if (CanPlaceGameObject(gameObject.Rect))
            {
                gameObject.LoadContent(ScreenManager.Content);
                gameObjects.Add(gameObject);
                mapGameObjects.Add(gameObject.Clone() as GameObjects.GameObject);
            }
            else
            {
                // Play area taken sound?
            }
        }

        public void AddWater(GameObjects.Water water)
        {
            water.LoadContent(ScreenManager.Content);
            waterObjects.Add(water);
        }

        public void RemoveGameObject(GameObjects.GameObject gameObject)
        {
            if(gameObjects.Contains(gameObject))
            {
                gameObjects.Remove(gameObject);
            }
        }

        public void RemoveAllGameObjects()
        {
            gameObjects.Clear();
            mapGameObjects.Clear();
            waterObjects.Clear();
        }

        public void FillMapGameObjects()
        {
            gameObjects.Clear();
            gameObjects = mapGameObjects.ToList();
        }

        public void SetMapGameObjects()
        {
            mapGameObjects.Clear();
            mapGameObjects = gameObjects.ToList();
        }

        public void RemoveAllEntities()
        {
            for(int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is Entities.Player)
                {
                    continue;
                }

                entities.RemoveAt(i);
                --i;
            }
        }

        public bool CanPlaceGameObject(Rectangle rectangleArea)
        {
            var list = GetAllNoneBackgroundObjects();

            foreach (var g in list)
            {
                if (g.Rect.Intersects(rectangleArea))
                    return false;
            }

            return true;
        }

        public void ShowMessage(String message)
        {
            powerUpsWindow.Text = message;
            powerUpsWindow.Visible = true;
        }

        public void FillClouds(int amount, Map.CloudType Type)
        {
            for(int i = 0; i < amount; i++)
            {
                Vector2 v = new Vector2(Engine.Randomizer.Next(0, Engine.Monitor.VirtualWidth), Engine.Randomizer.Next(50, 350));
                Map.Cloud c = new Map.Cloud(v, Type);
                c.LoadContent(ScreenManager.Content);

                clouds.Add(c);
            }
        }

        public override void LoadContent(ContentManager Content)
        {
            Graphics.Textures.LoadContent(Content);

            base.LoadContent(Content);
            Engine.Paused = false;
            APEngine.SoundManager.SoundManager.LoadContent(Content);
            SoundManager = new APEngine.SoundManager.SoundManager();
            SoundManager.Volume = GameSettings.SFXVol;
            GossipManager = new Entities.Gossip.GossipManager();
            EditorMode = false;

            paused = Content.Load<Texture2D>("Data\\GFX\\Paused");
            Congrats = Content.Load<Texture2D>("Data\\GFX\\Congratz");
            lifeCounter = Content.Load<Texture2D>("Data\\GFX\\LivesIcon");
            ammoCounter = Content.Load<Texture2D>("Data\\GFX\\AmmoIcon");
            starPowerCounter = Content.Load<Texture2D>("Data\\GFX\\StarPowerBar");

            level = new Map.Level();
            level.CheckpointReached = false;

            EditUI = new Map.Editor.EditorUI(UIScale);
            EditUI.LoadContent(Content);

            lifeCountTimer = new UITimer(6000f);
            ammoTimer = new UITimer(6000f);
            starPowerTimer = new UITimer(6000f);
            uiBackgroundTimer = new UITimer(6500f);
            uiBackgroundPos = new Vector2(0, 0);

            projectiles = new List<Entities.Projectile>();
            entities = new List<Entities.Entity>();
            gameObjects = new List<GameObjects.GameObject>();
            mapGameObjects = new List<GameObjects.GameObject>();
            waterObjects = new List<GameObjects.Water>();
            clouds = new List<Map.Cloud>();

            powerUpsWindow = new UI.InfoBox(new Rectangle((int)(Engine.Monitor.VirtualWidth / 2) - 200, (int)(Engine.Monitor.VirtualHeight / 2) - 100, 400, 200));
            powerUpsWindow.LoadContent(Content);
            powerUpsWindow.Visible = false;

            projectiles.Clear();
            entities.Clear();
            gameObjects.Clear();

            backgroundFront = Content.Load<Texture2D>("Data\\GFX\\Forest\\backgroundFront");
            backgroundSky = Content.Load<Texture2D>("Data\\GFX\\Forest\\backgroundSkyNight");
            uiBackgroundTexture = Content.Load<Texture2D>("Data\\GFX\\UIBackground");

            FillClouds(10, Map.CloudType.Dark);

            Entities.Player player = new Entities.Player(level.PlayerSpawn, 1);
            player.Ability.HasDoubleJump = true;
            AddEntity(player);

            camera.LevelHeight = level.Height * Map.Tile.HEIGHT;
            camera.LevelWidth = level.Width * Map.Tile.WIDTH;

            healthText = Content.Load<Texture2D>("Data\\GFX\\healthText");
            healthBar = Content.Load<Texture2D>("Data\\GFX\\healthBar");

            textBox = new UI.GameFont(new Rectangle(100, Engine.Monitor.Height - 20, 800, 20), 800f);
            textBox.LoadContent(Content);
            textBox.Text = "Twitch: www.twitch.tv/corpseloleu | Twitter: @corpsegrindr";

            lifePos = new Vector2(10, 10);
            ammoPos = new Vector2(10, 15 + lifeCounter.Height * UIScale);
            starPowerPos = new Vector2(Engine.Monitor.VirtualWidth - 80 * UIScale, 12);

            lives = new UI.GameFont(new Rectangle((int)(lifePos.X + lifeCounter.Width * UIScale), (int)(lifePos.Y + 12 * UIScale), 200, 18), 200);
            lives.LoadContent(Content);
            lives.Text = player.Lives.ToString();

            ammo = new UI.GameFont(new Rectangle((int)(ammoPos.X + ammoCounter.Width * UIScale), (int)(ammoPos.Y + 8 * UIScale), 200, 18), 200);
            ammo.LoadContent(Content);
            ammo.Text = player.Clip.ToString();

            starPowerFont = new UI.GameFont(new Rectangle((int)(starPowerPos.X + starPowerCounter.Width * UIScale), (int)(starPowerPos.Y + 10 * UIScale), 100, 18), 100);
            starPowerFont.LoadContent(Content);
            starPowerFont.Text = player.Lives.ToString();

            AddGameObject(new GameObjects.DoubleJump(new Vector2(level.PlayerSpawn.X + 32 * 25, level.PlayerSpawn.Y + 16 - (32 * 5))));

            //level.CreateNewLevel(Map.Theme.JungleForest, 40, 40, "Empty Level");
            level.LoadMap("Empty Level");
        }

        public override void Unload()
        {
            base.Unload();
        }

        public List<GameObjects.ItemCrate> GetItemBoxesWithinRange(Rectangle rectToCheck)
        {
            List<GameObjects.ItemCrate> retList = new List<GameObjects.ItemCrate>();

            foreach(var go in gameObjects)
            {
                if(go is GameObjects.ItemCrate)
                {
                    // Make sure we only get crates around the rectangle
                    if(go.Rect.X >= rectToCheck.X - rectToCheck.Width &&
                        go.Rect.X <= rectToCheck.X + rectToCheck.Width * 2 && 
                        go.Rect.Y >= rectToCheck.Y - rectToCheck.Height &&
                        go.Rect.Y <= rectToCheck.Y + rectToCheck.Height * 2)
                    {
                        retList.Add((GameObjects.ItemCrate)go);
                    }
                }
            }

            return retList;
        }

        public List<GameObjects.SpringBounce> GetSpringBouncers(Rectangle rectToCheck)
        {
            List<GameObjects.SpringBounce> retList = new List<GameObjects.SpringBounce>();

            foreach (var go in gameObjects)
            {
                if (go is GameObjects.SpringBounce)
                {
                    // Make sure we only get crates around the rectangle
                    if (go.Rect.X >= rectToCheck.X - rectToCheck.Width &&
                        go.Rect.X <= rectToCheck.X + rectToCheck.Width * 2 &&
                        go.Rect.Y >= rectToCheck.Y - rectToCheck.Height &&
                        go.Rect.Y <= rectToCheck.Y + rectToCheck.Height * 2)
                    {
                        retList.Add((GameObjects.SpringBounce)go);
                    }
                }
            }

            return retList;
        }

        public void RestartGame()
        {
            projectiles.Clear();
            entities.Clear();
            gameObjects.Clear();
            clouds.Clear();

            bool checkpoint = level.CheckpointReached;
            Vector2 spawnPos = level.PlayerSpawn;
            level = new Map.Level();
            level.CheckpointReached = checkpoint;
            level.PlayerSpawn = spawnPos;

            Entities.Player player = new Entities.Player(level.PlayerSpawn, 1);
            player.LoadContent(ScreenManager.Content);
            player.Health.Add(1);
            AddEntity(player);
        }

        public void ResetObjectsAndEntities(GameTime gameTime)
        {
            var gobjList = gameObjects.ToList();
            foreach(GameObjects.GameObject g in gobjList)
            {
                g.MoveToStart();
                g.Update(gameTime);
            }

            foreach(Entities.Entity e in entities)
            {
                if (e is Entities.Player)
                    continue;

                e.MoveToStart();
                e.Update(gameTime);
            }
        }

        public void SetEditorMode(bool mode)
        {
            EditorMode = mode;
            EditUI.Visible = mode;

            RemoveAllBackgroundObjects();
            if (EditorMode)
            {
                FillMapGameObjects();
                camera.Zoom = 1.5f;

                Engine.CurrentGame.IsMouseVisible = true;
                Engine.SetMouseCursor(GameSettings.MOUSE_POINTER_NORMAL);
            }
            else
            {
                SetMapGameObjects();
                Engine.CurrentGame.IsMouseVisible = false;
                level.GenerateNewBackground();
                camera.Zoom = 2.0f;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(AInput.KeyPressed(Keys.F1))
            {
                EditorMode = !EditorMode;
                SetEditorMode(EditorMode);
            }
            else if(AInput.KeyPressed(Keys.F2) && EditorMode)
            {
                EditorMode = false;
                EditUI.Visible = EditorMode;
                Engine.CurrentGame.IsMouseVisible = false;
                ResetObjectsAndEntities(gameTime);
                Player.Position = level.PlayerSpawn;
                level.GenerateNewBackground();
                camera.Zoom = 2.0f;

                var player = Player;
                player.Position = Vector2.Zero;
            }

            if(EditorMode)
                UpdateEditorMode(gameTime);
            else
                UpdateNormalMode(gameTime);
        }
        #region EDITOR UPDATE
        private void UpdateEditorMode(GameTime gameTime)
        {
            base.Update(gameTime);

            EditUI.Update(gameTime);

            Player.Update(gameTime);
            camera.Update(gameTime, Player);

            foreach(GameObjects.Water w in waterObjects)
            {
                w.Update(gameTime);
            }
        }
        #endregion

        #region NORMAL UPDATE
        private void UpdateNormalMode(GameTime gameTime)
        {
            if(Player == null || Player.Health.Value == 0)
                RestartGame();

            base.Update(gameTime);

            if((AInput.KeyPressed(Keys.P) || AInput.ButtonPressed(Buttons.Start, PlayerIndex.One)) && !congrats)
            {
                Engine.Paused = !Engine.Paused;
            }

            if(!Engine.Paused)
            {
                powerUpsWindow.Update(gameTime);
                silentPause = powerUpsWindow.Visible;

                if(powerUpsWindow.Visible)
                {
                    if(AInput.KeyPressed(Keys.V) || AInput.KeyPressed(Keys.Enter) || AInput.ButtonPressed(Buttons.X, PlayerIndex.One))
                    {
                        powerUpsWindow.Visible = false;

                        if(!showWelcomeMsg)
                            showWelcomeMsg = true;
                    }
                }

                if(!silentPause)
                {
                    if(AInput.KeyPressed(Keys.N) || AInput.ButtonPressed(Buttons.Y, PlayerIndex.One))
                    {
                        lifeCountTimer.Show();
                        ammoTimer.Show();
                        starPowerTimer.Show();
                        uiBackgroundTimer.Show();
                    }

                    foreach(Map.Cloud c in clouds)
                    {
                        c.Update(gameTime);
                    }

                    textBox.Update(gameTime);
                    ammo.Text = Player.Clip.ToString();
                    ammo.Update(gameTime);
                    ammo.Alpha = ammoTimer.Alpha;

                    starPowerFont.Text = Player.StarPower.ToString() + "%";
                    starPowerFont.Update(gameTime);
                    starPowerFont.Alpha = starPowerTimer.Alpha;

                    uiBackgroundTimer.Update(gameTime);
                    starPowerTimer.Update(gameTime);
                    ammoTimer.Update(gameTime);
                    lifeCountTimer.Update(gameTime);
                    lives.Update(gameTime);
                    lives.Text = Player.Lives.ToString();
                    lives.Alpha = lifeCountTimer.Alpha;
                    camera.Update(gameTime, Player);

                    for(int j = 0; j < entities.Count; j++)
                    {
                        if(entities[j].CollisionRectangle.Intersects(camera.ViewRectangle))
                        {
                            entities[j].Update(gameTime);
                        }
                    }

                    List<GameObjects.GameObject> tmpObjects = gameObjects.ToList();
                    foreach(var gameObject in tmpObjects)
                    {
                        if(gameObject.Rect.Intersects(camera.ViewRectangle))
                            gameObject.Update(gameTime);
                    }

                    foreach(GameObjects.Water w in waterObjects)
                    {
                        if(!EditorMode)
                            w.Update(gameTime);
                    }

                    List<Entities.Projectile> tmpProj = projectiles.ToList();
                    for(int i = 0; i < tmpProj.Count; i++)
                    {
                        tmpProj[i].Update(gameTime, level);

                        for(int k = 0; k < entities.Count; k++)
                        {
                            if(tmpProj[i].Owner != entities[k].Owner)
                            {
                                if(tmpProj[i].Rect.Intersects(entities[k].CollisionRectangle))
                                {
                                    entities[k].TakeDamage(Engine.Randomizer.Next(tmpProj[i].Damage.MinValue, tmpProj[i].Damage.MaxValue));
                                    projectiles[i].DestroyNextFrame = true;
                                    if(entities[k].Health.Value <= 0)
                                    {
                                        entities.RemoveAt(k);
                                        k--;
                                    }
                                }
                            }
                        }
                    }

                    for(int i = 0; i < projectiles.Count; i++)
                    {
                        if(projectiles[i].DestroyNextFrame)
                        {
                            projectiles.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            if(AInput.KeyPressed(Keys.Escape) || (AInput.ButtonPressed(Buttons.Back, PlayerIndex.One) && Engine.Paused))
            {
                Engine.ScreenManager.SetScreen(new MainMenu());
            }

            if(!showWelcomeMsg)
            {
                ShowMessage("Welcome to the Alien Prisoner Alpha!\n \n \nThis is a test level for you to try various functions for the game, hope you enjoy it's current state!\n \n \n//Corpse-lol-EU\n   Press ACTION button to close!");
            }
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Engine.Monitor.GetTransformationMatrix());
            spriteBatch.Draw(backgroundSky, new Rectangle(0, 0, 800, 600), Color.White);
            foreach (Map.Cloud c in clouds)
            {
                c.Draw(spriteBatch);
            }
            spriteBatch.Draw(backgroundFront, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);

            level.Draw(spriteBatch, camera);

            foreach(Entities.Entity e in entities)
            {
                if (e.CollisionRectangle.Intersects(camera.ViewRectangle))
                    e.Draw(spriteBatch);
            }

            foreach(Entities.Projectile p in projectiles)
            {
                if (p.Rect.Intersects(camera.Rect))
                    p.Draw(spriteBatch);
            }

            foreach(var gameObject in gameObjects)
            {
                if (gameObject.Rect.Intersects(camera.ViewRectangle))
                    gameObject.Draw(spriteBatch);
                else if(!gameObject.Rect.Intersects(camera.ViewRectangle) && gameObject.DrawAtAllTimes)
                    gameObject.Draw(spriteBatch);
            }

            foreach(var w in waterObjects)
            {
                if (w.Rect.Intersects(camera.ViewRectangle))
                    w.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Engine.Monitor.GetTransformationMatrix());
            /*
            spriteBatch.Draw(healthText, new Vector2(10, 10), Color.White);

            spriteBatch.Draw(healthBar, new Vector2(10, 24), new Rectangle(0, 0, 1, 24), Color.White);
            for (int i = 1; i < player.Health.Value; i++)
            {
                spriteBatch.Draw(healthBar, new Vector2(10 + i, 24), new Rectangle(1, 0, 1, 24), Color.White);
            }

            spriteBatch.Draw(healthBar, new Vector2(10 + player.Health.Value, 24), new Rectangle(2, 0, 1, 24), Color.White);
            */
            
            spriteBatch.Draw(lifeCounter, lifePos, null, Color.White * lifeCountTimer.Alpha, 0, Vector2.Zero, UIScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(ammoCounter, ammoPos, null, Color.White * ammoTimer.Alpha, 0, Vector2.Zero, UIScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(starPowerCounter, starPowerPos, null, Color.White * starPowerTimer.Alpha, 0, Vector2.Zero, UIScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(uiBackgroundTexture, uiBackgroundPos, null, Color.White * uiBackgroundTimer.Alpha, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            powerUpsWindow.Draw(spriteBatch);

            if (Engine.Paused)
            {
                spriteBatch.Draw(paused, new Vector2(Engine.Monitor.VirtualWidth / 2 - paused.Width / 2, Engine.Monitor.VirtualHeight / 2 - paused.Height / 2), Color.White);
            }

            textBox.Draw(spriteBatch);
            ammo.Draw(spriteBatch, UIScale);
            lives.Draw(spriteBatch, UIScale);
            starPowerFont.Draw(spriteBatch, UIScale);

            EditUI.Draw(spriteBatch, UIScale);

            spriteBatch.End();
        }
    }
}

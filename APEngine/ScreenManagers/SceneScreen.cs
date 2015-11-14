using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using AtriLib2;
using System.Globalization;

namespace APEngine.ScreenManagers
{
    public class SceneScreen : Screen
    {
        bool hasInit = false;
        bool inScene = false;
        bool doPreScene = false;

        Texture2D background;

        static List<SceneObjects.SceneObject> sceneObjects;
        static List<SceneEffects.SceneEffect> sceneEffects;

        public SceneScreen()
            : base()
        {
            sceneObjects = new List<SceneObjects.SceneObject>();
            sceneEffects = new List<SceneEffects.SceneEffect>();
        }

        public static SceneObjects.SceneObject GetSceneObject(int ID)
        {
            foreach(SceneObjects.SceneObject so in sceneObjects)
            {
                if (so.ID == ID)
                    return so;
            }

            return null;
        }

        public static List<SceneObjects.SceneObject> GetSceneObjects()
        {
            return sceneObjects;
        }

        public static void MoveObject(int ObjectID, float speed, Vector2 dest)
        {
            foreach(SceneObjects.SceneObject so in sceneObjects)
            {
                if(so.ID == ObjectID)
                {
                    so.DoMove = true;
                    so.Speed = speed;
                    so.Destination = dest;
                }
            }
        }

        public void NextScene()
        {
            foreach (SceneEffects.SceneEffect se in sceneEffects)
            {
                se.PlayThis = false;
                se.HasPlayed = true;
            }

            foreach(SceneEffects.SceneEffect se in sceneEffects)
            {
                if(se is SceneEffects.ChangeScreenEffect)
                {
                    se.PlayThis = true;
                    se.HasPlayed = false;
                }
            }
        }

        private void AddEffect(SceneEffects.SceneEffect effect)
        {
            if (sceneEffects.Count <= 0)
                effect.PlayThis = true;

            sceneEffects.Add(effect);
        }

        private int GetPlayingEffect()
        {
            for(int i = 0; i < sceneEffects.Count; i++)
            {
                if (sceneEffects[i].HasPlayed == false)
                    return i;
            }

            return -1;
        }

        public override void LoadContent(ContentManager Content, string args)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Content\\Data\\Scenes\\" + args + ".aps";

            int amountOfZ = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                int lineCount = 0;
                while (sr.Peek() > 0)
                {
                    lineCount++;
                    string currLine = sr.ReadLine();

                    if (hasInit == true && inScene == true)
                    {
                        // #3
                        // Do scene stuff

                        if (ATools.ContainsAtRange(currLine, "[SceneEnd]", 0, 10))
                        {
                            // We have reached the end, ignore the rest of the file
                            break;
                        }
                        else if (ATools.ContainsAtRange(currLine, "FadeIn()", 0, 8))
                        {
                            SceneEffects.FadeEffect effect = new SceneEffects.FadeEffect();
                            effect.LoadContent(Content);

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "FadeOut()", 0, 9))
                        {
                            SceneEffects.FadeEffect effect = new SceneEffects.FadeEffect();
                            effect.Type = SceneEffects.FadeEffectType.Out;
                            effect.LoadContent(Content);

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "Pause(", 0, 6))
                        {
                            SceneEffects.PauseEffect effect = new SceneEffects.PauseEffect();
                            effect.LoadContent(Content);

                            int len = ATools.GetNextChar(currLine, ')', 6);
                            string argLine = "";

                            for (int i = 6; i < len; i++)
                            {
                                argLine += currLine[i];
                            }

                            List<string> arguments = ATools.GetArgsFromFunction(argLine);

                            if (arguments.Count < 1 || arguments.Count > 1)
                            {
                                throw new Exception("GenerateBackground() takes 1 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                            }

                            effect.Duration = (float)(int.Parse(arguments[0]));

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "ChangeScreen(\"", 0, 14))
                        {
                            int len = ATools.GetNextChar(currLine, ')', 14);
                            string argLine = "";

                            for (int i = 14; i < len; i++)
                            {
                                argLine += currLine[i];
                            }

                            List<string> arguments = ATools.GetArgsFromFunction(argLine);

                            if (arguments.Count < 2 || arguments.Count > 2)
                            {
                                throw new Exception("ChangeScreen() takes 2 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                            }

                            SceneEffects.ChangeScreenEffect effect = new SceneEffects.ChangeScreenEffect();
                            effect.ScreenName = arguments[0];
                            effect.Argument = arguments[1];

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "Print(\"", 0, 7))
                        {
                            int len = ATools.GetNextChar(currLine, ')', 7);
                            string argLine = "";

                            for (int i = 6; i < len; i++)
                            {
                                argLine += currLine[i];
                            }

                            List<string> arguments = ATools.GetArgsFromFunction(argLine);

                            if (arguments.Count < 4 || arguments.Count > 4)
                            {
                                throw new Exception("Print() takes 4 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                            }

                            SceneEffects.PrintEffect effect = new SceneEffects.PrintEffect();
                            effect.Text = arguments[0];
                            effect.Duration = Convert.ToSingle(arguments[1], CultureInfo.InvariantCulture.NumberFormat);

                            Vector2 pos = Vector2.Zero;
                            float height = 0f;
                            float width = 0f;

                            if(ATools.IsFloatOrNumeric(arguments[2]))
                            {
                                pos.X = Convert.ToSingle(arguments[2], CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else
                            {
                                if(arguments[2] == "CENTER_HORIZONTAL")
                                {
                                    width = effect.Text.Length * 11;
                                    pos.X = Engine.Monitor.VirtualWidth / 2 - width / 2;
                                }
                                else
                                {
                                    throw new Exception("Expected string was not found in Print(), excpected 'CENTER_HORIZONTAL' at 3rd argument!\nLine: " + lineCount.ToString());
                                }
                            }

                            if (ATools.IsFloatOrNumeric(arguments[3]))
                            {
                                pos.Y = Convert.ToSingle(arguments[3], CultureInfo.InvariantCulture.NumberFormat);
                            }
                            else
                            {
                                if (arguments[3] == "CENTER_VERTICAL")
                                {
                                    height = 17 / 2;
                                    pos.Y = Engine.Monitor.VirtualHeight / 2 - height / 2;
                                }
                                else
                                {
                                    throw new Exception("Expected string was not found in Print(), excpected 'CENTER_VERTICAL' at 4th argument!\nLine: " + lineCount.ToString());
                                }
                            }

                            effect.Rect = new Rectangle((int)pos.X, (int)pos.Y, (int)width, (int)height);
                            effect.LoadContent(Content);

                            AddEffect(effect);
                        }
                        else if(ATools.ContainsAtRange(currLine, "Move(", 0, 5))
                        {
                            int len = ATools.GetNextChar(currLine, ')', 5);
                            string argLine = "";

                            for (int i = 5; i < len; i++)
                            {
                                argLine += currLine[i];
                            }

                            List<string> arguments = ATools.GetArgsFromFunction(argLine);

                            if (arguments.Count < 4 || arguments.Count > 4)
                            {
                                throw new Exception("Move takes 4 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                            }

                            SceneEffects.MoveEffect effect = new SceneEffects.MoveEffect();
                            effect.ObjectID = int.Parse(arguments[0]);
                            effect.Speed = Convert.ToSingle(arguments[1], CultureInfo.InvariantCulture.NumberFormat);
                            effect.Destination = new Vector2(Convert.ToSingle(arguments[2], CultureInfo.InvariantCulture.NumberFormat), Convert.ToSingle(arguments[3], CultureInfo.InvariantCulture.NumberFormat));

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "HideAll()", 0, 9))
                        {
                            SceneEffects.HideAllEffect effect = new SceneEffects.HideAllEffect();

                            AddEffect(effect);
                        }
                        else if (ATools.ContainsAtRange(currLine, "SetVisible(", 0, 11))
                        {
                            int len = ATools.GetNextChar(currLine, ')', 11);
                            string argLine = "";

                            for (int i = 11; i < len; i++)
                            {
                                argLine += currLine[i];
                            }

                            List<string> arguments = ATools.GetArgsFromFunction(argLine);

                            if (arguments.Count < 2 || arguments.Count > 2)
                            {
                                throw new Exception("SetVisible() takes 2 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                            }

                            SceneEffects.SetVisibleEffect effect = new SceneEffects.SetVisibleEffect();
                            effect.ObjectID = int.Parse(arguments[0]);
                            effect.Visibility = int.Parse(arguments[1]);

                            AddEffect(effect);
                        }
                    }
                    else if (hasInit == false && inScene == false)
                    {
                        // #1
                        // Nothing has happend, so do initialization stuff
                        if(ATools.ContainsAtRange(currLine, "[InitStart]", 0, 11))
                        {
                            hasInit = true;
                        }
                    }
                    else if (hasInit == true && inScene == false)
                    {
                        // #2
                        // Do post-Init and pre-Scene stuff

                        if (!doPreScene)
                        {
                            // Do post-init here
                            if (ATools.ContainsAtRange(currLine, "[InitEnd]", 0, 9))
                            {
                                doPreScene = true;
                            }
                            else if(currLine.StartsWith("Background=\""))
                            {
                                string bgName = "";
                                for(int i = 12; i < currLine.Length; i++)
                                {
                                    if (currLine[i] != '\"')
                                    {
                                        bgName += currLine[i];
                                    }
                                    else
                                        break;
                                }

                                if(bgName != "")
                                {
                                    background = Content.Load<Texture2D>("Data\\GFX\\Scenes\\" + bgName);
                                }
                            }
                            else if(currLine.StartsWith("GenerateBackground(\""))
                            {
                                // Generate a texture Z amounts at X and Y position
                                int len = ATools.GetNextChar(currLine, ')', 19);
                                string argLine = "";

                                for (int i = 19; i < len; i++)
                                {
                                    argLine += currLine[i];
                                }

                                List<string> arguments = ATools.GetArgsFromFunction(argLine);

                                if(arguments.Count < 4 || arguments.Count > 4)
                                {
                                    throw new Exception("GenerateBackground() takes 4 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                                }

                                arguments[0] = arguments[0].Trim('\"');

                                for(int i = 0; i < int.Parse(arguments[1]); i++)
                                {
                                    SceneObjects.SceneObject obj = GetSceneObject(arguments[0]);
                                    obj.LoadContent(Content);

                                    int x = Engine.Randomizer.Next(-7, 807);
                                    int y = Engine.Randomizer.Next(int.Parse(arguments[2]), int.Parse(arguments[3]));
                                    obj.Position = new Vector2(x, y);

                                    sceneObjects.Add(obj);
                                }
                            }
                            else if(currLine.StartsWith("AddBackground(\""))
                            {
                                int len = ATools.GetNextChar(currLine, ')', 14);
                                string argLine = "";

                                for (int i = 14; i < len; i++)
                                {
                                    argLine += currLine[i];
                                }

                                List<string> arguments = ATools.GetArgsFromFunction(argLine);

                                if (arguments.Count < 3 || arguments.Count > 3)
                                {
                                    throw new Exception("AddBackground() takes 3 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                                }

                                arguments[0] = arguments[0].Trim('\"');

                                SceneObjects.SceneObject obj = GetSceneObject(arguments[0]);
                                obj.LoadContent(Content);
                                obj.Position = new Vector2(int.Parse(arguments[1]), int.Parse(arguments[2]));

                                sceneObjects.Add(obj);
                            }
                            else if(currLine.StartsWith("AddObject("))
                            {
                                int len = ATools.GetNextChar(currLine, ')', 10);
                                string argLine = "";

                                for (int i = 10; i < len; i++)
                                {
                                    argLine += currLine[i];
                                }

                                List<string> arguments = ATools.GetArgsFromFunction(argLine);

                                if (arguments.Count < 5 || arguments.Count > 5)
                                {
                                    throw new Exception("AddBackground() takes 5 arguments! Found " + arguments.Count.ToString() + "!\nLine: " + lineCount.ToString());
                                }

                                SceneObjects.SceneObject obj = GetSceneObject(arguments[2]);
                                obj.LoadContent(Content);
                                obj.Position = new Vector2(int.Parse(arguments[3]), int.Parse(arguments[4]));
                                obj.ID = int.Parse(arguments[0]);
                                obj.Visible = ATools.IntToBool(int.Parse(arguments[1]));

                                if (obj is SceneObjects.Zzz)
                                {
                                    obj.SetFrame(amountOfZ);
                                    amountOfZ++;

                                    if (amountOfZ > 3)
                                        amountOfZ = 0;
                                }

                                sceneObjects.Add(obj);
                            }
                        }
                        else
                        {
                            if (ATools.ContainsAtRange(currLine, "[SceneStart]", 0, 12))
                            {
                                inScene = true;
                            }
                        }
                    }
                }
            }
        }

        private SceneObjects.SceneObject GetSceneObject(String objName)
        {
            if (objName == "Star")
                return new SceneObjects.Star();
            else if (objName == "Moon")
                return new SceneObjects.Moon();
            else if (objName == "Cactus")
                return new SceneObjects.Cactus();
            else if (objName == "HouseShadow")
                return new SceneObjects.HouseShadow();
            else if (objName == "Zzz")
                return new SceneObjects.Zzz();
            else if (objName == "SpaceShipShadow")
                return new SceneObjects.SpaceShipShadow();
            else if (objName == "Alien1Shadow")
                return new SceneObjects.Alien1Shadow();
            else
            {
                throw new Exception("Unexpected object type: '" + objName + "'");
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach(SceneObjects.SceneObject obj in sceneObjects)
            {
                obj.Update(gameTime);
            }

            for (int i = 0; i < sceneEffects.Count; i++)
            {
                if(i < sceneEffects.Count - 1)
                {
                    if(sceneEffects[i].HasPlayed == true && sceneEffects[i + 1].PlayThis == false)
                    {
                        sceneEffects[i + 1].PlayThis = true;
                    }
                }
            }

            foreach (SceneEffects.SceneEffect effect in sceneEffects)
            {
                effect.Update(gameTime);
            }

            if(AInput.KeyPressed(Keys.Escape))
            {
                NextScene();
            }
        }

        public override void Unload()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(background == null)
            {
                return;
            }

            base.Draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null, Engine.Monitor.GetTransformationMatrix());
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            foreach (SceneObjects.SceneObject obj in sceneObjects)
            {
                obj.Draw(spriteBatch);
            }

            foreach (SceneEffects.SceneEffect effect in sceneEffects)
            {
                effect.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}

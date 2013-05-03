using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    /// <summary>
    /// A simple splash screen that fades in/out
    /// and loads the next screen after fading out
    /// </summary>
    class SplashScreen : GameScreen
    {
        ContentManager content;
        Texture2D splashTexture;
        SoundEffect scream;


        public enum splashScreenType { 
                studio,
                Game
        }

        const float timeToStayOnScreen = 1.5f;
        float timer = 0f;

        splashScreenType screenType;

        public SplashScreen(splashScreenType s)
        {

            screenType = s;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            scream = content.Load<SoundEffect>("Sounds\\menuscream");
            if (screenType == splashScreenType.studio) splashTexture = content.Load<Texture2D>("backgrounds\\Splashscreen2");
            if (screenType == splashScreenType.Game) splashTexture = content.Load<Texture2D>("backgrounds\\StartScreen2Z");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (ScreenState == ScreenState.Active)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timer += elapsed;
                if (timer >= timeToStayOnScreen)
                {
                    ExitScreen();
                }
            }
            else if (ScreenState == ScreenState.TransitionOff)
            {
                if (TransitionPosition == 1)
                {

                    if (screenType == splashScreenType.studio)
                    {
                        ExitScreen();
                        ScreenManager.AddScreen(new SplashScreen(splashScreenType.Game), null);
                    }

                    if (screenType == splashScreenType.Game)
                    {
                        ExitScreen();
                        ScreenManager.AddScreen(new MainMenuScreen(true), null);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            Vector2 center = new Vector2(fullscreen.Center.X, fullscreen.Center.Y);

            spriteBatch.Begin();


            spriteBatch.Draw(splashTexture,
                center,
                null,
                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha),
                0f,
                new Vector2(splashTexture.Width / 2, splashTexture.Height / 2),
                0.6f,
                SpriteEffects.None,
                0f);

            spriteBatch.End();
        }
    }
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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


        public enum splashScreenType { 
                studio,
                Game
        }

        //How long should the screen stay fully visible
        const float timeToStayOnScreen = 1.5f;

        //Keep track of how much time has passed
        float timer = 0f;

        splashScreenType screenType;

        public SplashScreen(splashScreenType s)
        {

            screenType = s;

            //How long to fade in
            TransitionOnTime = TimeSpan.FromSeconds(0.5);

            //How long to fade out
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            //Load a new ContentManager so when we're done
            //showing this screen we can unload the content
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            //Splash screen texture
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
                //When this screen is fully active, we want to
                //begin our timer so we know when to fade out
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

                timer += elapsed;
                if (timer >= timeToStayOnScreen)
                {
                    //When we've passed the 'timeToStayOnScreen' time,
                    //we call ExitScreen() which will fade out then
                    //kill the screen afterwards
                    ExitScreen();
                }
            }
            else if (ScreenState == ScreenState.TransitionOff)
            {
                if (TransitionPosition == 1)
                {
                    //When 'TransistionPosition' hits 1 then our screen
                    //is fully faded out. Anything in this block of
                    //code is the last thing to be called before this
                    //screen is killed forever so we add the next screen(s)
                    //ScreenManager.AddScreen(new BackgroundScreen(), null);
                    if (screenType == splashScreenType.studio) ScreenManager.AddScreen(new SplashScreen(splashScreenType.Game), null); 
                    // add menu after second splash screen
                    if (screenType == splashScreenType.Game)
                    {
                        //ScreenManager.AddScreen(new GamePlayScreen(), null);
                        ScreenManager.AddScreen(new BackgroundScreen(), null);
                        ScreenManager.AddScreen(new MainMenuScreen(), null);
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

            //Draw our logo centered to the screen
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
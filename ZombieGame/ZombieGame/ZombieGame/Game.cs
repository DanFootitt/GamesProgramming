using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ZombieGame
{

    public class ZombieGame : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        static readonly string[] preloadAssets =
        {

        };



        public ZombieGame()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            screenManager = new ScreenManager(this);

            Components.Add(screenManager);

            screenManager.AddScreen(new SplashScreen(SplashScreen.splashScreenType.studio), null);
            //screenManager.AddScreen(new NameEntryScreen(), null); 
            //screenManager.AddScreen(new InstructionsScreen(), null);
            //screenManager.AddScreen(new GameOverScreen(null, GameOverScreen.gameOverType.COMPLETE), null);
            //screenManager.AddScreen(new LevelCompleteScreen(100, 10, 3, GameDifficulty.EASY_MODE, 80.0f), null);
            //screenManager.AddScreen(new GamePlayScreen(GameDifficulty.EASY_MODE), null);
            //screenManager.AddScreen(new HighScoreScreen(), null);
        }

        protected override void LoadContent()
        {
            foreach (string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

    }


    static class Program
    {
        static void Main()
        {
            using (ZombieGame game = new ZombieGame())
            {
                game.Run();
            }
        }

    }
}
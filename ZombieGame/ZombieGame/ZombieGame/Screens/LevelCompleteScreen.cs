using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    class LevelCompleteScreen : GameScreen
    {
        Texture2D background;
        Texture2D gameOver;
        Mouse mouse;
        MenuButton exitButton;
        SpriteFont font;

        SoundEffect levelCompleteSound;
        SoundEffect sound;

        // player stats

        int score;
        int lives;
        GameDifficulty gameDiff;
        Player player;


        public LevelCompleteScreen(Player player, GameDifficulty gd, float acc)
        {

            this.player = player;

            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            this.score = player.score;
            this.lives = player._lives;
            this.gameDiff = gd;
        }


        public override void LoadContent()
        {
            System.Threading.Thread.Sleep(100);
            ContentManager content = ScreenManager.Game.Content;

            sound = content.Load<SoundEffect>("Sounds\\clicksound");
            levelCompleteSound = content.Load<SoundEffect>("sounds\\complete");

            background = content.Load<Texture2D>("backgrounds\\title");
            gameOver = content.Load<Texture2D>("backgrounds\\levelcomplete");

            font = content.Load<SpriteFont>("spriteFont1");

            Texture2D norm = content.Load<Texture2D>("Buttons\\continueGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\continue");
            exitButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2) , 520), sound);

            mouse = new Mouse(content, "cursor");

            levelCompleteSound.Play();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            exitButton.Update(mouse.rectangle);
            if (exitButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                exitButton.sound.Play();
                ExitScreen();
                if (player.level < 3)ScreenManager.AddScreen(new GamePlayScreen(GameDifficulty.EASY_MODE, player.name, player.score, player._lives, player._health, player.level + 1), null);
                else ScreenManager.AddScreen(new GameOverScreen(player, GameOverScreen.gameOverType.COMPLETE),null);
            }


        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

    
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 4 / 3);
            Color color = Color.White * TransitionAlpha;



            int diffBonus;

            if (gameDiff == GameDifficulty.EASY_MODE) diffBonus = 1;
            else diffBonus = 2;

            int totalScore = score +  ((10 * lives) * diffBonus);

            player.score = totalScore;

            String scoreString = String.Format("Level Score        :   {0:00}" +  
                                             "\nLives Left         :   {1:00}" + 
                                             "\nDifficulty Bonus   :   {2:00}" + 
                                           "\n\nTOTAL SCORE        :   {3:00}" 
                                             , score, lives, diffBonus,totalScore);

            spriteBatch.Begin();


            spriteBatch.Draw(background, new Rectangle(0, 10, background.Width, background.Height), color);
            spriteBatch.DrawString(font, scoreString, new Vector2(800 / 2 - 150, 250), Color.White);
            spriteBatch.Draw(gameOver, new Rectangle(800 / 2 - gameOver.Width / 2, 100, gameOver.Width, gameOver.Height), color);

            spriteBatch.End();

            exitButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);


        }
    }
}

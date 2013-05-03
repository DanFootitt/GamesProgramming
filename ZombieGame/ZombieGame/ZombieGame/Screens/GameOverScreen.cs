using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    class GameOverScreen : GameScreen

    {

        public enum gameOverType
        {
           GAMEOVER,
           COMPLETE
        }

        Texture2D background;
        Texture2D gameOver;
        Mouse mouse;
        MenuButton exitButton;
        gameOverType got;
        Player player;
        SoundEffect sound;
        Texture2D menuTitle;

        public GameOverScreen(Player player, gameOverType gtt)
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            if (player != null)HighScores.sendScore(player.name, player.score);
            got = gtt;
            this.player = player;
        }

        public GameOverScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            background = content.Load<Texture2D>("backgrounds\\title");
            if (got == gameOverType.GAMEOVER)
            {
                gameOver = content.Load<Texture2D>("zombie");
                sound = content.Load<SoundEffect>("Sounds//gameover");
                menuTitle = content.Load<Texture2D>("backgrounds\\gameover");
            }
            else
            {
                gameOver = content.Load<Texture2D>("backgrounds\\gamecomplete");
                menuTitle = content.Load<Texture2D>("backgrounds\\victory");
                sound = content.Load<SoundEffect>("Sounds//victory");
            }

            SoundEffect click = content.Load<SoundEffect>("Sounds\\ClickSound");
            Texture2D norm = content.Load<Texture2D>("Buttons\\continueGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\continue");
            exitButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 530), click);

            mouse = new Mouse(content, "cursor");

            if (sound != null )sound.Play();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            exitButton.Update(mouse.rectangle);
            if (exitButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                exitButton.sound.Play();
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new HighScoreScreen(this.player), null);
            }


        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 4 / 3);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;


            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0,0,background.Width,background.Height), color);
            spriteBatch.Draw(menuTitle, new Rectangle(400 - (menuTitle.Width / 2), 100, menuTitle.Width, menuTitle.Height), color);

            int y = 0;
            if (got == gameOverType.COMPLETE)
                spriteBatch.Draw(gameOver, new Rectangle(400 - gameOver.Width / (2*2), 200, gameOver.Width / 2, gameOver.Height / 2), color);
            else 
                spriteBatch.Draw(gameOver, new Rectangle(400 - gameOver.Width / (2*2), 200, gameOver.Width / 2, gameOver.Height / 2), color);

            

            spriteBatch.End();

            exitButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

 
        }



    }
}

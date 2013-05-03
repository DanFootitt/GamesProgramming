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
    public class HighScoreScreen : GameScreen
    {

        Mouse mouse;
        MenuButton exitButton;
        HighScoreTable highScores;
        Texture2D banner;
        Texture2D zombie;
        Player p;

        public HighScoreScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            highScores = HighScores.getScores("");
        }

        public HighScoreScreen(Player player)
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            highScores = HighScores.getScores("");

            p = player;
        }



        public override void LoadContent()
        {
            System.Threading.Thread.Sleep(100);

            ContentManager content = ScreenManager.Game.Content;

            Texture2D norm = content.Load<Texture2D>("buttons\\exitbuttonGlow");
            Texture2D glow = content.Load<Texture2D>("buttons\\exitbutton");
            SoundEffect sound = content.Load<SoundEffect>("sounds\\clicksound");

            exitButton = new MenuButton(glow,norm,new Vector2((800 / 2) - (norm.Width / 2),530), sound);
            mouse = new Mouse(content, "cursor");
            zombie = content.Load<Texture2D>("Backgrounds\\title");
            banner = content.Load<Texture2D>("Backgrounds\\highscores");
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
                ScreenManager.AddScreen(new MainMenuScreen(), null);
            }

        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 4 / 3);
            Color color = Color.White * TransitionAlpha;


            spriteBatch.Begin();

            spriteBatch.DrawString(font, "#", new Vector2(200, 200), Color.White);
            spriteBatch.DrawString(font, "Name", new Vector2(360, 200), Color.White);
            spriteBatch.DrawString(font, "Score", new Vector2(520, 200), Color.White);

            Color c = Color.White;

            for (int i = 0; i < 10; i++)
            {
                int y = 220 + (i+1) * 25;
                int j = i + 1;

                if (p != null && p.score == highScores.scores[i] && p.name == highScores.names[i]) c = Color.Red;
                else c = Color.White;

                spriteBatch.DrawString(font, j.ToString(), new Vector2(200, y), c);
                spriteBatch.DrawString(font, highScores.names[i].ToUpper(), new Vector2(360, y), c);
                spriteBatch.DrawString(font, highScores.scores[i].ToString(), new Vector2(520, y), c);
            }
            spriteBatch.Draw(zombie, new Rectangle(0, 10, zombie.Width, zombie.Height), Color.White);
            spriteBatch.Draw(banner, new Rectangle((800 / 2) - (banner.Width / 2), 100, banner.Width, banner.Height), Color.White);

            spriteBatch.End();

            exitButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);


        }
    }
}

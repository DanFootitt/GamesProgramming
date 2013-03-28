using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieGame
{
    class GameOverScreen : GameScreen

    {
        Player player;
        SpriteFont font;
        Texture2D background;
        string screenTitle;
        string screenMessage;
        Mouse mouse;
        bool exitIntersect = false;

        public GameOverScreen(Player player)
        {
            this.player = player;
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2); 
        }


        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            font = content.Load<SpriteFont>("test");
            background = content.Load<Texture2D>("images\\background");
            Texture2D exit = content.Load<Texture2D>("images\\exitButton");
            mouse = new Mouse(content, "images\\cursor_white");
        }

        public override void HandleInput(InputState input)
        {
            
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

                if (mouse.leftClick)
                {   PlayerIndex playerindex = ControllingPlayer.Value;
                ScreenManager.AddScreen(new BackgroundScreen(), playerindex);
                    ScreenManager.AddScreen(new MainMenuScreen(),playerindex);

                }
            else
            {
                exitIntersect = false;

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

            Vector2 messageV = font.MeasureString(screenMessage);
            Vector2 titleV = font.MeasureString(screenTitle);
            spriteBatch.Draw(background, new Rectangle(0,0,background.Width,background.Height), color);
            spriteBatch.DrawString(font, screenTitle, new Vector2(800 / 2 - titleV.X / 2, 20), color);
            spriteBatch.DrawString(font, screenMessage, new Vector2(800 / 2 - messageV.X / 2, 120), color);
            /*spriteBatch.DrawString(font, "Time : " + elapsedTime, new Vector2(300, 300), color);
            spriteBatch.DrawString(font, "Score : " + player.playerScore.ToString() , new Vector2(300, 250), color);*/
            spriteBatch.End();

            mouse.Draw(spriteBatch);

 
        }



    }
}

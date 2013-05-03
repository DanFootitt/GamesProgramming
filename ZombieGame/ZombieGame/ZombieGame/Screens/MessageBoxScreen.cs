using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieGame
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : GameScreen
    {
        string message;
        Texture2D gradientTexture;
        Mouse mouse;


        public MessageBoxScreen(string message, Texture2D mouseText)
        {

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            mouse = new Mouse(mouseText);


        }

        public override void LoadContent()
        {
            System.Threading.Thread.Sleep(100);
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("levelpopup");
        }


        public override void HandleInput(InputState input)
        {
            mouse.Update();
            if (mouse.newLeftClick)
                ExitScreen();
        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 0.6f);


            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            spriteBatch.Draw(gradientTexture, new Rectangle((400 - gradientTexture.Width / 2), 200, gradientTexture.Width,gradientTexture.Height), color);
            spriteBatch.End();

            mouse.Draw(spriteBatch);
        }

    }
}

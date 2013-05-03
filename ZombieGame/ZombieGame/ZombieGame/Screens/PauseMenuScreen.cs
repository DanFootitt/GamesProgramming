using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieGame
{

    class PauseMenuScreen : GameScreen
    {

        Mouse mouse;
        MenuButton easyButton;
        MenuButton hardButton;
        Texture2D zombie;
        Texture2D title;
        

        public PauseMenuScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        public override void LoadContent()
        {

            ContentManager content = ScreenManager.Game.Content;

            zombie = content.Load<Texture2D>("backgrounds\\title");
            title = content.Load<Texture2D>("backgrounds\\pause");
            Texture2D norm = content.Load<Texture2D>("Buttons\\continueGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\continue");
            easyButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 300));

            norm = content.Load<Texture2D>("Buttons\\exitbuttonGlow");
            glow = content.Load<Texture2D>("Buttons\\exitbutton");
            hardButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 380));


            mouse = new Mouse(content, "cursor");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            easyButton.Update(mouse.rectangle);
            if (easyButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                ExitScreen();
            }

            hardButton.Update(mouse.rectangle);
            if (hardButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                ExitScreen();
                System.Threading.Thread.Sleep(100);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
            }


        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            spriteBatch.Draw(zombie, new Rectangle(0, 10, zombie.Width, zombie.Height), Color.White);
            spriteBatch.Draw(title, new Rectangle((800 / 2) - (title.Width / 2), 120, title.Width, title.Height), Color.White);
            spriteBatch.End();

            easyButton.Draw(spriteBatch);
            hardButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);


        }


    }
}

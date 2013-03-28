using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieGame
{
    class MainMenuScreen : GameScreen

    {
        Mouse mouse;
        MenuButton highscoresButton;
        MenuButton newGameButton;
        MenuButton exitButton;
        MenuButton helpButton;


        public MainMenuScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2); 
        }


        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            Texture2D norm = content.Load<Texture2D>("Buttons\\highscoresbutton");
            Texture2D glow = content.Load<Texture2D>("Buttons\\highscoresGlow");
            highscoresButton = new MenuButton(glow, norm, new Vector2(20, 350));

            norm = content.Load<Texture2D>("Buttons\\newgamebutton");
            glow = content.Load<Texture2D>("Buttons\\newgameGlow");
            newGameButton = new MenuButton(glow, norm, new Vector2(20, 300));

            norm = content.Load<Texture2D>("Buttons\\helpButton");
            glow = content.Load<Texture2D>("Buttons\\helpButtonGlow");
            helpButton = new MenuButton(glow, norm, new Vector2(20, 400));

            norm = content.Load<Texture2D>("Buttons\\exitButton");
            glow = content.Load<Texture2D>("Buttons\\exitButtonGlow");
            exitButton = new MenuButton(glow, norm, new Vector2(20, 450));

            mouse = new Mouse(content, "cursor");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            highscoresButton.Update(mouse.rectangle);
            if (highscoresButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new GamePlayScreen(GameDifficulty.EASY_MODE), null);
            }

            newGameButton.Update(mouse.rectangle);
            if (newGameButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new GamePlayScreen(GameDifficulty.EASY_MODE), null);
            }

            exitButton.Update(mouse.rectangle);

            helpButton.Update(mouse.rectangle);
            if (helpButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new GamePlayScreen(GameDifficulty.EASY_MODE), null);
            }

        }


        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            highscoresButton.Draw(spriteBatch);
            newGameButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            helpButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

 
        }



    }
}


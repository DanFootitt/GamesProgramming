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
    class MainMenuScreen : GameScreen

    {
        Mouse mouse;
        MenuButton highscoresButton;
        MenuButton newGameButton;
        MenuButton exitButton;
        MenuButton helpButton;
        Texture2D zombie;
        SoundEffect scream;
        SoundEffect click;
        bool play;


        public MainMenuScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2); 
        }

        public MainMenuScreen(bool playSound)
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            play = playSound;
        }


        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            zombie = content.Load<Texture2D>("backgrounds\\menubackground");

            Texture2D norm = content.Load<Texture2D>("Buttons\\highscoresGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\highscoresbutton");
            highscoresButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 350));

            norm = content.Load<Texture2D>("Buttons\\newgameGlow");
            glow = content.Load<Texture2D>("Buttons\\newgamebutton");
            newGameButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 300));

            norm = content.Load<Texture2D>("Buttons\\helpButtonGlow");
            glow = content.Load<Texture2D>("Buttons\\helpButton");
            helpButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 400));

            norm = content.Load<Texture2D>("Buttons\\exitButtonGlow");
            glow = content.Load<Texture2D>("Buttons\\exitButton");
            exitButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 450));

            mouse = new Mouse(content, "cursor");
            scream = content.Load<SoundEffect>("Sounds\\menuscream");
            click = content.Load<SoundEffect>("Sounds\\clicksound");

            if (play) scream.Play();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            highscoresButton.Update(mouse.rectangle);
            if (highscoresButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                click.Play();
                ExitScreen();
                ScreenManager.AddScreen(new HighScoreScreen(), null);
            }

            newGameButton.Update(mouse.rectangle);
            if (newGameButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {

                click.Play();
                ExitScreen();
                ScreenManager.AddScreen(new ModeSelectScreen(), null);
            }

            exitButton.Update(mouse.rectangle);
            if (exitButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {

                click.Play();
                ExitScreen();
                ScreenManager.Game.Exit();
            }

            helpButton.Update(mouse.rectangle);
            if (helpButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {

                click.Play();
                ExitScreen();
                ScreenManager.AddScreen(new InstructionsScreen(), null);
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
            spriteBatch.End();


            highscoresButton.Draw(spriteBatch);
            newGameButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            helpButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);

 
        }



    }
}


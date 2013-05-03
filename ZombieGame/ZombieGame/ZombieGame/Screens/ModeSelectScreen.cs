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
    class ModeSelectScreen : GameScreen
    {
        Mouse mouse;
        MenuButton easyButton;
        MenuButton hardButton;
        Texture2D zombie;
        Texture2D title;
        SoundEffect sound;



        public ModeSelectScreen()
        {
            IsPopup = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        public override void LoadContent()
        {
            System.Threading.Thread.Sleep(100);

            ContentManager content = ScreenManager.Game.Content;

            sound = content.Load<SoundEffect>("Sounds\\clicksound");

            zombie = content.Load<Texture2D>("backgrounds\\title");
            title = content.Load<Texture2D>("backgrounds\\modeselect");
            Texture2D norm = content.Load<Texture2D>("Buttons\\easymodeGLow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\easymode");
            easyButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 300), sound);

            norm = content.Load<Texture2D>("Buttons\\hardmodeGlow");
            glow = content.Load<Texture2D>("Buttons\\hardmode");
            hardButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 380), sound);


            mouse = new Mouse(content, "cursor");
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            mouse.Update();

            easyButton.Update(mouse.rectangle);
            if (easyButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                easyButton.sound.Play();
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new NameEntryScreen(GameDifficulty.EASY_MODE), null);
            }

            hardButton.Update(mouse.rectangle);
            if (hardButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                hardButton.sound.Play();
                ScreenManager.RemoveScreen(this);
                ScreenManager.AddScreen(new NameEntryScreen(GameDifficulty.HARD_MODE), null);
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
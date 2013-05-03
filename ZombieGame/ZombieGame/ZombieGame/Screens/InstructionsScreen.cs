using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    class InstructionsScreen : GameScreen
    {
        String menuTitle;
        MenuButton exitButton;
        Mouse mouse;
        Texture2D instructions;
        Texture2D background;
        Texture2D title;
        Player player;
        SoundEffect sound;
        

        public InstructionsScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            sound = content.Load<SoundEffect>("Sounds\\clicksound");

            Texture2D norm = content.Load<Texture2D>("Buttons\\exitButtonGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\exitButton");
            exitButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 510), sound);

            instructions = content.Load<Texture2D>("instructions");
            title = content.Load<Texture2D>("backgrounds\\instructions");
            background = content.Load<Texture2D>("backgrounds\\title");

            Texture2D playerTexture = content.Load<Texture2D>("manwalk");
            mouse = new Mouse(content, "cursor");
            player = new Player(playerTexture, mouse.texture, new Vector2(700, 500), content);
            
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
            player.UpdateSimple(gameTime);

        }



        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0, 10, background.Width, background.Height), Color.White);
            spriteBatch.Draw(title, new Rectangle(800 / 2 - title.Width / 2, 100, title.Width, title.Height), Color.White);
            spriteBatch.Draw(instructions, new Rectangle(400 - instructions.Width / 2, 200,instructions.Width, instructions.Height), Color.White);
            spriteBatch.End();

            exitButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
            player.DrawSimple(spriteBatch);

        }

    }
}

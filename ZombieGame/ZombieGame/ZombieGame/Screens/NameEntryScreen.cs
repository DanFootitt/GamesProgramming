using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace ZombieGame
{
    class NameEntryScreen : GameScreen
    {
        Texture2D title;
        Texture2D zombie;
        String name;
        MenuButton contButton;
        SoundEffect confirm;
        SoundEffect error;
        SoundEffect buttonconfirm;
        SoundEffect buttonerror;
        SpriteFont font;
        Mouse mouse;

        Keys[] keysToCheck = new Keys[] {
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
                Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
                Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
                Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
                Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
                Keys.Z, Keys.Back, Keys.Space };

        KeyboardState currentKbs;
        KeyboardState prevKbs;
        GameDifficulty gamediff;


        public NameEntryScreen(GameDifficulty gd)
        {
           name = "_";
           this.gamediff = gd;
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            title = content.Load<Texture2D>("backgrounds\\nameentry");
            confirm = content.Load<SoundEffect>("Sounds\\confirm");
            buttonerror = content.Load<SoundEffect>("Sounds\\error");
            buttonconfirm = content.Load<SoundEffect>("Sounds\\clicksound");
            error = content.Load<SoundEffect>("Sounds\\cancel");
            font = content.Load<SpriteFont>("Fonts\\Mono20");
            zombie = content.Load<Texture2D>("backgrounds\\title");
            Texture2D norm = content.Load<Texture2D>("Buttons\\continueGlow");
            Texture2D glow = content.Load<Texture2D>("Buttons\\continue");
            contButton = new MenuButton(glow, norm, new Vector2(800 / 2 - (norm.Width / 2), 500));


            mouse = new Mouse(content, "cursor");

        }



        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            prevKbs = currentKbs;
            currentKbs = Keyboard.GetState();

            foreach (Keys k in keysToCheck)
            {
                if (checkKey(k))
                {
                    addKeyToString(k);
                }
            }

            contButton.Update(mouse.rectangle);
            if (contButton.Intersects(mouse.rectangle) && mouse.newLeftClick)
            {
                if (!string.IsNullOrEmpty(name) && !name.Contains("_"))
                {
                    buttonconfirm.Play();
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.AddScreen(new GamePlayScreen(this.gamediff, name), null);
                }
                else {
                    buttonerror.Play();
                }
            }

            mouse.Update();



        }



        public override void Draw(GameTime gameTime)
        {


            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(zombie, new Rectangle(0, 10, zombie.Width, zombie.Height), Color.White);
            spriteBatch.Draw(title, new Rectangle(800 / 2 - title.Width / 2 , 100, title.Width, title.Height),Color.White);
            spriteBatch.DrawString(font, name, new Vector2(800 / 2 - name.Length * 8, 300), Color.White);



            spriteBatch.End();

            contButton.Draw(spriteBatch);
            mouse.Draw(spriteBatch);
        }

        public void addKeyToString(Keys k)
        {
            if (name == "_") name = "";

            string newChar = "";

            if (name.Length > 5 && k != Keys.Back)
            {
                error.Play();
                return;
            }

            switch (k)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (name.Length != 0)
                        name = name.Remove(name.Length - 1);
                    return;
            }

            if (currentKbs.IsKeyDown(Keys.RightShift) || currentKbs.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }

            confirm.Play();
            name += newChar;
        }

        public bool checkKey(Keys k)
        {
            return prevKbs.IsKeyDown(k) && currentKbs.IsKeyUp(k);

        }

    }
}

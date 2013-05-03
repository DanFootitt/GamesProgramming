using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.IO.IsolatedStorage;

namespace ZombieGame
{
    public enum GameDifficulty
    {
        EASY_MODE,
        HARD_MODE
    }


    public class GamePlayScreen : GameScreen
    {
        SpriteFont spriteFont;
        Texture2D playerTexture;
        Texture2D mouseTexture;
        Texture2D bulletTexture;
        Texture2D enemyTexture;
        Texture2D explosionTexture;
        Player player;
        List<Enemy> enemyList;
        List<Enemy> enemyDeleteList;
        List<Bullet> bulletDeleteList;
        List<Bullet> eBulletDeleteList;
        List<TileObject> objList;
        List<AnimatedSprite> animList;
        SoundEffect sound;
        float pauseAlpha;
        Random rand = new Random();

        ParticleComponent particleComponent;
        Texture2D heart;

        List<Texture2D> tileList = new List<Texture2D>();
        List<Texture2D> tileList2 = new List<Texture2D>();


        Vector2 previousPlayerPos = Vector2.Zero;
        Vector2 playerPos = Vector2.Zero;
        Vector2 playerSpawn;

        TileLayer backgroundLayer;
        Camera cam;

        ContentManager content;
        bool isLevelComplete;
        String name;

        bool isFirstLevel;
        int pLevel;
        int pScore;
        int pHealth;
        int pLives;
        GameDifficulty diff;
        Random random;
        bool levelstart;

        public GamePlayScreen(GameDifficulty diff)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            isFirstLevel = true;
            this.diff = diff;

            random = new Random();

        }

        public GamePlayScreen(GameDifficulty diff, String playerName)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            name = playerName;
            isFirstLevel = true;
            this.diff = diff;

            random = new Random();

       }

        public GamePlayScreen(GameDifficulty diff, string name, int score, int lives, int health, int level)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            this.name = name;
            pLevel = level;
            pScore = score;
            pHealth = health;
            pLives = lives;

            isFirstLevel = false;
            this.diff = diff;

            random = new Random();
        }

       
        public override void LoadContent()
        {

            System.Threading.Thread.Sleep(10);
            levelstart = true;
            animList = new List<AnimatedSprite>();

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");


            sound = content.Load<SoundEffect>("Sounds\\explode");

            explosionTexture = content.Load<Texture2D>("sprites/explosion2");
            playerTexture = content.Load<Texture2D>("manwalk");
            mouseTexture = content.Load<Texture2D>("cursor");
            playerSpawn = new Vector2(150,150);

           
            player = new Player(playerTexture, mouseTexture, playerSpawn, content);
            player.name = name;
            if (!isFirstLevel)
            {
                player.level = pLevel;
                player.score = pScore;
                player._health = pHealth;
                player._lives = pLives;
            }
            if (diff == GameDifficulty.HARD_MODE) player.MAX_SHOTS = 3;
            heart = content.Load<Texture2D>("1up");
            isLevelComplete = false;

            enemyDeleteList = new List<Enemy>();
            bulletDeleteList = new List<Bullet>();
            eBulletDeleteList = new List<Bullet>();

            cam = new Camera(new Vector2(0,0));


            particleComponent = new ParticleComponent(ScreenManager.Game);
            ScreenManager.Game.Components.Add(particleComponent);

            if (player.level == 1)
            {
                objList = new List<TileObject>();
                objList = TileHandler.getTileObjectLayout(content, "Content/Layers/LevelOneObjects.txt");

                enemyList = new List<Enemy>();
                if (diff == GameDifficulty.EASY_MODE ) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelOneEnemyEasy.txt");
                if (diff == GameDifficulty.HARD_MODE) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelOneEnemyHard.txt");

                spriteFont = content.Load<SpriteFont>("SpriteFont1");

                backgroundLayer = TileLayer.FromFile(content, "Content/Layers/LevelOneBackgroundLayer.txt");
            }

            if (player.level == 2)
            {
                objList = new List<TileObject>();
                objList = TileHandler.getTileObjectLayout(content, "Content/Layers/LevelTwoObjects.txt");

                enemyList = new List<Enemy>();
                if (diff == GameDifficulty.EASY_MODE) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelTwoEnemyEasy.txt");
                if (diff == GameDifficulty.HARD_MODE) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelTwoEnemyHard.txt");

                spriteFont = content.Load<SpriteFont>("SpriteFont1");

                backgroundLayer = TileLayer.FromFile(content, "Content/Layers/LevelOneBackgroundLayer.txt");
            }



            if (player.level == 3)
            {
                objList = new List<TileObject>();
                objList = TileHandler.getTileObjectLayout(content, "Content/Layers/LevelThreeObjects.txt");

                enemyList = new List<Enemy>();
                if (diff == GameDifficulty.EASY_MODE) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelThreeEnemyEasy.txt");
                if (diff == GameDifficulty.HARD_MODE) enemyList = TileHandler.getEnemyLayout(content, "Content/Layers/LevelThreeEnemyHard.txt");

                spriteFont = content.Load<SpriteFont>("SpriteFont1");

                backgroundLayer = TileLayer.FromFile(content, "Content/Layers/LevelTwoBackgroundLayer.txt");

                Emitter testEmitter2 = new Emitter();
                testEmitter2.Active = true;
                testEmitter2.TextureList.Add(content.Load<Texture2D>("raindrop"));
                testEmitter2.RandomEmissionInterval = new RandomMinMax(16.0d);
                testEmitter2.ParticleLifeTime = 1000;
                testEmitter2.ParticleDirection = new RandomMinMax(170);
                testEmitter2.ParticleSpeed = new RandomMinMax(10.0f);
                testEmitter2.ParticleRotation = new RandomMinMax(0);
                testEmitter2.RotationSpeed = new RandomMinMax(0f);
                testEmitter2.ParticleFader = new ParticleFader(false, true, 800);
                testEmitter2.ParticleScaler = new ParticleScaler(false, 1.0f);
                testEmitter2.Opacity = 255;

                particleComponent.particleEmitterList.Add(testEmitter2);
            }

            ScreenManager.AddScreen(new MessageBoxScreen("", mouseTexture), null);
        }


        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (levelstart && !otherScreenHasFocus)
            {
                ScreenManager.AddScreen(new MessageBoxScreen("", mouseTexture), null);
                levelstart = false;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (!otherScreenHasFocus)
            {

                Vector2 prevCamPos = cam.position;
                cam.Update(player, objList, backgroundLayer);

                if (!player.isAlive && player.pSprite.isFinished)
                {
                    cam.position = new Vector2(0, 0);
                    backgroundLayer = TileLayer.FromFile(content, "Content/Layers/LevelOneBackgroundLayer.txt");
                    player._playerPos = new Vector2(150, 150);
                }

                previousPlayerPos = player._playerPos;
                player.Update(gameTime, objList, cam);

                if (player._lives == 0)
                {
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.AddScreen(new GameOverScreen(player, GameOverScreen.gameOverType.GAMEOVER), null);
                }

                // COLLISION
                List<TileObject> objRemoveList = new List<TileObject>();
                List<TileObject> objCreateList = new List<TileObject>();
                List<AnimatedSprite> animRemoveList = new List<AnimatedSprite>();

                foreach (TileObject c in objList)
                {
                    if (cam.position != prevCamPos) c.objPos = c.objPos - (cam.position - prevCamPos);
                    c.Update();

                    if (player._playerRec.Intersects(c.objRec))
                    {
                        if (c.objType == ObjectType.PICKUP)
                        {
                            if (player._health >= 50) player._health = 100;
                            else player._health += 50;
                            objRemoveList.Add(c);
                        }


                    }

                    foreach (Bullet b in player.bulletList)
                    {
                        if (b.intersectsTarget(c.objRec))
                        {
                            if (c.objType == ObjectType.DESTRUCTIBLE)
                            {
                                bulletDeleteList.Add(b);
                                objRemoveList.Add(c);
                                animList.Add(new AnimatedSprite(explosionTexture, AnimatedSprite.AnimType.PLAY_ONCE, 6, 6, 0.02f, new Vector2(c.objPos.X + c.objRec.Width / 2, c.objPos.Y + c.objRec.Width / 2), 1.5f));
                                sound.Play();
                            }
                            if (c.objType == ObjectType.CRATE)
                            {
                                bulletDeleteList.Add(b);
                                objRemoveList.Add(c);
                                int test = 1;
                                if (this.diff == GameDifficulty.HARD_MODE) test = rand.Next(0, 7);
                                else test = rand.Next(0, 5);
                                if (test == 0)objCreateList.Add(new TileObject(heart, ObjectType.PICKUP, new Vector2(c.objRec.X, c.objRec.Y)));
                            }
                        }
                    }
                }



                foreach (AnimatedSprite a in animList)
                {
                    if (cam.position != prevCamPos) a.pos = a.pos - (cam.position - prevCamPos);
                    a.Update(gameTime);


                    if (a.spriteRec.Intersects(player._playerRec) && !a.playerDamaged)
                    {
                        player._health -= 30;
                        a.playerDamaged = true;
                    }
                    if (a.isFinished) animRemoveList.Add(a);
                }

                foreach (AnimatedSprite a in animRemoveList)
                {
                    animList.Remove(a);
                }

                foreach (TileObject c in objCreateList)
                {
                    objList.Add(c);
                }

                foreach (TileObject c in objRemoveList)
                {
                    objList.Remove(c);
                }



                foreach (Enemy e in enemyList)
                {
                    if (cam.position != prevCamPos) e._enemyPos = e._enemyPos - (cam.position - prevCamPos);
                    e.Update(player._playerRec, gameTime);

                    if (e.returnIntersect(player._playerRec))
                    {
                        if (e.canDamage) player._health -= 5;
                    }

                    foreach (Bullet b in e.bulletList)
                    {
                        if (cam.position != prevCamPos) b._bulletPos = b._bulletPos - (cam.position - prevCamPos);
                        if (b.intersectsTarget(player._playerRec))
                        {
                            player._health -= 10;
                            eBulletDeleteList.Add(b);
                        }
                    }

                    foreach (Bullet b in eBulletDeleteList)
                    {
                        e.bulletList.Remove(b);
                    }

                    foreach (Bullet b in player.bulletList)
                    {
                        if (b.intersectsTarget(e.enemyRec))
                        {
                            e.health -= 10;
                            bulletDeleteList.Add(b);


                        }
                    }

                    foreach (AnimatedSprite a in animList)
                    {
                        if (a.spriteRec.Intersects(e.enemyRec) && !a.damagedEnemies.Contains(e))
                        {
                            e.health -= 100;
                            a.damagedEnemies.Add(e);
                        }
                    }


                    if (e.health <= 0)
                    {
                        enemyDeleteList.Add(e);
                        if (e._type == EnemyType.normal) player.score += 10;
                        if (e._type == EnemyType.fast) player.score += 20;
                        if (e._type == EnemyType.hard) player.score += 50;
                        if (e._type == EnemyType.projectile) player.score += 70;
                        if (e._type == EnemyType.boss) player.score += 100;
                    }
                }


                foreach (Bullet b in bulletDeleteList)
                {
                    player.bulletList.Remove(b);
                }

                foreach (Enemy e in enemyDeleteList)
                {
                    enemyList.Remove(e);
                }


                //LEVEL COMPLETE

                if (enemyList.Count == 0)
                {
                    ScreenManager.Game.Components.Remove(particleComponent);
                    isLevelComplete = true;

                }


                if (isLevelComplete)
                {
                    ScreenManager.RemoveScreen(this);
                    ScreenManager.AddScreen(new LevelCompleteScreen(player, this.diff, 50.0f), null);
                }

            }

            if (player.level == 3)
            {
                Emitter t2 = particleComponent.particleEmitterList[0];
                t2.Position = new Vector2((float)random.NextDouble() * (ScreenManager.Game.GraphicsDevice.Viewport.Width), 0);
                if (t2.EmittedNewParticle)
                {
                    float f = MathHelper.ToRadians(t2.LastEmittedParticle.Direction + 180);
                    t2.LastEmittedParticle.Rotation = f;
                }
            }



        }



        public override void HandleInput(InputState input)
        {

            int playerIndex = 1;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];


            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);

            }


        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            backgroundLayer.Draw(spriteBatch, cam.position);


            foreach (TileObject c in objList)
            {
                c.Draw(spriteBatch);
            }

            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }

            foreach (AnimatedSprite a in animList)
            {
                a.Draw(spriteBatch, a.pos, 0.0f);
            }

            player.Draw(spriteBatch);

            int hx = 10;
            int hy = 50;


            spriteBatch.Begin();

            for (int i = 0; i < player._lives; i++)
            {
                spriteBatch.Draw(heart, new Rectangle(hx, hy, heart.Width / 2, heart.Height / 2), Color.White);
                hx += heart.Width / 2 + 10;

            }

            spriteBatch.DrawString(spriteFont, "Score : " + player.score, new Vector2(10, 90), Color.White);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }

        }
    }
}

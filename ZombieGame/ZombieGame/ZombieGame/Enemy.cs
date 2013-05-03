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
    public enum EnemyType
    {
        normal,
        fast,
        hard,
        projectile,
        boss
    }

    public class Enemy
    {
        SoundEffect growl;
        public EnemyType _type;
        public Texture2D enemyTexture;
        public Texture2D bulletTexture;
        public Vector2 _enemyPos;
        float _enemyRot;
        bool intersectsTarget;
        public Rectangle enemyRec;
        SoundEffect spit;

        public int health;
        int speed;
        int attackRadius;
        float scale;

        //damage control
        float damageTime;
        float elapsedFrameTime;
        public bool canDamage;
        public bool canAttack;

        Texture2D healthbarOut;
        Texture2D healthbarIn;

        public List<Bullet> bulletList;

        public Enemy(Texture2D enemyTexture, Vector2 enemyPos, EnemyType e, ContentManager content)
        {

            this.enemyTexture = enemyTexture;
            this._enemyPos = enemyPos;


            this._type = e;
            initializeEnemy(e);

            bulletList = new List<Bullet>();

            _enemyRot = 180.0f;
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, (int)(enemyTexture.Width * scale), (int)(enemyTexture.Height * scale));
            elapsedFrameTime = 0;
            canDamage = true;
            canAttack = false;

            spit = content.Load<SoundEffect>("Sounds\\zombiespit");
            this.healthbarIn = content.Load<Texture2D>("healthbarinside");
            this.healthbarOut = content.Load<Texture2D>("healthbaroutside");
            this.bulletTexture = content.Load<Texture2D>("enemyBullet");
            this.growl = content.Load<SoundEffect>("Sounds\\growl");
        }


        public void Update(Rectangle targetRec, GameTime gameTime)
        {
            Vector2 targetPos = new Vector2(targetRec.X, targetRec.Y);
            enemyRec = new Rectangle((int)_enemyPos.X, (int)_enemyPos.Y, enemyTexture.Width * 3, enemyTexture.Height * 3);

            Vector2 diff = _enemyPos - targetPos;
            bool prevAttack = canAttack;

            if (diff.X < attackRadius && diff.Y < attackRadius)
            {
                if (!canAttack) growl.Play();
                canAttack = true;
            }
            else canAttack = false;

            if (!targetRec.Intersects(enemyRec) && canAttack)
            {

                Vector2 direction = _enemyPos - targetPos;
                direction.Normalize();
                _enemyRot = (float)Math.Atan2(direction.Y, direction.X) - (float)(Math.PI * 1.0f);

                if (_type != EnemyType.projectile)
                {
                    _enemyPos -= direction * speed;
                }

                else {

                    if (canDamage)
                    {
                        Bullet b = new Bullet(bulletTexture, new Vector2(targetRec.X, targetRec.Y), this._enemyPos);
                        b._bulletRot = (float)Math.Atan2(direction.Y, direction.X) - (float)(Math.PI * 1.0f);
                        b.scale = 0.8f;
                        bulletList.Add(b);
                        spit.Play();
                    }
                    canDamage = false;
                }

            }
            else
            {
                canDamage = false;
            }

            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedFrameTime > damageTime)
            {
                elapsedFrameTime = 0.0f;
                canDamage = true;
            }

            foreach (Bullet b in bulletList)
            {
                b.Update();
            }


        }


        public void initializeEnemy(EnemyType e)
        {
            if (e == EnemyType.normal)
            {
                this.health = 10;
                this.speed = 1;
                this.attackRadius = 300;
                this.damageTime = 1.0f;
                this.scale = 3.0f;
            }

            if (e == EnemyType.fast)
            {
                this.health = 10;
                this.speed = 3;
                this.attackRadius = 300;
                this.damageTime = 1.0f;
                this.scale = 3.0f;
            }

            if (e == EnemyType.hard)
            {
                this.health = 10;
                this.speed = 3;
                this.attackRadius = 300;
                this.damageTime = 0.5f;
                this.scale = 3.0f;
            }

            if (e == EnemyType.boss)
            {
                this.health = 500;
                this.speed = 3;
                this.attackRadius = 300;
                this.damageTime = 0.5f;
                this.scale = 4.0f;
            }

            if (e == EnemyType.projectile)
            {
                this.health = 30;
                this.speed = 5;
                this.attackRadius = 400;
                this.damageTime = 2.0f;
                this.scale = 3.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (this._type == EnemyType.boss)
            {
                spriteBatch.Draw(healthbarIn, new Rectangle((int)_enemyPos.X - 30, (int)_enemyPos.Y - enemyTexture.Height - 40, healthbarIn.Width / 4, healthbarIn.Height / 4), Color.Gray);
                spriteBatch.Draw(healthbarIn, new Rectangle((int)_enemyPos.X - 30, (int)_enemyPos.Y - enemyTexture.Height - 40, (int)(healthbarIn.Width / 4 * ((double)this.health / 500)), healthbarIn.Height / 4), Color.Red);
                spriteBatch.Draw(healthbarOut, new Rectangle((int)_enemyPos.X - 30, (int)_enemyPos.Y - enemyTexture.Height - 40, healthbarOut.Width / 4, healthbarOut.Height / 4), Color.White);
            }
            spriteBatch.Draw(enemyTexture, _enemyPos, null, Microsoft.Xna.Framework.Color.White, _enemyRot, new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.End();

            foreach (Bullet b in bulletList)
            {
                b.Draw(spriteBatch);
            }

        }

        public bool returnIntersect(Rectangle targetRec) {
            if (targetRec.Intersects(enemyRec)) return true;
            else return false;
        }

    }
}

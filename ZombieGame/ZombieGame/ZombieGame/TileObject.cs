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
    public enum ObjectType { 
        NORMAL,
        COLLISION,
        PICKUP,
        DESTRUCTIBLE
    }

    class TileObject
    {
        Texture2D objTexture;
        public Vector2 objPos;
        public Rectangle objRec;
        public ObjectType objType;


        public TileObject()
        {
        }

        public TileObject(Texture2D text, ObjectType objType, Vector2 pos)
        {
            this.objType = objType;
            this.objTexture = text;
            this.objPos = pos;
            this.objRec = new Rectangle((int)objPos.X, (int)objPos.Y, objTexture.Width, objTexture.Height);
        }

        public void Update()
        {
            this.objRec = new Rectangle((int)objPos.X, (int)objPos.Y, objTexture.Width, objTexture.Height); 
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(objTexture, objRec, Color.White);
            sb.End();
        }
    }
}

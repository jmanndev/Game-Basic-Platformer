using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Platformer_Tutorial
{
    class Sprite
    {
        SpriteBatch spriteBatch { get; set; }

        public Vector2 position { get; set; }
        Texture2D texture { get; set; }
        public Rectangle rectangle
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public Sprite(Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
        {
            this.texture = texture;
            this.position = position;
            this.spriteBatch = spriteBatch;
        }

        public virtual void Draw()
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}

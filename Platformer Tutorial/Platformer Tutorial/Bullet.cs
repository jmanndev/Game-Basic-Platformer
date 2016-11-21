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
    class Bullet : Sprite
    {
        Vector2 direction;
        float speed {get; set;}
        public int strength { get; set; }

        public Bullet(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, Vector2 direction, float speed, int strength)
            : base(texture, position, spriteBatch)
        {
            this.direction = direction;
            this.speed = speed;
            this.strength = strength;
        }

        public void Update(GameTime gameTime)
        {
            position += speed * direction;
        }
    }
}

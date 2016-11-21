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
    class Tile : Sprite
    {
        public bool isBlocked { get; set; }

        public Tile(Texture2D texture, Vector2 position, SpriteBatch spriteBatch, bool isBlocked)
            : base(texture, position, spriteBatch)
        {
            this.isBlocked = isBlocked;
        }

        public override void Draw()
        {
            if (isBlocked)
                base.Draw();
        }
    }
}

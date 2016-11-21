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
    class Player : Sprite
    {
        public SimplePlatformer simplePlatformer;

        public Vector2 movement { get; set; }
        Vector2 oldPosition;
        Vector2 direction = Vector2.UnitX;

        Keys keyJump = Keys.Space;
        Keys keyShoot = Keys.E;
        Keys keyLeft = Keys.A;
        Keys keyRight = Keys.D;

        float bulletTime = 0.2f;
        float bulletCooldownTime = 0.1f;

        int bulletGroupMax = 1;
        int bulletGroup = 0;
        float bulletSpeed = 3.0f;
        int bulletStrength = 30;

        public Player(SimplePlatformer game, Texture2D texture, Vector2 position, SpriteBatch spriteBatch)
            : base(texture, position, spriteBatch)
        {
            this.simplePlatformer = game;
        }

        public void Update(GameTime gameTime)
        {
            UpdateControls(gameTime);
            AffectWithGravity();
            SimulateFriction();
            MoveAsFarAsPossible(gameTime);
            StopMovingIfBlocked();
            UpdateDirection();
        }

        void UpdateControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //left
            if (keyboardState.IsKeyDown(keyLeft))
            {
                if (!IsOnFirmGround())
                {
                    if (IsInAirFalling())
                        movement += new Vector2(-0.55f, 0);
                    else
                        movement += new Vector2(-0.65f, 0);
                }
                else
                    movement += new Vector2(-1, 0);
            }

            //right
            if (keyboardState.IsKeyDown(keyRight))
            {
                if (!IsOnFirmGround())
                {
                    if (IsInAirFalling())
                        movement -= new Vector2(-0.55f, 0);
                    else
                        movement -= new Vector2(-0.65f, 0);
                }
                else
                    movement -= new Vector2(-1, 0);
            }

            //jump
            if (keyboardState.IsKeyDown(keyJump) && IsOnFirmGround())
                movement -= Vector2.UnitY * 25;
            
            bulletCooldownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Keyboard.GetState().IsKeyDown(keyShoot))
            {
                Shoot(gameTime);
            }
        }

        void Shoot(GameTime gameTime)
        {
            if (bulletCooldownTime < 0.0f)
            {
                Vector2 shootPosition;
                //find pos.x
                if (direction.X > 0)
                    shootPosition.X = position.X + base.rectangle.Width - simplePlatformer.bulletTexture.Width / 2;
                else
                    shootPosition.X = position.X - simplePlatformer.bulletTexture.Width / 2;

                shootPosition.Y = position.Y + (base.rectangle.Height / 2) - (simplePlatformer.bulletTexture.Height / 2);

                Vector2 shootDirection = (movement + direction * bulletSpeed) * Vector2.UnitX;

                simplePlatformer.Shoot(shootPosition, shootDirection, bulletSpeed, bulletStrength);
                bulletGroup++;

                if (bulletGroup >= bulletGroupMax)
                {
                    bulletCooldownTime = bulletTime;
                    bulletGroup = 0;
                }
            }

        }

        private void AffectWithGravity()
        {
            movement += new Vector2(0, .5f);
        }

        void MoveAsFarAsPossible(GameTime gameTime)
        {
            oldPosition = position;
            UpdatePositionBasedOnMovement(gameTime);
            position = Board.CurrentBoard.WhereCanIGetTo(oldPosition, position, rectangle);
        }

        void SimulateFriction()
        {
            if (IsOnFirmGround())
                movement -= movement * Vector2.One * .08f;
            else
                movement -= movement * Vector2.One * .06f;
        }

        void UpdatePositionBasedOnMovement(GameTime gameTime)
        {
            position += movement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 20;
        }

        public bool IsOnFirmGround()
        {
            Rectangle onePixelLower = rectangle;
            onePixelLower.Offset(0, 1);
            return !Board.CurrentBoard.HasRoomForRectangle(onePixelLower);
        }

        public bool IsInAirFalling()
        {
            if (movement.Y > 0 && !IsOnFirmGround())
                return true;
            else
                return false;
        }

        void StopMovingIfBlocked()
        {
            Vector2 lastMovement = position - oldPosition;
            if (lastMovement.X == 0) { movement *= Vector2.UnitY; }
            if (lastMovement.Y == 0) { movement *= Vector2.UnitX; }
        }

        void UpdateDirection()
        {
            if (movement.X > 0)
                direction.X = 1;
            else if (movement.X < 0)
                direction.X = -1;

            if (movement.Y > 0)
                direction.Y = 1;
            else if (movement.Y < 0)
                direction.Y = -1;
        }
    }
}

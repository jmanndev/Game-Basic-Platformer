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
    public class SimplePlatformer : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int windowWidth = 1200;
        int windowHeight = 800;

        Board board;
        int boardColumns;
        int boardRows;

        Texture2D tileTexture;
        Texture2D playerTexture;
        public Texture2D bulletTexture;

        Player player;
        List<Bullet> bulletList = new List<Bullet>();
        List<Enemy> enemyList = new List<Enemy>();

        private SpriteFont debugFont;

        public SimplePlatformer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.PreferredBackBufferWidth = windowWidth;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileTexture = Content.Load<Texture2D>("tile");
            playerTexture = Content.Load<Texture2D>("jumper");
            bulletTexture = Content.Load<Texture2D>("bullet");
            debugFont = Content.Load<SpriteFont>("DebugFont");

            BuildBoard();
            player = new Player(this, playerTexture, Board.CurrentBoard.tileGrid[1,1].position + Vector2.One * ((tileTexture.Width - playerTexture.Width) /2) , spriteBatch);
        }

        private void BuildBoard()
        {
            boardColumns = windowWidth / tileTexture.Width;
            boardRows = windowHeight / tileTexture.Height;
            Vector2 boardOffset;
            boardOffset.X = (windowWidth - tileTexture.Width * boardColumns) / 2;
            boardOffset.Y = (windowHeight - tileTexture.Height * boardRows) / 2;
            board = new Board(spriteBatch, tileTexture, boardColumns, boardRows, boardOffset);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);

            player.Update(gameTime);
            CheckKeyboardAndReact();

            for (int i = bulletList.Count-1; i >=0 ; i--)
            {
                bulletList[i].Update(gameTime);

                if (bulletList[i].position.X < 0 ||
                    bulletList[i].position.X > graphics.PreferredBackBufferWidth ||
                    bulletList[i].position.Y < 0 ||
                    bulletList[i].position.Y > graphics.PreferredBackBufferHeight)
                {
                    bulletList.RemoveAt(i);
                }
            }

            CheckBulletCollisions();
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5))
                RestartGame();
            if (state.IsKeyDown(Keys.Escape))
                Exit();
        }

        private void RestartGame()
        {
            Board.CurrentBoard.CreateNewBoard();
            PutPlayerInTopLeftCorner();
        }

        private void PutPlayerInTopLeftCorner()
        {
            player.position = Board.CurrentBoard.tileGrid[1, 1].position + Vector2.One * ((tileTexture.Width - playerTexture.Width) / 2);
            player.movement = Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            spriteBatch.Begin();

            board.Draw();
            player.Draw();

            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                bulletList[i].Draw();
            }

            string positionInText =
                string.Format("Pos of Player: ({0:0.0}, {1:0.0})", player.position.X, player.position.Y);
            string movementInText =
                string.Format("Movement: ({0:0.0}, {1:0.0})", player.movement.X, player.movement.Y);
            string firmGroundInText =
                string.Format("On Firm Ground: ({0:0})", player.IsOnFirmGround().ToString());
            spriteBatch.DrawString(debugFont, positionInText, new Vector2(10, 0), Color.White);
            spriteBatch.DrawString(debugFont, movementInText, new Vector2(10, 20), Color.White);
            spriteBatch.DrawString(debugFont, firmGroundInText, new Vector2(10, 35), Color.White);

            spriteBatch.End();
        }

        void CheckBulletCollisions()
        {
            for (int i = bulletList.Count - 1; i >= 0; i--)
            {
                foreach (var tile in Board.CurrentBoard.tileGrid)
                {
                    //  float distance = (bulletList[i].position - enemyList[j].position).Length();
                    // if (distance < (bulletList[i].height / 2.0f + enemyList[j].height / 2.0f))
                    if (bulletList[i].rectangle.Intersects(tile.rectangle) && tile.isBlocked)
                    {
                        bulletList.RemoveAt(i);

                        break;
                    }
                }
            }
        }

        public void Shoot(Vector2 pos, Vector2 dir, float speed, int strength)
        {
            Bullet b = new Bullet(bulletTexture,pos,spriteBatch,dir,speed,strength);
            bulletList.Add(b);
        }
    }
}
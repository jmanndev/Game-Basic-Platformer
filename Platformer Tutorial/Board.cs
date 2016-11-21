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
    class Board
    {
        public static Board CurrentBoard { get; private set; }
        Vector2 topLeftBlockPos;
        public Tile[,] tileGrid { get; set; }
        Texture2D tileTexture { get; set; }
        SpriteBatch spriteBatch { get; set; }
        int columns { get; set; }
        int rows { get; set; }

        private Random rand = new Random();
        int percOfTiles = 20;

        public Board(SpriteBatch spriteBatch, Texture2D tileTexture, int columns, int rows, Vector2 topLeftBlockPos)
        {
            this.spriteBatch = spriteBatch;
            this.tileTexture = tileTexture;
            this.topLeftBlockPos = topLeftBlockPos;
            this.columns = columns;
            this.rows = rows;
            tileGrid = new Tile[columns, rows];
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            BuildTileGrid();
            SetAllBorderTilesBlocked();
            SetTopLeftTileTileUnblocked();
        }

        void BuildTileGrid()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * tileTexture.Width, y * tileTexture.Height) + topLeftBlockPos;
                    tileGrid[x, y] = new Tile(tileTexture, tilePosition, spriteBatch, 
                        rand.Next((int)ConvertPercentageToRand(percOfTiles)) == 0);
                }
            }
        }

        void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    if (x == 0 || x == columns - 1 || y == 0 || y == rows - 1)
                    {
                        tileGrid[x, y].isBlocked = true;
                    }
                }
            }
        }

        void SetTopLeftTileTileUnblocked()
        {
            tileGrid[1, 1].isBlocked = false;
        }

        //used in BuildTileGrid() method using percOfTiles global var
        float ConvertPercentageToRand(int number)
        {
            return 100 / number;
        }

        public void Draw()
        {
            foreach (var tile in tileGrid)
            {
                tile.Draw();
            }
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var tile in tileGrid)
            {
                if (tile.isBlocked && tile.rectangle.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle rectangleToCheck)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, rectangleToCheck);

            for (int i = 1; i <= move.numberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.oneStep * i;
                Rectangle newBoundary =
                    CreateRectangleAtPosition(positionToTry, rectangleToCheck.Width, rectangleToCheck.Height);
                if (HasRoomForRectangle(newBoundary))
                    move.furthestAvailableLocationSoFar = positionToTry;
                else
                {
                    if (move.isDiagonalMove)
                    {
                        move.furthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.furthestAvailableLocationSoFar;
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.isDiagonalMove)
            {
                int stepsLeft = wrapper.numberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.oneStep.X * Vector2.UnitX * stepsLeft;
                Vector2 finalPositionIfMovingHorizontally = wrapper.furthestAvailableLocationSoFar + remainingHorizontalMovement;
                wrapper.furthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.furthestAvailableLocationSoFar, finalPositionIfMovingHorizontally, wrapper.rectangleToCheck);

                Vector2 remainingVerticalMovement = wrapper.oneStep.Y * Vector2.UnitY * stepsLeft;
                Vector2 finalPositionIfMovingVertically = wrapper.furthestAvailableLocationSoFar + remainingVerticalMovement;
                wrapper.furthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.furthestAvailableLocationSoFar, finalPositionIfMovingVertically, wrapper.rectangleToCheck);
            }

            return wrapper.furthestAvailableLocationSoFar;
        }

        Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }
    }
}

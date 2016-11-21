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
    public struct MovementWrapper
    {
        public Vector2 movementToTry { get; private set; }
        public Vector2 furthestAvailableLocationSoFar { get; set; }
        public int numberOfStepsToBreakMovementInto { get; private set; }
        public bool isDiagonalMove { get; private set; }
        public Vector2 oneStep { get; private set; }
        public Rectangle rectangleToCheck { get; set; }

        public MovementWrapper(Vector2 originalPosition, Vector2 destination, Rectangle rectangleToTry)
            : this()
        {
            movementToTry = destination - originalPosition;
            furthestAvailableLocationSoFar = originalPosition;
            numberOfStepsToBreakMovementInto = (int)(movementToTry.Length() * 2) + 1;
            isDiagonalMove = movementToTry.X != 0 && movementToTry.Y != 0;
            oneStep = movementToTry / numberOfStepsToBreakMovementInto;
            this.rectangleToCheck = rectangleToTry;
        }

        
    }
}

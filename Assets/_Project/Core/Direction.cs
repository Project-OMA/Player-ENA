using System;
using UnityEngine;

namespace ENA
{
    public static class Direction
    {
        #region Enums
        public enum Basic
        {
            Left, UpLeft, Up, UpRight, Right, DownRight, Down, DownLeft
        }

        public enum Cardinal
        {
            North, West, South, East
        }
        #endregion
        #region Static Methods
        public static Cardinal CardinalFor(float angle)
        {
            angle = ConvertToRange(angle);

            if (315 <= angle && angle < 360 || 0 <= angle && angle < 45) return Cardinal.East;
            else if(45 <= angle && angle < 135) return Cardinal.North;
            else if(135 <= angle && angle < 225) return Cardinal.West;
            else if(225 <= angle && angle < 315) return Cardinal.South;

            return default;
        }

        public static float ConvertToRange(float angleDegrees)
        {
            if (angleDegrees < 0) {
                angleDegrees = 360 - (Mathf.Abs(angleDegrees) % 360);
            } else if (angleDegrees > 360) {
                angleDegrees %= 360;
            }

            return angleDegrees;
        }

        public static Basic DirectionFor(float angle)
        {
            angle = ConvertToRange(angle);

            if (337.5 <= angle && angle < 360 || 0 <= angle && angle < 22.5) return Basic.Right;
            else if (22.5 <= angle && angle < 67.5) return Basic.UpRight;
            else if (67.5 <= angle && angle < 112.5) return Basic.Up;
            else if (112.5 <= angle && angle < 157.5) return Basic.UpLeft;
            else if (157.5 <= angle && angle < 202.5) return Basic.Left;
            else if (202.5 <= angle && angle < 247.5) return Basic.DownLeft;
            else if (247.5 <= angle && angle < 292.5) return Basic.Down;
            else if (292.5 <= angle && angle < 337.5) return Basic.DownRight;

            return default;
        }

        public static Cardinal DetermineCardinal(Vector3 reference, Vector3 target)
        {
            var signedAngle = reference.AngleForDirection(target);
            return CardinalFor(signedAngle);
        }

        public static Basic DetermineDirection(Vector3 reference, Vector3 target)
        {
            var signedAngle = reference.AngleForDirection(target);
            return DirectionFor(signedAngle);
        }
        #endregion
        #region Extensions
        public static Vector2 ToVector2(this Cardinal cardinal)
        {
            return cardinal switch
            {
                Cardinal.North => Vector2.up,
                Cardinal.South => Vector2.down,
                Cardinal.East => Vector2.right,
                Cardinal.West => Vector2.left,
                _ => Vector2.zero,
            };
        }

        public static float ToAngle(this Cardinal cardinal)
        {
            return cardinal switch
            {
                Cardinal.North => 0,
                Cardinal.South => 180,
                Cardinal.East => 90,
                Cardinal.West => 270,
                _ => 0.0f,
            };
        }
        #endregion
    }
}
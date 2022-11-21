using System;
using UnityEngine;

namespace ENA.Utilities
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
            angle = (angle+360) % 360;

            if (315 <= angle && angle < 360 || 0 <= angle && angle < 45) return Cardinal.East;
            else if(45 <= angle && angle < 135) return Cardinal.South;
            else if(135 <= angle && angle < 225) return Cardinal.West;
            else if(225 <= angle && angle < 315) return Cardinal.North;

            return default;
        }

        public static Basic DirectionFor(float angle)
        {
            angle = (angle+360) % 360;

            if (337.5 <= angle && angle < 360 || 0 <= angle && angle < 22.5) return Basic.Right;
            else if (22.5 <= angle && angle < 67.5) return Basic.DownRight;
            else if (67.5 <= angle && angle < 112.5) return Basic.Down;
            else if (112.5 <= angle && angle < 157.5) return Basic.DownLeft;
            else if (157.5 <= angle && angle < 202.5) return Basic.Left;
            else if (202.5 <= angle && angle < 247.5) return Basic.UpLeft;
            else if (247.5 <= angle && angle < 292.5) return Basic.Up;
            else if (292.5 <= angle && angle < 337.5) return Basic.UpRight;

            return default;
        }

        public static Cardinal DetermineCardinal(Vector3 reference, Vector3 target)
        {
            var signedAngle = reference.GetAngleOnAxis(target, Vector3.up);
            return CardinalFor(signedAngle+360);
        }

        public static Basic DetermineDirection(Vector3 reference, Vector3 target)
        {
            var signedAngle = reference.GetAngleOnAxis(target, Vector3.up);
            return DirectionFor(signedAngle+360);
        }
        #endregion
        #region Extensions
        public static Vector2 ToVector2(this Cardinal cardinal)
        {
            switch(cardinal) {
                case Cardinal.North:
                    return Vector2.up;
                case Cardinal.South:
                    return Vector2.down;
                case Cardinal.East:
                    return Vector2.right;
                case Cardinal.West:
                    return Vector2.left;
                default:
                    return Vector2.zero;
            }
        }

        public static float ToAngle(this Cardinal cardinal)
        {
            switch(cardinal) {
                case Cardinal.North:
                    return 0;
                case Cardinal.South:
                    return 180;
                case Cardinal.East:
                    return 90;
                case Cardinal.West:
                    return 270;
                default:
                    return 0;
            }
        }

        public static Cardinal RotateLeft(Cardinal direction)
        {
            throw new System.NotImplementedException();
        }

        public static Cardinal RotateRight(Cardinal direction)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
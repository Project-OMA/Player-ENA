namespace ENA.Physics
{
    public class Direction
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
            angle = angle % 360;
            if(angle >= 315 && angle < 360) {
                return Cardinal.East;
            } else if(angle >= 0 && angle < 45) {
                return Cardinal.East;
            } else if(angle >= 45 && angle < 135) {
                return Cardinal.South;
            } else if(angle >= 135 && angle < 225) {
                return Cardinal.West;
            } else if(angle >= 225 && angle < 315) {
                return Cardinal.North;
            }

            return default;
        }
        #endregion
    }
}
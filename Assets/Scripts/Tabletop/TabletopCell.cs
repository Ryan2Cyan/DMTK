using UnityEngine;

namespace Tabletop
{
    public class TabletopCell
    {
        public TabletopCell(Vector2 position, bool isOccupied = false)
        {
            Position = position;
            IsOccupied = isOccupied;
        }

        public Vector2 Position;
        public bool IsOccupied;
    }
}

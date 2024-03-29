using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tabletop.Tabletop
{
    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
    
    public class TabletopCell : MonoBehaviour
    {
        [FormerlySerializedAs("EnabledColour")] public Color OccupiedColour;
        [FormerlySerializedAs("EnabledSprite")] public Sprite OccupiedSprite;
        public Vector2Int Coordinate;
        public Vector2 Position;
        public bool IsOccupied;

        [Header("Pathfinding Sprites")]
        public Color PathColour;
        public Sprite PathStartIdle;
        public Sprite PathStart;
        public Sprite PathSprite;
        public Sprite PathEnd;
        
        [HideInInspector] public SpriteRenderer Sprite0;
        [HideInInspector] public SpriteRenderer Sprite1;

        // Cell States:
        public readonly OccupiedCell OccupiedState = new ();
        public readonly DisabledCell DisabledState = new ();
        public readonly PathCell PathState = new ();
        public readonly PathStartCell PathStartState = new ();
        public readonly PathStartIdleCell PathStartIdleState = new ();
        public readonly PathEndCell PathEndState = new ();
        
        private ITableTopCellState _currentState;

        #region UnityFunctions
        private void Awake()
        {
            // Two sprite renders needed for distance path (need two lines with separate rotations):
            Sprite0 = transform.GetChild(0).GetComponent<SpriteRenderer>();
            var transform0 = Sprite0.transform;
            var eulerAngles0 = transform0.eulerAngles;
            eulerAngles0 = new Vector3(90f, eulerAngles0.y, eulerAngles0.z);
            transform0.eulerAngles = eulerAngles0;
            
            Sprite1 = transform.GetChild(1).GetComponent<SpriteRenderer>();
            var transform1 = Sprite0.transform;
            var eulerAngles1 = transform1.eulerAngles;
            eulerAngles1 = new Vector3(90f, eulerAngles1.y, eulerAngles1.z);
            transform1.eulerAngles = eulerAngles1;
            _currentState = DisabledState;
        }
        #endregion

        #region PublicFunctions
        public void SetCellState(ITableTopCellState state)
        {
            _currentState?.OnExit(this);
            _currentState = state;
            _currentState.OnStart(this);
        }
        
        internal static void SetSprite(SpriteRenderer spriteRenderer, Sprite sprite, Color color)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
        }

        /// <summary>Get tabletop space rotation corresponding to a direction. Rotations only occur on y-axis.</summary>
        /// <param name="direction">Facing direction.</param>
        /// <returns>Y-axis tabletop space rotation.</returns>
        internal static float GetTabletopSpaceRotation(Direction direction)
        {
            float yRotation;
            switch (direction)
            {
                case Direction.None: return 0;
                case Direction.Up:
                {
                    yRotation = 90f;
                } break;
                case Direction.Down:
                {
                    yRotation = -90f;
                } break;
                case Direction.Right:
                {
                    yRotation = 180f;
                } break;
                case Direction.Left:
                {
                    yRotation = 0f;
                } break;
                case Direction.TopLeft:
                {
                    yRotation = 45f;
                } break;
                case Direction.TopRight:
                {
                    yRotation = 135f;
                } break;
                case Direction.BottomLeft:
                {
                    yRotation = -45f;
                } break;
                case Direction.BottomRight:
                {
                    yRotation = 225f;
                } break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            return yRotation;
        }

        /// <summary>Rotate sprite in tabletop space (y-axis only).</summary>
        /// <param name="transform">Sprite transform.</param>
        /// <param name="angle">Tabletop space y-axis rotation [see TabletopCell.GetTabletopSpaceRotation].</param>
        internal static void RotateSprite(Transform transform, float angle)
        {
            var eulerAngles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(eulerAngles.x, angle, eulerAngles.z);
        }
        #endregion
    }

    public interface ITableTopCellState
    {
        public void OnStart(TabletopCell cell) {}
        public void OnExit(TabletopCell cell) {}
    }

    public class DisabledCell : ITableTopCellState
    {
        public void OnStart(TabletopCell cell)
        {
            cell.IsOccupied = false;
            cell.Sprite0.enabled = false;
            cell.Sprite1.enabled = false;
        }
    }
    
    public class OccupiedCell : ITableTopCellState
    {
        public void OnStart(TabletopCell cell)
        {
            cell.IsOccupied = true;
            TabletopCell.SetSprite(cell.Sprite0, cell.OccupiedSprite, cell.OccupiedColour);
        }
    }
    
    public class PathCell : ITableTopCellState
    {
        public Direction Direction0;
        public Direction Direction1;
        
        public void OnStart(TabletopCell cell)
        {
            // Handle first sprite:
            cell.Sprite0.transform.localScale = new Vector3(1f, 0.4f, 1f);
            TabletopCell.SetSprite(cell.Sprite0, cell.PathSprite, cell.PathColour);
            TabletopCell.RotateSprite(cell.Sprite0.transform, TabletopCell.GetTabletopSpaceRotation(Direction0));
            
            // Handle second sprite:
            cell.Sprite1.transform.localScale = new Vector3(1f, 0.4f, 1f);
            TabletopCell.SetSprite(cell.Sprite1, cell.PathSprite, cell.PathColour);
            TabletopCell.RotateSprite(cell.Sprite1.transform, TabletopCell.GetTabletopSpaceRotation(Direction1));
        }
        
        public void OnExit(TabletopCell cell)
        {
            var transform0 = cell.Sprite0.transform;
            transform0.localScale = Vector3.one;
            TabletopCell.RotateSprite(transform0, 0f);
            
            var transform1 = cell.Sprite1.transform;
            transform1.localScale = Vector3.one;
            TabletopCell.RotateSprite(transform1, 0f);
            cell.Sprite1.enabled = false;
        }
    }
    
    public class PathStartIdleCell : ITableTopCellState
    {
        public void OnStart(TabletopCell cell)
        {
            cell.Sprite0.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            TabletopCell.SetSprite(cell.Sprite0, cell.PathStartIdle, cell.PathColour);
        }
        
        public void OnExit(TabletopCell cell)
        {
          cell.Sprite0.transform.localScale = Vector3.one;
        }
    }
    
    public class PathStartCell : ITableTopCellState
    {
        public Direction Direction;
        public void OnStart(TabletopCell cell)
        {
            cell.Sprite0.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            TabletopCell.SetSprite(cell.Sprite0, cell.PathStart, cell.PathColour);
            TabletopCell.RotateSprite(cell.Sprite0.transform, TabletopCell.GetTabletopSpaceRotation(Direction));
        }
        
        public void OnExit(TabletopCell cell)
        {
            var transform = cell.Sprite0.transform;
            transform.localScale = Vector3.one;
            TabletopCell.RotateSprite(transform, 0f);
        }
    }
    
    public class PathEndCell : ITableTopCellState
    {
        public Direction Direction;
        public void OnStart(TabletopCell cell)
        {
            cell.Sprite0.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
            TabletopCell.SetSprite(cell.Sprite0, cell.PathEnd, cell.PathColour);
            TabletopCell.RotateSprite(cell.Sprite0.transform, TabletopCell.GetTabletopSpaceRotation(Direction));
        }
        
        public void OnExit(TabletopCell cell)
        {
            var transform = cell.Sprite0.transform;
            transform.localScale = Vector3.one;
            TabletopCell.RotateSprite(transform, 0f);
        }
    }
}

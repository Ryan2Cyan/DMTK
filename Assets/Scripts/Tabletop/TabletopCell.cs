using System;
using UnityEngine;

namespace Tabletop
{
    public enum CellAppearance
    {
        Enabled,
        Disabled,
        PathStart,
        PathStartIdle,
        Path,
        PathEnd
    }

    public enum Direction
    {
        None,
        Up,
        Down,
        Right,
        Left
    }
    
    public class TabletopCell : MonoBehaviour
    {
        public Color EnabledColour;
        public Vector2Int Coordinate;
        public Vector2 Position;
        public bool IsOccupied;

        [Header("Pathfinding Sprites")]
        public Color PathColour;
        public Sprite PathStartIdle;
        public Sprite PathStart;
        public Sprite Path;
        public Sprite PathEnd;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Set the state of the cell, this determines what the cell's sprite will display. 
        /// </summary>
        /// <param name="state">New state to set.</param>
        /// <param name="direction">New direction for the sprite (used for distance path finding).</param>
        public void SetState(CellAppearance state, Direction direction = Direction.None)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            _spriteRenderer.size = new Vector2(1f, 1f);
            gameObject.SetActive(true);
            
            switch (state)
            {
                case CellAppearance.Enabled:
                {
                    _spriteRenderer.color = EnabledColour;
                    IsOccupied = true;
                } break;
                
                case CellAppearance.Disabled:
                {
                    gameObject.SetActive(false);
                    IsOccupied = false;
                } break;
                
                case CellAppearance.PathStartIdle:
                {
                    SetSprite(PathStartIdle, PathColour);
                } break;
                
                case CellAppearance.PathStart:
                {
                    transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                    _spriteRenderer.size = new Vector2(1.25f, 1f);
                    SetSprite(PathStart, PathColour);
                    RotateSprite(direction);
                } break;
                
                case CellAppearance.Path:
                {
                    // _spriteRenderer.size = new Vector2(1f, 0.4f);
                    SetSprite(Path, PathColour);
                    var eulerAngles = transform.eulerAngles;
                    eulerAngles = direction switch
                    {
                        Direction.Up or Direction.Down => new Vector3(eulerAngles.x, 0,
                            eulerAngles.z),
                        Direction.Right or Direction.Left => new Vector3(eulerAngles.x, 90,
                            eulerAngles.z),
                        _ => eulerAngles
                    };
                    transform.eulerAngles = eulerAngles;
                } break;
                
                case CellAppearance.PathEnd:
                {
                    transform.localScale = new Vector3(0.75f, 0.75f, 1f);
                    SetSprite(PathEnd, PathColour);
                    RotateSprite(direction);
                } break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void SetSprite(Sprite sprite, Color color)
        {
            _spriteRenderer.sprite = sprite;
            _spriteRenderer.color = color;
        }

        private void RotateSprite(Direction direction)
        {
            var yRotation = 0f;
            switch (direction)
            {
                case Direction.None: return;
                case Direction.Up: yRotation = 90f;
                    break;
                case Direction.Down: yRotation = -90f;
                    break;
                case Direction.Right: yRotation = 180f;
                    break;
                case Direction.Left: yRotation = 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
        }
    }
}

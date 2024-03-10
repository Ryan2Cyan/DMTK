using System;
using UnityEngine;

namespace Tabletop
{
    public enum CellState
    {
        Enabled,
        Disabled,
        Occupied
    }
    
    public class TabletopCell : MonoBehaviour
    {
        public Color EnabledColour;
        public Color OccupiedColour;
        public Vector2Int Coordinate;
        public Vector2 Position;
        public bool IsOccupied;
        
        private SpriteRenderer _spriteRenderer;
        private CellState _state;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Set the state of the cell, this determines what the cell's sprite will display. 
        /// </summary>
        /// <param name="state">New state to set.</param>
        public void SetState(CellState state)
        {
            switch (state)
            {
                case CellState.Enabled:
                { 
                    gameObject.SetActive(true);
                    _spriteRenderer.color = EnabledColour;
                    IsOccupied = true;
                } break;
                case CellState.Disabled:
                {
                    gameObject.SetActive(false);
                    IsOccupied = false;
                } break;
                case CellState.Occupied:
                {
                    gameObject.SetActive(true);
                    _spriteRenderer.color = OccupiedColour;  
                } break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            _state = state;
        }
    }
}

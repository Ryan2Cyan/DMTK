using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Miniatures
{
    public enum StatusCondition
    {
        Blinded = 0,
        Deafened = 1,
        Frightened = 2,
        Paralysed = 3,
        Petrified = 4,
        Restrained = 5,
        Unconscious = 6,
        Charmed = 7,
        Grappled = 8,
        Invisible = 9,
        Incapacitated = 10,
        Poisoned = 11,
        Stunned = 12
    }
    public class MiniatureData : MonoBehaviour
    {
        public readonly Dictionary<StatusCondition, bool> StatusConditions = new()
        {
            { StatusCondition.Blinded, false},
            { StatusCondition.Deafened, false},
            { StatusCondition.Frightened, false},
            { StatusCondition.Paralysed, false},
            { StatusCondition.Petrified, false},
            { StatusCondition.Restrained, false},
            { StatusCondition.Unconscious, false},
            { StatusCondition.Charmed, false},
            { StatusCondition.Grappled, false},
            { StatusCondition.Invisible, false},
            { StatusCondition.Incapacitated, false},
            { StatusCondition.Poisoned, false},
            { StatusCondition.Stunned, false},
        };

        public string Label;
        public int MaximumHitPoints;
        public int CurrentHitPoints;
        public int ExhaustionLevel;

        private void Awake()
        {
            if (MaximumHitPoints <= 0) MaximumHitPoints = 100;
            if (Label == "") Label = "No Name";
        }
    }
}

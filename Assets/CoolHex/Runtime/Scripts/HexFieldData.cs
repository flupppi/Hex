using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace flupppi.CoolHex
{
    [System.Serializable]
    public class HexFieldData
    {
        public int Weight;
        public bool IsBlocked;
        public Vector2 Position; // Grid position
                                 // Other parameters
    }
}
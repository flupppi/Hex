using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace flupppi.CoolHex
{
    [CreateAssetMenu(menuName = "HexMapAsset")]
    public class HexMapAsset : ScriptableObject
    {
        public List<HexFieldData> HexFields;
        public int Width;
        public int Height;
    }
}
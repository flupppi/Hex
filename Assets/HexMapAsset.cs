using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "HexMapAsset")]
public class HexMapAsset : ScriptableObject
{
    public List<HexFieldData> HexFields;
    public int Width;
    public int Height;
}

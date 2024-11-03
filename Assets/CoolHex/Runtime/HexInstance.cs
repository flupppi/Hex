using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace flupppi.CoolHex
{
    public class HexInstance : MonoBehaviour
    {
        public int Weight;
        public bool IsBlocked;

        public void Setup(HexFieldData data)
        {
            this.Weight = data.Weight;
            this.IsBlocked = data.IsBlocked;


            HexUtils.UpdateHexColor(gameObject, data.IsBlocked);
        }
    }
}

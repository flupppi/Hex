using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HexMapLoader : MonoBehaviour
{
    public HexMapAsset MapData;
    public GameObject HexPrefab;

    public GameObject hexMapParent;

    void Start() {
        if (MapData == null || HexPrefab == null)
        {
            Debug.LogError("MapData or HexPrefab is not assigned.");
            return;
        }

        // Load the hex map based on the data in the asset
        hexMapParent = new GameObject("HexMap");
        

        // Set up the prefab with data like weight, blocked status, etc.
        foreach (var hexData in MapData.HexFields) {
            int q = (int)hexData.Position.x;
            int r = (int)hexData.Position.y;

            // Calculate the world postion using q, r
            Vector3 position = HexUtils.CalculateHexPosition(q, r);
            var hexInstance = Instantiate(HexPrefab, position, Quaternion.Euler(0, 0, 30), hexMapParent.transform);
            
            HexInstance hexComponent = hexInstance.GetComponent<HexInstance>();
            if (hexComponent != null) {
                hexComponent.Setup(hexData);
            }
        }
        hexMapParent.transform.rotation =  Quaternion.Euler(90, 0, 0);

    }
}

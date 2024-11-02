using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexUtils
{

    // Calculate hex position for hexagonal layout (offset grid as example)
    public static Vector3 CalculateHexPosition(int q, int r)
    {
        float hexHeight = 1.0f; // Replace with your actual hex tile height
        float hexWidth = Mathf.Sqrt(3) / 2 * hexHeight; // Standard width for pointy-top hex
        float x = q * hexWidth + (r % 2 == 0 ? 0 : hexWidth / 2); // Stagger odd rows
        float y = r * (hexHeight * 0.75f); // Y is staggered by 3/4th the height
        return new Vector3(x, y, 0);
    }

    // Update hex tile color based on blocked status
    public static void UpdateHexColor(GameObject hex, bool isBlocked)
    {
        var renderer = hex.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = isBlocked ? Color.red : Color.green;
        }
    }
}

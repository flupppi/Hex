using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HexEditor : EditorWindow
{
    // Hex prefab to be assigned in the Editor Window
    public GameObject hexPrefab;
    public HexMapAsset MapData; // Add this to assign the ScriptableObject

    public int currentWidth;
    public int currentHeight;


    public HexFieldData[,] hexFieldDataArray;
    // Store the selected hex data
    // Map q, r to hex GameObjects
    public Dictionary<Vector2Int, GameObject> hexGameObjectMap;
    public HexFieldData selectedHex;

    // Menu to open the HexEditor window
    [MenuItem("My/HexEditor")]
    public static void ShowExample()
    {
        HexEditor wnd = GetWindow<HexEditor>();
        wnd.titleContent = new GUIContent("HexEditor");
    }

    // Show selected hex parameters in the Editor window
    public static void ShowHexParameters(HexFieldData hexData)
    {
        var window = GetWindow<HexEditor>();
        window.selectedHex = hexData;
        window.Repaint(); // Update the editor window with the new data
    }

    // Generate or regenerate the hex map
    void RegenerateHexMap(int newWidth, int newHeight)
    {
        if (hexPrefab == null)
        {
            Debug.LogError("Hex prefab is not assigned.");
            return; // Don't proceed if no prefab is assigned
        }

        // Initialize the hexFieldDataArray to store the hex data
        hexFieldDataArray = new HexFieldData[newWidth, newHeight]; // Initialize the array
        hexGameObjectMap = new Dictionary<Vector2Int, GameObject>();

        // Delete the old hex map
        GameObject hexMapParent = GameObject.Find("HexMap");
        if (hexMapParent != null)
        {
            DestroyImmediate(hexMapParent);
        }

        // Generate new hex map with updated size
        hexMapParent = new GameObject("HexMap");
        for (int q = 0; q < newWidth; q++)
        {
            for (int r = 0; r < newHeight; r++)
            {
                Vector3 position = HexUtils.CalculateHexPosition(q, r); // Hex layout logic
                GameObject hex = Instantiate(hexPrefab, position, Quaternion.Euler(0, 0, 30), hexMapParent.transform);
                
                
                //hex.GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
                
                // Set up hex properties if needed
                HexFieldData hexData = new HexFieldData{Weight = 1, IsBlocked = false, Position = new Vector2(q, r)};
                
                            // Set the hex's color based on the blocked status
                HexUtils.UpdateHexColor(hex, hexData.IsBlocked);
                hexFieldDataArray[q, r]  = hexData;
                hexGameObjectMap[new Vector2Int(q, r)] = hex;

            }
        }
    }



    void LoadHexMapFromAsset(){
        if (MapData == null)
        {
            Debug.LogError("MapData is not assigned.");
            return;
        }

        currentWidth = MapData.Width;
        currentHeight = MapData.Height;



        // Delete the old hex map
        GameObject hexMapParent = GameObject.Find("HexMap");
        if (hexMapParent != null)
        {
            DestroyImmediate(hexMapParent);
        }

        hexFieldDataArray = new HexFieldData[currentWidth, currentHeight];
        hexGameObjectMap = new Dictionary<Vector2Int, GameObject>();

        hexMapParent = new GameObject("HexMap");
        for (int i = 0; i < MapData.HexFields.Count; i++){
            var hexData = MapData.HexFields[i];
            int q = (int)hexData.Position.x;
            int r = (int)hexData.Position.y;

            Vector3 position = HexUtils.CalculateHexPosition(q, r);
            GameObject hex = Instantiate(hexPrefab, position, Quaternion.Euler(0,0,30), hexMapParent.transform);

            // Set the hex's color based on the blocked status
            HexUtils.UpdateHexColor(hex, hexData.IsBlocked);

            // Store the hex data in the array
            hexFieldDataArray[q, r] = hexData;
            hexGameObjectMap[new Vector2Int(q, r)] = hex;

        }


    }

        // Editor window GUI
    void OnGUI()
    {
            // Add field for hex prefab assignment
            hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", hexPrefab, typeof(GameObject), false);

            // Add field for HexMapAsset (ScriptableObject) assignment
            MapData = (HexMapAsset)EditorGUILayout.ObjectField("Hex Map Data", MapData, typeof(HexMapAsset), false);

            // Add fields for map size
            int newWidth = EditorGUILayout.IntField("Map Width", currentWidth);
            int newHeight = EditorGUILayout.IntField("Map Height", currentHeight);

            if (GUILayout.Button("Load Hex Map"))
            {
                LoadHexMapFromAsset();
            }
            // If size changes, regenerate the hex map
            if (newWidth != currentWidth || newHeight != currentHeight)
            {
                RegenerateHexMap(newWidth, newHeight);
                currentWidth = newWidth;
                currentHeight = newHeight;
            }

            // Show selected hex field parameters if a hex is selected
            if (selectedHex != null)
            {
                selectedHex.Weight = EditorGUILayout.IntField("Weight", selectedHex.Weight);
                selectedHex.IsBlocked = EditorGUILayout.Toggle("Blocked", selectedHex.IsBlocked);

                // Find the corresponding hex object and update it's color based on the blocked status
                GameObject selectedHexObject = FindHexGameObjectByGridPosition((int)selectedHex.Position.x, (int)selectedHex.Position.y);
                if (selectedHexObject != null)
                {
                    HexUtils.UpdateHexColor(selectedHexObject, selectedHex.IsBlocked);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No hex selected.");
            }

            // Add a save button to explicitly save the hex data to the asset
            if (GUILayout.Button("Save Hex Map"))
            {
                SaveHexMapToAsset();
            }
    }
    GameObject FindHexGameObjectByGridPosition(int q, int r){
        Vector2Int gridPosition = new Vector2Int(q, r);
        if (hexGameObjectMap.ContainsKey(gridPosition))
        {
            return hexGameObjectMap[gridPosition];
        }
        return null;
    }

            // Method to save the hex map data to the ScriptableObject
        void SaveHexMapToAsset()
        {
            if (MapData == null)
            {
                Debug.LogError("MapData is not assigned.");
                return;
            }

            // Update the HexMapAsset
            UpdateHexMapAsset();

            // Save the asset and mark it dirty to ensure changes are written to disk
            AssetDatabase.SaveAssets();
            EditorUtility.SetDirty(MapData);
        }
    void UpdateHexMapAsset()
    {
        // Create a list to store the hex field data
        List<HexFieldData> hexFields = new List<HexFieldData>();

        // Loop through the hex field data array and add each entry to the list
        for (int q = 0; q < currentWidth; q++)
        {
            for (int r = 0; r < currentHeight; r++)
            {
                hexFields.Add(hexFieldDataArray[q, r]);
            }
        }
        MapData.Width = currentWidth;
        MapData.Height = currentHeight;
        // Save the list of hex fields to the HexMapAsset
        MapData.HexFields = hexFields;
        EditorUtility.SetDirty(MapData); // Mark the ScriptableObject as dirty so Unity knows it has changed
    }
    void OnEnable()
    {
        // Subscribe to Selection changes
        Selection.selectionChanged += OnSelectionChanged;
    }

  
    // Cleanup when the window is closed
    void OnDisable()
    {
        GameObject hexMapParent = GameObject.Find("HexMap");
        if (hexMapParent != null)
        {
            DestroyImmediate(hexMapParent);
        }
                // Unsubscribe to avoid memory leaks
        Selection.selectionChanged -= OnSelectionChanged;

    }

        // This method is called whenever the selected object changes in the Scene
    void OnSelectionChanged()
    {
        // Get the currently selected GameObject

        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject != null)
        {
                        // Try to find the corresponding hex field data based on position
            Vector3 selectedPosition = selectedObject.transform.position;
            HexFieldData hexData = GetHexFieldDataAtPosition(selectedPosition);
            if (hexData != null)
            {
                ShowHexParameters(hexData); // Show in the editor window
            }
            else
            {
                // If the selected object isn't a hex tile, clear the selection
                selectedHex = null;
                Repaint(); // Clear the editor window
            }
        }
    }

    // Determine which hex field data corresponds to the selected object
    HexFieldData GetHexFieldDataAtPosition(Vector3 position)
    {
        // Loop through the 2D array and find the closest hex field by position
        for (int q = 0; q < currentWidth; q++)
        {
            for (int r = 0; r < currentHeight; r++)
            {
                Vector3 hexPosition = HexUtils.CalculateHexPosition(q, r);

                if (Vector3.Distance(hexPosition, position) < 0.1f) // Use a small threshold to match positions
                {
                    return hexFieldDataArray[q, r];
                }
            }
        }
        return null; // Return null if no matching hex is found
    }
}
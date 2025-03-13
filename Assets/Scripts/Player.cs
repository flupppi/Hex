using UnityEngine;
namespace HexTrain
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Vector3Int position;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Set up the players position

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Move(Vector3Int direction)
        {
            // Move the player in the direction
        }
    }
}
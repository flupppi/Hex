using UnityEngine;
using System.Collections.Generic;
namespace HexTrain
{
    public class Core : MonoBehaviour
    {
        List<Player> players = new List<Player>();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Spawn the players
            SpawnPlayers();
            // Set up the level
            InitializeLevel();


        }



        void SpawnPlayers()
        {
            // Set up the players positions

            // Set up the players visuals
        }

        void InitializeLevel()
        {
            // Set up the level
        }

        void EndLevel()
        {
            // Play sound, maybe animation, show winner, transition to the next stage.

        }

        void StartTurn()
        {
            // Play sound, maybe animation
        }
        void EndTurn()
        {
            // Play sound, maybe animation

        }

        void ProcessEvent()
        {

        }

        void EndGame()
        {
            // Quit the game, Load main menu
        }

        void PickAttack()
        {

        }

        void PlayAttackAnimation()
        {

        }



        void PlayDeathAnimation()
        {

        }


    }
}
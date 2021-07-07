
using UnityEngine;

namespace NetworkLobbyGameSettings
{


    public enum GameModeSelections
    {
        One_VS_One
        
    }


    public class GameSettings : MonoBehaviour
    {
        // we can expand the game modes if we want later on
        public static GameModeSelections GameMode = GameModeSelections.One_VS_One;
        
        // if we decide to make it 2 vs 2
        public static bool IsAwayTeam = false;
        
    }

}
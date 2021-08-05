
using UnityEngine;
using UnityEngine.SceneManagement;


public class NetworkLobbyMenu : MonoBehaviour
{
    public NetworkLobby launcher;

    [Min(0)] public int mainTitlesSceneBuildIndex = 0;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void JoinMatch()
    {
        launcher.Join();
    }

    public void CreateMatch()
    {
        launcher.Create();
    }

    public void BackToMainMenu()
    {
        //GameObject netManagerRef = GameManager.networkManager.gameObject;
        
        // clear GameManager networkManager reference
        GameManager.networkManager = null;
        
        
        
        // kill neetwork manager
        Destroy(gameObject);

        SceneManager.LoadScene(mainTitlesSceneBuildIndex);

    }
    

    public void QuitGame()
    {
        Application.Quit();
    }
}

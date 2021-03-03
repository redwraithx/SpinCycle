
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen2CanvasManager : MonoBehaviour

{
    private AsyncOperation loadingOperation;
    public Slider progressBar;
    public Text percentLoaded;
    
    
    void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync("SampleScene"); // TEMP LOAD FOR SAMPLE SCENE
        //loadingOperation = SceneManager.LoadSceneAsync("Scenes/Levels/OwenLevel"); // TEMP LOAD FOR SAMPLE SCENE
        //loadingOperation = SceneManager.LoadSceneAsync("AnhHoaiScene");
        //loadingOperation = SceneManager.LoadSceneAsync("MultiplayerTestLevel");
        //loadingOperation = SceneManager.LoadSceneAsync("EvanItemLevel");
        loadingOperation = SceneManager.LoadSceneAsync("ChiragCameraScene");
    }

    void Update()
    {
        progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        percentLoaded.text = Mathf.Round(progressValue * 100) + "%";
    }
}

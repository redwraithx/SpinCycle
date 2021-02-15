
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreenCanvasManager : MonoBehaviour
{
    private AsyncOperation loadingOperation;
    public Slider progressBar;
    public Text percentLoaded;
    void Start()
    {
        loadingOperation = SceneManager.LoadSceneAsync("BasicLobbyRoom");
    }

    void Update()
    {
        progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        float progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        percentLoaded.text = Mathf.Round(progressValue * 100) + "%";
    }
}

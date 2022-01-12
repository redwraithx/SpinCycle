using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioClip[] sfxClips = null;

    [Space] [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;

    [SerializeField] private GameObject textVisualsContainer;

    private void Update()
    {
        // THIS IS TO BE REMOVED
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // enable / disable the visual add for audio testing
            textVisualsContainer.SetActive(!textVisualsContainer.activeInHierarchy);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            GameManager.audioManager.PlaySfx(sfxClips[0]);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            GameManager.audioManager.PlaySfx(GetRandomAudioClip());

        if (Input.GetKeyDown(KeyCode.Alpha3))
            GameManager.audioManager.PlayMusic(music1);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            GameManager.audioManager.PlayMusic(music2);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            GameManager.audioManager.PlayMusicWithFade(music1);

        if (Input.GetKeyDown(KeyCode.Alpha6))
            GameManager.audioManager.PlayMusicWithFade(music2);

        if (Input.GetKeyDown(KeyCode.Alpha7))
            GameManager.audioManager.PlayMusicWithCrossFade(music1);

        if (Input.GetKeyDown(KeyCode.Alpha8))
            GameManager.audioManager.PlayMusicWithCrossFade(music2);

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GameManager.audioManager.StopMusic();
            GameManager.audioManager.StopSfx();
        }
    }

    // TESTING FUNCTION ONLY
    private AudioClip GetRandomAudioClip()
    {
        int newClip = (Random.Range(1, sfxClips.Length));

        return sfxClips[newClip];
    }
}
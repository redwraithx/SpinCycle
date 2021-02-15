using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public Image GameOverImage;
    public Image BlackImage;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.L) == true)
        {
            GameOverImage.gameObject.SetActive(true);
            StartCoroutine(Fading());
        }
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(()=> BlackImage.color.a==1);
        SceneManager.LoadScene(8);
    
    }
}

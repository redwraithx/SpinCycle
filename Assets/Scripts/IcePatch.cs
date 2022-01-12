using UnityEngine;

public class IcePatch : MonoBehaviour
{
    private void Start()
    {
        Invoke("Dissapear", 10f);
    }

    private void Dissapear()
    {
        Destroy(gameObject);
    }
}
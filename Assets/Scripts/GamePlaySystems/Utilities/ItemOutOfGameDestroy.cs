using UnityEngine;

public class ItemOutOfGameDestroy : MonoBehaviour
{
    public float depthOfItemWhenOutOfWorld = -500f;

    private void Update()
    {
        if (transform.position.y <= depthOfItemWhenOutOfWorld)
            Destroy(this.gameObject);
    }
}
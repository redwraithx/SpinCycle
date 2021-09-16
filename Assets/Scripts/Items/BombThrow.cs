using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviourPun, IBombThrow
{
    
    public float elevationAngle { get { return m_ElevationAngle; } set { m_ElevationAngle = value; } }
    [SerializeField]
    float m_ElevationAngle = 45f;

    public float impulse { get { return m_Impulse; } set { m_Impulse = value; } }
    [SerializeField]
    float m_Impulse = 20f;

    public Vector3 facingDirection
    {
        get
        {
            Vector3 result = transform.forward;
            result.y = 0f;
            return result.sqrMagnitude == 0f ? Vector3.forward : result.normalized;
        }
    }

    public void Throw()
    {
        Debug.Log("Throw Bomb Script");
        AudioClip throwBombSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Sabotages/Bombs/Soapbomb/BubbleLaunchAndBurst_5_SEC");
        GameManager.audioManager.PlaySfx(throwBombSound);
        GameObject bomb = this.gameObject;

        Vector3 direction = facingDirection;
        direction = Quaternion.AngleAxis(elevationAngle, Vector3.Cross(direction, Vector3.up)) * direction;
        bomb.GetComponent<Rigidbody>().AddForce(direction * impulse, ForceMode.Impulse);
    }

}

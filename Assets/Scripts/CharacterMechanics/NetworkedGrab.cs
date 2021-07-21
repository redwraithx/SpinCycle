using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkedGrab : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject targetPlayer = null;
    public Transform grabber;
    Transform otherGrabber;
    private Grab grab = null;
    private StrengthBarUI strengthBar = null;
    private PlayerMovementCC playerCC = null;
    internal bool isBeingGrabbed = false;
    public CapsuleCollider grabCollider = null;
    internal bool isGrabbing = false;
    public float holdTimeDuration = 5f;
    public float currentHoldTimer = 0f;
    public bool isHoldTimerEnabled = false;
    public bool hasLostGripOfPlayer = false;

    PhotonView originalClientPhotonViewID = null;
    PhotonView myPhotonViewID = null;
    PhotonView originalTargetView = null;
    Player originalPlayer = null;
    NetworkedGrab otherPlayersNetworkedGrab = null;
    RaycastHit hit;

    public string textString = "Sending";
    public string showTextString = "";
    public int counter = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        if (!myPhotonViewID)
            myPhotonViewID = GetComponent<PhotonView>();

        if (!grabCollider)
            grabCollider = GetComponentInChildren<CapsuleCollider>();

        if (!grab)
            grab = GetComponent<Grab>();

        if (!strengthBar)
            strengthBar = GetComponent<StrengthBarUI>();

        if (!playerCC)
            playerCC = GetComponent<PlayerMovementCC>();

        if (targetPlayer)
            targetPlayer = null;

        foreach (var collider in GetComponents<Collider>())
        {
            Physics.IgnoreCollision(grabCollider, collider, true);
        }

        currentHoldTimer = holdTimeDuration;

        Physics.SphereCast(transform.position + new Vector3(0f, 0.5f, 0f), 0.5f, transform.forward, out hit, 0.5f);

        if (hit.collider != null)
            Debug.Log("can hit: " + hit.collider.name);

        otherGrabber.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer && !hasLostGripOfPlayer)
        {
            CheckGrab();
        }
        else if (targetPlayer && hasLostGripOfPlayer)
        {
            ReleaseHeldPlayer();
        }

        if (isHoldTimerEnabled)
        {
            currentHoldTimer -= Time.deltaTime;

            if (currentHoldTimer <= 0f)
            {
                isHoldTimerEnabled = false;
                currentHoldTimer = holdTimeDuration;
                isBeingGrabbed = false;
                hasLostGripOfPlayer = true;

            }
        }


    }

    private void CheckGrab()
    {
        otherPlayersNetworkedGrab = targetPlayer.GetComponent<NetworkedGrab>();

        if (!otherPlayersNetworkedGrab)
            return;

        if (targetPlayer && Input.GetMouseButtonDown(0) && !isBeingGrabbed)
        {
            if (!targetPlayer)
                return;

            isGrabbing = true;
        }

        if (targetPlayer && Input.GetMouseButtonUp(0) && isGrabbing)
        {
            isGrabbing = false;
        }


    }

    public void ReleaseHeldPlayer()
    {
        if (!targetPlayer)
            return;

        isGrabbing = false;
        hasLostGripOfPlayer = false;
        targetPlayer = null;
    }


    private void OnTriggerEnter(Collider other)
    {
        GetTarget(other);
    }
    private void OnTriggerStay(Collider other)
    {
        GetTarget(other);
    }

    private void GetTarget(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        if (grab)
            if (grab.itemInHand)
                return;

        if (targetPlayer)
            return;

        if (!other.gameObject)
            return;

        targetPlayer = other.gameObject;
        Debug.Log("Target player = true");
    }

    private void OnTriggerExit(Collider other)
    {
        targetPlayer = null;
        Debug.Log("Target lost");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Debug.Log("Stream.IsWriting");

            stream.SendNext(textString);
            stream.SendNext(counter);
            showTextString = $"Sending: {counter}";
            counter++;

            Vector3 pos;
            if (isGrabbing)
            {
                pos = grabber.position;
            }
            else
            {
                pos = otherGrabber.position;
            }
            stream.SendNext(pos);

            bool isBeingHeld;
            if (isGrabbing)
            {
                isBeingHeld = true;
            }
            else
            {
                isBeingHeld = false;
            }
            stream.SendNext(isBeingHeld);
        }
        else if (stream.IsReading)
        {
            Debug.Log("stream.IsReading");
            string newTextString = (string)stream.ReceiveNext();
            int newCounter = (int)stream.ReceiveNext();
            showTextString = $"Receiving: {newCounter}";

            Vector3 grabLocation = (Vector3)stream.ReceiveNext();
            if(!isGrabbing)
            {
                playerCC.enemyGrab.transform.position = grabLocation;
            }

            playerCC.isGrabbed = (bool)stream.ReceiveNext();

            
        }
            
    }
}

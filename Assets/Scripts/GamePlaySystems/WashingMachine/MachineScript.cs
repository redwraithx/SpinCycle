
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
//using emotitron;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GamePlaySystems.Utilities;
using EnumSpace;

using Photon.Pun;
using Photon.Realtime;
using Debug = UnityEngine.Debug;


public class MachineScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView _photonView = null;
    public string networkItemToSpawn = "";
    
    public float cycleLength;
    public GameObject itemSpawnPoint;
    public float laundryTimer;
    public float sabotageTimer;
    public float ruinTimer;
    public bool isSabotaged = false;
    public bool isEnabled = false;
    public bool isBoosted = false;
    public bool isRuined = false;
    public Slider sliderTime;
    public LaundryType laundryType;
    public MachineType machineType;
    public ParticleSystem part;
    public TMP_Text pointsAdded;
    
    public ItemType SpawnFinishedProductItemType = ItemType.ClothingWet;

    public string textString = "Sending";
    public string showTextString = "";
    public int counter = 0;

    public float priceAddition;
    public float initialPrice;

    public GameObject loadRuinerMachinePart;
    public GameObject boostMachinePart;

    private void Awake()
    {
        if (!_photonView)
        {
            _photonView = GetComponent<PhotonView>();

            //PhotonNetwork.SendRate = 22;
            //PhotonNetwork.SerializationRate = 22;
        }
    }

    private void Start()
    {
        cycleLength = 15;
        sliderTime.maxValue = cycleLength;

        if (isSabotaged == true)
        {
            part.Play();
        }
    }

    void Update()
    {
        if (_photonView == null)
            return;

       // Debug.Log("Is it sabotaged?" + isSabotaged + photonView.ViewID);

        if (isSabotaged == false)
        {
            if (laundryTimer > 0)
            {
                laundryTimer -= Time.deltaTime;
                if (isRuined)
                {
                    ruinTimer += Time.deltaTime;
                }
            }
            if (laundryTimer <= 0 && isEnabled == true)
            {
                SpawnFinishedProduct(laundryType);
                isEnabled = false;
            }
        }

        //if (isSabotaged == true)
        //{
        //    if (sabotageTimer > 0)
        //    {
        //        sabotageTimer -= Time.deltaTime;
        //    }
        //    if (sabotageTimer <= 0)
        //    {
        //        part.Stop();
        //        isSabotaged = false;
        //    }
        //}

        sliderTime.value = laundryTimer;

        if(isSabotaged == true && part.isPlaying == false)
        {
            part.Play();
        }

        if (isSabotaged == false && part.isPlaying == true)
        {
            part.Stop();
        }
    }

    public void SpawnFinishedProduct(LaundryType type)
    {

        GameObject newGO = null;
        if (PhotonNetwork.IsMasterClient)
        {
            newGO = PhotonNetwork.Instantiate(Path.Combine("PhotonItemPrefabs", networkItemToSpawn), itemSpawnPoint.transform.position, itemSpawnPoint.transform.rotation, 0);
        }

        if (newGO)
        {
            Debug.Log($"new Item Type: {SpawnFinishedProductItemType}");
            
            newGO.GetComponent<ItemTypeForItem>().itemType = SpawnFinishedProductItemType;
            float ruinedPrice = (10 * ruinTimer);
            pointsAdded.text = (((int)initialPrice + (int)priceAddition) - (int)ruinedPrice).ToString() + "!";
            newGO.GetComponent<Item>().Price += ((int)initialPrice + (int)priceAddition) - (int)ruinedPrice;
            
        }
            

        Debug.Log("final owner is: " + _photonView.Owner);
        
        RequestTransferOwnershipToHost();

    }

    public void ProcessItems()
    {
        Debug.Log("processing item in machine");
        ruinTimer = 0;
        sliderTime.maxValue = cycleLength;
        laundryTimer = cycleLength;
        isEnabled = true;
    }


    public void UseMachine(GameObject other)
    {
        OnOwnershipRequest(_photonView, PhotonNetwork.LocalPlayer);

        StartCoroutine(UseMachineDelay(other, 0.25f));
    }
    
    private IEnumerator UseMachineDelay(GameObject other, float delayTime)
    {

        yield return new WaitForSeconds(delayTime);
        
        
        if (other.CompareTag("Item"))
        {
            laundryType = other.GetComponent<ItemTypeForItem>().laundryType;

            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.WasherBoost)
            {
                BoostMachine();
                PhotonNetwork.Destroy(other.gameObject);
                //other.gameObject.SetActive(false);
                //Destroy(other.gameObject);

            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.RepairTool)
            {
                FixMachine();
                RepairToolZoneSpawn.instance.RemoveObject();
                //other.gameObject.SetActive(false);
                PhotonNetwork.Destroy(other.gameObject);
                //Destroy(other.gameObject);
            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.LoadRuiner)
            {
                RuinLoad();
                PhotonNetwork.Destroy(other.gameObject);
                //other.gameObject.SetActive(false);
            }
            else if (other.GetComponent<ItemTypeForItem>().itemType == this.gameObject.GetComponent<ItemTypeForUsingItem>().itemType[0])
            {
                if (isEnabled == false)
                {
                    if (isSabotaged == false)
                    {
                        initialPrice = other.GetComponent<Item>().Price;
                        //Once player is created, call to destroy the item in their hand here
                        ProcessItems();
                        // we may want to use a bool in case the machine is full we dont destroy or use the object
                        other.transform.parent = null;
                        //other.gameObject.GetComponent<Item>().DisableObject();


                        // send msg to destroy other copies of this object on other network clients here
                        //other.GetComponent<Item>().RemoveThisObject();

                        PhotonNetwork.Destroy(other.gameObject);
                        //other.gameObject.SetActive(false);
                        //Destroy(other.gameObject);
                    }
                }
            }
        }

        if(other.gameObject)
            RequestTransferOwnershipToHost();
    }

    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "AOE Effects(Clone)")
        {
            SabotageMachine();
        }

        if (other.gameObject.name == "EMPSphere")
        {
            if (isSabotaged == true)
            {
                Destroy(other.gameObject);
            }
            else
            {
                SabotageMachine();
                Destroy(other.gameObject);
            }
        }
    }

    public void SabotageMachine()
    {
        isSabotaged = true;
        part.Play();
    }
    public void FixMachine()
    {
        isSabotaged = false;
        sabotageTimer = 0;
        part.Stop();
    }

    public void BoostMachine()
    {
        isBoosted = true;
        cycleLength = cycleLength * 0.75f;
        if(boostMachinePart != null)
        {
            boostMachinePart.SetActive(true);
        }
    }
    
    public void RuinLoad()
    {
        isRuined = true;
        if (loadRuinerMachinePart != null)
        {
            loadRuinerMachinePart.SetActive(true);
        }
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("requesting ownership by: " + requestingPlayer.NickName + " of machine viewID: " + targetView.ViewID);

        if (targetView != base.photonView)
            return;
        
        
        base.photonView.TransferOwnership(requestingPlayer);
        
        Debug.Log("ownership of this object is: " + photonView.Owner);
    }
   
    public void RequestOwnership()
    {
        Debug.Log("request ownership from host");

        // get ownership of the object were about to pickup
        base.photonView.RequestOwnership();


    }

    public void RequestTransferOwnershipToHost()
    {
        Debug.Log("give ownership back to host");
        
        // return ownership back to master client.
        base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
            
        Debug.Log("master client is new owner. ownership of this object is: " + photonView.Owner);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        Debug.Log("OnPhotonSerializeView called");
        
        if (stream.IsWriting)
        {
            Debug.Log("stream.IsWriting");
            
            stream.SendNext(textString);
            stream.SendNext(counter);
            showTextString = $"Sending: {counter}";


            stream.SendNext(laundryTimer);
            stream.SendNext(cycleLength);
            stream.SendNext(isEnabled);
            stream.SendNext(isSabotaged);
            stream.SendNext(isRuined);
            stream.SendNext(isBoosted);


            counter++;
        }
        else if(stream.IsReading)
        {
            Debug.Log("stream.IsReading");
            
            string newTextString = (string) stream.ReceiveNext();
            int newCounter = (int) stream.ReceiveNext();
            showTextString = $"Receiving: {newCounter}";


            float laundry = (float) stream.ReceiveNext();
            float cycle = (float) stream.ReceiveNext();
            bool sliderIsEnabled = (bool) stream.ReceiveNext();
            bool machineSabotaged = (bool)stream.ReceiveNext();
            bool loadRuined = (bool) stream.ReceiveNext();
            bool machineBoosted = (bool)stream.ReceiveNext();

            if (laundry > laundryTimer || laundry < laundryTimer)
                laundryTimer = laundry;

            if (cycle > cycleLength || cycle < cycleLength)
                cycleLength = cycle;

            if (isEnabled != sliderIsEnabled)
                isEnabled = sliderIsEnabled;

            if (isSabotaged != machineSabotaged)
                isSabotaged = machineSabotaged;

            if (isRuined != loadRuined)
                isRuined = loadRuined;

            if (isBoosted != machineBoosted)
                isBoosted = machineBoosted;


            Debug.Log($"LaundryTimer: {laundryTimer}\ncycleLength: {cycleLength}\nsiEnabled: {isEnabled}/nisSabotaged: {isSabotaged}");
        }
    }
}

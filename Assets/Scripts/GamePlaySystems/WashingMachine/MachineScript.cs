
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
    //public Slider sliderTime;
    public LaundryType laundryType;
    public MachineType machineType;
    public ParticleSystem part;
    //public TMP_Text pointsAdded;
    
    public ItemType SpawnFinishedProductItemType = ItemType.ClothingWet;

    public string textString = "Sending";
    public string showTextString = "";
    public int counter = 0;

    public float priceAddition;
    public float initialPrice;

    public GameObject loadRuinerMachinePart;
    public GameObject boostMachinePart;

    //public Animator animator;
    public float percent;
    public TMP_Text percentCounter;

    public Sprite disabledSprite;
    public Sprite normalSprite;
    public Sprite barNormal;
    public Sprite barDisabled;
    public GameObject theSprite;
    public Image fillBarImage;

    public SpriteRenderer spinner;
    public Sprite goodSpinner;
    public Sprite badSpinner;

    private void Awake()
    {
        if (!_photonView)
        {
            _photonView = GetComponent<PhotonView>();


        }
    }

    private void Start()
    {
        //animator.speed = 0.25f;

        if(MachineType.washer == this.machineType)
        {
            cycleLength = 20;

        }
        else if (MachineType.dryer == this.machineType)
        {
            cycleLength = 25;
        }
        else if (MachineType.folder == this.machineType)
        {
            cycleLength = 15;
        }
        //sliderTime.maxValue = cycleLength;

        if (isSabotaged == true)
        {
            part.Play();
        }
    }

    void Update()
    {
        if (_photonView == null)
            return;


        if (isSabotaged == false)
        {
            if (laundryTimer > 0)
            {
                laundryTimer -= Time.deltaTime;
                percent = laundryTimer/cycleLength;
                percentCounter.text = (100 - Mathf.Round(percent * 100) + "%");
                spinner.transform.Rotate(0, 0, 0.1f);
                fillBarImage.fillAmount = 1 - percent;

                if (isRuined)
                {
                    ruinTimer += Time.deltaTime;
                }
            }
            if (laundryTimer <= 0 && isEnabled == true)
            {
                percentCounter.text = ("0%");
                SpawnFinishedProduct(laundryType);
                fillBarImage.fillAmount = 0;
                isEnabled = false;
            }
        }


        //sliderTime.value = laundryTimer;

        if(isSabotaged == true && part.isPlaying == false)
        {
            part.Play();
        }

        if (isSabotaged == true && theSprite.GetComponent<Image>().sprite != disabledSprite)
        {
            theSprite.GetComponent<Image>().sprite = disabledSprite;
        }

        if (isSabotaged == false && theSprite.GetComponent<Image>().sprite == disabledSprite)
        {
            theSprite.GetComponent<Image>().sprite = normalSprite;
        }

        if (isSabotaged == false && part.isPlaying == true)
        {
            part.Stop();
        }

        if (isSabotaged == true && fillBarImage.GetComponent<Image>().sprite != barDisabled)
        {
            fillBarImage.GetComponent<Image>().sprite = barDisabled;
        }

        if (isSabotaged == false && fillBarImage.GetComponent<Image>().sprite == barDisabled)
        {
            fillBarImage.GetComponent<Image>().sprite = barNormal;
        }


        if (isSabotaged == true && spinner.GetComponent<SpriteRenderer>().sprite != badSpinner)
        {
            spinner.GetComponent<SpriteRenderer>().sprite = badSpinner;
        }

        if (isSabotaged == false && spinner.GetComponent<SpriteRenderer>().sprite == badSpinner)
        {
            spinner.GetComponent<SpriteRenderer>().sprite = goodSpinner;
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
            
            newGO.GetComponent<ItemTypeForItem>().itemType = SpawnFinishedProductItemType;
            float ruinedPrice = (10 * ruinTimer);
            newGO.GetComponent<Item>().Price += ((int)initialPrice + (int)priceAddition) - (int)ruinedPrice;
            
        }
            


        
        RequestTransferOwnershipToHost();

    }

    public void ProcessItems()
    {
        percentCounter.text = "0%";
        //animator.ResetTrigger("Stop");
        //animator.SetTrigger("Go");
        ruinTimer = 0;
        //sliderTime.maxValue = cycleLength;
        laundryTimer = cycleLength;
        isEnabled = true;


        if (MachineType.washer == this.machineType)
        {
            AudioClip washingSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Washer/Washer_Machine_Special_Sound_C_10-SEC");
            GameManager.audioManager.PlaySfx(washingSound);

            /*var sound = GetComponent<AudioSource>();
            sound.loop = true;
            sound.clip = washingSound;
            sound.Play();*/
        }
        else if (MachineType.dryer == this.machineType)
        {
            AudioClip dryingSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Dryer/Dryer_Machine_Special_Sound_B_10-SEC");
            GameManager.audioManager.PlaySfx(dryingSound);
        }
        else if (MachineType.folder == this.machineType)
        {
            AudioClip foldingSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Folder/Ambient_Noises_A_10-SEC");
            GameManager.audioManager.PlaySfx(foldingSound);
        }
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



            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.RepairTool)
            {
                FixMachine();
                RepairToolZoneSpawn.instance.RemoveObject();
                
                PhotonNetwork.Destroy(other.gameObject);
                
            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.LoadRuiner)
            {
                RuinLoad();
                PhotonNetwork.Destroy(other.gameObject);
            }
            else if (other.GetComponent<ItemTypeForItem>().itemType == this.gameObject.GetComponent<ItemTypeForUsingItem>().itemType[0])
            {
                if (isEnabled == false)
                {
                    if (isSabotaged == false)
                    {
                        
                       

                        initialPrice = other.GetComponent<Item>().Price;
                        ProcessItems();
                        other.transform.parent = null;
                        PhotonNetwork.Destroy(other.gameObject);
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
        //animator.ResetTrigger("Go");
        //animator.SetTrigger("Stop");
        part.Play();
    }
    public void FixMachine()
    {
        //animator.ResetTrigger("Stop");
        isSabotaged = false;
        sabotageTimer = 0;
        part.Stop();

        if (isEnabled)
        {
            //animator.SetTrigger("Go");
        }
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


        if (targetView != base.photonView)
            return;
        
        
        base.photonView.TransferOwnership(requestingPlayer);
        

    }
   
    public void RequestOwnership()
    {


        // get ownership of the object were about to pickup
        base.photonView.RequestOwnership();


    }

    public void RequestTransferOwnershipToHost()
    {      
        // return ownership back to master client.
        base.photonView.TransferOwnership(PhotonNetwork.MasterClient);
            
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            
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


        }
    }
}

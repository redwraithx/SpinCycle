
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
    public MachineConveyor conveyor;
    public PhotonView _photonView = null;
    public string networkItemToSpawn = "";
    
    public float cycleLength;
    public float cycleLengthHold;
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

    //old particle effects
    public ParticleSystem part;
    //new particle effects
    public GameObject sabotageEffects;
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

    public AudioClip washerAudioDis;
    public AudioClip dryingAudioDis;
    public AudioClip foldingAudioDis;
    public AudioSource audioSource;
    public AudioSource convAudioSource;
    public AudioSource sabMachineSound;

    PlayerPoints playerPoints;
    private void Awake()
    {
        if (!_photonView)
        {
            _photonView = GetComponent<PhotonView>();


        }

        audioSource = GetComponent<AudioSource>();
        washerAudioDis = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Washer/Washer_Machine_Special_Sound_C_20-SEC");
        dryingAudioDis = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Dryer/Dryer_Machine_Special_Sound_B_25-SEC");
        foldingAudioDis = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Folder/Ambient_Noises_A_15-SEC");
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
            //part.Play();
            sabotageEffects.SetActive(true);
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
                percent = laundryTimer/cycleLengthHold;
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

            if(laundryTimer <= 0 && isEnabled == false)
            {
                percentCounter.text = ("0%");
            }
        }



        //sliderTime.value = laundryTimer;

        if(isSabotaged == true && sabotageEffects.activeInHierarchy == false)
        {
            //part.Play();
            sabotageEffects.SetActive(true);
        }

        if (isSabotaged == true && theSprite.GetComponent<Image>().sprite != disabledSprite)
        {
            theSprite.GetComponent<Image>().sprite = disabledSprite;
        }

        if (isSabotaged == false && theSprite.GetComponent<Image>().sprite == disabledSprite)
        {
            theSprite.GetComponent<Image>().sprite = normalSprite;
        }

        if (isSabotaged == false && sabotageEffects.activeInHierarchy == true)
        {
            part.Stop();
            sabotageEffects.SetActive(false);
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
    private bool UpdatePlayerPoints(GameObject other)
    {
        PlayerPoints playerPointsReference = PhotonView.Find(other.gameObject.GetComponent<Item>().OwnerID).GetComponent<PlayerPoints>();

        return playerPoints = playerPointsReference;
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
        ruinTimer = 0;
        laundryTimer = cycleLength;
        cycleLengthHold = cycleLength;
        isEnabled = true;



        if (MachineType.washer == this.machineType)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = washerAudioDis;
                audioSource.Play();
                Debug.Log("Audio is playing herer!@!#@#@#2");
            }


        }
        else if (MachineType.dryer == this.machineType)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = dryingAudioDis;
                audioSource.Play();

            }
        }
        else if (MachineType.folder == this.machineType)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = foldingAudioDis;
                audioSource.Play();
            }
            if (!conveyor.isRunning)
            {
                conveyor.SpawnObject();

                if (!convAudioSource.isPlaying)
                {
                    AudioClip convAudioClip = Resources.Load<AudioClip>("AudioFiles/SoundFX/Machines/Conveyor/Conveyor_Belt_Folding_Machine");
                    convAudioSource.clip = convAudioClip;
                    convAudioSource.Play();
                }
            }

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
                if (isBoosted == false)
                {
                    BoostMachine();
                    PhotonNetwork.Destroy(other.gameObject);
                }



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
                        bool updatedPlayerPoints = UpdatePlayerPoints(other);

                        if (updatedPlayerPoints)
                            Debug.Log("Players Points where updated");
                        else
                            Debug.Log("Players Points were not found to be updated.");

                        if(GameManager.networkLevelManager.playersJoined.Count > 1)
                        {
                            playerPoints.Points += other.gameObject.GetComponent<Item>().Price;
                        }

                        Debug.Log(other.gameObject + "is being removed from parent");

                        other.transform.parent = null;

                        Debug.Log(other.gameObject + "is being destroyed");


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

        if (!sabMachineSound.isPlaying)
        {
            AudioClip sabotageSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/Sabotages/Bombs/Soapbomb/Sparks_SFX_SparkSFX-St");
            sabMachineSound.clip = sabotageSound;
            sabMachineSound.Play();
        }

        isSabotaged = true;
        //animator.ResetTrigger("Go");
        //animator.SetTrigger("Stop");
        sabotageEffects.SetActive(true);
    }
    public void FixMachine()
    {

        AudioClip repairToolSound = Resources.Load<AudioClip>("AudioFiles/SoundFX/NotSabotages/RepairTool/584174__unfa__mining-consume");
        GameManager.audioManager.PlaySfx(repairToolSound);
        //animator.ResetTrigger("Stop");
        isSabotaged = false;
        sabotageTimer = 0;
        //part.Stop();
        sabotageEffects.SetActive(false);

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
            stream.SendNext(cycleLengthHold);


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
            float cycleHold = (float)stream.ReceiveNext();

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

            if (cycleLengthHold != cycleHold)
                cycleLengthHold = cycleHold;


        }
    }

    public void StopSound()
    {
        convAudioSource.Stop();
        convAudioSource.clip = null;
    }
}

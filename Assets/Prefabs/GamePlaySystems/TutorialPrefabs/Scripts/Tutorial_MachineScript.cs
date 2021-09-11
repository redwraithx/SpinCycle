
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using GamePlaySystems.Utilities;
using EnumSpace;

using Debug = UnityEngine.Debug;


public class Tutorial_MachineScript : MonoBehaviour
{
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


    TutorialPlayerPoints playerPoints;

    public GameObject spawnFinishedProductPrefab = null;

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
        //PlayerPoints playerPointsReference = GameObject.Find(other.gameObject.GetComponent<Item>().OwnerID).GetComponent<PlayerPoints>();


        return playerPoints = other.gameObject.GetComponent<TutorialPlayerPoints>();
    }
    public void SpawnFinishedProduct(LaundryType type)
    {

        GameObject newGO = null;
        {
            newGO = Instantiate(spawnFinishedProductPrefab, itemSpawnPoint.transform.position, itemSpawnPoint.transform.rotation);
        }

        if (newGO)
        {
            
            newGO.GetComponent<ItemTypeForItem>().itemType = SpawnFinishedProductItemType;
            float ruinedPrice = (10 * ruinTimer);
            newGO.GetComponent<Item>().Price += ((int)initialPrice + (int)priceAddition) - (int)ruinedPrice;
            
        }
            
    }

    public void ProcessItems()
    {
        percentCounter.text = "0%";
        //animator.ResetTrigger("Stop");
        //animator.SetTrigger("Go");
        ruinTimer = 0;
        //sliderTime.maxValue = cycleLength;
        laundryTimer = cycleLength;
        cycleLengthHold = cycleLength;
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
                    Destroy(other.gameObject);
                }



            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.RepairTool)
            {
                FixMachine();
                RepairToolZoneSpawn.instance.RemoveObject();
                
                Destroy(other.gameObject);
                
            }
            if (other.GetComponent<ItemTypeForItem>().itemType == ItemType.LoadRuiner)
            {
                RuinLoad();
                Destroy(other.gameObject);
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


                        playerPoints.Points += other.gameObject.GetComponent<Item>().Price;

                        other.transform.parent = null;
                        Destroy(other.gameObject);
                    }
                }
            }
        }

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
        sabotageEffects.SetActive(true);
    }
    public void FixMachine()
    {
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
    
}

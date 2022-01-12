using System;
using TMPro;
using UnityEngine;

public class LoadingNetworkLobby : MonoBehaviour
{
    public TMP_Text loadingText = null;

    public GameObject tabsContainer = null;

    public int tickCounter = 5;
    public float tickTimer = 0.5f;
    private float currentTickTimer = 0f;
    private bool isUpdatingTicks = false;

    private bool hasLoadedNetworkLobby = false;

    private void OnEnable()
    {
        if (tabsContainer.activeInHierarchy)
            tabsContainer.SetActive(false);

        loadingText.text = "";

        tickCounter = 5;

        hasLoadedNetworkLobby = false;

        ResetTickTimer();
    }

    private void Start()
    {
        if (!loadingText || !tabsContainer)
            throw new Exception("Error! missing loadingText and/or tabsContainer reference, Fix this and retry");

        ResetTickTimer();
    }

    private void Update()
    {
        if (hasLoadedNetworkLobby)
            return;

        if (!isUpdatingTicks)
        {
            currentTickTimer -= Time.deltaTime;

            if (currentTickTimer <= 0f)
            {
                isUpdatingTicks = true;

                loadingText.text += ".";

                tickCounter--;

                ResetTickTimer();
            }
        }

        if (tickCounter <= 0)
        {
            //Debug.Log("show profile and hide wait container");
            hasLoadedNetworkLobby = true;

            tabsContainer.SetActive(true);

            LocalLobbyManager.localInstance.TabOpenMain();

            gameObject.SetActive(false);
        }
    }

    private void ResetTickTimer()
    {
        currentTickTimer = tickTimer;

        isUpdatingTicks = false;
    }
}
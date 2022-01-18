using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class Banner : MonoBehaviour
{
    public static Banner instance;

    private BannerView bannerView;


    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);

        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();
    }

    private void RequestBanner()
    {
        string adUnitId = "ca-app-pub-8921506867398125/1863217836";

        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();

        this.bannerView.LoadAd(request);

        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.RequestBanner();
    }

    private void OnDestroy()
    {
        //bannerView.Destroy();
        Debug.Log("Banner Destroyed");
        this.RequestBanner();
    }
}

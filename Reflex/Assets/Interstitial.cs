using UnityEngine;
using GoogleMobileAds.Api;

public class Interstitial : MonoBehaviour
{
    public static Interstitial instance;

    private InterstitialAd interstitial;

    private void Start()
    {
        RequestInterstitial();
    }
    private void Update()
    {
        ShowInterstitial();
    }
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
    private void RequestInterstitial()
    {
        string adUnitId = "ca-app-pub-8921506867398125/9605976487";
        this.interstitial = new InterstitialAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);

        
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestInterstitial();
    }

    void ShowInterstitial()
    {
        int value = PlayerPrefs.GetInt("Interstitial", 0);
        if(value >= 5)
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
                PlayerPrefs.SetInt("Interstitial", 0);
                RequestInterstitial();


            }
        }
        
    }

    private void OnDestroy()
    {
        //interstitial.Destroy();
    }
}

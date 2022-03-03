using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Ads
{
    public class GoogleAds : MonoBehaviour
    {
        private static GoogleAds instance = null;

        [Header("Kimlikler")] public string interstitialKimligi = "";
        public string rewardedVideoKimligi = "";

        [Header("Test Modu")] public bool testModu = false;
        public string testDeviceID = "";

        [Header("Diğer Ayarlar")] public bool cocuklaraYonelikReklamGoster = false;

        private InterstitialAd interstitialReklam;
        private RewardedAd rewardedVideoReklam;

        private float interstitialIstekTimeoutZamani;
        private float rewardedVideoIstekTimeoutZamani;

        private float interstitialOtomatikYeniIstekZamani = float.PositiveInfinity;
        private float rewardedVideoOtomatikYeniIstekZamani = float.PositiveInfinity;

        private IEnumerator interstitialGosterCoroutine;
        private IEnumerator rewardedVideoGosterCoroutine;

        public delegate void RewardedVideoOdul(Reward odul);

        private RewardedVideoOdul odulDelegate;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);

                interstitialKimligi = interstitialKimligi.Trim();
                rewardedVideoKimligi = rewardedVideoKimligi.Trim();
                testDeviceID = testDeviceID.Trim();

                MobileAds.Initialize(reklamDurumu => { });

                RequestConfiguration.Builder reklamKonfigurasyonu = new RequestConfiguration.Builder();

                if (testModu && !string.IsNullOrEmpty(testDeviceID))
                    reklamKonfigurasyonu.SetTestDeviceIds(new List<string>() {testDeviceID});

                if (cocuklaraYonelikReklamGoster)
                    reklamKonfigurasyonu.SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.True);

                MobileAds.SetRequestConfiguration(reklamKonfigurasyonu.build());

                InterstitialReklamYukle();
                RewardedReklamYukle();
            }
            else if (this != instance)
                Destroy(this);
        }


        private IEnumerator Start()
        {
            int time = 0;
            
            while (true)
            {
               

                time++;
                
                if (time % 120 == 0)
                {
                    InsterstitialGoster();
                }
                
                yield return new WaitForSeconds(1f);

            }
        }


        private void Update()
        {
            float zaman = Time.realtimeSinceStartup;

            if (zaman >= interstitialOtomatikYeniIstekZamani)
            {
                interstitialOtomatikYeniIstekZamani = float.PositiveInfinity;
                InterstitialReklamYukle();
            }

            if (zaman >= rewardedVideoOtomatikYeniIstekZamani)
            {
                rewardedVideoOtomatikYeniIstekZamani = float.PositiveInfinity;
                RewardedReklamYukle();
            }
        }

        private void InterstitialReklamYukle()
        {
            if (!testModu && string.IsNullOrEmpty(interstitialKimligi))
                return;

            if (interstitialReklam != null && interstitialReklam.IsLoaded())
                return;

            if (interstitialReklam != null)
                interstitialReklam.Destroy();

            if (testModu && (string.IsNullOrEmpty(testDeviceID) || string.IsNullOrEmpty(interstitialKimligi)))
            {
#if UNITY_ANDROID
                interstitialReklam = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
#else
            interstitialReklam = new InterstitialAd( "ca-app-pub-3940256099942544/4411468910" );
#endif
            }
            else
                interstitialReklam = new InterstitialAd(interstitialKimligi);

            interstitialReklam.OnAdClosed += InterstitialDelegate;
            interstitialReklam.OnAdFailedToLoad += InterstitialYuklenemedi;
            interstitialReklam.LoadAd(ReklamIstegiOlustur());

            interstitialIstekTimeoutZamani = Time.realtimeSinceStartup + 10f;
        }

        private void RewardedReklamYukle()
        {
            if (!testModu && string.IsNullOrEmpty(rewardedVideoKimligi))
                return;

            if (rewardedVideoReklam != null && rewardedVideoReklam.IsLoaded())
                return;

            if (rewardedVideoReklam != null)
                rewardedVideoReklam.Destroy();

            if (testModu && (string.IsNullOrEmpty(testDeviceID) || string.IsNullOrEmpty(rewardedVideoKimligi)))
            {
#if UNITY_ANDROID
                rewardedVideoReklam = new RewardedAd("ca-app-pub-3940256099942544/5224354917");
#else
            rewardedVideoReklam = new RewardedAd( "ca-app-pub-3940256099942544/1712485313" );
#endif
            }
            else
                rewardedVideoReklam = new RewardedAd(rewardedVideoKimligi);

            rewardedVideoReklam.OnAdClosed += RewardedVideoDelegate;
            rewardedVideoReklam.OnAdFailedToLoad += RewardedVideoYuklenemedi;
            rewardedVideoReklam.OnUserEarnedReward += RewardedVideoOdullendir;
            rewardedVideoReklam.LoadAd(ReklamIstegiOlustur());

            rewardedVideoIstekTimeoutZamani = Time.realtimeSinceStartup + 30f;
        }

        private AdRequest ReklamIstegiOlustur()
        {
            return new AdRequest.Builder().Build();
        }

        private void InterstitialDelegate(object sender, EventArgs args)
        {
            InterstitialReklamYukle();
        }

        private void RewardedVideoDelegate(object sender, EventArgs args)
        {
            RewardedReklamYukle();
        }

        private void InterstitialYuklenemedi(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log(args.LoadAdError.ToString());
            interstitialOtomatikYeniIstekZamani = Time.realtimeSinceStartup + 30f;

            if (interstitialReklam != null)
            {
                interstitialReklam.Destroy();
                interstitialReklam = null;
            }
        }

        private void RewardedVideoYuklenemedi(object sender, AdFailedToLoadEventArgs args)
        {
            Debug.Log(args.LoadAdError.ToString());
            rewardedVideoOtomatikYeniIstekZamani = Time.realtimeSinceStartup + 30f;

            if (rewardedVideoReklam != null)
            {
                rewardedVideoReklam.Destroy();
                rewardedVideoReklam = null;
            }
        }

        public static bool InterstitialHazirMi()
        {
            if (instance == null)
                return false;

            if (instance.interstitialReklam == null)
                return false;

            return instance.interstitialReklam.IsLoaded();
        }

        public static void InterstitialReklamAl()
        {
            if (instance == null)
                return;

            instance.InterstitialReklamYukle();
        }

        public static void InsterstitialGoster()
        {
            if (instance == null)
                return;

            if (instance.interstitialReklam == null)
            {
                instance.InterstitialReklamYukle();
                if (instance.interstitialReklam == null)
                    return;
            }

            if (instance.interstitialGosterCoroutine != null)
            {
                instance.StopCoroutine(instance.interstitialGosterCoroutine);
                instance.interstitialGosterCoroutine = null;
            }

            if (instance.interstitialReklam.IsLoaded())
                instance.interstitialReklam.Show();
            else
            {
                if (Time.realtimeSinceStartup >= instance.interstitialIstekTimeoutZamani)
                    instance.InterstitialReklamYukle();

                instance.interstitialGosterCoroutine = instance.InsterstitialGosterCoroutine();
                instance.StartCoroutine(instance.interstitialGosterCoroutine);
            }
        }

        public static bool RewardedReklamHazirMi()
        {
            if (instance == null)
                return false;

            if (instance.rewardedVideoReklam == null)
                return false;

            return instance.rewardedVideoReklam.IsLoaded();
        }

        public static void RewardedReklamAl()
        {
            if (instance == null)
                return;

            instance.RewardedReklamYukle();
        }

        public static void RewardedReklamGoster(RewardedVideoOdul odulFonksiyonu)
        {
            if (instance == null)
                return;

            if (instance.rewardedVideoReklam == null)
            {
                instance.RewardedReklamYukle();
                if (instance.rewardedVideoReklam == null)
                    return;
            }
    
            if (instance.rewardedVideoGosterCoroutine != null)
            {
                instance.StopCoroutine(instance.rewardedVideoGosterCoroutine);
                instance.rewardedVideoGosterCoroutine = null;
            }

            instance.odulDelegate = odulFonksiyonu;

            if (instance.rewardedVideoReklam.IsLoaded())
                instance.rewardedVideoReklam.Show();
            else
            {
                if (Time.realtimeSinceStartup >= instance.rewardedVideoIstekTimeoutZamani)
                    instance.RewardedReklamYukle();

                instance.rewardedVideoGosterCoroutine = instance.RewardedVideoGosterCoroutine();
                instance.StartCoroutine(instance.rewardedVideoGosterCoroutine);
            }
        }

        private IEnumerator InsterstitialGosterCoroutine()
        {
            float istekTimeoutAni = Time.realtimeSinceStartup + 2.5f;
            while (!interstitialReklam.IsLoaded())
            {
                if (Time.realtimeSinceStartup > istekTimeoutAni)
                    yield break;

                yield return null;

                if (interstitialReklam == null)
                    yield break;
            }

            interstitialReklam.Show();
        }

        private IEnumerator RewardedVideoGosterCoroutine()
        {
            float istekTimeoutAni = Time.realtimeSinceStartup + 10f;
            while (!rewardedVideoReklam.IsLoaded())
            {
                if (Time.realtimeSinceStartup > istekTimeoutAni)
                    yield break;

                yield return null;

                if (rewardedVideoReklam == null)
                    yield break;
            }

            rewardedVideoReklam.Show();
        }

        private void RewardedVideoOdullendir(object sender, Reward odul)
        {
            if (odulDelegate != null)
            {
                odulDelegate(odul);
                odulDelegate = null;
            }
        }
        
        void RewardedReklamGosterildi( GoogleMobileAds.Api.Reward odul )
        {
            AdsSystem.AdsPrize.Invoke();
        }

        private void RewardGoster()
        {
            if (RewardedReklamHazirMi())
            {
                RewardedReklamGoster(RewardedReklamGosterildi);
            }
            else
            {
                MenuSystem.OpenWarning.Invoke("SORRY ,NO ADS :( TRY AGAIN LATER");
            }
        }

        private void InsterGoster()
        {
            if (InterstitialHazirMi())
            {
                InsterstitialGoster();
            }
        }
        
        private void OnEnable()
        {
            AdsSystem.ShowRewardEvent.AddListener(RewardGoster);
            AdsSystem.ShowFScreenAdsEvent.AddListener(InsterGoster);
        }

        private void OnDisable()
        {
            AdsSystem.ShowRewardEvent.RemoveListener(RewardGoster);
            AdsSystem.ShowFScreenAdsEvent.RemoveListener(InsterGoster);
        }
    }
}
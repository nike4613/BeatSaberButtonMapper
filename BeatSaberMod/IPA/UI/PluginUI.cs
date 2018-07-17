using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMod.IPA.UI
{
    class PluginUI : MonoBehaviour
    {
        static PluginUI _instance;

        //private Logger log = new Logger("BeatSaverDownloader");

        //public BeatSaverMasterViewController _beatSaverViewController;

        private RectTransform _mainMenuRectTransform;
        private MainMenuViewController _mainMenuViewController;
        private BindingsConfigMainViewController mainViewController;

        public static void OnLoad()
        {
            if (_instance != null)
            {
                return;
            }
            new GameObject("BeatSaver Plugin").AddComponent<PluginUI>();
        }

        public void Awake()
        {
            _instance = this;
            //_votingUI = gameObject.AddComponent<VotingUI>();
            //_tweaks = gameObject.AddComponent<SongListUITweaks>();
        }

        public void Start()
        {
            //playerId = ReflectionUtil.GetPrivateField<string>(PersistentSingleton<PlatformLeaderboardsModel>.instance, "_playerId");

            //StartCoroutine(_votingUI.WaitForResults());
            //_votingUI.continuePressed += _votingUI_continuePressed;
            //StartCoroutine(WaitForSongListUI());

            //_levelCollections = Resources.FindObjectsOfTypeAll<LevelCollectionsForGameplayModes>().FirstOrDefault();
            //_levelCollectionsForGameModes = ReflectionUtil.GetPrivateField<LevelCollectionsForGameplayModes.LevelCollectionForGameplayMode[]>(_levelCollections, "_collections").ToList();

            try
            {
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _mainMenuRectTransform = _mainMenuViewController.transform as RectTransform;

                //_levelSelectionFlowCoordinator = Resources.FindObjectsOfTypeAll<LevelSelectionFlowCoordinator>().First();

                //ReflectionUtil.GetPrivateField<SongListViewController>(_levelSelectionFlowCoordinator, "_songListViewController").didSelectSongEvent += PluginUI_didSelectSongEvent;

                CreateButton();
            }
            catch (Exception e)
            {
                //log.Exception("EXCEPTION ON AWAKE(TRY CREATE BUTTON): " + e);
            }
        }

        private void CreateButton()
        {
            Button button = BeatSaberUI.CreateUIButton(_mainMenuRectTransform, "SettingsButton");

            try
            {
                (button.transform as RectTransform).anchoredPosition = new Vector2(-7f, 18f);
                (button.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

                BeatSaberUI.SetButtonText(ref button, "Key Mapper");

                button.onClick.AddListener(delegate () {

                    try
                    {
                        if (mainViewController == null)
                        {
                            mainViewController = BeatSaberUI.CreateViewController<BindingsConfigMainViewController>();
                        }
                        _mainMenuViewController.PresentModalViewController(mainViewController, null, false);

                    }
                    catch (Exception e)
                    {
                        //log.Exception("EXCETPION IN BUTTON: " + e.Message);
                    }

                });

            }
            catch (Exception e)
            {
                //log.Exception("EXCEPTION: " + e.Message);
            }

        }
    }
}

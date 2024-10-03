using LoLSDK;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using util;

namespace RM_MST
{
    // The title manager.
    public class TitleManager : MonoBehaviour
    {
        // The singleton instance.
        private static TitleManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The title screen UI.
        public TitleUI titleUI;

        // The title audio for the game.
        public TitleAudio titleAudio;

        // The scene loaded when start is selected.
        public string startScene = "WorldScene";

        // Constructor
        private TitleManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;

                // Checks if LOL SDK has been initialized.
                GameSettings settings = GameSettings.Instance;

                // Gets an instance of the LOL manager.
                LOLManager lolManager = LOLManager.Instance;

                // Language
                JSONNode defs = SharedState.LanguageDefs;

                // Translate text.
                if (defs != null)
                {
                    // ...
                }
                else
                {
                    // Mark all of the text.
                    LanguageMarker marker = LanguageMarker.Instance;

                    // ...
                }

                // Checks for initialization
                if (LOLSDK.Instance.IsInitialized)
                {
                    // NOTE: the buttons disappear for a frame if there is no save state.
                    // It doesn't effect anything, but it's jarring visually.
                    // As such, the Update loop keeps them both on.

                    // TODO: maybe search for the title UI instance?

                    // Set up the game initializations.
                    if (titleUI.newGameButton != null && titleUI.continueButton != null)
                        lolManager.saveSystem.Initialize(titleUI.newGameButton, titleUI.continueButton);

                    // Don't disable the continue button, otherwise the save data can't be loaded.
                    // Enables/disables the continue button based on if there is loaded data or not.
                    // continueButton.interactable = lolManager.saveSystem.HasLoadedData();
                    // Continue button is left alone.

                    // Since the player can't change the tutorial settings anyway when loaded from InitScene...
                    // These are turned off just as a safety precaution. 
                    // This isn't needed since the tutorial is activated by default if going from InitScene...
                    // And can't be turned off.
                    // overrideTutorial = true;
                    // continueTutorial = true;

                    // LOLSDK.Instance.SubmitProgress();
                }
                else
                {
                    Debug.LogError("LOL SDK NOT INITIALIZED.");

                    // You can save and go back to the menu, so the continue button is usable under that circumstance.
                    if (lolManager.saveSystem.HasLoadedData()) // Game has loaded data.
                    {
                        // TODO: manage tutorial content.
                    }
                    else // No loaded data.
                    {
                        // TODO: manage tutorial content.
                    }

                    // TODO: do you need this?
                    // // Have the button be turned on no matter what for testing purposes.
                    // titleUI.continueButton.interactable = true;

                    // Adjust the audio settings since the InitScene was not used.
                    settings.AdjustAllAudioLevels();
                }
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...

        }

        // Gets the instance.
        public static TitleManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<TitleManager>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Title Manager (singleton)");
                        instance = go.AddComponent<TitleManager>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been initialized.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Starts the game (general function for moving to the GameScene).
        public void StartGame()
        {
            // If the loading screen is being used.
            if (LoadingScreenCanvas.Instance.IsUsingLoadingScreen())
            {
                LoadingScreenCanvas.Instance.LoadScene(startScene);
            }
            else
            {
                SceneManager.LoadScene(startScene);
            }
        }

        // Starts a new game.
        public void StartNewGame()
        {
            // Clear out the loaded data and last save if the LOLSDK has been initialized.
            LOLManager.Instance.saveSystem.ClearLoadedAndLastSaveData();

            // Start the game.
            StartGame();
        }

        // Continues a saved game.
        public void ContinueGame()
        {
            // New
            // NOTE: a callback is setup onclick to load the save data.
            // Since that might happen after this function is processed...
            // Loaded data does not need to be checked for at this stage.

            //// If the user's tutorial settings should be overwritten, do so.
            //if (overrideTutorial)
            //    GameSettings.Instance.UseTutorial = continueTutorial;

            // Starts the game.
            StartGame();
        }


        // Clears out the save.
        // TODO: This is only for testing, and the button for this should not be shown in the final game.
        public void ClearSave()
        {
            LOLManager.Instance.saveSystem.lastSave = null;
            LOLManager.Instance.saveSystem.loadedData = null;

            titleUI.continueButton.interactable = false;
        }

        // Quits the game (will not be used in LOL version).
        public void QuitGame()
        {
            Application.Quit();
        }

        // Update is called once per frame
        void Update()
        {
            // ...
        }

        // This function is called when the MonoBehaviour will be destroyed.
        private void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }
        }
    }
}
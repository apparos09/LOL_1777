using LoLSDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RM_MST
{
    // The text translator for TMP objects.
    // Translates a TMP_Text object's text using the provided key.
    public class TMP_TextTranslator : MonoBehaviour
    {
        // The text object.
        public TMP_Text text;

        // The translation key.
        public string key = "";

        // Marks text if the language file is not loaded.
        public bool markIfFailed = true;

        // Start is called before the first frame update
        void Start()
        {
            // Grabs the text mesh pro component from the object this script is attached to.
            if (text == null)
                text = GetComponent<TMP_Text>();

            // If the SDK is initialized, the text is set, and the key is set.
            if (LOLSDK.Instance.IsInitialized && text != null && key != "")
            {
                text.text = LOLManager.Instance.GetLanguageText(key);
            }
            else
            {
                // Mark the text.
                LanguageMarker.Instance.MarkText(text);
            }
        }

        // Speaks out the provided text.
        public void SpeakText()
        {

            // Checks if the TTS is set up, if the TTS is active, and if the key string exists.
            if (LOLSDK.Instance.IsInitialized && GameSettings.Instance.UseTextToSpeech && key != string.Empty)
            {
                // Read out the text.
                LOLManager.Instance.textToSpeech.SpeakText(key);
            }
        }
    }
}
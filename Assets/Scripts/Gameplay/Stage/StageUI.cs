using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using util;

namespace RM_MST
{
    // The stage UI.
    public class StageUI : GameplayUI
    {
        // The singleton instance.
        private static StageUI instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // The stage manager.
        public StageManager stageManager;

        [Header("Text")]

        // The stage name text.
        public TMP_Text stageNameText;

        // The time text.
        public TMP_Text timeText;

        // The score text.
        public TMP_Text pointsText;

        // The units table.
        public UnitsTable unitsTable;


        [Header("Progress Bars")]

        // The points bar.
        public ProgressBar pointsBar;

        // The damage bar.
        public ProgressBar surfaceHealthBar;

        [Header("End Windows")]

        // The game win window.
        public GameObject stageWonWindow;

        // The stage clear time text.
        public TMP_Text stageWonTimeText;

        // The time clear score text.
        public TMP_Text stageWonScoreText;

        // The game lost window.
        public GameObject stageLostWindow;
        // Constructor
        private StageUI()
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
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Gets the instance.
            if (stageManager == null)
                stageManager = StageManager.Instance;
        }

        // Late start is called on the first frame update.
        protected override void LateStart()
        {
            base.LateStart();

            // Set the name.
            stageNameText.text = stageManager.stageName;

            // Updates all the UI.
            UpdateAllUI();
        }

        // Gets the instance.
        public static StageUI Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindObjectOfType<StageUI>(true);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Stage UI (singleton)");
                        instance = go.AddComponent<StageUI>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // WINDOWS
        // Closes all the windows.
        public override void CloseAllWindows()
        {
            base.CloseAllWindows();

            stageWonWindow.SetActive(false);
            stageLostWindow.SetActive(false);
        }


        // UI

        // Updates all the UI.
        public void UpdateAllUI()
        {
            UpdateTimeText();
            UpdatePointsText();
            UpdatePointsBar();
            UpdateSurfaceHealthBar();
            UpdateUnitsTable();
        }

        // Updates the time text.
        public void UpdateTimeText()
        {
            timeText.text = StringFormatter.FormatTime(stageManager.stageTime, false, true, false);
        }

        // Updates the points text.
        public void UpdatePointsText()
        {
            pointsText.text = Mathf.Round(stageManager.player.GetPoints()).ToString();
        }

        // Updates the points bar.
        public void UpdatePointsBar()
        {
            // Get the points percentage and clamp it.
            float percent = stageManager.player.GetPoints() / stageManager.pointsGoal;
            percent = Mathf.Clamp01(percent);
            pointsBar.SetValueAsPercentage(percent);
        }

        // Updates the surface health bar.
        public void UpdateSurfaceHealthBar()
        {
            // Get the percent and update the damage bar.
            float percent = stageManager.stageSurface.health / stageManager.stageSurface.maxHealth;
            percent = Mathf.Clamp01(percent);
            surfaceHealthBar.SetValueAsPercentage(percent);
        }

        // Updates the units table with the provied conversion. By default it's set to 'none'.
        public void UpdateUnitsTable(UnitsInfo.unitGroups group = UnitsInfo.unitGroups.none)
        {
            unitsTable.SetGroup(group);
        }


        // STAGE END
        // Called when the stage has been won.
        public void OnStageWon()
        {
            CloseAllWindows();
            stageWonWindow.SetActive(true);

            // Set the time and score text.
            stageWonTimeText.text = StringFormatter.FormatTime(stageManager.stageTime, false, true, false);
            stageWonScoreText.text = stageManager.stageFinalScore.ToString(); // Already calculated.

        }

        // Called when the stage has been lost.
        public void OnStageLost()
        {
            CloseAllWindows();
            stageLostWindow.SetActive(true);
        }

        // Called to restart the stage.
        public void RestartStage()
        {
            stageManager.ResetStage();
        }

        // Called when the start has been restarted.
        public void OnStageReset()
        {
            CloseAllWindows();
        }

        // Goes to the world.
        public void ToWorld()
        {
            stageManager.ToWorld();
        }


        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
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
/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using UnityEngine.SceneManagement;

namespace CGameStudio
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public static GameManager instance = null;

        [Header("Level Preferences")]
        public int levelCounts = 1;
        public int levelOneBuildIndex = 1;
        public int randomLevelStartIndex = 2;

        GameManager()
        {
            if (instance == null) instance = this;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;

            if (instance != this)
            {
                Destroy(this.transform.parent.gameObject);
            }
            else if (instance == this)
            {
                DontDestroyOnLoad(this.transform.parent.gameObject);
            }
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnLevelFinishedLoading;

        private void OnDisable() => SceneManager.sceneLoaded -= OnLevelFinishedLoading;

        #region Private Methods

        private int GenerateSceneIndex()
        {
            if (levelCounts < 2) return randomLevelStartIndex;

            int randomSceneIndex = Random.Range(randomLevelStartIndex, SceneManager.sceneCountInBuildSettings);
            while (randomSceneIndex == SceneManager.GetActiveScene().buildIndex)
                randomSceneIndex = Random.Range(randomLevelStartIndex, SceneManager.sceneCountInBuildSettings);
            return randomSceneIndex;
        }

        private void HandleRandomLevels()
        {
            if (GetCurrentLevel() == levelCounts)
                PlayerPrefs.SetInt(GameConstants.Prefs.RANDOM_LEVELS_ACTIVE, 1);
        }

        private bool PlayRandomLevel()
        {
            if (PlayerPrefs.GetInt(GameConstants.Prefs.RANDOM_LEVELS_ACTIVE) == 1)
            {
                SceneManager.LoadScene(GenerateSceneIndex());
                return true;
            }
            return false;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name.StartsWith(GameConstants.LEVEL_PREFIX))
            {
                if (SceneManager.GetActiveScene().name.Equals(GameConstants.LEVEL_FIRST))
                {
                    levelOneBuildIndex = SceneManager.GetActiveScene().buildIndex;
                    PlayerPrefs.SetInt(GameConstants.Prefs.LEVEL_START_INDEX, levelOneBuildIndex);
                }
            }
        }

        #endregion

        #region Public Methods

        public void PlayCurrentLevel()
        {
            if (PlayRandomLevel()) return;

            if (PlayerPrefs.HasKey(GameConstants.Prefs.LEVEL_CURRENT))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt(GameConstants.Prefs.LEVEL_START_INDEX) + PlayerPrefs.GetInt(GameConstants.Prefs.LEVEL_CURRENT));
            }
            else
            {
                // HANDLE: Check the value of SplashScene.GameManager.levelOneIndex
                PlayerPrefs.SetInt(GameConstants.Prefs.LEVEL_START_INDEX, levelOneBuildIndex);
                PlayerPrefs.SetInt(GameConstants.Prefs.LEVEL_CURRENT, 0);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void PlayNextLevel()
        {
            if (PlayRandomLevel()) return;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        #endregion

        #region GameController

        private GameController activeGameController;

        public GameController GetActiveController() => instance.activeGameController;
        public InputManager GetInputManager() => activeGameController.inputManager;
        public PathPlot GetPathPlot() => activeGameController.GetComponent<PathPlot>();
        public PlayerStack GetPlayerStack() => activeGameController.GetComponent<PathPlot>().playerStack;
        public SetGradientColor GetGradientColor() => activeGameController.GetComponent<SetGradientColor>();
        public ParticleController GetParticleController() => activeGameController.GetComponent<ParticleController>();

        public void SetActiveController(GameController currentGameController) => instance.activeGameController = currentGameController;

        #endregion


        #region IGameManager

        public int GetCurrentLevel() => PlayerPrefs.GetInt(GameConstants.Prefs.LEVEL_CURRENT) + 1;

        public void LevelCompleted()
        {
            // print($"onLevel_Completed: {GetCurrentLevel()}");

            // NOTE Do not chance order.
            HandleRandomLevels();
            PlayerPrefs.SetInt(GameConstants.Prefs.LEVEL_CURRENT, GetCurrentLevel());
        }

        public void LevelFailed()
        {
            // print($"onLevel_Failed: {GetCurrentLevel()}");
        }

        public void LevelStarted()
        {
            // print($"onLevel_Started: {GetCurrentLevel()}");
        }

        #endregion
    }
}
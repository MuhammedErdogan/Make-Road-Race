/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;

namespace CGameStudio
{
    public class BaseGameController : MonoBehaviour
    {
        private float _inGameTimeScale = 1f;
        private GameStates _currentState = GameStates.PrePlay;

        [Header("Controllers")]
        [HideInInspector] public InputManager inputManager;
        public CameraController cameraController;
        public PlayerController playerController;
        public UIController uiController;

        [Header("Preferences")]
        public bool autoPause = true;

        private void Awake()
        {
            Time.timeScale = 1f;
            inputManager = playerController.GetComponent<InputManager>();
        }

        public virtual void Start()
        {
            // Check the StartUI
            if (uiController.startUIActive)
                // Register with the InputManager that will fire the event when the player touch for the first time.
                inputManager.StartInput += TrackStartInput;
            else
                // Start the game
                LevelStarted();

            // Write code from here!!!
        }

        #region InputManager Events

        private void TrackStartInput()
        {
            // Unregister with the event
            playerController.GetComponent<InputManager>().StartInput -= TrackStartInput;
            // Start the game
            LevelStarted();
        }

        #endregion

        public virtual void LevelCompleted()
        {
            if (_currentState == GameStates.Completed ||
                _currentState == GameStates.Paused ||
                _currentState == GameStates.PrePlay ||
                _currentState == GameStates.Failed)
                return;

            _currentState = GameStates.Completed;

            cameraController.LevelCompleted();
            playerController.LevelCompleted();
            uiController.LevelCompleted();

            if (autoPause)
                PauseGame();

            GameManager.instance.LevelCompleted();
        }

        public virtual void LevelFailed()
        {
            if (_currentState == GameStates.Failed ||
                _currentState == GameStates.Paused ||
                _currentState == GameStates.Completed ||
                _currentState == GameStates.PrePlay)
                return;

            _currentState = GameStates.Failed;

            cameraController.LevelFailed();
            playerController.LevelFailed();
            uiController.LevelFailed();

            if (autoPause)
                PauseGame();

            GameManager.instance.LevelFailed();
        }

        public virtual void LevelStarted()
        {
            if (_currentState == GameStates.Playing ||
                _currentState == GameStates.Completed ||
                _currentState == GameStates.Failed ||
                _currentState == GameStates.Paused)
                return;

            _currentState = GameStates.Playing;

            cameraController.LevelStarted();
            playerController.LevelStarted();
            uiController.LevelStarted();

            GameManager.instance.LevelStarted();
        }

        public bool IsGamePlayable() => Time.timeScale != 0 && (_currentState == GameStates.Playing);

        public GameStates GetCurrentState() => _currentState;

        public void PauseGame()
        {
            if (Time.timeScale != 0)
            {
                _inGameTimeScale = Time.timeScale;
                Time.timeScale = 0;
                _currentState = GameStates.Paused;
            }
        }

        public void ResumeGame()
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = _inGameTimeScale;
                _currentState = GameStates.Playing;
            }
        }
    }
}
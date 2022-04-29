/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */
namespace CGameStudio
{
    interface IGameManager
    {
        int GetCurrentLevel();
        void LevelCompleted();
        void LevelFailed();
        void LevelStarted();
    }
}
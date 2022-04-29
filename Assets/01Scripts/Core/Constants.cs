/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

public class Constants
{
    public static class Animations
    {
        public static string CHARACTER_IDLE_ACTION = "IdleAction";

        public static string SKATE_ACTION = "SkateAction";
    }

    public static class Objects
    {
        public static string CANVAS = "Canvas";
        public static string CHARACTER = "Main Character";
    }

    public static class Prefs { }

    public static class Tags
    {
        public static string FINISHPROVISION = "FinishProvision";
        public static string FINISHPLATFORM = "FinishPlatform";
        public static string COLLECTIBLE = "Collectible";
        public static string PARQUETPATH = "ParquetPath";
        public static string FAILPLANE = "FailPlane";
        public static string PLOTPLANE = "PlotPlane";
        public static string OBSTACLE = "Obstacle";
        public static string JUMPAREA = "JumpArea";
        public static string PLATFORM = "Platform";
        public static string PATHEND = "PathEnd";
        public static string FINISH = "Finish";
        public static string LENS = "Lens";
    }

    public static class Layers
    {
        public static string NOINTERACTION = "NoInteraction";
        public static string PLATFORM = "Platform";
        public static string PARQUET = "Parquet";
    }

    public enum CamerasType
    {
        FollowCam, ActionCam, FinishCam, FallCam,FinishCam_2
    };
}
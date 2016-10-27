namespace App
{
    using System;
    using System.Collections.Generic;

    public static class ConfigCamera
    {
        public static float CAMERA_BEHIND_FOLLOW_SPEED;
        public static float CAMERA_BEHIND_OFFSET_Y;
        public static float CAMERA_BEHIND_OFFSET_Z;
        public static float CAMERA_BEHIND_SENSITIVITY;
        public static float CAMERA_FOLLOW_SPEED;
        public static CameraMode CAMERA_MODE;
        public static float CAMERA_PREDICTIVE_DISTANCE_FACTOR;
        public static float CAMERA_PREDICTIVE_RUN_SMOOTHING;
        public static float CAMERA_PREDICTIVE_SENSITIVITY;
        public static float CAMERA_PREDICTIVE_STATIONARY_SMOOTHING;
        public static float CAMERA_TARGET_ENEMY_FOLLOW_SPEED;
        public static float CAMERA_ULTIMATE1_DISTANCE_FACTOR;
        public static float CAMERA_ULTIMATE1_FOLLOW_SPEED;
        public static float CAMERA_ULTIMATE1_RUNNING_SENSITIVITY;
        public static float CAMERA_ULTIMATE1_STATIONARY_SENSITIVITY;
        public static float CAMERA_ZOOM_FOLLOW_SPEED;
        public static float CAMERA_ZOOM_IN_SENSITIVITY;
        public static float CAMERA_ZOOM_MAX_DIST;
        public static float CAMERA_ZOOM_OUT_SENSITIVITY;
        public static float CRIT_SHAKE_DECAY;
        public static float CRIT_SHAKE_INTENSITY;
        public static List<float> RANDOMIZED_STARTING_ROTATION_X;
        public static float RETREATED_FOV;

        static ConfigCamera()
        {
            List<float> list = new List<float>();
            list.Add(0f);
            list.Add(90f);
            list.Add(180f);
            list.Add(270f);
            RANDOMIZED_STARTING_ROTATION_X = list;
            RETREATED_FOV = 60f;
            CRIT_SHAKE_INTENSITY = 0.1f;
            CRIT_SHAKE_DECAY = 0.01f;
            CAMERA_MODE = CameraMode.Ultimate1;
            CAMERA_FOLLOW_SPEED = 11f;
            CAMERA_ZOOM_FOLLOW_SPEED = 20f;
            CAMERA_ZOOM_OUT_SENSITIVITY = 10f;
            CAMERA_ZOOM_IN_SENSITIVITY = 0.3f;
            CAMERA_ZOOM_MAX_DIST = 130f;
            CAMERA_TARGET_ENEMY_FOLLOW_SPEED = 2f;
            CAMERA_BEHIND_FOLLOW_SPEED = 5f;
            CAMERA_BEHIND_SENSITIVITY = 0.02f;
            CAMERA_BEHIND_OFFSET_Y = 7f;
            CAMERA_BEHIND_OFFSET_Z = 10f;
            CAMERA_PREDICTIVE_STATIONARY_SMOOTHING = 2f;
            CAMERA_PREDICTIVE_RUN_SMOOTHING = 4f;
            CAMERA_PREDICTIVE_DISTANCE_FACTOR = 1.3f;
            CAMERA_PREDICTIVE_SENSITIVITY = 0.05f;
            CAMERA_ULTIMATE1_FOLLOW_SPEED = 15f;
            CAMERA_ULTIMATE1_DISTANCE_FACTOR = 1f;
            CAMERA_ULTIMATE1_STATIONARY_SENSITIVITY = 0.005f;
            CAMERA_ULTIMATE1_RUNNING_SENSITIVITY = 0.015f;
        }

        public enum CameraMode
        {
            Follow,
            ZoomInOut,
            TargetEnemy,
            Behind,
            Predictive,
            Ultimate1
        }
    }
}


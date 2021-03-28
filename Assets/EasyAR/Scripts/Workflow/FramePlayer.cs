//================================================================================================================================
//
//  Copyright (c) 2015-2021 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
//  EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
//  and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
//
//================================================================================================================================

using System.Collections;
using UnityEngine;

namespace easyar
{
    /// <summary>
    /// <para xml:lang="en"><see cref="MonoBehaviour"/> which controls <see cref="InputFramePlayer"/> in the scene, providing a few extensions in the Unity environment. There is no need to use <see cref="InputFramePlayer"/> directly.</para>
    /// <para xml:lang="zh">在场景中控制<see cref="InputFramePlayer"/>的<see cref="MonoBehaviour"/>，在Unity环境下提供功能扩展。不需要直接使用<see cref="InputFramePlayer"/>。</para>
    /// </summary>
    public class FramePlayer : FrameSource
    {
        /// <summary>
        /// <para xml:lang="en">File path type. Set before OnEnable or Start.</para>
        /// <para xml:lang="zh">路径类型。可以在OnEnable或Start之前设置。</para>
        /// </summary>
        public WritablePathType FilePathType;

        /// <summary>
        /// <para xml:lang="en">File path. Set before OnEnable or Start.</para>
        /// <para xml:lang="zh">文件路径。可以在OnEnable或Start之前设置。</para>
        /// </summary>
        public string FilePath = string.Empty;

        private InputFramePlayer player;
        private bool isStarted;
        private bool isPrepared;
        private bool isPaused;
        private DisplayEmulator display;

        /// <summary>
        /// <para xml:lang="en"> Whether the playback is completed.</para>
        /// <para xml:lang="zh"> 是否已完成播放。</para>
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                if (isPrepared)
                {
                    return player.isCompleted();
                }
                return false;
            }
        }

        /// <summary>
        /// <para xml:lang="en"> Total expected playback time. The unit is second.</para>
        /// <para xml:lang="zh"> 预期的总播放时间。单位为秒。</para>
        /// </summary>
        public float Length
        {
            get
            {
                if (isPrepared)
                {
                    return (float)player.totalTime();
                }
                return 0;
            }
        }

        /// <summary>
        /// <para xml:lang="en"> Current time played.</para>
        /// <para xml:lang="zh"> 已经播放的时间。</para>
        /// </summary>
        public float Time
        {
            get
            {
                if (isPrepared)
                {
                    return (float)player.currentTime();
                }
                return 0;
            }
        }

        public override bool HasSpatialInformation
        {
            get { return true; }
        }

        internal IDisplay Display
        {
            get { return display; }
        }

        /// <summary>
        /// MonoBehaviour Awake
        /// </summary>
        protected virtual void Awake()
        {
            if (!EasyARController.Initialized)
            {
                return;
            }
            player = InputFramePlayer.create();
        }

        /// <summary>
        /// MonoBehaviour OnEnable
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            if (player != null && isStarted && !isPaused)
            {
                player.resume();
            }
        }

        /// <summary>
        /// MonoBehaviour Start
        /// </summary>
        protected virtual void Start()
        {
            isStarted = true;
            Play();
        }

        /// <summary>
        /// MonoBehaviour OnDisable
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            if (player != null)
            {
                player.pause();
            }
        }

        /// <summary>
        /// MonoBehaviour OnDestroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (player != null)
            {
                player.Dispose();
            }
        }

        public bool Play()
        {
            if (isPrepared)
            {
                isPaused = false;
                if (enabled)
                {
                    player.resume();
                }
                return true;
            }
            var path = FilePath;
            if (FilePathType == WritablePathType.PersistentDataPath)
            {
                path = Application.persistentDataPath + "/" + path;
            }
            isPrepared = player.start(path);
            isPaused = false;
            if (isPrepared)
            {
                display = new DisplayEmulator();
                display.EmulateRotation(player.initalScreenRotation());
            }
            else
            {
                GUIPopup.EnqueueMessage(typeof(FramePlayer) + " fail to start with file: " + path, 5);
            }
            if (enabled)
            {
                OnEnable();
            }
            return isPrepared;
        }

        public void Stop()
        {
            isPrepared = false;
            isPaused = false;
            display = null;
            OnDisable();
            if (player != null)
            {
                player.stop();
            }
        }

        public void Pause()
        {
            if (isPrepared)
            {
                isPaused = true;
                player.pause();
            }
        }

        public override void Connect(InputFrameSink val)
        {
            base.Connect(val);
            if (player != null)
            {
                player.output().connect(val);
            }
        }
    }
}

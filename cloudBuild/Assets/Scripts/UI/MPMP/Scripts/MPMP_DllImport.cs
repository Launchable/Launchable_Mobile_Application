/*
* MPMP
* Copyright © 2016 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/

#define MPMP_FULL
//#define MPMP_MOBILE
//#define MPMP_WINDOWS
using UnityEngine;
using System.Collections;

using System;
using System.Runtime.InteropServices;

namespace monoflow
{
    public partial class MPMP : MonoBehaviour
    {

        private const string NOIMPL = "Sorry! <color='yellow'>{0}</color> is not implemented in this version";

#if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
     private const string DLL_PATH = "__Internal";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_ANDROID || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

		#if ((UNITY_STANDALONE_WIN && !UNITY_EDITOR_OSX)|| UNITY_EDITOR_WIN) && VLC_BACKEND
        private const string DLL_PATH = @"MPMP_VLC";
#else
        private const string DLL_PATH = @"MPMP";
#endif
        //private const string DLL_PATH = @"MPMP";

#endif


#if MPMP_FULL || (MPMP_MOBILE && ((UNITY_IOS && !UNITY_EDITOR_WIN) || (UNITY_ANDROID && !UNITY_EDITOR))) || (MPMP_WINDOWS && (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN))

#if (UNITY_ANDROID && !UNITY_EDITOR)
        [DllImport(DLL_PATH)]
        private static extern void SetOBBPath(int id, [MarshalAs(UnmanagedType.LPStr)] string path);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
          [DllImport(DLL_PATH)]
		private static extern void MPMP_SetNativeTextureID(int id,int texId);
          [DllImport(DLL_PATH)]
		private static extern void MPMP_UpdateNativeTexture(int _id, int texId);
#else

#endif
        [DllImport(DLL_PATH)]
		private static extern void _ext2_SetLogCallback(int id, nativeCallback cb);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_UnityPluginInit();

#if !(UNITY_WEBGL && !UNITY_EDITOR)
        [DllImport(DLL_PATH)]
        private static extern IntPtr MPMP_GetRenderEventFunc();
#endif

        [DllImport(DLL_PATH)]
        private static extern int MPMP_New(bool isLinear);


        [DllImport(DLL_PATH)]
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || (UNITY_IPHONE && !UNITY_EDITOR_WIN)
         public static extern void MPMP_SetCallbackFunction(int id, NativeCallbackDelegateAOT fp);
#else
        public static extern void MPMP_SetCallbackFunction(int id, IntPtr fp);
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport(DLL_PATH)]
        public static extern void MPMP_SetCallbackFunction(int id, NativeCallbackDelegateAOT fp);
       // public static extern void MPMP_SetCallbackFunction(webGLCallback fp);
#endif


#if ((UNITY_STANDALONE_WIN && !UNITY_EDITOR_OSX) || UNITY_EDITOR_WIN)
        [DllImport(DLL_PATH)]
        public static extern void MPMP_UpdateCallbacks(int id);
#endif
        ///*
#if ((UNITY_STANDALONE_WIN && !UNITY_EDITOR_OSX) || UNITY_EDITOR_WIN) && MPMP_DEBUG
        [DllImport(DLL_PATH)]
        private static extern void MPMP_InitDebugConsole();

        [DllImport(DLL_PATH)]
        private static extern void MPMP_CloseDebugConsole();   
        
        [DllImport(DLL_PATH)]
        private static extern bool MPMP_IsMEPlayerInitialized();

        [DllImport(DLL_PATH)]
        public static extern void MPMP_SetDebugFunction(IntPtr fp);
#endif
        // */

#if !(UNITY_WEBGL && !UNITY_EDITOR)

        [DllImport(DLL_PATH, EntryPoint = "MPMP_Load", CharSet = CharSet.Unicode)]
   
        #if (((UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN) && VLC_BACKEND) || (!UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_OSX 
        private static extern void MPMP_Load(int id, [MarshalAs(UnmanagedType.LPStr)] string path);
#else
        private static extern void MPMP_Load(int id, [MarshalAs(UnmanagedType.LPWStr)] string path);
       
#endif


#else
        //WebGL
        [DllImport(DLL_PATH)]
         private static extern void MPMP_Load(int id, string path);
#endif

        // [DllImport(DLL_PATH, EntryPoint = "Load")]
        // private static extern void MPMP_Load(int id, [MarshalAs(UnmanagedType.LPStr)] string path);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_Play(int id);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_Pause(int id);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_Stop(int id);
        
        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetAutoPlay(int id, bool status);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SeekTo(int id, float t, bool normalized);



#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || (UNITY_IPHONE && !UNITY_EDITOR_WIN)
		[DllImport(DLL_PATH)]
		private static extern void MPMP_SeekToWithTolerance(int _id, float seek,float tolerance,bool normalized);
#endif

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetVolume(int id, float t);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetBalance(int id, double fBal);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetLooping(int id, bool status);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetPlaybackRate(int id, float rate);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_SetAudioTrack(int id, int track);

        [DllImport(DLL_PATH)]
        private static extern bool MPMP_HasAudioTrack(int id, int track);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_Destroy(int id);    

        [DllImport(DLL_PATH)]
        private static extern IntPtr MPMP_GetNativeTexture(int id);

        [DllImport(DLL_PATH)]
        private static extern void MPMP_GetNativeVideoSize(int id, ref Vector2 videoSize);
#if !(UNITY_WEBGL && !UNITY_EDITOR)

#else
         [DllImport(DLL_PATH)]
         private static extern int MPMP_GetNativeVideoSizeW(int id);
         [DllImport(DLL_PATH)]
         private static extern int MPMP_GetNativeVideoSizeH(int id);
#endif

#if !(UNITY_WEBGL && !UNITY_EDITOR)
        [DllImport(DLL_PATH)]
        private static extern double MPMP_GetCurrentPosition(int id);

        [DllImport(DLL_PATH)]
        private static extern double MPMP_GetDuration(int id);
#else
         [DllImport(DLL_PATH)]
        private static extern float MPMP_GetCurrentPosition(int id);

        [DllImport(DLL_PATH)]
        private static extern float MPMP_GetDuration(int id);
#endif

        [DllImport(DLL_PATH)]
        private static extern float MPMP_GetCurrentVolume(int id);

        [DllImport(DLL_PATH)]
        private static extern float MPMP_GetBufferLevel(int id);

       [DllImport(DLL_PATH)]
        private static extern bool MPMP_IsPlaying(int id);

        [DllImport(DLL_PATH)]
        private static extern bool MPMP_IsPaused(int id);

        [DllImport(DLL_PATH)]
        private static extern bool MPMP_IsStopped(int id);

        [DllImport(DLL_PATH)]
        private static extern bool MPMP_IsLoading(int id);

        [DllImport(DLL_PATH)]
        private static extern float MPMP_GetPlaybackRate(int _id);


#else

        private static  void SetOBBPath(int id,string path){
            Debug.Log(String.Format(NOIMPL, "SetOBBPath"));
            return ;
        }

         private static int NewMpMediaPlayer( )
        {
            Debug.Log(String.Format(NOIMPL, "NewMpMediaPlayer"));
            return 0;
        }

          private static  IntPtr GetNativeTexture(int id)
        {
            Debug.Log(String.Format(NOIMPL, "GetNativeTexture"));
            return IntPtr.Zero;
        }

       private static void GetNativeVideoSize(int id, ref Vector2 videoSize)
        {
         Debug.Log(String.Format(NOIMPL, "GetNativeVideoSize"));
        }

        private static IntPtr GetRenderEventFunc()
        {
            //Debug.Log(String.Format(NOIMPL, "GetRenderEventFunc"));
            return IntPtr.Zero;
        }
        private static void Load(int id, [MarshalAs(UnmanagedType.LPStr)] string path)
        {
            Debug.Log(String.Format(NOIMPL, "Load"));
        }

        private static void Play(int id)
        {
            Debug.Log(String.Format(NOIMPL, "Play"));
        }

        private static  void Pause(int id)
        {
            Debug.Log(String.Format(NOIMPL, "Pause"));
        }

        private static void Destroy(int id)
        {
            Debug.Log(String.Format(NOIMPL, "Destroy"));
        }

        private static  void SetAutoPlay(int id, bool status)
        {
            Debug.Log(String.Format(NOIMPL, "SetAutoPlay"));
        }

        private static  void SeekTo(int id, float t)
         {
            Debug.Log(String.Format(NOIMPL, "SeekTo"));
        }

         private static  void SetVolume(int id, float t)
         {
            Debug.Log(String.Format(NOIMPL, "SetVolume"));
        }

         private static  void SetBalance(int id, double fBal)
         {
            Debug.Log(String.Format(NOIMPL, "SetBalance"));
        }

         private static  void SetLooping(int id, bool status)
        {
            Debug.Log(String.Format(NOIMPL, "SetLooping"));
        }


        private static  double GetCurrentPosition(int id)
         {
            Debug.Log(String.Format(NOIMPL, "GetCurrentPosition"));
            return 0;
        }

         private  static double GetDuration(int id)
         {
            Debug.Log(String.Format(NOIMPL, "GetDuration"));
            return 0;
        }

         private  static float GetCurrentVolume(int id)
         {
            Debug.Log(String.Format(NOIMPL, "GetCurrentVolume"));
            return 0;
        }

        
         private static bool IsPlaying(int id)
        {
            Debug.Log(String.Format(NOIMPL, "IsPlaying"));
            return false;
        }


        private static bool IsPaused(int id)
        {
            Debug.Log(String.Format(NOIMPL, "IsPaused"));
            return false;
        }


        private static bool IsLoading(int id)
        {
            Debug.Log(String.Format(NOIMPL, "IsLoading"));
            return false;
        }

#endif


        /*
       #elif MPMP_MOBILE

               //#if (UNITY_IOS && ! UNITY_EDITOR_WIN ) || (UNITY_ANDROID && !UNITY_EDITOR) 

       #elif MPMP_WINDOWS

               //#if (UNITY_STANDALONE_WIN ||  UNITY_EDITOR_WIN )  

       #endif
       */


    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Completeds
{
    public class GameStop : MonoBehaviour
    {
       

        public void Click()
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//编辑状态下退出
#else
            Application.Quit();//打包编译后退出
#endif
        }

        public void Click_1()
        {
            

        }


    }
}
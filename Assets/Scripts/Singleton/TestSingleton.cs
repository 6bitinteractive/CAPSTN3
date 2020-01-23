using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Meowfia.Test
{
    public class TestSingleton : Singleton<TestSingleton>
    {
        private void OnEnable()
        {
            // Test
            SingletonManager.UnRegister<TestSingleton>();
        }
    }
}

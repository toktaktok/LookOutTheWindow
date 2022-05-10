using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileUtil : MonoBehaviour
{
    static void GetPlayerPrefKey(string key)
    {
        key = string.Format("{0}_{1}", Application.productName, key);
        
    }

}

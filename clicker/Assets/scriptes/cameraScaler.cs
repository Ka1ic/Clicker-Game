using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScaler : MonoBehaviour
{
    private void Awake()
    {
        Camera.main.orthographicSize = (float)Screen.height / (float)Screen.width * 5.625f;
    }
}

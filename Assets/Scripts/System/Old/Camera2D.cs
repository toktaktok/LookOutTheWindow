using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Camera2D : Subject
{
    //[HideInInspector]
    //public int minWidth;
    //[HideInInspector]
    //public int minHeight;
    //[HideInInspector]
    //public bool matchWidth;

    //[HideInInspector]
    //public int screenWidth;
    //[HideInInspector]
    //public int screenHeight;

    //public int Priority { get; set; }
    //public OBSERVER_STATE State { get; set; }

    //private Camera _cam;

    //public void OnEnable()
    //{
    //    if(Game.Instance == null)
    //    {
    //        return;

    //    }

    //    ScreenManager screenManager = Game.Instance.screenManager;
    //    OnScreenSize( screenManager.ScreenWidth, screenManager.ScreenHeight);

    //    screenManager.AddObserver(this);
    //}

    //public void OnDisable()
    //{
    //    if(Game.Instance == null)
    //    {
    //        return;
    //    }

    //    ScreenManager screenManager = Game.Instance.screenManager;
    //    screenManager.RemoveObserver(this);

    //}
    //public void OnScreenSize(int screenWidth, int screenHeight)
    //{
    //    if(_cam == null)
    //    {
    //        _cam = GetComponent<Camera>();
    //    }

    //    if(_cam.orthographic == false)
    //    {
    //        _cam.orthographic = true;
    //    }
    //    int orthographicSize = GetOrthographicSize(screenWidth, screenHeight);
    //    _cam.orthographicSize = orthographicSize;

    //    orthographicSize *= 2;
    //    this.screenWidth = (screenWidth * orthographicSize) / screenHeight;
    //    this.screenHeight = orthographicSize;

    //    OnNotify();
    //}

    //public int GetOrthographicSize(int screenWidth, int screenHeight)
    //{
    //    int orthographicSize = 0;
    //    return Mathf.RoundToInt(orthographicSize * 0.5f);
    //}

}

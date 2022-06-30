using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using UnityEngine;
using DG.Tweening;

public class IntroMovement : MonoBehaviour
{
    public GameObject player;
    
    void Start()
    {
        StartCoroutine(IntroAnimation());
    }

    IEnumerator IntroAnimation()
    {
        
        
        yield return null;
    }



}

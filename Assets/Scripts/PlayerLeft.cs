using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeft : MonoBehaviour
{
    public Player player;

    private void OnTriggerEnter(Collider interacted)
    {
        if (interacted.gameObject.layer == GlobalVariables.LayerNumber.building)
        {
            // MapManager.Instance.leftBuilding = interacted.gameObject;
        }
    }
}

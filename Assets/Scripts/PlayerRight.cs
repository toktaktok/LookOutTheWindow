using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRight : MonoBehaviour
{
    public Player player;
    
    private void OnTriggerEnter(Collider interacted)
    {
        if (interacted.gameObject.layer == GlobalVariables.LayerNumber.building)
        {
            // MapManager.Instance.rightBuilding = interacted.gameObject;
        }
    }
}

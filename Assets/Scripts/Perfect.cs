using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perfect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        switch (other.tag)
        {
            case "Egg":
                BirdBehaviour.instance._ResetPrefect();
                break;
        }
    }
}

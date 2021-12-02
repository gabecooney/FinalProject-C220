using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHighlight : MonoBehaviour
{
    private bool rotationEnabled = true;

    void OnMouseEnter()
    {
        transform.Find("Spot Light").gameObject.SetActive(true);

        if (rotationEnabled)
        {
            transform.parent.Rotate(new Vector3(0, 10, 0));
            rotationEnabled = false;
        }
    }

    private void OnMouseExit()
    {
        transform.Find("Spot Light").gameObject.SetActive(false);

        if (!rotationEnabled)
        {
            transform.parent.Rotate(new Vector3(0, -10, 0));
            rotationEnabled = true;
        }
    }

    private void OnMouseDown()
    {
        rotationEnabled = false;
        GameController.instance.currentOpeningDoor(transform.name);
    }

}

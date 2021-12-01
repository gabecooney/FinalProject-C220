using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
   [SerializeField] int _sanityScore;
    [SerializeField] string _itemName;
   

    private void OnMouseOver()
    {
        // Display Item Name
    }
    private void OnMouseUpAsButton()
    {   //Update Global Sanity and execute audio and visual effects
        // _sanityScore + SanityLevel

    }
}

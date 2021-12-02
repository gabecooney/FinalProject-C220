using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
   [SerializeField] int _sanityScore;
   [SerializeField] string _itemName;
   [SerializeField] GameObject _item;

    public GameObject getItem()
    {
        return _item;
    }

        public int getSanity()
    {
        return _sanityScore;
    }
    public string getName()
    {
        return _itemName;
    }

    public static Items chooseItem(Items[] a)
    {
        int rand = Random.Range(0, a.Length);
        Debug.Log(rand);
        return a[rand];


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController instance;
    public HealthBar healthBar;
    public AudioClip[] audioClips;

    private string currentDoor;
    private int numDoors = 12;
    private int curDoor;
    private int playerCurrentSanity;
    private GameObject outcome;

    [SerializeField] GameObject monster;
    //[SerializeField] Serialized no longer needed because the item is chosen through th
    GameObject itemObject;
    Items item;
    [SerializeField] GameObject safety;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Text doorsLeft;
    [SerializeField] Text outcomeText;
    [SerializeField] Image fadingToBlack;
    [SerializeField] int playerMaxSanity = 100;
    [SerializeField] int monsterDamage = 20;
    [SerializeField] Items[] items;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        playerCurrentSanity = playerMaxSanity;
        healthBar.SetMaxHealth(playerMaxSanity);

        curDoor = numDoors;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCurrentSanity <= 1)
        {
            Debug.Log("Game Over");
        }

        if (curDoor == 0)
        {
            Debug.Log("Player Wins");
        }
    }

    private void endSet()
    {
        outcomeText.gameObject.SetActive(true);
        fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 100f);

        if (outcome.name.Equals("Monster"))
        {
            outcomeText.text = "MONSTER";
            audioSource.PlayOneShot(audioClips[0]);
            healthBar.SetHealth(playerCurrentSanity - monsterDamage);
        }
        if (outcome.name.Equals("Item"))
        {
            //outcomeText.text = "ITEM";
            //added code for items
            

            outcomeText.text = item.getName();
            healthBar.SetHealth(playerCurrentSanity + item.getSanity());

            audioSource.PlayOneShot(audioClips[1]);
            Debug.Log("ITEM");
        }
        if (outcome.name.Equals("Safety"))
        {
            outcomeText.text = "SAFETY";
            audioSource.PlayOneShot(audioClips[2]);
            Debug.Log("SAFETY");
        }

        curDoor -= 1;
        doorsLeft.text = curDoor + " Doors Remaining";

        audioSource.PlayOneShot(audioClips[5]);

        Invoke("ResetScene", 4);

    }

    private void ResetScene()
    {
        outcomeText.gameObject.SetActive(false);
        GameObject.Find(currentDoor).transform.parent.Rotate(new Vector3(0, -80, 0));
        fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 0f);
        audioSource.PlayOneShot(audioClips[4]);

    }

    public void currentOpeningDoor(string doorName)
    {
        currentDoor = doorName;
        Debug.Log(currentDoor);
        OpenDoor();
    }

    private void OpenDoor()
    {
        GameObject.Find(currentDoor).transform.parent.Rotate(new Vector3(0, 80, 0));

        outcome = chooseOutcome();

        GameObject spawnLoc = GetSpawn();

        Instantiate(outcome, spawnLoc.transform.position, Quaternion.identity);

        audioSource.PlayOneShot(audioClips[3]);

        Invoke("endSet", 2f);
    }

    private GameObject GetSpawn()
    {
        switch (currentDoor)
        {
            case "DoorOne":
                return GameObject.Find("DoorOneSpawn");
            case "DoorTwo":
                return GameObject.Find("DoorTwoSpawn");
            case "DoorThree":
                return GameObject.Find("DoorThreeSpawn");
            default:
                return safety;
        }
    }

    private GameObject chooseOutcome()
    {
        int rand = Random.Range(0, 90);

        if (rand < 30)
        {
            return monster;
        }
        else if(rand < 60)
        {
            item = Items.chooseItem(items);
            itemObject = item.getItem(); //get GameObject
            itemObject.name = "Item";
            return itemObject;
        }
        else if (rand < 90)
        {
            return safety;
        }
        else
        {
            return null;
        }
    }
}

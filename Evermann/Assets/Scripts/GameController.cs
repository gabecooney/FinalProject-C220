using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private GameObject itemName;
    private bool currentlyEnding = false;

    // ITEM SCRIPT RELATED
    GameObject itemObject;
    Items item;
    [SerializeField] Items[] items;
    ////////////////////////////////

    [SerializeField] GameObject monster;
    //[SerializeField] GameObject item;
    [SerializeField] GameObject safety;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Text doorsLeft;
    [SerializeField] Text outcomeText;
    [SerializeField] Text damageDesc;
    [SerializeField] Image fadingToBlack;
    [SerializeField] int playerMaxSanity = 100;
    [SerializeField] int monsterDamage = 20;
    [SerializeField] BoxCollider blockDoorsCollider;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        playerCurrentSanity = playerMaxSanity;
        healthBar.SetMaxHealth(playerMaxSanity);

        curDoor = numDoors;

        blockDoorsCollider.gameObject.SetActive(false);

        doorsLeft.text = numDoors + " Doors Remaining";
    }

    // Update is called once per frame
    void Update()
    {
        if (!currentlyEnding)
        {
            if (playerCurrentSanity <= 1)
            {
                currentlyEnding = true;

                Debug.Log("Game Over");

                outcomeText.gameObject.SetActive(true);
                fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 100f);

                outcomeText.text = "You've lost your mind...";
                damageDesc.text = "Sanity is 0";

                audioSource.PlayOneShot(audioClips[7]);
                audioSource.PlayOneShot(audioClips[8]);

                Invoke("GoToMainMenu", 4);
            }

            if (curDoor == 0)
            {
                currentlyEnding = true;

                Debug.Log("Player Wins");

                outcomeText.gameObject.SetActive(true);
                fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 100f);

                outcomeText.text = "You've escaped... for now";
                damageDesc.text = "Final Sanity is " + playerCurrentSanity;

                audioSource.PlayOneShot(audioClips[6]);
                audioSource.PlayOneShot(audioClips[8]);


                Invoke("GoToMainMenu", 4);

            }
        }
        
    }

    private void endSet()
    {

        outcomeText.gameObject.SetActive(true);
        damageDesc.gameObject.SetActive(true);
        fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 100f);

        if (outcome.name.Equals("Monster(Clone)"))
        {

            outcomeText.text = "MONSTER";
            damageDesc.text = "-20 Sanity";
            playerCurrentSanity -= monsterDamage;
            healthBar.SetHealth(playerCurrentSanity);

            if (playerCurrentSanity > 1)
                audioSource.PlayOneShot(audioClips[0]);
        }
        if (outcome.name.Equals("Item(Clone)"))
        {
            //outcomeText.text = "ITEM";
            //damageDesc.text = "-X Sanity";

            outcomeText.text = item.getName();
            damageDesc.text = item.getSanity() + " Sanity";

            Debug.Log(item.getSanity());

            playerCurrentSanity += item.getSanity();

            healthBar.SetHealth(playerCurrentSanity);


            audioSource.PlayOneShot(audioClips[1]);
            Debug.Log("ITEM");
        }
        if (outcome.name.Equals("Safety(Clone)"))
        {
            outcomeText.text = "SAFETY";
            damageDesc.text = "Sanity Unchanged";
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
        damageDesc.gameObject.SetActive(false);
        GameObject.Find(currentDoor).transform.parent.Rotate(new Vector3(0, -80, 0));
        fadingToBlack.color = new Color(fadingToBlack.color.r, fadingToBlack.color.g, fadingToBlack.color.b, 0f);
        audioSource.PlayOneShot(audioClips[4]);
        Destroy(outcome);

        Invoke("ResetCollider", 3);
    }

    private void ResetCollider()
    {
        blockDoorsCollider.gameObject.SetActive(false);

    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
        Debug.Log(outcome);

        itemName = outcome;

        GameObject spawnLoc = GetSpawn();

        outcome = Instantiate(outcome, spawnLoc.transform.position, Quaternion.identity);

        if (outcome.gameObject.name.Equals("Monster(Clone)"))
        {
            outcome.transform.Rotate(new Vector3(0, 90, 0));
            outcome.transform.Translate(new Vector3(.5f, -.5f, 0));
        }
        if (outcome.gameObject.name.Equals("Safety(Clone)"))
        {
            //outcome.transform.Rotate(new Vector3(180, 0, 0));
            outcome.transform.Translate(new Vector3(0, 0, -.5f));
        }

        Debug.Log(outcome.gameObject.name);

        audioSource.PlayOneShot(audioClips[3]);

        blockDoorsCollider.gameObject.SetActive(true);


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

            //return item;
        }
        else if (rand <= 90)
        {
            return safety;
        }
        else
        {
            return null;
        }
    }
}

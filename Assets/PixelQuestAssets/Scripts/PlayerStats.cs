using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Respawn 
    public Transform respawnPoint; // Keeps track of where the player we respawn at 

    // Player stats 
    public int playerLife = 1;   // How much health the player currently has 
    public int currentCoins = 0;   // How many coins has the player collected 
    private int playerMaxHealth = 3; // What is the max health the player can have 
    private int maxCoins = 0; // What is the amount of coins in the level 

    // Rigidbody 
    private Rigidbody2D rigidbody2D; // Controls player speed 

    // Tags 
    private const string deathTag = "Death";
    private const string healthTag = "Health";
    private const string coinTag = "Coin";
    private const string respawnTag = "Respawn";
    private const string finishTag = "Finish";

    // UI 
    public TextMeshProUGUI heartText;   // Update the text showing current health
    public TextMeshProUGUI coinText;    // Update the text showing coins collected 
    public GameObject CoinParent;       // Parent we check to see how many coins are in the level 

    // Audio 
    public AudioSource deathSFX;  // Death sound effect 
    public AudioSource coinSFX;   // Coin pick up sound effect 

    void Start()
    {
        // Connect to the rigidbody 
        rigidbody2D = GetComponent<Rigidbody2D>();
        // Looks at the Coin Parent Game Object and check how many children it has, thats' the number
        // Of coins that are in the level, each time we destroy a coin the childCount would lower 
        // So we save the inforomation at the start of the game 
        maxCoins = CoinParent.transform.childCount;
        // Updates the UI to show the proper values of the level 
        coinText.text = currentCoins + "/" + maxCoins;
        heartText.text = playerLife + "/" + playerMaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag){
            // Coin tag, dicates what happens after player touches coins 
            case coinTag:
                {
                    // Plays Sound Effect 
                    coinSFX.Play();
                    // Increase the value of coins by 1
                    currentCoins++;
                    // Updates the UI 
                    coinText.text = currentCoins + "/" + maxCoins;
                    // Destroys the coins
                    Destroy(collision.gameObject);
                    break;
                }
            // Death tag, dicates what happens after player touches something that could kill them
            case deathTag:
                {
                    // Play Sound Effect
                    deathSFX.Play();
                    // Make the speed zero 
                    rigidbody2D.velocity = Vector2.zero;
                    // Take away players life 
                    playerLife--;
                    // Updates the UI 
                    heartText.text = playerLife + "/" + playerMaxHealth;
                    // If the player has lost all of their lives reset the level 
                    if (playerLife <= 0)
                    {
                        // Moves the player to the respawn point 
                        transform.position = respawnPoint.position;
                        playerLife = 1;
                        heartText.text = playerLife + "/" + playerMaxHealth;
                    }
                    break;
                }
            // Health tag, dicates what happens after player touches a heart
            case healthTag:
                {
                    // Checks if the player is full on health, if they are nothing happens 
                    if(playerLife < playerMaxHealth)
                    {
                        // If the player is missing health we increase their life
                        playerLife++;
                        // Update the UI to show new health 
                        heartText.text = playerLife + "/" + playerMaxHealth;
                        // Destroy the health object 
                        Destroy(collision.gameObject);
                    }
                    break;
            }
            // Respawn tag, dicates what happens after player touches a respawnpoint 
            case respawnTag:
                {
                    // We look for a child object called RespawnPoint, and we copy it's position
                    // This will be the new spot the player respawns from 
                    respawnPoint.position = collision.transform.Find("RespawnPoint").position;
                    break;
                }
            // Finish tag, dicates what happens after player touches a respawnpoint 
            case finishTag:
                {
                    // Gets the name of the next level from the end game object 
                    string nextLevel = collision.gameObject.GetComponent<GameEnd>().nextLevel;
                    // Loads us into that next level 
                    SceneManager.LoadScene(nextLevel);
                    break;
                }
        }
    }

}

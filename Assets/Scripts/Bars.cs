using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bars : MonoBehaviour {


    public Image currentStaminaBar; // Front image of the stamina bar
    public float maxstamina;
    public float stamina; // Represents the current stamina
    public Image currentHealthBar; // Front image of the health bar
    public float maxhp;
    public float hp; //Represents the current hp

	private GameController gameController;

    // Use this for initialization
    private void Start() {
        UpdateStaminaBar();
        UpdateHealthBar();

        // Gets the gameController to be able to end the game when if hp is < 0
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController> ();
		}

    }

    // Always scale the stamina bar
    private void UpdateStaminaBar() { 

        float r = stamina / maxstamina;
        currentStaminaBar.rectTransform.localScale = new Vector3(r, 1, 1); // Vector to scale the front stamina bar Image
    }

    //Always scale the health bar
    private void UpdateHealthBar() 
    {

        float r = hp / maxhp;
        currentHealthBar.rectTransform.localScale = new Vector3(r, 1, 1); // Vector to scale the front health bar Image

    }

    // Drops the health bar over the damage received
    private void DropHP(float damage)
    {
        hp = hp - damage; //HP Damage
        if (hp < 0)
        {
            hp = 0; // Doesn't allow to go under 0
			gameController.GameOver (); // Ends the game if hp < 0
        }

        UpdateHealthBar(); // Always Update the UI
    }

    //Used to drop the stamina when running
    private void DropStamina(){
		stamina -= 0.2f; //Stamina drop rate
		if (stamina < 0) {
			stamina = 0; // Doesn't allow stamina to go under 0
			PlayerController.run = 1.0f; // Forces the player to walk
		}
		UpdateStaminaBar (); //Always Update the UI
	}

	//Used for recovering stamina
	private void RecoverStamina(){
		stamina += 0.2f; //Stamina recover rate
		if (stamina > maxstamina) {
			stamina = maxstamina; // Doesn't allow overflow
		}
		UpdateStaminaBar (); //Always Update the UI
	}

}

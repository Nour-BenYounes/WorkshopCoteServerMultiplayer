using UnityEngine;
using TMPro;
using Mirror;
public class PlayerHealth : NetworkBehaviour
{

    public float maxHealth = 100;
    [SyncVar(hook = nameof(OnHealthChanged))]
    private float currentHealth ;
    public static bool isDead = false; 
    public TextMeshProUGUI Healthnbrtxt;
   // public TextMeshProUGUI deathanoucementtxt;


    private void OnHealthChanged(float oldHealth, float newHealth)
    {
        UpdateHealthText(newHealth);
    }

    [Command]
    public void cmdcommandhealth(float newHealth)
    {
        currentHealth = newHealth;
        UpdateHealthText(newHealth);
       
    }
  

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        isDead = false;
        cmdcommandhealth(maxHealth);

    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; 

       // currentHealth -= damageAmount;
       
        Debug.Log("Player Health: " + currentHealth);
        cmdcommandhealth(currentHealth - damageAmount);


        if (currentHealth <= 0)
        {
            Die();
        }
    }

   
    private void Die()
    {
        isDead = true;
       // Debug.Log("Player has died.");
        Healthnbrtxt.text = "Dead";  
    }

    [Command]
    private void cmddeathannoucement()
    {

        DieRPC();

    }
    [ClientRpc]
    private void DieRPC()
    {

        Debug.Log("Player has died.");

    }


    private void UpdateHealthText(float newHealth)
    {
        Healthnbrtxt.text = newHealth > 0 ? "Health: " + newHealth.ToString("F0") : "Dead";
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponHolder; // The weapon the spaceship currently holds
    private Weapon weapon; // Reference to the weapon being picked up

    void Awake()
    {
        weapon = weaponHolder;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null)
        {
            TurnVisual(false, weapon); // Disable visuals initially
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Memasuki Objek Trigger");

            // Check if player already has a weapon
            if (weaponHolder != null)
            {
                // Detach the old weapon (if needed, drop or remove from scene)
                weaponHolder.transform.SetParent(null);
                TurnVisual(false, weaponHolder);
            }

            // Set the new weapon as the player's weapon
            weaponHolder = weapon;
            
            // Attach the new weapon to the player
            weaponHolder.transform.SetParent(other.transform);
            weaponHolder.transform.localPosition = Vector3.zero;

            // Enable all components of the new weapon (visual feedback)
            TurnVisual(true, weaponHolder);
        }
    }

    // Method to handle visual feedback for weapon pickup
    void TurnVisual(bool on)
    {
        if (weapon != null)
        {
            foreach (Component component in weapon.GetComponents<Component>())
            {
                if (component is Renderer renderer)
                {
                    renderer.enabled = on; // Enable or disable renderers
                }
                else if (component is Collider2D collider)
                {
                    collider.enabled = on; // Enable or disable colliders
                }
                // Add more conditions if you have other components to enable/disable
            }
        }
    }

    // Overloaded method to handle visual feedback with weapon parameter
    void TurnVisual(bool on, Weapon weapon)
    {
        if (weapon != null)
        {
            foreach (Component component in weapon.GetComponents<Component>())
            {
                if (component is Renderer renderer)
                {
                    renderer.enabled = on;
                }
                else if (component is Collider2D collider)
                {
                    collider.enabled = on;
                }
                // Add more conditions if you have other components to enable/disable
            }
        }
    }
}
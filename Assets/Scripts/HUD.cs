using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Slider healthBar;
    public Slider cooldownBar;
    public Image activeWeaponIcon;
    public Image inactiveWeaponIcon;

    internal void UpdateEquipment(Weapon active, Weapon inactive)
    {
        if (active != null)
        {
            activeWeaponIcon.sprite = active.icon;
        }
        if (inactive != null)
        {
            inactiveWeaponIcon.sprite = inactive.icon;
        }
    }

    internal void UpdateHealth(float health)
    {
        
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //public FractionBar healthBar;
    //public FractionBar cooldownBar;
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
}

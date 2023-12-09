using UnityEngine;
using UnityEngine.UI;

public class ChestMenu : MonoBehaviour
{
    public Image chestWeaponIcon;
    public Image activeWeaponIcon;
    public Image inactiveWeaponIcon;
    Player player;
    Chest chest;

    internal void SetContents(Player player, Chest chest)
    {
        this.player = player;
        this.chest = chest;
        UpdateIcons();
    }

    void UpdateIcons()
    {
        if (chest.weapon != null)
        {
            chestWeaponIcon.sprite = chest.weapon.icon;
        }
        if (player.activeWeapon != null)
        {
            activeWeaponIcon.sprite = player.activeWeapon.icon;
        }
        if (player.inactiveWeapon != null)
        {
            inactiveWeaponIcon.sprite = player.inactiveWeapon.icon;
        }
    }

    public void SwapActive()
    {
        if (player != null && chest != null)
        {
            (player.activeWeapon, chest.weapon) = (chest.weapon, player.activeWeapon);
            UpdateIcons();
            chest.UpdateHint();
            player.UpdateActiveEquipment();
        }
    }

    public void SwapInactive()
    {
        if (player != null && chest != null)
        {
            (player.inactiveWeapon, chest.weapon) = (chest.weapon, player.inactiveWeapon);
            UpdateIcons();
            chest.UpdateHint();
            player.UpdateActiveEquipment();
        }
    }

    public void Exit()
    {
        GameStateManager.Instance.UpdateEscape();
    }
}

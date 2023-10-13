using UnityEditor.Animations;
using UnityEngine;

public abstract class Weapon : ScriptableObject
{
    public Sprite icon;
    public AnimatorController animationController;

    public abstract void PerformAttack(Player player);
}
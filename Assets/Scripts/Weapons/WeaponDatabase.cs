using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/WeaponDatabase")]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> AllWeapons;
}

using UnityEngine;
using System.Collections;

public static class WeaponExtensions {
	public static bool Beats(this WeaponType first, WeaponType second) {
		if(first == WeaponType.Sharp)
			return second == WeaponType.Long;
		else if(first == WeaponType.Blunt)
			return second == WeaponType.Sharp;
		else if(first == WeaponType.Long)
			return second == WeaponType.Blunt;
		else
			return false;
	}
}

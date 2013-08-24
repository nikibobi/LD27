using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ColorUtil {
	public static readonly Color[] Palete;
	
	static ColorUtil() {
		Palete = new Color[] {
			Color.blue,
			Color.cyan,
			Color.yellow,
			Color.green,
			Color.red,
			Color.magenta
		};
	}
	
	public static Color RandomPalete {
		get {
			return Palete[Random.Range(0, Palete.Length)];
		}
	}
}

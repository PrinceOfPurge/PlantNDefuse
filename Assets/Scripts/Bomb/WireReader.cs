using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WireColor
{
	None,
	Red,
	Green,
	Blue,

	COLOR_MAX
}

public class WireReader : MonoBehaviour
{
	[SerializeField] WireColor debug_grabbedWire = WireColor.None;

	public readonly WireColor[] slots = new WireColor[3];
	[SerializeField] Image[] debug_wirePreviews;

	public bool DisableUpdate;

	void Start()
	{
		UpdateDisplay();
	}

	void Update()
	{
		if (DisableUpdate) return;

		// Select wire
		if (Input.GetKeyDown(KeyCode.R))
		{
			debug_grabbedWire = WireColor.Red;
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			debug_grabbedWire = WireColor.Green;
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			debug_grabbedWire = WireColor.Blue;
		}

		// Connect selected wire to slot
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ConnectWire(debug_grabbedWire, 0);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ConnectWire(debug_grabbedWire, 1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			ConnectWire(debug_grabbedWire, 2);
		}
	}

	void ConnectWire(WireColor wire, int slotIndex)
	{
		// Clear any other instances of this wire
		for (int i = 0; i < slots.Length; ++i)
		{
			if (slots[i] == wire)
			{
				slots[i] = WireColor.None;
			}
		}

		// Connect the wire
		slots[slotIndex] = wire;

		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		for (int i = 0; i < debug_wirePreviews.Length; ++i)
		{
			debug_wirePreviews[i].color = GetWireColor(slots[i]);
		}
	}

	public static Color GetWireColor(WireColor wire)
	{
		return wire switch
		{
			WireColor.Red => Color.red,
			WireColor.Green => Color.green,
			WireColor.Blue => Color.blue,
			_ => Color.black,
		};
	}
}

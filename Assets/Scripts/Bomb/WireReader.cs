using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

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

	private SerialPort data_stream = new("COM3", 9600);

	public bool DisableUpdate;

	void Start()
	{
		UpdateDisplay();

		data_stream.Open();
	}

	void Update()
	{
		if (DisableUpdate) return;

		ReadFromArduino();
		debug_ConnectWires();
		UpdateDisplay();
	}

	void debug_ConnectWires()
	{
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

	int readingByte = -1;
	void ReadFromArduino()
	{
		if (readingByte != -1)
		{
			if (!ReadUntilDone())
			{
				return;
			}
		}

		// Read until start byte
		for (int counter = 0; counter <= 100 && data_stream.ReadByte() != 233; ++counter)
		{
			if (counter == 99)
			{
				Debug.LogError("Not reading properly");
				return;
			}
		}

		readingByte = 0;

		ReadUntilDone();
	}

	bool ReadUntilDone()
	{
		// Read all available bytes
		for (; readingByte < 3; ++readingByte)
		{
			if (data_stream.BytesToRead == 0)
				break;

			int b = data_stream.ReadByte();

			WireColor wireColor = (WireColor)(readingByte + 1);

			if (b == 255)
			{
				// This wire has been disconnected, find which slot it's in

				for (int i = 0; i < slots.Length; ++i)
				{
					if (slots[i] == wireColor)
					{
						slots[i] = WireColor.None;
						break;
					}
				}

				continue;
			}

			if (b < 0 || b > 2)
			{
				Debug.LogError($"Read {b}");
				return true;
			}
			
			slots[b] = wireColor;
		}

		// Done reading
		if (readingByte == 3)
		{
			readingByte = -1;
			return true;
		}

		return false;
	}
}

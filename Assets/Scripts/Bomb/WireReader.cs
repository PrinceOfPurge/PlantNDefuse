using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using Unity.VisualScripting;

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
	public readonly WireColor[] slots = new WireColor[3];
	public bool DisableUpdate;

	private readonly SerialPort data_stream = new("COM3", 9600);

	[SerializeField] SkinnedMeshRenderer[] m_Wires;

	void Start()
	{
		UpdateDisplay();

		data_stream?.Open();
	}

	void OnDestroy()
	{
		data_stream?.Close();
	}

	void Update()
	{
		if (DisableUpdate) return;

		ReadFromArduino();
		debug_ConnectWires();

		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		for (int i = 0; i < m_Wires.Length; ++i)
		{
			if (!m_Wires[i]) continue;

			int slot = GetSlotOfWire((WireColor)i + 1);

			m_Wires[i].SetBlendShapeWeight(0, slot == 1 ? 100 : 0);
			m_Wires[i].SetBlendShapeWeight(1, slot == 2 ? 100 : 0);
			m_Wires[i].SetBlendShapeWeight(2, slot == -1 ? 100 : 0);
		}
	}

	public static Color GetWireColor(WireColor wire)
	{
		return wire switch
		{
			WireColor.Red => Color.red * 3,
			WireColor.Green => Color.green * 3,
			WireColor.Blue => Color.blue * 3,
			_ => Color.black,
		};
	}

	int GetSlotOfWire(WireColor wire)
	{
		for (int i = 0; i < slots.Length; ++i)
		{
			if (slots[i] == wire)
			{
				return i;
			}
		}

		return -1;
	}

	#region Arduino

	int readingByte = -1;
	void ReadFromArduino()
	{
		if (!data_stream.IsOpen)
			return;

		if (readingByte != -1)
		{
			if (!ReadUntilDone())
			{
				return;
			}
		}

		if (data_stream.BytesToRead == 0) return;

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

			// Disconnect all of this colour
			for (int i = 0; i < slots.Length; ++i)
			{
				if (slots[i] == wireColor)
				{
					slots[i] = WireColor.None;
				}
			}

			if (b == 255)
			{
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

	#endregion

	#region Debug

	[SerializeField] WireColor debug_grabbedWire = WireColor.None;

	// Allows connecting wires with keyboard input for testing
	void debug_ConnectWires()
	{
		// Select wire
		if (Input.GetKeyDown(KeyCode.R))
		{
			debug_grabbedWire = WireColor.Red;
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			debug_grabbedWire = WireColor.Green;
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

		if (Input.GetKeyDown(KeyCode.B))
		{
			debug_grabbedWire = WireColor.Blue;
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

		// Connect selected wire to slot
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			debug_ConnectWire(debug_grabbedWire, 0);
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			debug_ConnectWire(debug_grabbedWire, 1);
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			debug_ConnectWire(debug_grabbedWire, 2);
			AudioManager.instance.PlayOneShot(FMODEvents.instance.Wire, this.transform.position);
		}

	}

	void debug_ConnectWire(WireColor wire, int slotIndex)
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

	#endregion
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	[SerializeField] WireReader m_reader;
	[SerializeField] TMP_Text m_levelText;
	[SerializeField] TMP_Text m_timerText;

	[SerializeField] GameObject m_loseUI;

	[SerializeField] Image[] debug_testImages;
	readonly WireColor[] goalSlots = new WireColor[3];

	float m_timer;
	int m_level = 0;

	bool m_bLevelOver = false;

	void Start()
	{
		NextLevel();
		m_loseUI.SetActive(false);
	}

	void Update()
	{
		if (m_bLevelOver) return;

		UpdateTimerText();
		m_timer -= Time.deltaTime;

		// Check for match
		bool win = true;
		for (int i = 0; i < goalSlots.Length; ++i)
		{
			if (m_reader.slots[i] != goalSlots[i])
			{
				win = false;
				break;
			}
		}

		if (win)
		{
			StartCoroutine(NextLevelTransition());
			return;
		}

		// Check if time is up
		if (m_timer <= 0)
		{
			StartCoroutine(LoseTransition());
			return;
		}
	}

	IEnumerator NextLevelTransition()
	{
		m_bLevelOver = true;

		yield return new WaitForSeconds(2);

		NextLevel();
	}

	IEnumerator LoseTransition()
	{
		m_bLevelOver = true;

		yield return new WaitForSeconds(2);

		ShowLoseText();
	}

	public void Restart()
	{
		SceneManager.LoadScene("BombTest");
	}

	void ShowLoseText()
	{
		m_loseUI.SetActive(true);
	}

	void CreateRandomConfig_Internal()
	{
		// Pick 3 random colors
		for (int i = 0; i < 3; ++i)
		{
			int wireColorIndex = Random.Range(1, (int)WireColor.COLOR_MAX);

			// Check for duplicates with any already picked wires
			for (int iter = 0; iter < 100; ++iter)
			{
				bool duplicate = false;
				for (int w = 0; w < goalSlots.Length; ++w)
				{
					if (wireColorIndex == (int)goalSlots[w])
					{
						duplicate = true;
						break;
					}
				}

				if (!duplicate) break;

				// Increment until it's not a duplicate
				++wireColorIndex;
				if (wireColorIndex >= (int)WireColor.COLOR_MAX)
					wireColorIndex = 1;

				if (iter >= 50)
				{
					Debug.Log("Stuck in a loop");
					break;
				}
			}

			goalSlots[i] = (WireColor)wireColorIndex;
		}
	}

	void CreateRandomConfig()
	{
		// Reset wires to none
		for (int i = 0; i < goalSlots.Length; ++i)
			goalSlots[i] = WireColor.None;

		// Don't generate a config that matches what the player
		// already has done
		for (int i = 0; i < 100; ++i)
		{
			CreateRandomConfig_Internal();

			for (int j = 0; j < goalSlots.Length; ++j)
			{
				if (goalSlots[j] != m_reader.slots[j])
					return;
			}

			// Reset wires to none
			for (int j = 0; j < goalSlots.Length; ++j)
				goalSlots[j] = WireColor.None;
		}
	}

	void UpdatePreview()
	{
		for (int i = 0; i < goalSlots.Length; ++i)
		{
			debug_testImages[i].color = WireReader.GetWireColor(goalSlots[i]);
		}
	}

	void UpdateTimerText()
	{
		m_timerText.text = $"{(int)m_timer:00}:{(int)(m_timer % 1 * 100):00}";
	}

	float GetTimeForLevel(int level)
	{
		return (int)Mathf.Max(5, -0.01f * Mathf.Pow(level, 3) + 30.0f);
	}

	void ResetTimer()
	{
		m_timer = GetTimeForLevel(m_level);
	}

	void NextLevel()
	{
		++m_level;
		m_levelText.text = $"Level: {m_level}";

		CreateRandomConfig();
		ResetTimer();
		m_bLevelOver = false;

		UpdatePreview();
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class WaveMessage : MonoBehaviour
{
    public GameObject messagePanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button skipButton;

    private bool skipped = false;

    private string[] waveTitles = {
        "WAVE 1 — VIRUS DETECTED!",
        "WAVE 2 — DDoS ATTACK!",
        "WAVE 3 — WORM OUTBREAK!",
        "WAVE 4 — TROJAN HORSE!",
        "WAVE 5 — RANSOMWARE ALERT!",
        "INCOMING THREAT!"
    };

    private string[] waveDescriptions = {
        "Viruses attach to programs and replicate,\ncorrupting data and slowing systems.\n\n⚠  OBJECTIVE: Destroy all viruses before\nthey reach and corrupt your Firewall!",
        "Distributed Denial of Service floods your\nfirewall with fake requests to overwhelm it.\n\n⚠ OBJECTIVE: Eliminate the DDoS bots before\nthey crash your Firewall!",
        "Unlike viruses, worms self-replicate across\nnetworks without needing a host program.\n\n⚠ OBJECTIVE: Stop the worms from spreading\nthrough your Firewall!",
        "Malware disguised as legitimate software\nthat opens backdoors for attackers.\n\n⚠ OBJECTIVE: Destroy the Trojans before\nthey breach your Firewall!",
        "Malware that encrypts your data and\ndemands payment to restore access.\n\n⚠ OBJECTIVE: Eliminate all Ransomware before\nit encrypts your Firewall!",
        "Unknown threat detected!\n\n⚠ OBJECTIVE: Destroy all threats before\nthey reach your Firewall!"
    };

    void Start()
    {
        if (messagePanel != null)
            messagePanel.SetActive(false);
        if (skipButton != null)
            skipButton.onClick.AddListener(SkipMessage);
    }

    public void SkipMessage()
    {
        skipped = true;
    }

    public IEnumerator ShowWaveMessage(int waveNumber)
    {
        skipped = false;

        if (messagePanel != null)
            messagePanel.SetActive(true);

        int index = Mathf.Clamp(waveNumber - 1, 0, waveTitles.Length - 1);

        if (titleText != null)
            titleText.text = waveTitles[index];
        if (descriptionText != null)
            descriptionText.text = waveDescriptions[index];

        // Unlock cursor so player can click skip
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wait until skipped
        yield return new WaitUntil(() => skipped);

        // Re-lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (messagePanel != null)
            messagePanel.SetActive(false);
    }
}
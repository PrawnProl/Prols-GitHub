using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{
    public Image reputationBar; // Reference to the UI Image for the reputation bar
    public float maxReputation = 100f; // Maximum reputation value
    public float currentReputation; // Current reputation value

    void Start()
    {
        // Initialize the reputation bar
        currentReputation = maxReputation; // Start with full reputation
        reputationBar.fillAmount = 1f; // Set the fill amount to full (1 = 100%)
    }

    void Update()
    {
        // Example: Decrease reputation over time (for testing)
        currentReputation -= Time.deltaTime * 5f; // Adjust the speed as needed

        // Clamp the reputation value to stay within bounds
        currentReputation = Mathf.Clamp(currentReputation, 0f, maxReputation);

        // Update the reputation bar's fill amount
        reputationBar.fillAmount = currentReputation / maxReputation;
    }
}

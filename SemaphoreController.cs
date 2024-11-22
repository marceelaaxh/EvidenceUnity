using System.Collections;
using UnityEngine;

public class SemaphoreController : MonoBehaviour
{
    public Light redLight;  
    public Light greenLight;  

    public float greenDuration = 5f; 
    public float redDuration = 5f;   

    private bool isGreen;  // Current state of the semaphore (true = green, false = red)

    private void Start()
    {
        // Determine the initial state based on the object's name
        // If the name contains an odd number, it starts with the green light
        isGreen = IsOddName(gameObject.name);

        SetLightState(isGreen);

        StartCoroutine(SemaphoreCycle());
    }

    private IEnumerator SemaphoreCycle()
    {
        while (true)
        {
            isGreen = !isGreen;
            SetLightState(isGreen);

            yield return new WaitForSeconds(isGreen ? greenDuration : redDuration);
        }
    }

    private void SetLightState(bool green)
    {
        greenLight.enabled = green;
        redLight.enabled = !green;
    }

    private bool IsOddName(string name)
    {
        string digits = System.Text.RegularExpressions.Regex.Match(name, @"\d+").Value;
        if (int.TryParse(digits, out int number))
        {
            // Return true if the number is odd
            return number % 2 != 0;
        }

        // If the name doesn't contain a number, default to starting with the red light
        return false;
    }
}

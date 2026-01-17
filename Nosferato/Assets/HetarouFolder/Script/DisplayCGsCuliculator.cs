using UnityEngine;

public class DisplayCGsCalculater : MonoBehaviour
{
    private static int[] activeCGRowNumbers = {2, 38, 64, 114, 199, 231, 243, 419, 427, 493, 512, 557, 608, 625, 661};


    public static DisplayCGsCalculater Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public static int CalculateCGsToDisplay(int currentRowNumber)
    {
        int CGsCount = 0;

        for (int i = 0; i < activeCGRowNumbers.Length; i++)
        {
            if (activeCGRowNumbers[i] > currentRowNumber)
            {
                break;
            }
            CGsCount++;
        }

        return CGsCount;
    }
}

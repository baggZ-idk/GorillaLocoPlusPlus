using UnityEngine;

public class Surface : MonoBehaviour
{
    [Range(0, 1)]
    public float slipPercentage = 0f;
    [Range(0, 1)]
    public float bouncePercentage = 0f;
}

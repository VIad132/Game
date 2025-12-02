using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightPulse : MonoBehaviour
{
    public Light2D playerLight;
    public float speed = 2f;
    public float minRadius = 2f;
    public float maxRadius = 4f;

    void Update()
    {
        float radius = Mathf.Lerp(minRadius, maxRadius,
            (Mathf.Sin(Time.time * speed) + 1f) / 2f);

        playerLight.pointLightOuterRadius = radius;
    }
}
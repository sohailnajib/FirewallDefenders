using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        // Always face the camera - billboard effect
        transform.LookAt(transform.position + cam.forward);
    }

    public void SetHealth(int current, int max)
    {
        if (healthSlider != null)
            healthSlider.value = (float)current / max;
    }
}
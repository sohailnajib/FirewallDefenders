using UnityEngine;
using System.Collections;

public class ShootingEffect : MonoBehaviour
{
    public Light muzzleLight;
    public float flashDuration = 0.05f;

    void Start()
    {
        if (muzzleLight != null)
            muzzleLight.enabled = false;
    }

    public void PlayMuzzleFlash()
    {
        StartCoroutine(MuzzleFlash());
    }

    IEnumerator MuzzleFlash()
    {
        if (muzzleLight != null)
        {
            muzzleLight.enabled = true;
            yield return new WaitForSeconds(flashDuration);
            muzzleLight.enabled = false;
        }
    }
}

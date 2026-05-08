using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour
{
    public float range = 500f;
    public int damage = 30;
    public Camera playerCamera;
    public Transform gunBarrel;
    public float tracerDuration = 0.05f;

    private Animator animator;
    private ShootingEffect shootingEffect;
    private LineRenderer lineRenderer;
    private GameSceneAudio gameAudio;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        shootingEffect = GetComponent<ShootingEffect>();
        gameAudio = FindObjectOfType<GameSceneAudio>();

        // Set up the bullet tracer line
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.02f;
        lineRenderer.endWidth = 0.005f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = new Color(1f, 1f, 0f, 0f);
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            if (animator != null)
                animator.SetTrigger("Shoot");
        }
    }

    void Shoot()
    {
        // Cast a ray from the centre of the screen forward
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Vector3 startPoint = gunBarrel != null ? gunBarrel.position : ray.origin;
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            BugEnemy bug = hit.transform.GetComponentInParent<BugEnemy>();
            if (bug != null)
                bug.TakeDamage(damage);
        }
        else
        {
            endPoint = ray.GetPoint(range);
        }

        StartCoroutine(DrawTracer(startPoint, endPoint));

        if (shootingEffect != null)
            shootingEffect.PlayMuzzleFlash();

        if (gameAudio != null)
            gameAudio.PlayBulletSound();
    }

    IEnumerator DrawTracer(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(tracerDuration);
        lineRenderer.enabled = false;
    }

    // Draw a simple crosshair in the centre of the screen
    void OnGUI()
    {
        float cx = Screen.width / 2;
        float cy = Screen.height / 2;
        float size = 10f;

        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(cx - size / 2, cy - 1, size, 2), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(cx - 1, cy - size / 2, 2, size), Texture2D.whiteTexture);
    }
}

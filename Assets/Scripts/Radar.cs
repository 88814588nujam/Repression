using UnityEngine;
using System.Collections.Generic;

public class SimplifiedRadar : MonoBehaviour
{
    [Header("Radar Settings")]
    public float detectionRadius = 1000f;
    public float radarRefreshRate = 0.5f;
    public LayerMask enemyLayer;

    [Header("Display Settings")]
    public Transform radarDisplay;
    public float displayRadius = 5f;
    public Color enemyColor = Color.red;
    public Color selfColor = Color.green;
    public float blipSize = 0.2f;

    private List<Transform> trackedEnemies = new List<Transform>();
    private List<GameObject> blips = new List<GameObject>();
    private GameObject selfBlip;
    private float nextScanTime;

    void Start()
    {
        nextScanTime = Time.time;
        CreateSelfBlip();
    }

    void Update()
    {
        if (Time.time >= nextScanTime)
        {
            ScanForEnemies();
            nextScanTime = Time.time + radarRefreshRate;
        }
        UpdateBlipPositions();
        UpdateSelfBlipPosition();
    }

    void CreateSelfBlip()
    {
        selfBlip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        selfBlip.transform.SetParent(radarDisplay);
        selfBlip.transform.localScale = Vector3.one * blipSize;

        Renderer renderer = selfBlip.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Unlit/Color"));
            renderer.material.color = selfColor;
        }
    }

    void UpdateSelfBlipPosition()
    {
        if (selfBlip != null)
        {
            selfBlip.transform.localPosition = Vector3.zero;
            // selfBlip.transform.localRotation = Quaternion.Euler(0, 0, -transform.eulerAngles.y);
        }
    }

    void ScanForEnemies()
    {
        CleanDestroyedEnemies();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        while (blips.Count < hits.Length)
        {
            GameObject blip = CreateBlip();
            blips.Add(blip);
        }
        for (int i = hits.Length; i < blips.Count; i++)
        {
            blips[i].SetActive(false);
        }
        trackedEnemies.Clear();
        foreach (Collider hit in hits)
        {
            trackedEnemies.Add(hit.transform);
        }
    }

    void UpdateBlipPositions()
    {
        for (int i = 0; i < trackedEnemies.Count; i++)
        {
            if (i >= blips.Count) break;
            Transform enemy = trackedEnemies[i];
            GameObject blip = blips[i];
            if (enemy == null || blip == null) continue;
            blip.SetActive(true);
            Vector3 relativePos = transform.InverseTransformPoint(enemy.position);
            relativePos.y = 0;
            Vector2 radarPos = new Vector2(relativePos.x, relativePos.z).normalized *
                              Mathf.Clamp01(relativePos.magnitude / detectionRadius) * displayRadius;

            blip.transform.localPosition = new Vector3(radarPos.x, radarPos.y, 0);
        }
    }

    GameObject CreateBlip()
    {
        GameObject blip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        blip.transform.SetParent(radarDisplay);
        blip.transform.localScale = Vector3.one * blipSize;
        Renderer renderer = blip.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(Shader.Find("Unlit/Color"));
            renderer.material.color = enemyColor;
        }
        return blip;
    }

    void CleanDestroyedEnemies()
    {
        for (int i = trackedEnemies.Count - 1; i >= 0; i--)
        {
            if (trackedEnemies[i] == null)
            {
                trackedEnemies.RemoveAt(i);
            }
        }
    }

    void OnDestroy()
    {
        foreach (GameObject blip in blips)
        {
            if (blip != null)
            {
                Destroy(blip);
            }
        }
        blips.Clear();
        if (selfBlip != null)
        {
            Destroy(selfBlip);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
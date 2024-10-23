using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transitioner : MonoBehaviour
{
    private RectTransform canvas;
    private Vector2 canvasSize;

    public int randomness;
    public int placementPerUnits;
    public GameObject popcorn;
    public RectTransform basket;

    [Range(0,1)]
    public float pop;

    public List<GameObject> popcorns;

    // Start is called before the first frame update
    void Start()
    {
        popcorns = new List<GameObject>();

        canvas = transform.GetComponent<RectTransform>();

        canvasSize = canvas.rect.size;

        if (placementPerUnits < 10) return;

        for (int i = -(placementPerUnits/2); i < (canvasSize.x+(placementPerUnits / 2)); i += placementPerUnits)
        {
            for (int j = -(placementPerUnits / 2); j < (canvasSize.y + (placementPerUnits / 2)); j += placementPerUnits)
            {
                GameObject newPopcorn = Instantiate(popcorn, new Vector3(i-(canvasSize.x/2)+Random.Range(-randomness,randomness), j- (canvasSize.y / 2)+ Random.Range(-randomness, randomness)), Quaternion.Euler(0,0,Random.Range(0f,360f)));
                newPopcorn.transform.SetParent(gameObject.transform, false);
                popcorns.Add(newPopcorn);
            }
        }
    }

    private void Update()
    {
        float currentScreenPosition = Remap(pop, 0, 1, (canvasSize.x / 2), 0-(canvasSize.x / 2));
        foreach (var cernel in popcorns)
        {
            float pos = cernel.GetComponent<RectTransform>().anchoredPosition.x;

            if (pos > currentScreenPosition && cernel.activeSelf == false)
            {
                cernel.SetActive(true);
            }
            else if (pos <= currentScreenPosition && cernel.activeSelf == true)
            {
                cernel.SetActive(false);
            }
        }
        currentScreenPosition = Remap(pop, 0, 1, canvasSize.x+45, -45);
        basket.anchoredPosition = new Vector2(currentScreenPosition,0);
    }

    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}

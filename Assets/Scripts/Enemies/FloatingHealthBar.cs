using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    private bool isFlipped = false;

    public void UpdateHealthBar (float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = cam.transform.rotation;

            // Adjust orientation based on flip state
            if (isFlipped)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = -Mathf.Abs(newScale.x); // Ensure it's negative
                transform.localScale = newScale;
            }
            else
            {
                Vector3 newScale = transform.localScale;
                newScale.x = Mathf.Abs(newScale.x); // Ensure it's positive
                transform.localScale = newScale;
            }
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetFlipped(bool flipped)
    {
        isFlipped = flipped;
    }
}

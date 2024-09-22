using UnityEngine;
using UnityEngine.UI;

public class PowerupIconController : MonoBehaviour
{
    private Image iconImage;
    private RectTransform rectTransform;

    void Awake()
    {
        iconImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        if (iconImage == null)
        {
            Debug.LogError("Image component not found on Powerup Icon!");
        }
    }

    public void ShowIcon()
    {
        if (iconImage != null)
        {
            Debug.Log("PowerupIconController: Showing icon");
            gameObject.SetActive(true);
            iconImage.enabled = true;
            
            // Force icon to be visible within the canvas
            rectTransform.anchoredPosition = new Vector2(100, 100); // Adjust these values as needed
            
            // Ensure the color isn't transparent
            Color color = iconImage.color;
            color.a = 1f;
            iconImage.color = color;

            Debug.Log($"Icon position: {rectTransform.anchoredPosition}, Color: {iconImage.color}");
        }
    }

    public void HideIcon()
    {
        if (iconImage != null)
        {
            Debug.Log("PowerupIconController: Hiding icon");
            iconImage.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
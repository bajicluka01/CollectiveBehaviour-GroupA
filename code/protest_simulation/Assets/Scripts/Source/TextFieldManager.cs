using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements; // Import the UI namespace

public static class TextFieldManager
{
    public static Text myTextField; // Reference to the Text UI component
    public static int protestors = 0;
    public static int bystanders = 0;
    public static int police = 0;

    public static void Initialize()
    {
        // Check if a Canvas already exists in the scene
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            // Create a new Canvas if none exists
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            // Add a CanvasScaler and GraphicRaycaster for proper scaling and interaction
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        // Create a new GameObject for the Text
        GameObject textObject = new GameObject("TextField");
        textObject.transform.SetParent(canvas.transform); // Make it a child of the Canvas

        // Add a Text component to the GameObject
        myTextField = textObject.AddComponent<Text>();
        myTextField.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Use a built-in font
        myTextField.fontSize = 24;
        myTextField.alignment = TextAnchor.UpperRight;
        myTextField.color = Color.black; // Set the text color to black
        myTextField.text = "Protestors:  " + protestors + "  \nBystanders:  " + bystanders + "  \nPolice:  " + police + "  \n\n";
        myTextField.fontSize = 18; // Adjust font size to fit better
        myTextField.horizontalOverflow = HorizontalWrapMode.Wrap; // Allow wrapping
        myTextField.verticalOverflow = VerticalWrapMode.Overflow; // Let text overflow vertically if needed

        // Position the text field in the top-right corner
        RectTransform rectTransform = myTextField.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1); // Anchor to top-right
        rectTransform.anchorMax = new Vector2(1, 1); // Anchor to top-right
        rectTransform.pivot = new Vector2(1, 1);     // Pivot at top-right
        rectTransform.sizeDelta = new Vector2(400, 200);
        rectTransform.anchoredPosition = new Vector2(-20, -20); // Offset inward
    }

    public static void UpdateTextField()
    {
        // Method to update the text dynamically
        myTextField.text = "Protestors:  " + protestors + "  \nBystanders:  " + bystanders + "  \nPolice:  " + police + "  \n\n";
    }

    public static void setProtestors(int num)
    {
        protestors = num;
        UpdateTextField();
    }

    public static void setBystanders(int num)
    {
        bystanders = num;
        UpdateTextField();
    }

    public static void setPolice(int num)
    {
        police = num;
        UpdateTextField();
    }
}

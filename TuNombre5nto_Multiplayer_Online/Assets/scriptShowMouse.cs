using UnityEngine;

public class MouseCursorControl : MonoBehaviour
{
    // Call this method to show and unlock the cursor
    public void ShowAndUnlockCursor()
    {
        Cursor.visible = true; // Make the cursor visible
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    }

    // Call this method to hide and lock the cursor
    public void HideAndLockCursor()
    {
        Cursor.visible = false; // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    // Example of how to use these methods with key presses
    void Update()
    {
        // Press 'Escape' to show and unlock the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowAndUnlockCursor();
        }

        // Press 'Space' to hide and lock the cursor (or any other key)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HideAndLockCursor();
        }
    }
}

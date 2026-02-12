using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Continue : MonoBehaviour
{

    public TMP_Text continueText;
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            continueText.text = "Loading...";
            SceneManager.LoadScene("Level 1 - Forest");
        }
    }
}

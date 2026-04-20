using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadSceneByEnter : MonoBehaviour
{
    public TMP_Text continueText;
    public string sceneToLoad = "Main Menu";

    [Header("Delay")]
    public float delaySeconds = 1f;
    public bool useDelay = false;

    private bool isLoading = false;

    void Update()
    {
        if (isLoading)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isLoading = true;

            if (continueText != null)
                continueText.text = "Loading...";

            if (useDelay)
            {
                StartCoroutine(WaitThenLoad(delaySeconds));
            }
            else
            {
                LoadScene();
            }
        }
    }

    IEnumerator WaitThenLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LoadScene();
    }

    private void LoadScene()
    {
        if (sceneToLoad == "Main Menu")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
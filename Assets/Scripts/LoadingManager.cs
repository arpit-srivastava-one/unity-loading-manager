using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    private static LoadingManager instance;
    public static LoadingManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("References")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Image loadingBar;
    [SerializeField] private TextMeshProUGUI loadingTitle;
    [SerializeField] private TextMeshProUGUI loadingPercent;
    [SerializeField] private Button continueButton;

    [Header("Properties")]
    [Range(0.1f, 100f)][SerializeField] private float smoothness = 1f;
    private float target = 0f;
    [SerializeField] private bool autoLoad = true;



    private void Start()
    {
        loadingCanvas.gameObject.SetActive(false);
        loadingTitle.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        enabled = false;    // To disable Update()
    }


    // Call this function from another class, to load to a different scene using this Loading Menu
    public void LoadScene(string sceneName)
    {
        enabled = true; // To enable Update()

        loadingTitle.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);

        StartCoroutine(LoadingScene(sceneName));
    }


    private IEnumerator LoadingScene(string sceneName)
    {
        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);

        // Resetting values, to start the loading progress
        loadingBar.fillAmount = 0f;
        loadingPercent.text = "0%";
        loadingCanvas.SetActive(true);

        // Set target to scene load progress, until scene is fully loaded
        while (scene.isDone == false)
        {
            target = scene.progress;
            yield return null;
        }

        // After scene's loading's done, setting target to 1. Because scene.progress never reaches 1 fully.
        target = 1f;

        // Calculating wait time, in order to show full loading animation.
        // Dependent upon the value of Smoothness we set.
        // As such, scene will be loaded before the progress bar reaches 100. Something to take note of.
        float waitTime = ((target - loadingBar.fillAmount) / smoothness) + 0.1f;
        yield return new WaitForSeconds(waitTime);

        // If true, loading canvas is disabled.
        if (autoLoad)
        {
            DisableLoadingScreen();
        }

        // Otherwise, continue button is shown. On pressing it, loading canvas is disabled.
        else
        {
            loadingTitle.gameObject.SetActive(false);
            continueButton.gameObject.SetActive(true);
        }

        // For disabling Update().
        enabled = false;
    }


    private void Update()
    {
        // Smoothly moves loading progress bar towards the target.
        loadingBar.fillAmount = Mathf.MoveTowards(loadingBar.fillAmount, target, smoothness * Time.deltaTime);
        loadingPercent.text = ((int)(loadingBar.fillAmount * 100f)).ToString() + "%";
    }


    private void DisableLoadingScreen()
    {
        continueButton.gameObject.SetActive(false);
        loadingCanvas.SetActive(false);
    }


    public void ContinueBTN()
    {
        DisableLoadingScreen();
    }
}
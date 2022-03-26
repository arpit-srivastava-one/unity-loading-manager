using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public void ChangeSceneTo(string sceneName)
    {
        LoadingManager.Instance.LoadScene(sceneName);
    }
}

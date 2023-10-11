using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUi : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#endif
        Application.Quit();
    }

    private void StartGame()
    {
        SceneManager.LoadSceneAsync("HomeScene");
    }
}
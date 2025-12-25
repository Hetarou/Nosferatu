using UnityEngine;
using UnityEngine.SceneManagement;

public class PreviousSceneRecorder : MonoBehaviour
{
    public static PreviousSceneRecorder Instance;

    public string PreviousSceneName { get; private set; }
    private string currentSceneName;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentSceneName = SceneManager.GetActiveScene().name;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PreviousSceneName = currentSceneName;
        currentSceneName = scene.name;
        PublicStaticStatus.PreviousScene = PreviousSceneName;
    }
}

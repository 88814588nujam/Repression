using UnityEngine;
using UnityEngine.SceneManagement;

public class Common : MonoBehaviour
{
    public string nowSceneName;
    public string nextSceneName;

    private EnemySpawner playerExplosion;

    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null) playerExplosion = gameController.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if (nowSceneName.Equals("TitleScene")) {
            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
            else
                if (Input.anyKeyDown)
                    SceneManager.LoadScene(nextSceneName);
        } else if (nowSceneName.Equals("GameScene"))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Quit();
            else
            {
                if (playerExplosion != null)
                {
                    if (playerExplosion.isShowOver)
                    {
                        if (Input.anyKeyDown)
                            SceneManager.LoadScene(nextSceneName);
                    }
                }
            }
        }
    }

    private void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
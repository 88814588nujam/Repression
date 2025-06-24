using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class EnemyCursorCheck : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isGameOver = false;
    private EnemySpawner enemySpawner;

    void Start()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");
        if (gameController != null) enemySpawner = gameController.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        if(enemySpawner != null) isGameOver = enemySpawner.isGameOver;
    }

    void OnMouseEnter()
    {
        if(!isGameOver) CursorManager.SetAttackCursor();
    }

    void OnMouseExit()
    {
        CursorManager.SetNormalCursor();
    }

    // 如果使用UGUI事件系统
    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.SetAttackCursor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.SetNormalCursor();
    }
}
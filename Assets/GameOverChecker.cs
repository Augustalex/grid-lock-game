using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class GameOverChecker : MonoBehaviour
{
    public EndGameManager endGameManager;
    public GridSettings gridSettings;
    public UIController uiController;

    private GridRepository _gridRepository;
    private Coroutine _routine;

    private const float Delta = .5f;

    void Awake()
    {
        _gridRepository = GetComponent<GridRepository>();
    }

    private void OnEnable()
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(Cycle());
    }

    private void OnDisable()
    {
        StopCoroutine(_routine);
        _routine = null;
    }

    IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(Delta);
            foreach (var gridItem in _gridRepository.gridItems)
            {
                if (gridItem.GridLocked())
                {
                    uiController.GridLock();
                    endGameManager.gameOver = true;
                    endGameManager.TriggerFall();
                    
                    yield break;
                }
            }
        }
    }
}
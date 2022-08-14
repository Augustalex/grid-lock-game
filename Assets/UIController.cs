using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject gridLockStatus;

    void Start()
    {
        gridLockStatus.SetActive(false);
    }

    public void GridLock()
    {
        gridLockStatus.SetActive(true);
    }

    public void Reset()
    {
        gridLockStatus.SetActive(false);
    }
}
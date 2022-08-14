using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public bool gameOver = false;
    public GridGenerator gridGenerator;

    public void TriggerFall()
    {
        StartCoroutine(BlowUpAndDestroy());
    }

    private IEnumerator BlowUpAndDestroy()
    {
        var toRemove = new List<GameObject>();
        foreach (var block in FindObjectsOfType<BasicBlockController>())
        {
            BlowUp(block.gameObject);
            toRemove.Add(block.gameObject);
        }

        foreach (var block in FindObjectsOfType<BlockBlockController>())
        {
            BlowUp(block.gameObject);
            toRemove.Add(block.gameObject);
        }

        foreach (var block in FindObjectsOfType<FactoryBlockController>())
        {
            BlowUp(block.gameObject);
            toRemove.Add(block.gameObject);
        }

        foreach (var block in FindObjectsOfType<GridItem>())
        {
            block.Shrink();
            toRemove.Add(block.gameObject);
        }

        yield return new WaitForSeconds(4);
        foreach (var o in toRemove)
        {
            Destroy(o);
        }

        gridGenerator.ZeroGrid();
    }

    private static void BlowUp(GameObject block)
    {
        var rb = block.AddComponent<Rigidbody2D>();

        var move = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(0, 1f)
        );
        rb.AddForce(move.normalized * Random.Range(6f, 10f), ForceMode2D.Impulse);
    }
}
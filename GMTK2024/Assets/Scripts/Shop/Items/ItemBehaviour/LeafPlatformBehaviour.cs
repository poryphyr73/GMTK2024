using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPlatformBehaviour : MonoBehaviour
{
    [SerializeField] float terminalVelocity, timeToAccelerate, errorAcceptance;
    float yDelta;

    private void Start()
    {
        StartCoroutine(Accelerate());
    }

    private void Update()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y - terminalVelocity * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target, yDelta * Time.deltaTime);
    }

    IEnumerator Accelerate()
    {
        float t = 0;

        while(yDelta < terminalVelocity)
        {
            yDelta = Mathf.Lerp(0, terminalVelocity, t / timeToAccelerate);
            t += Time.deltaTime;
            if (terminalVelocity - yDelta < errorAcceptance) break;
            yield return null;
        }

        yDelta = terminalVelocity;
    }
}

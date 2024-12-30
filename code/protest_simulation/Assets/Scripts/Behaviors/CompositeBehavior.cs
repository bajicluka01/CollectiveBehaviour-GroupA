using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{

    public FlockBehavior[] behaviors;
    public float[] weights;

    // having higher smoothing means that the turns will be sharper
    [Range(0f, 1f)]
    public float smoothing = 0.01f;

    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {

        if (weights.Length != behaviors.Length)
        {
            Debug.LogError("Data mismatch in " + name, this);
            return Vector2.zero;
        }

        Vector2 move = Vector2.zero;

        //for testing purposes
        //string current = "";

        for (int i = 0; i < behaviors.Length; i++)
        {
            Vector2 partialMove = behaviors[i].CalculateMove(agent, context, flock) * weights[i];

            //move+=partialMove;
            //current+= partialMove+" ";

            if (partialMove != Vector2.zero)
            {
                //Debug.Log(partialMove.sqrMagnitude);
                if (partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
                //current+= partialMove+" ";
            }
        }
        //Debug.Log(current+" "+move);

        //return move;
        return move.normalized;
    }

    //doesn't work appropriately
    Vector2 SmoothenMove(FlockAgent agent, Vector2 desiredMove)
    {
        if (smoothing == 0f)
        {
            return desiredMove;
        }
        Vector2 differenceVector = desiredMove - agent.PreviousMove;
        differenceVector.Normalize();
        differenceVector *= smoothing;
        if (Vector2.Distance(agent.PreviousMove + differenceVector, desiredMove) < smoothing)
        {
            differenceVector = Vector2.zero;
        }
        return differenceVector + agent.PreviousMove;
    }
}

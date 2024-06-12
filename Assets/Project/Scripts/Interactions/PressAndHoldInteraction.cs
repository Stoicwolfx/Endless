using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullAndHoldInteraction : IInputInteraction
{
    InputControl control;
    public void Process(ref InputInteractionContext context)
    {
        if (this.control != null)
        {

        }
        else
        {

        }
    }

    public void Reset()
    {
        control = null;
    }

    static PullAndHoldInteraction()
    {
        InputSystem.RegisterInteraction<PullAndHoldInteraction>();
    }

#if UNITY_EDITOR
    [InitializeOnLoadMethod()]
#endif
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {

    }
}

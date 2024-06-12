using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullAndHoldInteraction : IInputInteraction
{
    InputControl control;
    bool pulled;

    public void Process(ref InputInteractionContext context)
    {
        this.pulled = context.ControlIsActuated();

        if (this.control != null)
        {
            if (this.control != context.control)
                return;

            if (pulled)
            {
                context.PerformedAndStayStarted();
            }
            else
            {
                context.Canceled();
            }
        }
        else
        {
            if (pulled)
            {
                control = context.control;
                context.Started();
            }
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

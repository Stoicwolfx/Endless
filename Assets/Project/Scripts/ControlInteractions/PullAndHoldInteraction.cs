using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PullAndHoldInteraction : IInputInteraction
{
    InputControl control;
    bool pulled;

    public void Process(ref InputInteractionContext context)
    {
        this.pulled = (context.ReadValue<float>() > 0.0f);

        if (this.control != null)
        {
            if (pulled)
            {
                context.PerformedAndStayStarted();
                Globals.playerFiring = true;
            }
            else
            {
                context.Canceled();
                control = null;
                Globals.playerFiring = false;
            }
        }
        else
        {
            if (pulled)
            {
                control = context.control;
                context.Started();
                Globals.playerFiring = true;
            }
        }
    }

    public void Reset()
    {
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

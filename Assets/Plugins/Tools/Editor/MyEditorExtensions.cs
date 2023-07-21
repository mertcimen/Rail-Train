using UnityEditor;
using UnityEngine;

public abstract class MyEditorExtensions
{
    [MenuItem("CONTEXT/Animator/Create New Animator Controller", priority = 0)]
    private static void CreateNewAnimator(MenuCommand menuCommand)
    {
        // The Animator component can be extracted from the menu command using the context field.
        var animator = menuCommand.context as Animator;

        if (animator == null)
            return;
        
        var path = EditorUtility.SaveFilePanelInProject("Create new Animator Controller", animator.gameObject.name 
            + ".controller", "controller", "Please enter a file name to save the Animator Controller to");

        if (path.Length != 0)
        {
            animator.runtimeAnimatorController = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(path);
        }
    }

    [MenuItem("CONTEXT/Transform/Move to Closest NavMesh Point", priority = 0)]
    private static void MoveToNavMesh(MenuCommand menuCommand)
    {
        // The Transform component can be extracted from the menu command using the context field.
        var transform = menuCommand.context as Transform;

        transform.SampleTransformPosition();
    }

}
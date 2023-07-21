using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

[ExecuteAlways]
public class DrawGizmo : MonoBehaviour
{
    [SerializeField, ColorPalette("Breeze")]
    private Color color = Color.magenta;

    [SerializeField, EnumToggleButtons] private GizmoType type;

    [SerializeField] private bool useScale = false;

    [SerializeField, Range(0, 10f), HideIf(nameof(useScale))]
    private float radius = 1f;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        switch (type)
        {
            case GizmoType.Sphere:
                DrawPrimitiveMeshGizmo(PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Sphere));
                break;
            case GizmoType.Cube:
                Gizmos.DrawCube(transform.position, useScale ? transform.localScale : Vector3.one * radius);
                break;
            case GizmoType.Capsule:
                DrawPrimitiveMeshGizmo(PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Capsule));
                break;
            case GizmoType.Cylinder:
                DrawPrimitiveMeshGizmo(PrimitiveHelper.GetPrimitiveMesh(PrimitiveType.Cylinder));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
#else
    private void OnEnable()
    {
        enabled = false;
    }
#endif

    private void DrawPrimitiveMeshGizmo(Mesh mesh)
    {
        if (mesh)
        {
            var tempTransform = transform;
            Gizmos.DrawMesh(mesh, tempTransform.position, tempTransform.rotation,
                useScale ? tempTransform.localScale : Vector3.one * radius);
        }
        else
            Gizmos.DrawCube(transform.position, useScale ? transform.localScale : Vector3.one * radius);
    }

    private enum GizmoType
    {
        Sphere = 0,
        Cube = 1,
        Capsule = 2,
        Cylinder = 3
    }
}

public static class PrimitiveHelper
{
    private static readonly Dictionary<PrimitiveType, Mesh> PrimitiveMeshes = new();

    public static GameObject CreatePrimitive(PrimitiveType type, bool withCollider)
    {
        if (withCollider)
        {
            return GameObject.CreatePrimitive(type);
        }

        var gameObject = new GameObject(type.ToString());
        var meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = GetPrimitiveMesh(type);
        gameObject.AddComponent<MeshRenderer>();

        return gameObject;
    }

    public static Mesh GetPrimitiveMesh(PrimitiveType type)
    {
        if (!PrimitiveMeshes.ContainsKey(type))
        {
            CreatePrimitiveMesh(type);
        }

        return PrimitiveMeshes[type];
    }

    private static Mesh CreatePrimitiveMesh(PrimitiveType type)
    {
        var gameObject = GameObject.CreatePrimitive(type);
        var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        Object.DestroyImmediate(gameObject);

        PrimitiveMeshes[type] = mesh;
        return mesh;
    }
}
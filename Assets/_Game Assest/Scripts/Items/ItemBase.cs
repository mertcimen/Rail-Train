using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public Material outlineMaterial;
    public ParticleSystem markParticle;
   
    public void SetMaterialForCollect()
    {
         markParticle.Play();
       
    }
}
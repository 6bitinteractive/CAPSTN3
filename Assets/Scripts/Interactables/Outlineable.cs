using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlineable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<MeshRenderer> meshRendersToOutline;
    [SerializeField] private List<SkinnedMeshRenderer> skinnedMeshRendersToOutline;
    [SerializeField] private Shader outlineShader;

    private Shader defaultShader;

    private void Start()
    {
        if (meshRendersToOutline.Count > 0)
        {
            for (int i = 0; i < meshRendersToOutline.Count; i++)
            {
                defaultShader = meshRendersToOutline[i].materials[0].shader;
            }
        }

        else
        {
            for (int i = 0; i < skinnedMeshRendersToOutline.Count; i++)
            {
                defaultShader = skinnedMeshRendersToOutline[i].materials[0].shader;
            }
        }
       
    }

    public void DisplayInteractability()
    {
        if (meshRendersToOutline.Count > 0)
        {
            for (int i = 0; i < meshRendersToOutline.Count; i++)
            {
                meshRendersToOutline[i].materials[1].shader = outlineShader;
            }
        }

        else
        {
            for (int i = 0; i < skinnedMeshRendersToOutline.Count; i++)
            {
                skinnedMeshRendersToOutline[i].materials[1].shader = outlineShader;
            }
        }
    
    }

    public void HideInteractability()
    {
        if (meshRendersToOutline.Count > 0)
        {
            for (int i = 0; i < meshRendersToOutline.Count; i++)
            {
                meshRendersToOutline[i].materials[1].shader = defaultShader;
            }
        }

        else
        {
            for (int i = 0; i < skinnedMeshRendersToOutline.Count; i++)
            {
                skinnedMeshRendersToOutline[i].materials[1].shader = defaultShader;
            }
        }
    }

    public void Interact(Interactor source, IInteractable target)
    {
       
    }
}

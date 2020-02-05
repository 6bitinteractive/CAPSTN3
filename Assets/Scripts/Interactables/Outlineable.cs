using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outlineable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<MeshRenderer> meshRendersToOutline;
    [SerializeField] private Shader outLineShader;

    private Shader defaultShader;

    private void Start()
    {
        for (int i = 0; i < meshRendersToOutline.Count; i++)
        {
          defaultShader = meshRendersToOutline[i].materials[0].shader;
        }
    }

    public void DisplayInteractability()
    {
        for (int i = 0; i < meshRendersToOutline.Count; i++)
        {
            meshRendersToOutline[i].materials[1].shader = outLineShader;
        }
    }

    public void HideInteractability()
    {
        for (int i = 0; i < meshRendersToOutline.Count; i++)
        {
            meshRendersToOutline[i].materials[1].shader = defaultShader;
        }
    }

    public void Interact(Interactor source, IInteractable target)
    {
       
    }
}

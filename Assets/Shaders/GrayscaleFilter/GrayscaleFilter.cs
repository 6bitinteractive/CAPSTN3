using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrayscaleFilter : MonoBehaviour
{
    public Material m_Material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_Material != null)
            Graphics.Blit(source, destination, m_Material);
    }
}
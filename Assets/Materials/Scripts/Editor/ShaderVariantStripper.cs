using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderVariantStripper : IPreprocessShaders
{
    public int callbackOrder => 0;

    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data)
    {
        ShaderKeyword dotsInstancing = new ShaderKeyword("DOTS_INSTANCING_ON");

        for (int i = data.Count - 1; i >= 0; i--)
        {
            if (data[i].shaderKeywordSet.IsEnabled(dotsInstancing))
            {
                data.RemoveAt(i);
            }
        }
    }
}
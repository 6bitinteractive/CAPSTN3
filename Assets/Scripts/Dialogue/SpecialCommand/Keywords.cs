using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To use in dialogue lines:
// e.g. "Find the {kw:item}broom{kw:item:end} for {kw:character}Lolo{kw:character:end}."
// - Opening Tag: {kw:[value]}, where [value] is a keywordName defined in the Keywords object (e.g. location, item, character, hint)
// - Closing Tag: {kw:[value]:end}---Important! Each opening tag must be closed. [value] must match with opening tag's.

public class Keywords : Singleton<Keywords>
{
    [SerializeField] private List<Keyword> keywords;

    public Keyword GetKeyword(string keywordName) => keywords.Find(x => x.KeywordName == keywordName);
}

[System.Serializable]
public class Keyword
{
    [Tooltip("The value passed when using command {kw:keywordName}.")]
    [SerializeField] private string keywordName;
    public const string endKeyword = "end";

    public string KeywordName => keywordName.ToLowerInvariant();

    [Tooltip("The color of the text.")]
    public Color color;

    /// <summary>
    /// <para>Expected value: one of the following: pixels, font units, or as percentage; e.g. 50px, 5em, 50%.
    ///  Pixel adjustments can be either absolute or relative, like +1 and -1.
    ///  All relative sizes are based on the original font size, so they're not cumulative.</para>
    /// <para>See <a href="http://digitalnativestudios.com/textmeshpro/docs/rich-text/#size">TextMesh Pro Documentation - Rich Text: Size</a>.</para>
    /// </summary>
    [Tooltip("Expected value: one of the following: pixels, font units, or as percentage; e.g. 50px, 5em, 50%" +
        "\n\nPixel adjustments can be either absolute or relative, like +1 and -1." +
        "\n\nAll relative sizes are based on the original font size, so they're not cumulative.")]
    public string size = "100%";

    public bool bold;
    public bool italic;

    public string GetOpeningTags()
    {
        return string.Format("<color=#{0}><size={1}>{2}{3}", ColorUtility.ToHtmlStringRGBA(color), size, bold ? "<b>" : "", italic ? "<i>" : "");
    }

    public string GetClosingTags()
    {
        return string.Format("</color><size=100%>{0}{1}", bold ? "</b>" : "", italic ? "</i>" : "");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// See Keywords.cs to learn how to use the {keyword:x} command

public class KeywordSpecialCommand : SpecialCommand
{
    public Keyword keyword;

    private static Keywords keywords;

    /// <summary>
    /// KeywordSpecialCommand constructor
    /// </summary>
    /// <param name="keywordName">One of the keywords defined in Keywords.cs; "end" is a special keywordName.</param>
    public KeywordSpecialCommand(string keywordName)
    {
        keywords = keywords ?? SingletonManager.GetInstance<Keywords>();
        keyword = keywords.GetKeyword(keywordName);

        if (keyword == null)
            Debug.LogErrorFormat("Keyword {0} does not exits.", keywordName);
    }

    /// <summary>
    /// Use this to insert the opening tags
    /// </summary>
    /// <returns>Opening tags</returns>
    public string GetOpeningTags()
    {
        return keyword.GetOpeningTags();
    }

    /// <summary>
    /// Use this to insert the closing tags
    /// </summary>
    /// <returns>Closing tags</returns>
    public string GetClosingTags()
    {
        return keyword.GetClosingTags();
    }

    // Yes... cringe architecture... @_@
    public override void Execute()
    {
        base.Execute();
        Debug.LogWarning("Please use WriteOpening/ClosingTags()");
    }
}

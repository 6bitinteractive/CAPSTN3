using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpecialCommandHandler
{
    public List<SpecialCommand> SpecialCommands { get; private set; } = new List<SpecialCommand>();

    private Dictionary<string, Func<string, SpecialCommand>> specialCommandsFactory = new Dictionary<string, Func<string, SpecialCommand>>();
    private Func<string, SpecialCommand> createSpecialCommand;

    public SpecialCommandHandler()
    {
        Init();
    }

    public void Init()
    {
        //specialCommandsFactory["keyword"] = x => new KeywordSpecialCommand(x);
        specialCommandsFactory["sfx"] = x => new SfxSpecialCommand();
        specialCommandsFactory["emoticon"] = x => new EmoticonSpecialCommand();
    }

    /// <summary>
    /// Parses {keyword:x} commands
    /// </summary>
    /// <param name="text">The text it will need to format</param>
    /// <returns>Formatted text</returns>
    public string FormatText(string text)
    {
        string formattedText = text;

        // Find {keyword:x}, capture x
        Match match = Regex.Match(formattedText, @"{keyword:([A-Za-z0-9\-]+)}", RegexOptions.IgnoreCase);

        if (!match.Success)
        {
            //Debug.Log("No keyword commands to parse. Returning text as is.");
            return text;
        }

        KeywordSpecialCommand recentCommand = null;
        string tags = string.Empty;

        while (match.Success)
        {
            // Get the value
            string value = match.Groups[1].Value; // Note: Groups are indexed starting at 1, not 0

            if (value != Keyword.endKeyword) // If it's not {keyword:end}
            {
                // Create a new KeywordCommand
                recentCommand = new KeywordSpecialCommand(value);

                tags = recentCommand.GetOpeningTags();
            }
            else
            {
                // Note: It's assumed that the {keyword:end} has a {keyword:x} pair that comes before it, i.e. the recentCommand
                if (recentCommand == null)
                    Debug.LogError("A KeywordSpecialCommand has not been created! No pair for this end tag was found.");

                tags = recentCommand.GetClosingTags();
            }

            // Replace the command with actual tags
            formattedText = Regex.Replace(formattedText, match.Value, tags);

            match = match.NextMatch();
        }

        // Clean out the {keyword:x} commands
        formattedText = Regex.Replace(formattedText, @"{keyword:[A-Za-z0-9\-]+}", "");

        //Debug.LogFormat("FORMATTED TEXT: {0}", formattedText);
        return formattedText;
    }

    public List<SpecialCommand> BuildCommandList(string text)
    {
        List<SpecialCommand> listCommand = new List<SpecialCommand>();

        string command = "";               // Current command name
        char[] brackets = { '{', '}' };    // Trim these characters when the command is found

        // Go through the dialogue line, get all our special commands
        for (int i = 0; i < text.Length; i++)
        {
            string currentChar = text[i].ToString();

            // If true, we are getting a command
            if (currentChar == "{")
            {
                // Go ahead and get the command
                while (currentChar != "}" && i < text.Length)
                {
                    currentChar = text[i].ToString();
                    command += currentChar;
                    text = text.Remove(i, 1);  // Remove current character; We want to get the next character in the command
                }

                // Done getting the command
                if (currentChar == "}")
                {
                    // Trim "{" and "}"
                    command = command.Trim(brackets);

                    // Get Command Name and Value
                    SpecialCommand newCommand = ParseCommand(command);

                    // Command index position in the string
                    newCommand.Index = i;

                    // Add to list
                    listCommand.Add(newCommand);

                    // Reset
                    command = "";

                    // Take a step back otherwise a character will be skipped
                    // i = 0 also works, but it means going through characters we already checked
                    i--;
                }
                else
                {
                    Debug.LogError("Command in dialogue line not closed.");
                }
            }
        }

        SpecialCommands = listCommand;

        return listCommand;
    }

    public SpecialCommand ParseCommand(string command)
    {
        // Regex to get the command name and the command value
        string delimiter = "[:]";

        // Split the command and its values
        string[] matches = Regex.Split(command, delimiter);

        string commandName = string.Empty;
        string commandValue = string.Empty;

        // Get the command and its value
        if (matches.Length > 0)
        {
            // 0 = command name. 1 = value
            commandName = matches[0];
            commandValue = matches[1];
        }
        else
        {
            Debug.LogErrorFormat("Parsed command \"{0}\". No command identified.", command);
            return null;
        }

        // Create a new command, depending on its name
        if (specialCommandsFactory.TryGetValue(commandName, out createSpecialCommand))
        {
            return createSpecialCommand(commandValue);
        }
        else
        {
            Debug.LogErrorFormat("Command \"{0}\" does not have a creator defined.", commandName);
            return null;
        }
    }

    public void ExecuteCommand(int index)
    {
        List<SpecialCommand> commandsToExecute = new List<SpecialCommand>();
        commandsToExecute = SpecialCommands.FindAll(x => x.Index == index);

        if (commandsToExecute.Count == 0)
            return;

        foreach (var command in commandsToExecute)
            command.Execute();
    }

    public void ResetSpecialCommandList()
    {
        if (SpecialCommands.Count > 0)
        {
            for (int i = 0; i < SpecialCommands.Count; i++)
                SpecialCommands[i] = null;
        }

        SpecialCommands.Clear();
    }
}

// References
// https://bitbucket.org/flaredust/excerpts-of-video-game-code-for-unity/src/master/project-libra/RPG%20Dialogue%20System/DialogueBox.cs
// https://stackoverflow.com/questions/41163631/c-sharp-better-way-to-create-objects-of-a-subclass-in-runtime-other-then-a-switc

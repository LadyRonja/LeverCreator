using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : Singleton<CommandManager>
{
    private Stack<Command> history = new();

    public void AddToHistory(Command c)
    {
        history.Push(c);
    }

    public void Undo()
    {
        Command c = history.Peek();
        if(c != null) {
            history.Peek().Undo();
            history.Pop();
        }
    }
}

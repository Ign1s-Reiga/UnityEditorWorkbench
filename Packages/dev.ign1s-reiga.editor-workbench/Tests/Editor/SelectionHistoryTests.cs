using NUnit.Framework;
using UnityEngine;

namespace Ign1s.EditorWorkbench.Tests;

internal sealed class SelectionHistoryTests
{
    [Test]
    public void BackAndForward_NavigateTargets()
    {
        GameObject first = new("First");
        GameObject second = new("Second");
        try
        {
            SelectionHistory history = new();
            history.Push(first);
            history.Push(second);

            Assert.That(history.MoveBack(), Is.SameAs(first));
            Assert.That(history.MoveForward(), Is.SameAs(second));
        }
        finally
        {
            Object.DestroyImmediate(first);
            Object.DestroyImmediate(second);
        }
    }

    [Test]
    public void PushAfterBack_DiscardsForwardBranch()
    {
        GameObject first = new("First");
        GameObject second = new("Second");
        GameObject third = new("Third");
        try
        {
            SelectionHistory history = new();
            history.Push(first);
            history.Push(second);
            history.MoveBack();
            history.Push(third);

            Assert.That(history.CanMoveForward, Is.False);
            Assert.That(history.Current(), Is.SameAs(third));
        }
        finally
        {
            Object.DestroyImmediate(first);
            Object.DestroyImmediate(second);
            Object.DestroyImmediate(third);
        }
    }
}

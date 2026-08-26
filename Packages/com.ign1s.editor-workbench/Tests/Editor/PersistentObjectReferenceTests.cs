using NUnit.Framework;
using UnityEngine;

namespace Ign1s.EditorWorkbench.Tests;

internal sealed class PersistentObjectReferenceTests
{
    [Test]
    public void CaptureAndResolve_ReturnsDirectObject()
    {
        GameObject target = new("Persistent Reference Test");
        try
        {
            PersistentObjectReference reference = new();
            reference.Capture(target);

            Object resolved = reference.Resolve();

            Assert.That(resolved, Is.SameAs(target));
            Assert.That(reference.FallbackName, Is.EqualTo(target.name));
        }
        finally
        {
            Object.DestroyImmediate(target);
        }
    }
}

using NUnit.Framework;

namespace Ign1s.EditorWorkbench.Tests;

internal sealed class FavoriteFolderStoreTests
{
    [Test]
    public void Toggle_AddsThenRemovesGuid()
    {
        FavoriteFolderStore store = new();

        bool added = store.Toggle("folder-guid");
        bool removed = store.Toggle("folder-guid");

        Assert.That(added, Is.True);
        Assert.That(removed, Is.False);
        Assert.That(store.Contains("folder-guid"), Is.False);
    }
}

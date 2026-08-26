using NUnit.Framework;
using UnityEngine;

namespace Ign1s.EditorWorkbench.Tests;

internal sealed class FolderAppearanceStoreTests
{
    [Test]
    public void GetOrCreate_ReturnsSameEntryForGuid()
    {
        FolderAppearanceStore store = new();

        FolderAppearance first = store.GetOrCreate("abc123");
        FolderAppearance second = store.GetOrCreate("abc123");

        Assert.That(second, Is.SameAs(first));
        Assert.That(store.Entries.Count, Is.EqualTo(1));
    }

    [Test]
    public void Entry_RetainsAppearanceValues()
    {
        FolderAppearanceStore store = new();
        FolderAppearance entry = store.GetOrCreate("folder-guid");
        Color expectedColor = new(0.1f, 0.2f, 0.3f, 0.4f);

        entry.HasCustomTint = true;
        entry.Tint = expectedColor;
        entry.IconName = "Folder Icon";

        Assert.That(entry.HasCustomTint, Is.True);
        Assert.That(entry.Tint, Is.EqualTo(expectedColor));
        Assert.That(entry.IconName, Is.EqualTo("Folder Icon"));
    }

    [Test]
    public void Remove_DeletesMatchingEntry()
    {
        FolderAppearanceStore store = new();
        store.GetOrCreate("folder-guid");

        bool removed = store.Remove("folder-guid");

        Assert.That(removed, Is.True);
        Assert.That(store.TryGet("folder-guid", out _), Is.False);
    }
}

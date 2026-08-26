# Testing

## Static validation

`python scripts/validate_project.py` checks:

- JSON syntax
- Required project and package files
- Claude skill and agent frontmatter
- Duplicate agent and skill names
- Prohibited internal API usage outside the compatibility directory
- Basic C# brace balance
- Accidental generated Unity directories

This is not a C# compiler and does not replace Unity tests.

## Unity EditMode tests

Set `UNITY_EDITOR_PATH` to the Unity executable and run the platform script. Results are written to `TestResults`.

The initial tests cover:

- Folder appearance store creation, update, and removal
- Favorite GUID toggle behavior
- Persistent object reference capture and resolution
- Inspector history back and forward behavior

## Manual verification

1. Open the sandbox project.
2. Open `Window > Editor Workbench > Inspector`.
3. Select assets, scene objects, and components.
4. Add tabs, lock tabs, and use history.
5. Create folders under `Assets`, set appearance, rename and move them.
6. Add favorites, restart Unity, and confirm personal persistence.
7. Toggle hierarchy objects from the row indicator and verify Undo.
8. Disable decorations from Project Settings and confirm repaint behavior.

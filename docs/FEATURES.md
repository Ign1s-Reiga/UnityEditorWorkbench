# Feature specification

## Folder appearance

- Set a folder tint from predefined colors or a custom color field.
- Set an optional built-in Unity icon name.
- Keep appearance after folder rename or move by using the asset GUID.
- Clear invalid entries when requested, not during every repaint.
- Render only for visible Project Browser rows.

## Folder favorites

- Favorites are personal and must not modify project-shared files.
- Add or remove the currently selected folder.
- Select and ping a favorite from the Favorites window.
- Gracefully display or remove missing GUIDs.

## Workbench Inspector

- Follow the active Unity selection in the current unlocked tab.
- Add, select, lock, and close tabs.
- Navigate backward and forward through target history.
- Reuse registered custom editors for the target and, for a `GameObject`, its valid components.
- Destroy created editors whenever their target changes.
- Show a clear empty or missing-target state.

## Hierarchy decoration

- Display an active-state dot and prefab marker.
- Clicking the active-state dot toggles `GameObject.activeSelf` with Undo.
- Drawing must be lightweight and disabled from Project Settings.

## Settings

- Enable or disable Project Browser and Hierarchy decoration.
- Open Inspector and Favorites windows.
- Repaint relevant Unity windows after setting changes.

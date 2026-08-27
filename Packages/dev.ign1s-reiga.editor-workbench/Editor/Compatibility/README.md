# Compatibility

This folder is the only place in the package allowed to hold version-specific or
undocumented Unity Editor API access. See `docs/COMPATIBILITY.md` for the baseline
version and the list of public APIs the package relies on.

Rules for code added here:

- Feature modules must never contain preprocessor version checks. Put the check in
  an adapter here and expose a single stable entry point to the rest of the package.
- Every adapter documents the Unity versions it targets and the behavior it falls
  back to when the API is unavailable.
- Prefer a documented public API. Reach for anything else only when no public API
  exists, and record the assumption in `docs/COMPATIBILITY.md`.

The folder is empty of code by design. The MVP targets a single baseline and needs
no adapters yet.

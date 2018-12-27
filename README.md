This template is my starting point to bootstrap 2d unity projects. Additional READMEs can be found in individual asset folders.

## High-level workflow

At a high level, there are two basic patterns are available for use with this template:

 * The GameManager handles all game logics, including initialization of cameras and players then bootstrapping the first scene. This should be used for quick prototyping, as it involves the least amount of work to get to a playable game.

 * The GameManager handles macro-level pieces of game logic, but most of the details (including player/asset and camera init) are handled by the LevelManagers. This should be used for more complex games, and has an advantage over the all-gamemager design in that individual scenes are self-contained enough to be more easily tested in the unity editor.

To use GameManager only mode, check the "Initialize Players and Cams" box in the inspector for the GameManager in the bootstrap scene, then populate the players and cams in the inspector.

To use the LevelManager, only check "Initialize Players and Cams" in the LevelManager (or create them in the scene itself).

Regardless of which is chosen, the "First Scene" variable must also be populated in the inspector.

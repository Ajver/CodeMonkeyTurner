# TODO

- [ ] Invisible inactive level areas
- [ ] Pick suitcase animation
- [ ] Scrolling mission buttons in Menu (animation)
- [ ] Refactor UpdateNoActionPointsUIsVisibility to State machine
- [ ] Central GameEvents singleton to replace static Events from classes
- [ ] Allow selecting grid for action by clicking on object on this position (i.e. Enemy/Crate)

## Done

- [x] Selecting actions using Num keys
- [x] Save system
  - [x] Enabling next levels when previous is finished
- [x] Hide actions UI when no more Action Point
  - [x] Next unit button (selecting another unit with action points)
  - [x] When no unit has any action points - show button to end the turn
- [x] Displaying selected unit data in the corner + camera showing his face 
- [x] Prevent grenades/barrels destroying stuff through walls
- [x] Increase contrast between light-red and red (to make shootable targets better visible)
- [x] Pause screen with ability to restart level or back to menu
- [x] When completing level - go back to menu with the Missions camera selected
- [x] Fading between scenes
- [x] Exit to Windows in main menu
- [x] 3rd mission
- [x] Sound effects
- [x] Fix bug with lighting when changing level
- [x] Space to move camera to selected Unit
- [x] Rotate before other actions (i.e. Interact, Grenade, etc)
- [x] Improve rotation before shooting (wait for unit to fully face target)
- [x] Dynamic Mission Failed/Complete description (i.e. different reason of failure)
- [x] Don't run open/close door animation at level start
- [x] Main menu
- [x] Disabling Level areas and enabling them on doors opened
- [x] Fix weird Grenade trail
- [x] Fix bug when Shootable dies but the grid position is still highlighted as shootable (RED colored)
- [x] Fix bug when Selected Unit dies, but can still perform an action
- [x] Refactor Interactable objects (like door) - make general Interactable class with Event,
  and just add this class to every interactable object.
- [x] Remove grid System from GridObject
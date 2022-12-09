# TODO

- [ ] 3rd mission
- [ ] Sound effects
  - [ ] Aiming sound (when preparing to shoot)
  - [x] Music
  - [x] Menu sounds (shoot, door, etc)
  - [x] Door sounds
  - [x] Mission win/lose sounds
- [ ] Scrolling mission buttons in Menu (animation)
- [ ] Invisible inactive level areas
- [ ] Prevent grenades/barrels destroing stuff through walls
- [ ] Next unit button (selecting another unit with action points)
- [ ] Hide actions UI when no more Action Point
- [ ] Pick suitcase animation
- [ ] Central GameEvents singleton to replace static Events from classes
- [ ] Displaying selected unit data in the corner + camera showing his face 

## Done

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
# TODO

- [ ] Improve rotation before shooting (wait for unit to fully face target)
- [ ] Rotate before other actions (i.e. Interact, Grenade, etc)
- [ ] Hide actions UI when no more Action Point
- [ ] Next unit button (selecting another unit with action points)
- [ ] Central GameEvents singleton to replace static Events from classes
- [ ] Pick suitcase animation
- [ ] Different levels (missions)

## Done
 
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
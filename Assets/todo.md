# TODO

- [ ] Disabling Level areas and enabling them on doors opened
- [ ] Improve rotation before shooting (wait for unit to fully face target)
- [ ] Hide actions UI when no more Action Point
- [ ] Dynamic Mission Failed/Complete description (i.e. different reason of failure)
- [ ] Rotate before other actions (i.e. Interact, Grenade, etc)
- [ ] Main menu
- [ ] Next unit button (selecting another unit with action points)
- [ ] Central GameEvents singleton to replace static Events from classes
- [ ] Pick suitcase animation
- [ ] Don't run open/close door animation at level start
- [ ] Different levels (missions)

## Done

- [x] Fix weird Grenade trail
- [x] Fix bug when Shootable dies but the grid position is still highlighted as shootable (RED colored)
- [x] Fix bug when Selected Unit dies, but can still perform an action
- [x] Refactor Interactable objects (like door) - make general Interactable class with Event,
  and just add this class to every interactable object.
- [x] Remove grid System from GridObject
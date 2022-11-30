# TODO

- [ ] Hide actions UI when no more Action Point
- [ ] Next unit button ?? (selecting another unit with action points)
- [ ] Central GameEvents singleton to replace static Events from classes
- [ ] Pick suitcase animation
- [ ] Improve rotation before shooting (wait for unit to fully face target)
- [ ] Rotate before other actions (i.e. Interact, Grenade, etc)
- [ ] Don't run open/close door animation at level start
- [ ] Disabling Level areas and enabling them on doors opened
- [ ] Main menu
- [ ] Fix weird Grenade trail
- [ ] Dynamic Mission Failed/Complete description (i.e. different reason of failure)

## Done

- [x] Fix bug when Shootable dies but the grid position is still highlighted as shootable (RED colored)
- [x] Fix bug when Selected Unit dies, but can still perform an action
- [x] Refactor Interactable objects (like door) - make general Interactable class with Event,
  and just add this class to every interactable object.
- [x] Remove grid System from GridObject
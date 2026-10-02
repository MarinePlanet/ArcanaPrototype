# Unity Card System Setup

Follow these sections in order. These instructions extend the existing `SampleScene`; they do not replace it.

Important:

- Open `Assets/Scenes/SampleScene.unity`.
- Wait for Unity to finish importing and compiling before adding components.
- Continue using Legacy uGUI `Text`, not TextMeshPro, for every new text object below.
- Do not delete or reparent the existing `Canvas`, `Managers`, `GameFlow`, `HandPanel`, `HandContainer`, `PlayArea`, `EnvironmentPanel`, `EnemyArea`, `PlayerStatusArea`, `ControlsPanel`, menu panels, health displays, or existing buttons.
- Do not enter Play Mode until all required references in Sections 8–13 are assigned.

## 1. Create organization folders

In the Project window, create these folders if they do not already exist:

1. `Assets/Data`
2. `Assets/Data/Cards`
3. `Assets/Data/CardEffects`
4. `Assets/Data/Decks`
5. `Assets/Data/Environments`
6. `Assets/Data/Statuses`
7. `Assets/Data/StatusEffects`
8. `Assets/Prefabs/UI`

The folder names are organizational only. The scripts do not load assets by path.

## 2. Create reusable card-effect assets

Create these assets in `Assets/Data/CardEffects` by right-clicking the folder and choosing the stated Create menu item. Rename each asset exactly as shown and set its field.

1. Create > Arcana > Card Effects > Damage Enemy
   - Rename: `DamageEnemy10`
   - `Amount` = `10`
2. Create another Damage Enemy asset:
   - Rename: `DamageEnemy20`
   - `Amount` = `20`
3. Create another Damage Enemy asset:
   - Rename: `DamageEnemy25`
   - `Amount` = `25`
4. Create another Damage Enemy asset:
   - Rename: `DamageEnemy40`
   - `Amount` = `40`
5. Create > Arcana > Card Effects > Damage Player
   - Rename: `DamagePlayer20`
   - `Amount` = `20`
6. Create > Arcana > Card Effects > Draw Cards
   - Rename: `DrawCards2`
   - `Amount` = `2`
7. Create another Draw Cards asset:
   - Rename: `DrawCards4`
   - `Amount` = `4`
8. Create > Arcana > Card Effects > Random Discard
   - Rename: `RandomDiscard1`
   - `Amount` = `1`
9. Create > Arcana > Card Effects > Random Environment
   - Rename: `RandomEnvironment`
   - It has no configurable fields.

Do not create card-specific effect scripts. These same effect types can be reused by future cards.

## 3. Create the delayed-heal status assets

### 3.1 Expiration behavior

1. In `Assets/Data/StatusEffects`, choose Create > Arcana > Status Effects > Heal Status Owner.
2. Rename it `HealStatusOwner20`.
3. Set `Amount` = `20`.

### 3.2 Status data

1. In `Assets/Data/Statuses`, choose Create > Arcana > Status Data.
2. Rename it `DelayedHeal`.
3. Configure it:
   - `Status Name` = `Delayed Heal`
   - `Description` = `Heal 20 HP when the countdown expires.`
   - `Expiration Effects` Size = `1`
   - `Expiration Effects` Element 0 = drag `HealStatusOwner20`

### 3.3 Card effect that applies the status

1. In `Assets/Data/CardEffects`, choose Create > Arcana > Card Effects > Apply Status.
2. Rename it `ApplyDelayedHealToEnemy`.
3. Configure it:
   - `Status` = drag `DelayedHeal`
   - `Target` = `Enemy`
   - `Duration` = `2`

The status duration is processed at player-turn starts. The play turn does not decrement it: the next turn changes 2 to 1, and the following turn changes 1 to 0, heals the enemy, and removes the status.

## 4. Create and configure the four CardData assets

In `Assets/Data/Cards`, choose Create > Arcana > Card Data four times. Colors are only visual suggestions; any readable colors are acceptable.

### 4.1 The Tower

Rename the asset `TheTower` and configure:

- `Card Name` = `The Tower`
- `Card Type` = `Attack`
- `Cost` = `1`
- `Card Color` = a muted red
- `Upright Probability` = `0.5`
- `Upright Description` = `Deal 10 damage to the enemy.`
- `Upright Effects` Size = `1`
- `Upright Effects` Element 0 = `DamageEnemy10`
- `Reversed Probability` = `0.5`
- `Reversed Description` = `Deal 25 damage to the enemy. After 2 turns, the enemy heals 20 HP.`
- `Reversed Effects` Size = `2`
- `Reversed Effects` Element 0 = `DamageEnemy25`
- `Reversed Effects` Element 1 = `ApplyDelayedHealToEnemy`

Effect order matters: damage occurs before the status is applied.

### 4.2 The Chariot

Rename the asset `TheChariot` and configure:

- `Card Name` = `The Chariot`
- `Card Type` = `Attack`
- `Cost` = `3`
- `Card Color` = a muted orange
- `Upright Probability` = `0.7`
- `Upright Description` = `Deal 20 damage to the enemy.`
- `Upright Effects` Size = `1`
- `Upright Effects` Element 0 = `DamageEnemy20`
- `Reversed Probability` = `0.3`
- `Reversed Description` = `Deal 40 damage to the enemy and 20 damage to yourself.`
- `Reversed Effects` Size = `2`
- `Reversed Effects` Element 0 = `DamageEnemy40`
- `Reversed Effects` Element 1 = `DamagePlayer20`

### 4.3 The Magician

Rename the asset `TheMagician` and configure:

- `Card Name` = `The Magician`
- `Card Type` = `Utility`
- `Cost` = `1`
- `Card Color` = a muted blue
- `Upright Probability` = `0.8`
- `Upright Description` = `Draw 2 cards.`
- `Upright Effects` Size = `1`
- `Upright Effects` Element 0 = `DrawCards2`
- `Reversed Probability` = `0.2`
- `Reversed Description` = `Draw 4 cards, then randomly discard 1 card.`
- `Reversed Effects` Size = `2`
- `Reversed Effects` Element 0 = `DrawCards4`
- `Reversed Effects` Element 1 = `RandomDiscard1`

Keep this reversed list in that order. The Magician leaves the hand before its effects execute, so it cannot discard itself.

### 4.4 The Wheel of Fortune

First create and configure the CardData asset:

- Rename the asset `TheWheelOfFortune`
- `Card Name` = `The Wheel of Fortune`
- `Card Type` = `Utility`
- `Cost` = `1`
- `Card Color` = a muted green
- `Upright Probability` = `0.8`
- `Upright Description` = `Randomly change to a different Environment.`
- `Upright Effects` Size = `1`
- `Upright Effects` Element 0 = `RandomEnvironment`
- `Reversed Probability` = `0.2`
- `Reversed Description` = `Randomly change the Environment, then add a copy of this card to your hand.`

Now create its reusable copy effect:

1. In `Assets/Data/CardEffects`, choose Create > Arcana > Card Effects > Add Card Copy To Hand.
2. Rename it `AddWheelCopyToHand`.
3. `Card To Copy` = drag `TheWheelOfFortune`.
4. `Number Of Copies` = `1`.

Return to `TheWheelOfFortune` and finish:

- `Reversed Effects` Size = `2`
- `Reversed Effects` Element 0 = `RandomEnvironment`
- `Reversed Effects` Element 1 = `AddWheelCopyToHand`

The created copy is temporary battle state. It is not added to the authoritative deck asset and disappears on battle reset.

## 5. Create the authoritative 12-card deck

1. In `Assets/Data/Decks`, choose Create > Arcana > Deck Data.
2. Rename the asset `PlayerDeck`.
3. Set `Cards` Size = `12`.
4. Assign every element exactly:
   - Element 0 = `TheTower`
   - Element 1 = `TheTower`
   - Element 2 = `TheTower`
   - Element 3 = `TheChariot`
   - Element 4 = `TheChariot`
   - Element 5 = `TheChariot`
   - Element 6 = `TheMagician`
   - Element 7 = `TheMagician`
   - Element 8 = `TheMagician`
   - Element 9 = `TheWheelOfFortune`
   - Element 10 = `TheWheelOfFortune`
   - Element 11 = `TheWheelOfFortune`

Repeated references are intentional: each list entry creates a separate runtime `CardInstance`, while all three copies share the same reusable `CardData`.

## 6. Convert the existing environments to data assets

Create three assets in `Assets/Data/Environments` with Create > Arcana > Environment Data:

1. `FullMoon`
   - `Environment Name` = `Full Moon`
   - `Description` = `Global Effect: Reversed Arcana are stronger.`
2. `SolarFlare`
   - `Environment Name` = `Solar Flare`
   - `Description` = `Global Effect: Upright Arcana glow with energy.`
3. `TwilightFog`
   - `Environment Name` = `Twilight Fog`
   - `Description` = `Global Effect: Orientation is harder to predict.`

Select the existing `Managers` object and configure its existing `EnvironmentManager`:

- `Environment Name Text` → `Canvas/EnvironmentPanel/EnvironmentNameText` (`Text` component)
- `Environment Description Text` → `Canvas/EnvironmentPanel/EnvironmentDescText` (`Text` component)
- `Environments` Size = `3`
- `Environments` Element 0 = `FullMoon`
- `Environments` Element 1 = `SolarFlare`
- `Environments` Element 2 = `TwilightFog`

Element 0 is the environment used after reset. The Wheel effect selects a different configured entry when at least two exist. The existing Change Environment button can remain connected to `EnvironmentManager.ChangeEnvironment`.

## 7. Update the existing Arcana card prefab

Open `Assets/Prefabs/Arcana.prefab` in Prefab Mode. Keep its existing root, `Image`, `LayoutElement`, `CardView`, and `CardTitleText`.

### 7.1 Adjust the existing title

Select `CardTitleText`:

- Keep its Legacy `Text` component.
- Suggested anchors: horizontal stretch, centered vertically.
- Suggested offsets: Left `8`, Right `-8`, Top `-38`, Bottom `38`.
- Enable word wrapping if needed.
- Center-align the text.

### 7.2 Add permanent cost text

1. Under the prefab root, create UI > Legacy > Text.
2. Rename it `CostText`.
3. Suggested RectTransform:
   - Anchor preset: top-left
   - Width `36`, Height `30`
   - Pos X `22`, Pos Y `-18`
4. Set placeholder text to `1`, use a readable bold-looking size, and align center.
5. Clear `Raycast Target` on this Text.

### 7.3 Add permanent type text

1. Under the prefab root, create UI > Legacy > Text.
2. Rename it `TypeText`.
3. Suggested RectTransform:
   - Anchor preset: bottom stretch
   - Left `6`, Right `-6`, Bottom `6`, Height `26`
4. Set placeholder text to `ATTACK`, use a small readable size, and align center.
5. Clear `Raycast Target` on this Text.

### 7.4 Assign CardView fields

Select the prefab root and assign:

- `CardView > Title Text` → child `CardTitleText` (`Text`)
- `CardView > Cost Text` → child `CostText` (`Text`)
- `CardView > Type Text` → child `TypeText` (`Text`)
- `CardView > Background Image` → prefab root (`Image`)
- `CardView > Hover Scale` = `1.1`

Apply/save the prefab and exit Prefab Mode. Do not add card-specific data to the prefab; runtime initialization supplies it.

## 8. Create the shared card tooltip

Create this under the existing `Canvas`, as a top-level Canvas child:

```text
CardTooltipPanel   (Image, CardTooltip) [initially inactive]
└── CardTooltipText (Legacy Text)
```

Configure `CardTooltipPanel`:

- RectTransform width `420`, height `260`
- Pivot `(0.5, 0)` is recommended so it grows upward from its positioned point
- Add/use an `Image` with a dark, mostly opaque color
- Clear `Raycast Target` on the Image so the tooltip cannot steal hover input
- Add `CardTooltip`
- Keep it as a late/last Canvas sibling so it renders above cards

Configure `CardTooltipText`:

- Stretch to the panel on all sides
- Left/Right/Top/Bottom offsets about `16`
- Use Legacy `Text`, upper-left alignment, readable font size, and word wrapping
- Clear `Raycast Target`

Assign on `CardTooltipPanel`:

- `CardTooltip > Tooltip Rect` → `CardTooltipPanel` (`RectTransform`)
- `CardTooltip > Information Text` → `CardTooltipText` (`Text`)
- `CardTooltip > Screen Offset` = X `0`, Y `150` (increase if it overlaps cards)

Finally, uncheck the `CardTooltipPanel` GameObject so it is initially inactive. The script activates it on hover.

## 9. Add player resource and hand-count UI

### 9.1 Resource UI

Under existing `Canvas/PlayerStatusArea`, create UI > Legacy > Text and rename it `ResourceText`.

Suggested placement:

- Anchor: bottom-left
- Width `210`, height `35`
- Position it just above or beside the existing `PlayerHealthDisplay`
- Initial text: `Resource: 4 / 4`
- Left-align or center-align it

Because it is a child of `PlayerStatusArea`, the existing game-flow battle visibility will also hide/show it.

### 9.2 Hand count UI

Under existing `Canvas/HandPanel`, create UI > Legacy > Text and rename it `HandCountText`.

Suggested placement:

- Anchor: top-right
- Width `150`, height `35`
- Anchored Pos X `-90`, Pos Y `-20`
- Initial text: `Hand: 0 / 8`
- Right-align it

Keep it outside the area occupied by `HandContainer`. If needed, move it upward within `HandPanel`; do not reparent the hand.

## 10. Create the status prefab, bars, and tooltip

### 10.1 StatusView prefab

Temporarily create UI > Image under the Canvas and rename it `StatusView`.

- RectTransform width `150`, height `32`
- Image color: a readable muted color
- Keep the root Image `Raycast Target` enabled so hover events work
- Add `LayoutElement`; set Preferred Width `150`, Preferred Height `32`
- Add the `StatusView` script

Create a Legacy Text child named `StatusLabel`:

- Stretch to all four sides with small padding
- Center-align it
- Clear the Text's `Raycast Target`

Assign:

- `StatusView > Status Label` → child `StatusLabel` (`Text`)

Drag the configured root from Hierarchy to `Assets/Prefabs/UI` to create `StatusView.prefab`. Then delete only the temporary scene instance.

### 10.2 Status tooltip

Create this as another top-level child of the Canvas:

```text
StatusTooltipPanel   (Image, StatusTooltip) [initially inactive]
└── StatusTooltipText (Legacy Text)
```

Configure the panel:

- Width `400`, height `190`
- Dark, mostly opaque Image
- Clear the Image's `Raycast Target`
- Add `StatusTooltip`
- Keep it late in Canvas sibling order

Configure its text:

- Stretch with about `16` pixels padding
- Legacy Text, upper-left alignment, word wrapping
- Clear `Raycast Target`

Assign:

- `StatusTooltip > Tooltip Rect` → `StatusTooltipPanel` (`RectTransform`)
- `StatusTooltip > Information Text` → `StatusTooltipText` (`Text`)
- `StatusTooltip > Screen Offset` = X `0`, Y `100`

Uncheck `StatusTooltipPanel` so it is initially inactive.

### 10.3 Player status bar

Under existing `Canvas/PlayerStatusArea`, create an empty UI object named `PlayerStatusBar`.

- Suggested width `500`, height `40`
- Place it beside or immediately above the player HP/resource UI
- Add `HorizontalLayoutGroup`
- Set Spacing around `6`
- Enable Child Alignment `Middle Left`
- Disable Force Expand Width/Height
- The bar starts empty; do not place permanent status children in it

Add `StatusManager` to the existing `PlayerStatusArea` object and assign:

- `Owner` = `Player`
- `Player Health` → `Canvas/PlayerStatusArea` (`PlayerHealth`)
- `Enemy Health` → `Canvas/EnemyArea/Enemy` (`EnemyHealth`)
- `Status View Prefab` → `Assets/Prefabs/UI/StatusView.prefab`
- `Status Container` → `Canvas/PlayerStatusArea/PlayerStatusBar` (`Transform`)
- `Status Tooltip` → `Canvas/StatusTooltipPanel` (`StatusTooltip`)

### 10.4 Enemy status bar

Under existing `Canvas/EnemyArea`, create an empty UI object named `EnemyStatusBar`.

- Suggested width `500`, height `40`
- Place it immediately below or beside `EnemyHealthDisplay`, without covering the enemy hover area
- Add `HorizontalLayoutGroup`
- Spacing about `6`, Child Alignment `Middle Center`
- Disable Force Expand Width/Height
- Leave it empty

Add `StatusManager` to the existing `Enemy` object and assign:

- `Owner` = `Enemy`
- `Player Health` → `Canvas/PlayerStatusArea` (`PlayerHealth`)
- `Enemy Health` → `Canvas/EnemyArea/Enemy` (`EnemyHealth`)
- `Status View Prefab` → `Assets/Prefabs/UI/StatusView.prefab`
- `Status Container` → `Canvas/EnemyArea/EnemyStatusBar` (`Transform`)
- `Status Tooltip` → `Canvas/StatusTooltipPanel` (`StatusTooltip`)

## 11. Add and configure the battle manager components

Select the existing `Managers` GameObject. Add these three components:

1. `DeckManager`
2. `PlayerResource`
3. `BattleTurnManager`

Configure `DeckManager`:

- `Player Deck` → `Assets/Data/Decks/PlayerDeck`
- Leave Draw Pile, Discard Pile, and Exhaust Pile unchanged; they are runtime debug lists.

Configure `PlayerResource`:

- `Maximum Resource` = `4`
- `Resource Text` → `Canvas/PlayerStatusArea/ResourceText` (`Text`)
- `Current Resource` is runtime debug state; do not configure it as gameplay data.

Configure `BattleTurnManager`:

- `Cards Drawn Per Turn` = `5`
- `Hand Manager` → `Managers` (`HandManager`)
- `Player Resource` → `Managers` (`PlayerResource`)
- `Player Status Manager` → `Canvas/PlayerStatusArea` (`StatusManager`)
- `Enemy Status Manager` → `Canvas/EnemyArea/Enemy` (`StatusManager`)
- `Enemy Action Pool` → `Canvas/EnemyArea/Enemy` (`EnemyActionPool`)
- `Arcana Resolver` → `Managers` (`ArcanaResolver`)
- `Player Health` → `Canvas/PlayerStatusArea` (`PlayerHealth`)
- `Enemy Health` → `Canvas/EnemyArea/Enemy` (`EnemyHealth`)

## 12. Update existing manager references

### 12.1 HandManager on Managers

Keep its existing prefab, container, resolver, and message assignments, and assign every field as follows:

- `Card Prefab` → `Assets/Prefabs/Arcana.prefab` (`CardView`)
- `Hand Container` → `Canvas/HandPanel/HandContainer` (`Transform`)
- `Arcana Resolver` → `Managers` (`ArcanaResolver`)
- `Deck Manager` → `Managers` (`DeckManager`)
- `Card Tooltip` → `Canvas/CardTooltipPanel` (`CardTooltip`)
- `Maximum Hand Size` = `8`
- `Hand Message Text` → `Canvas/HandPanel/HandMessageText` (`Text`)
- `Hand Count Text` → `Canvas/HandPanel/HandCountText` (`Text`)
- Leave `Cards In Hand` alone; it is runtime debug state.

The existing Draw Card button may remain connected to `Managers > HandManager.DrawCard`. It now draws from the real draw pile and respects the hand limit. It is useful as a prototype/debug control.

### 12.2 ArcanaResolver on Managers

Keep its existing Play Area, Result Text, and Display Duration. Assign:

- `Play Area` → `Canvas/PlayArea` (`RectTransform`)
- `Result Text` → `Canvas/PlayArea/ResultText` (`Text`)
- `Display Duration` = `2` (or the existing preferred short delay)
- `Hand Manager` → `Managers` (`HandManager`)
- `Deck Manager` → `Managers` (`DeckManager`)
- `Player Resource` → `Managers` (`PlayerResource`)
- `Player Health` → `Canvas/PlayerStatusArea` (`PlayerHealth`)
- `Enemy Health` → `Canvas/EnemyArea/Enemy` (`EnemyHealth`)
- `Environment Manager` → `Managers` (`EnvironmentManager`)
- `Player Status Manager` → `Canvas/PlayerStatusArea` (`StatusManager`)
- `Enemy Status Manager` → `Canvas/EnemyArea/Enemy` (`StatusManager`)
- `Orientation Debug Mode` = `Use Card Probabilities` for normal play

The debug mode can temporarily force Upright or Reversed during testing. Always restore it to `Use Card Probabilities` afterward.

### 12.3 GameFlowManager on GameFlow

Keep all existing screen objects, battle UI objects, health references, and old reset references. Add the new references:

- `Deck Manager` → `Managers` (`DeckManager`)
- `Player Resource` → `Managers` (`PlayerResource`)
- `Battle Turn Manager` → `Managers` (`BattleTurnManager`)
- `Player Status Manager` → `Canvas/PlayerStatusArea` (`StatusManager`)
- `Enemy Status Manager` → `Canvas/EnemyArea/Enemy` (`StatusManager`)

For clarity, all GameFlow fields should now be:

- `Main Menu Panel` → `Canvas/MainMenuPanel`
- `Deck View Panel` → `Canvas/DeckViewPanel`
- `Victory Panel` → `Canvas/VictoryPanel`
- `Defeat Panel` → `Canvas/DefeatPanel`
- Keep the existing `Battle UI Objects` array and its existing top-level battle UI entries. Do not replace or reorder them merely for this feature.
- `Player Health` → `Canvas/PlayerStatusArea` (`PlayerHealth`)
- `Enemy Health` → `Canvas/EnemyArea/Enemy` (`EnemyHealth`)
- `Hand Manager` → `Managers` (`HandManager`)
- `Arcana Resolver` → `Managers` (`ArcanaResolver`)
- `Enemy Action Pool` → `Canvas/EnemyArea/Enemy` (`EnemyActionPool`)
- `Environment Manager` → `Managers` (`EnvironmentManager`)
- plus the five new assignments listed above

Resource, hand-count, and status-bar objects are children of existing battle UI roots, so they do not need new entries in `Battle UI Objects`.

## 13. Connect End Turn to the integrated flow

Select `Canvas/ControlsPanel/EndTurnButton` and inspect `Button > On Click()`.

1. Remove the old direct `EnemyActionPool.TakeTurn` listener from this button. Leaving it would make the enemy act twice.
2. Add one listener:
   - Target object → `Managers`
   - Function → `BattleTurnManager.EndTurn`

Keep these existing button connections:

- `DrawCardButton` → `HandManager.DrawCard`
- `ChangeEnvironmentButton` → `EnvironmentManager.ChangeEnvironment`
- Menu, View Deck, Back, and result-screen buttons → their existing `GameFlowManager` methods

## 14. Convert View Deck from placeholder to the real deck

Select the existing `Canvas/DeckViewPanel/DeckContentArea`.

1. Delete only its existing child `PlaceholderText`.
2. Add a `GridLayoutGroup` to `DeckContentArea`.
3. Suggested settings:
   - Cell Size = match the Arcana prefab's width and height
   - Spacing X/Y = `15`, `15`
   - Start Corner = Upper Left
   - Start Axis = Horizontal
   - Constraint = Fixed Column Count
   - Constraint Count = `4` (use `3` if the screen is narrow)
4. If all 12 cards do not fit, make `DeckContentArea` taller or place it inside a standard uGUI `ScrollRect`. Scrolling is optional; do not rebuild the panel if the grid already fits.

Add `DeckViewController` to the existing `DeckViewPanel` and assign:

- `Player Deck` → `Assets/Data/Decks/PlayerDeck`
- `Card Prefab` → `Assets/Prefabs/Arcana.prefab` (`CardView`)
- `Card Container` → `Canvas/DeckViewPanel/DeckContentArea` (`Transform`)
- `Card Tooltip` → `Canvas/CardTooltipPanel` (`CardTooltip`)

The controller builds all 12 entries from the same `PlayerDeck` asset used by battle. Duplicate entries remain visible. Deck-view cards have hover/tooltip behavior but no play click action.

## 15. Confirm initial active states and save

Before Play Mode:

1. `MainMenuPanel` may be active; `GameFlowManager.Start` will enforce it.
2. Keep the existing `DeckViewPanel`, `VictoryPanel`, and `DefeatPanel` initial state as already configured; GameFlow controls them.
3. `CardTooltipPanel` must be inactive.
4. `StatusTooltipPanel` must be inactive.
5. `PlayerStatusBar` and `EnemyStatusBar` should be active but empty.
6. Save `SampleScene`.
7. Check the Console. Resolve any `Missing Script` or unassigned-reference errors before testing.

## 16. Runtime debug support

During Play Mode you can inspect:

- `Managers > DeckManager`: Draw Pile, Discard Pile, and Exhaust Pile runtime lists and counts.
- `Managers > HandManager`: actual `Cards In Hand` list and `Maximum Hand Size`.
- `Managers > PlayerResource`: `Current Resource`.
- Player and Enemy `StatusManager`: `Active Statuses` and each remaining duration/source.
- `Managers > ArcanaResolver > Orientation Debug Mode`: force a branch for testing.

The component context menus also include:

- `DeckManager > Debug: Log Pile Counts`
- `StatusManager > Debug: Log Active Statuses`

Do not edit runtime list contents while testing normal behavior.

## 17. Complete test sequence

Run these tests after completing every setup section.

### A. Start and deck

1. Enter Play Mode. Verify the Main Menu opens and battle UI is hidden.
2. Click View Deck.
3. Verify exactly 12 cards: Tower ×3, Chariot ×3, Magician ×3, Wheel ×3.
4. Hover several deck cards. Verify name, both probabilities, and both descriptions appear; verify card enlargement remains.
5. Click Back and verify Main Menu returns.
6. Click Start Game. Verify exactly 5 cards and `Hand: 5 / 8`.

### B. Resource

1. Verify `Resource: 4 / 4`.
2. Play a Cost 1 card; after clicking, verify resource becomes 3.
3. On a fresh turn, play The Chariot (Cost 3); verify resource becomes 1.
4. With fewer than 3 resource, click another Chariot. Verify it remains in hand, does not resolve, and resource does not change.
5. Click End Turn. Verify the next turn restores `Resource: 4 / 4`.

### C. Hand and deck cycle

1. Verify `Hand: current / 8` changes immediately when drawing, playing, and discarding.
2. End a turn with cards remaining. Verify all remaining cards disappear from hand and enter DeckManager's Discard Pile.
3. Verify the next player turn draws 5, not “up to 5.”
4. Repeat turns until Draw Pile reaches zero.
5. Verify a later draw moves the discard pile into the draw pile, shuffles it, and completes as much of the requested draw as possible.
6. Inspect pile plus hand totals. Apart from a generated temporary Wheel, verify no unexplained copies or losses.

### D. The Tower

1. On `ArcanaResolver`, set `Orientation Debug Mode = Force Upright`.
2. Play The Tower and verify enemy HP decreases by 10.
3. Start a fresh battle or get another Tower; set `Force Reversed`.
4. Play it and verify enemy HP decreases by 25 and `Delayed Heal (2)` appears in Enemy Status Bar.
5. Hover the status. Verify its description, `Remaining: 2`, and `Source: Card — The Tower (Reversed)`.
6. End Turn once. At the next player-turn start, verify status shows `(1)` and the enemy has not healed.
7. End Turn again. Verify enemy heals 20 and the status disappears.

### E. The Chariot

1. Force Upright, play The Chariot, and verify enemy loses 20 HP.
2. Force Reversed, play another Chariot, and verify enemy loses 40 HP and player loses 20 HP immediately.
3. Verify both HP bars and texts update.
4. Repeat controlled damage as needed to verify lethal player damage opens Defeat and lethal enemy damage opens Victory.

### F. The Magician

1. Force Upright and play The Magician. Verify it leaves hand, then up to 2 cards are drawn without exceeding 8.
2. Force Reversed and play another. Verify it attempts to draw 4, then discards one random card from the remaining hand.
3. Use the Draw Card button or other Magicians to bring the hand near 8, then retest. Verify draw stops at 8 and does not draw-and-discard excess cards.
4. Verify the random discard still happens when the draw was capped or the piles could not provide all four cards.
5. Verify the played Magician itself is not the randomly discarded card; it resolves separately and then enters Discard Pile.

### G. The Wheel of Fortune

1. Note the current environment.
2. Force Upright and play The Wheel. Verify the environment changes to one of the other configured environments, never the same one when alternatives exist.
3. Force Reversed and play another Wheel. Verify the environment changes and one new Wheel appears in hand.
4. Verify `Hand Count` updates, the generated card shows Cost 1 and Utility, can be played normally, and spends 1 resource.
5. Inspect DeckManager: verify the original played Wheel entered Discard Pile while the generated copy initially exists only in hand.
6. To test the full-hand failure, arrange seven other cards in hand before playing Wheel so its removal and any intervening effects leave the hand at the configured maximum when the copy effect executes. If normal gameplay cannot naturally hold eight at that exact moment, temporarily set `Maximum Hand Size` to the current post-play hand count for this isolated test. Verify no copy is added, then restore `Maximum Hand Size = 8`.

### H. Status UI

1. With no status, verify both status bars are clean and empty.
2. Apply Tower's reversed status and verify it appears only on the enemy bar.
3. Hover it and verify name, effect, remaining turns, and typed source are readable.
4. Verify status UI refreshes after every countdown and removal.

### I. Reset

1. During one battle, alter both HP values, play/discard cards, create a temporary Wheel, create Delayed Heal, change Environment, and use enemy actions.
2. Reach a result screen or return through an existing Main Menu button.
3. Start Game again.
4. Verify:
   - Player and enemy HP are full.
   - Both status bars are empty.
   - No generated Wheel survives.
   - The authoritative deck is still exactly 12 cards.
   - Runtime Draw/Discard state was rebuilt.
   - Starting hand is exactly 5.
   - Resource is 4/4.
   - Environment is Full Moon (Element 0).
   - Enemy Action Pool has reset so all actions are available.
   - Result text and Play Area are reset.

### J. Regression

Verify all prior behavior:

1. Main Menu, Start Game, View Deck, and Back.
2. Victory and Defeat screens and their Main Menu buttons.
3. Environment text and manual Change Environment button.
4. Enemy hover reveals Action Pool; leaving hides it.
5. End Turn selects unused enemy actions; all actions reset after the pool is exhausted.
6. Player and enemy HP bars/text update.
7. Existing health context-menu tests still work where previously configured.
8. Card hover scaling works.
9. Upright cards stay upright and Reversed cards rotate 180 degrees.
10. Restore `ArcanaResolver > Orientation Debug Mode` to `Use Card Probabilities` and confirm normal per-card probabilities are used.

## 18. What is intentionally data-driven

- `PlayerDeck` defines the permanent deck; Draw/Discard/Hand consume runtime `CardInstance` objects instead of the asset.
- `CardData` owns type, cost, probabilities, descriptions, color, and ordered effect lists.
- `CardEffect` assets provide reusable operations. The resolver does not test card names.
- `StatusData` and expiration-effect assets define delayed behavior. Status managers do not test status names.
- `EnvironmentData` is shared by UI and random-environment effects.
- Default post-resolution destination is Discard, while the runtime context already has destination values for later Exhaust, Return to Hand, and Shuffle Into Draw Pile effects.

To add a normal fifth card later, create/configure another `CardData`, reuse or add small generic effect assets, and add the desired number of references to `PlayerDeck`. Central resolver/deck/hand code should not need a card-name branch.

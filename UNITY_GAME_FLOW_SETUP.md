# Game Flow — Unity Editor Setup

This guide adds a Main Menu, placeholder Deck View, Victory screen, and Defeat screen to the existing `SampleScene` without rebuilding or reparenting the current battle UI.

The `GameFlowManager` toggles the existing top-level battle objects individually. This preserves their current RectTransform layouts and Inspector references.

For the new visible text and button labels, use **UI > Legacy > Text** and **UI > Legacy > Button**. Do not replace any existing Text or TextMeshPro objects.

## 1. Intended final hierarchy

Keep all existing objects and add only the objects marked `NEW`:

```text
Main Camera                                      (existing)
Global Light 2D                                 (existing)
Canvas                                           (existing)
├── EnvironmentPanel                             (existing battle UI)
├── EnemyArea                                    (existing battle UI)
│   ├── Enemy
│   ├── SelectedActionText
│   └── EnemyHealthDisplay
├── PlayArea                                     (existing battle UI)
├── ResultText                                   (existing battle UI)
├── PlayerStatusArea                             (existing battle UI)
├── HandPanel                                    (existing battle UI)
├── ControlsPanel                                (existing battle UI)
├── MainMenuPanel                                (NEW)
│   ├── TitleText                                (NEW)
│   ├── StartGameButton                          (NEW)
│   │   └── Text                                 (NEW)
│   └── ViewDeckButton                           (NEW)
│       └── Text                                 (NEW)
├── DeckViewPanel                                (NEW)
│   ├── DeckTitleText                            (NEW)
│   ├── DeckContentArea                          (NEW)
│   │   └── PlaceholderText                      (NEW)
│   └── BackButton                               (NEW)
│       └── Text                                 (NEW)
├── VictoryPanel                                 (NEW)
│   ├── VictoryText                              (NEW)
│   └── MainMenuButton                           (NEW)
│       └── Text                                 (NEW)
└── DefeatPanel                                  (NEW)
    ├── DefeatText                               (NEW)
    └── MainMenuButton                           (NEW)
        └── Text                                 (NEW)

EventSystem                                      (existing)
Managers                                         (existing)
GameFlow                                         (NEW; scene root)
```

Do not place `GameFlow` under any screen or battle UI object. It must remain active while screens are being hidden.

## 2. Create the Main Menu

1. Select the existing Canvas.
2. Create **UI > Image** and rename it `MainMenuPanel`.
3. Stretch it over the entire Canvas:
   - Anchor Min: `(0, 0)`
   - Anchor Max: `(1, 1)`
   - Left, Right, Top, Bottom: `0`
4. Give the Image an opaque placeholder background color. Leave its **Raycast Target** enabled so clicks do not pass through the menu.
5. Create **UI > Legacy > Text** under MainMenuPanel and rename it `TitleText`.
6. Suggested TitleText RectTransform:
   - Anchor: middle-center
   - Width: `800`
   - Height: `100`
   - Pos X: `0`
   - Pos Y: `180`
7. Enter a title such as `ARCANA PROTOTYPE`, use middle-center alignment, and use a font size around `44`.
8. Create **UI > Legacy > Button** under MainMenuPanel and rename it `StartGameButton`.
9. Suggested button RectTransform: width `280`, height `60`, Pos X `0`, Pos Y `25`.
10. Change its child Text to `Start Game`.
11. Duplicate StartGameButton, rename the copy `ViewDeckButton`, set Pos Y to `-55`, and change its child Text to `View Deck`.

## 3. Create the Deck View

1. Select the Canvas and create **UI > Image** named `DeckViewPanel`.
2. Stretch it over the entire Canvas with zero offsets and give it an opaque background.
3. Under DeckViewPanel, create a Legacy Text named `DeckTitleText`:
   - Text: `My Deck`
   - Width: `700`
   - Height: `80`
   - Pos Y: `300`
   - Alignment: middle-center
   - Font Size: approximately `38`
4. Create **UI > Image** under DeckViewPanel named `DeckContentArea`.
5. Suggested size: `850 x 420`, centered with Pos Y around `20`.
6. Give it a contrasting, slightly lighter panel color.
7. Under DeckContentArea, create a Legacy Text named `PlaceholderText`.
8. Stretch PlaceholderText across DeckContentArea with approximately `30` pixels of padding.
9. Set its text to `Deck contents will appear here.` and use middle-center alignment with a font size around `24`.
10. Under DeckViewPanel, create a Legacy Button named `BackButton`.
11. Suggested size: `220 x 55`, centered with Pos Y around `-285`.
12. Set its child Text to `Back`.

The current prototype has no real deck collection, so this screen deliberately contains only a placeholder. No card-system refactor is required.

## 4. Create the Victory screen

1. Select the Canvas and create **UI > Image** named `VictoryPanel`.
2. Stretch it over the entire Canvas with zero offsets.
3. Use an opaque or mostly opaque dark background with a green tint.
4. Add a Legacy Text named `VictoryText`:
   - Text: `VICTORY`
   - Size: about `700 x 120`
   - Pos Y: `100`
   - Alignment: middle-center
   - Font Size: approximately `52`
5. Add a Legacy Button named `MainMenuButton`:
   - Size: `260 x 60`
   - Pos Y: `-40`
   - Child Text: `Main Menu`

## 5. Create the Defeat screen

1. Duplicate VictoryPanel and rename the copy `DefeatPanel`.
2. Change the background to a dark red tint.
3. Rename its text object `DefeatText` and change the displayed text to `DEFEAT`.
4. Keep its button named `MainMenuButton` with the label `Main Menu`.

## 6. Add and configure GameFlowManager

1. In the Hierarchy, create an empty scene-root GameObject named `GameFlow`.
2. Add the `GameFlowManager` component.
3. Assign the four screen references:
   - **Main Menu Panel** → `MainMenuPanel`
   - **Deck View Panel** → `DeckViewPanel`
   - **Victory Panel** → `VictoryPanel`
   - **Defeat Panel** → `DefeatPanel`
4. Expand **Battle UI Objects** and set its Size to `7`.
5. Assign these existing top-level UI objects, one per element:
   1. `EnvironmentPanel`
   2. `EnemyArea`
   3. `PlayArea`
   4. `ResultText`
   5. `PlayerStatusArea`
   6. `HandPanel`
   7. `ControlsPanel`
6. Assign the required health references:
   - **Player Health** → the `PlayerHealth` component on `PlayerStatusArea`
   - **Enemy Health** → the `EnemyHealth` component on the existing `Enemy`
7. Assign the optional reset references so every available prototype system resets between battles:
   - **Hand Manager** → `HandManager` on `Managers`
   - **Arcana Resolver** → `ArcanaResolver` on `Managers`
   - **Enemy Action Pool** → `EnemyActionPool` on `Enemy`
   - **Environment Manager** → `EnvironmentManager` on `Managers`

Do not add the Canvas, GameFlow, MainMenuPanel, DeckViewPanel, VictoryPanel, or DefeatPanel to Battle UI Objects.

## 7. Connect every new button

### Start Game

1. Select `MainMenuPanel/StartGameButton`.
2. In **Button > On Click ()**, press `+`.
3. Drag the `GameFlow` GameObject into the object field.
4. Choose `GameFlowManager > StartGame`.

### View Deck

1. Select `MainMenuPanel/ViewDeckButton`.
2. Add an On Click entry using `GameFlow`.
3. Choose `GameFlowManager > ShowDeckView`.

### Deck View Back

1. Select `DeckViewPanel/BackButton`.
2. Add an On Click entry using `GameFlow`.
3. Choose `GameFlowManager > ShowMainMenu`.

### Victory Main Menu

1. Select `VictoryPanel/MainMenuButton`.
2. Add an On Click entry using `GameFlow`.
3. Choose `GameFlowManager > ShowMainMenu`.

### Defeat Main Menu

1. Select `DefeatPanel/MainMenuButton`.
2. Add an On Click entry using `GameFlow`.
3. Choose `GameFlowManager > ShowMainMenu`.

Do not change the existing Draw Card, End Turn, or Change Environment On Click entries.

## 8. Initial active states

Set the Editor active states as follows:

- `GameFlow`: active
- `Canvas`: active
- `MainMenuPanel`: active
- `DeckViewPanel`: inactive
- `VictoryPanel`: inactive
- `DefeatPanel`: inactive
- Existing battle UI objects: they may remain active in the Editor

`GameFlowManager.Start()` always enforces the Main Menu state before the first rendered frame. It hides every assigned battle UI object and all non-menu screens when Play Mode starts.

Keep `Managers`, EventSystem, PlayerStatusArea's health components, and the Enemy's components configured exactly as they currently are.

## 9. What a fresh battle resets

Starting a battle, or returning to the Main Menu, resets the currently available runtime state:

- Player HP returns to maximum.
- Enemy HP returns to maximum.
- The HP depletion notifications are rearmed.
- Every instantiated card in the hand is removed.
- A card currently resolving in PlayArea is removed.
- Arcana resolution is unlocked and ResultText is reset.
- Every enemy action becomes available again.
- The Action Pool popup is hidden and its result text returns to `Enemy is waiting...`.
- Environment returns to the first configured condition, currently Full Moon.
- The game-over guard is cleared when Start Game begins a new battle.

There is no real deck collection yet, so there is no deck state to reset. The Deck View remains a placeholder framework.

## 10. Test Main Menu and Deck View

1. Enter Play Mode.
2. Confirm that MainMenuPanel is visible first.
3. Confirm that the Environment, enemy, Play Area, hand, HP bars, and battle controls are hidden.
4. Click **View Deck**.
5. Confirm that MainMenuPanel hides and DeckViewPanel appears.
6. Confirm that `My Deck` and `Deck contents will appear here.` are visible.
7. Click **Back** and confirm that the Main Menu returns.

## 11. Test Start Game

1. From the Main Menu, click **Start Game**.
2. Confirm that the Main Menu hides.
3. Confirm that all seven assigned battle UI objects become visible.
4. Confirm that both HP displays show `100 / 100`.
5. Confirm that the Environment is the first configured condition.
6. Hover over the Enemy and confirm every enemy action is AVAILABLE.

## 12. Test Victory

1. Start a battle.
2. Select the existing `Enemy` GameObject in the Hierarchy.
3. In the `EnemyHealth` component, set **Test Amount** to `100` for a one-click test, or leave it at `10` for repeated tests.
4. Open the component's three-dot/gear context menu.
5. Choose **Test: Take Damage**.
6. When Enemy HP reaches zero, confirm:
   - The battle UI disappears.
   - VictoryPanel appears.
   - Further HP callbacks do not trigger another result because the battle has already ended.
7. Click **Main Menu** and confirm the Main Menu appears.
8. Click **Start Game** again and confirm Enemy HP is restored to maximum.

## 13. Test Defeat

1. Start a battle.
2. Select `PlayerStatusArea` in the Hierarchy.
3. Set `PlayerHealth > Test Amount` to `100`, or apply the default test damage repeatedly.
4. Use **Test: Take Damage** from the component context menu.
5. When Player HP reaches zero, confirm:
   - The battle UI disappears.
   - DefeatPanel appears.
6. Click **Main Menu**, then start another battle and confirm Player HP is restored to maximum.

The existing **Test: Heal** and **Reset To Full Health** commands remain available on both health components.

## 14. Test battle-state reset

1. Start a battle.
2. Draw several cards.
3. Change the Environment at least once.
4. Use End Turn two or three times so enemy actions become USED.
5. Trigger Victory or Defeat through an HP test.
6. Return to the Main Menu and start another battle.
7. Confirm:
   - The hand is empty.
   - No played card remains in PlayArea.
   - ResultText says `Play an Arcana card`.
   - The first Environment is active.
   - All enemy actions are AVAILABLE.
   - Both HP bars are full.

## 15. Final regression checklist

During a newly started battle, verify all original features:

- Draw Card creates a placeholder Arcana card.
- Cards arrange in the hand.
- Card hover enlarges the card.
- Clicking a card moves it to PlayArea.
- Cards resolve Upright or Reversed.
- Reversed cards rotate 180 degrees.
- Played cards disappear after the configured delay.
- Change Environment still cycles conditions.
- Enemy hover still shows and hides the Action Pool.
- End Turn still selects unused actions and resets an exhausted pool.
- Player and Enemy HP bars still update.
- PlayerHealth and EnemyHealth Inspector test commands still work.
- View Deck remains a placeholder and does not affect the battle.

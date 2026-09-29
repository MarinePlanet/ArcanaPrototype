# Arcana Prototype — Unity Editor Setup

These instructions match the scripts in `Assets/Scripts`. They use Unity's built-in uGUI components and intentionally avoid extra gameplay systems.

## 1. Open and prepare the scene

1. Open the project in Unity `6000.5.10f1`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. If Unity asks to import TextMesh Pro essentials, you can skip it. This setup uses legacy uGUI `Text` components so the prototype has no TMP setup dependency.
4. In the Hierarchy, right-click and choose **UI > Canvas** if the scene does not already contain a Canvas.
5. Select the Canvas and set its **Canvas Scaler**:
   - UI Scale Mode: `Scale With Screen Size`
   - Reference Resolution: `1920 x 1080`
   - Match: `0.5`
6. Unity should also create an **EventSystem**. Keep it; pointer hover and click handling require it.
7. For every scripted text field below, create text with **GameObject > UI > Legacy > Text**. The button's own label may be either legacy Text or TMP.

## 2. Build this hierarchy

Create and rename objects until the Canvas looks like this:

```text
Canvas
├── EnvironmentPanel
│   ├── EnvironmentNameText
│   └── EnvironmentDescriptionText
├── EnemyArea
│   ├── Enemy
│   │   ├── EnemyLabel
│   │   └── ActionPoolPanel
│   │       └── ActionPoolText
│   └── SelectedActionText
├── PlayArea
├── ResultText
├── HandPanel
│   ├── HandMessageText
│   └── HandContainer
└── ControlsPanel
    ├── DrawCardButton
    ├── EndTurnButton
    └── ChangeEnvironmentButton

Managers
```

`Managers` is an empty GameObject at the scene root, outside or inside the Canvas; either works because its components do not render UI.

Suggested layout values are below. Exact positions and colors are not important.

## 3. Environment UI

1. Create **EnvironmentPanel** with **UI > Image**. Anchor it to the top stretch, set height to about `130`, and give it a dark color.
2. Add `EnvironmentNameText` and `EnvironmentDescriptionText` as legacy Text children.
3. Set both text RectTransforms to fill the panel, then position the name in the upper half and the description in the lower half.
4. Use a readable font size such as `28` for the name and `20` for the description. Set alignment to middle-center.
5. Add `EnvironmentManager` to `Managers`.
6. Assign:
   - **Environment Name Text** → `EnvironmentNameText`
   - **Environment Description Text** → `EnvironmentDescriptionText`
7. Leave the three default list entries in **Environments**. They can be edited in the Inspector later without changing code.

## 4. Enemy and hover panel

1. Create **EnemyArea** as an empty UI object in the upper-middle of the Canvas. A size around `700 x 300` works.
2. Under it, create **Enemy** with **UI > Image**, sized around `220 x 180`, and choose a clear placeholder color.
3. Add a legacy Text child named `EnemyLabel`, stretch it across the Enemy, and enter `PLACEHOLDER ENEMY`.
4. Make sure the Enemy's **Image > Raycast Target** box is checked. This is what allows hover events to be detected.
5. Add the `EnemyActionPool` component to Enemy.
6. Under Enemy, create **ActionPoolPanel** with **UI > Image**:
   - Size: about `330 x 220`
   - Position: beside the enemy, for example X = `300`, Y = `0`
   - Use a mostly opaque dark color.
7. Under ActionPoolPanel, create a legacy Text named `ActionPoolText`. Stretch it to fill the panel with roughly `15` pixels of padding. Use left-middle alignment and font size `18`.
8. Uncheck **Raycast Target** on ActionPoolPanel's Image and ActionPoolText. This prevents the popup from interfering with enemy hover detection.
9. You may leave ActionPoolPanel active in Edit Mode. The script hides it automatically when Play Mode begins.
10. Under EnemyArea, create a legacy Text named `SelectedActionText`, place it below the enemy, and size it around `500 x 50`.
11. On Enemy's `EnemyActionPool`, assign:
    - **Action Pool Panel** → `ActionPoolPanel`
    - **Action Pool Text** → `ActionPoolText`
    - **Selected Action Text** → `SelectedActionText`
12. Leave the default four Actions: Attack, Guard, Debuff, and Heavy Attack.

## 5. Play Area and resolver

1. Create **PlayArea** with **UI > Image** in the center of the Canvas.
2. Size it around `260 x 300`. Use a transparent or muted color so it reads as a card slot.
3. Create `ResultText` as a legacy Text directly under the Canvas, not inside PlayArea. Place it below or beside PlayArea, size it around `500 x 100`, and use middle-center alignment.
4. Add `ArcanaResolver` to `Managers`.
5. Assign:
   - **Play Area** → the PlayArea RectTransform
   - **Result Text** → `ResultText`
6. Leave **Display Duration** at `2` seconds, or change it to taste.

## 6. Create the Card prefab

1. Temporarily create **UI > Image** under the Canvas and rename it `ArcanaCard`.
2. Set its RectTransform size to about `140 x 200`.
3. Add a **Layout Element** component and set:
   - Preferred Width: `140`
   - Preferred Height: `200`
4. Add the `CardView` component.
5. Under ArcanaCard, create a legacy Text child named `CardTitleText`.
6. Stretch CardTitleText to fill the card with about `10` pixels of padding. Set its text to `ARCANA`, alignment to middle-center, font size around `20`, and choose a contrasting text color.
7. Uncheck **Raycast Target** on CardTitleText. Keep **Raycast Target** checked on ArcanaCard's Image.
8. On `CardView`, assign:
   - **Title Text** → `CardTitleText`
   - **Background Image** → ArcanaCard's own Image component
9. Leave **Hover Scale** at `1.1`.
10. In the Project window, create `Assets/Prefabs`.
11. Drag ArcanaCard from the Hierarchy into `Assets/Prefabs` to create a prefab.
12. Delete the temporary ArcanaCard instance from the scene. Cards will be instantiated at runtime.

## 7. Hand UI and hand manager

1. Create **HandPanel** with **UI > Image** and anchor it to the bottom stretch. Set height to about `260`.
2. Under HandPanel, create `HandMessageText` as legacy Text and place it along the top of the panel.
3. Under HandPanel, create an empty UI object named `HandContainer`. Stretch it across the remaining panel space.
4. Add **Horizontal Layout Group** to HandContainer and set:
   - Spacing: `15`
   - Child Alignment: `Middle Center`
   - Control Child Size Width: checked
   - Control Child Size Height: checked
   - Use Child Scale Width: unchecked
   - Use Child Scale Height: unchecked
   - Child Force Expand Width: unchecked
   - Child Force Expand Height: unchecked
5. Add `HandManager` to `Managers`.
6. Assign:
   - **Card Prefab** → `Assets/Prefabs/ArcanaCard`
   - **Hand Container** → `HandContainer`
   - **Arcana Resolver** → the `ArcanaResolver` component on Managers
   - **Hand Message Text** → `HandMessageText` (optional, but useful)
7. Leave **Maximum Hand Size** at `8`.

Important: keep HandMessageText outside HandContainer. HandManager counts HandContainer's children to know how many cards are in hand.

## 8. Buttons and On Click connections

1. Create **ControlsPanel** near the bottom, either just above HandPanel or inside unused space in it.
2. Add a **Horizontal Layout Group** if you want Unity to arrange the buttons automatically.
3. Create three **UI > Button** children and name them:
   - `DrawCardButton`
   - `EndTurnButton`
   - `ChangeEnvironmentButton`
4. Change their visible labels to `Draw Card`, `End Turn`, and `Change Environment`.
5. Select DrawCardButton. Under **Button > On Click ()**:
   - Press `+`.
   - Drag `Managers` into the object slot.
   - Select `HandManager > DrawCard` from the function menu.
6. Select EndTurnButton. Under **On Click ()**:
   - Press `+`.
   - Drag `Enemy` into the object slot.
   - Select `EnemyActionPool > TakeTurn`.
7. Select ChangeEnvironmentButton. Under **On Click ()**:
   - Press `+`.
   - Drag `Managers` into the object slot.
   - Select `EnvironmentManager > ChangeEnvironment`.

## 9. Final reference checklist

Before pressing Play, verify that no required slot says `None`:

- CardView: Title Text, Background Image
- HandManager: Card Prefab, Hand Container, Arcana Resolver
- ArcanaResolver: Play Area, Result Text
- EnvironmentManager: Environment Name Text, Environment Description Text
- EnemyActionPool: Action Pool Panel, Action Pool Text, Selected Action Text

Also verify that the scene contains one Canvas, one Graphic Raycaster on the Canvas, and one EventSystem.

## 10. Test the prototype

### Upright and Reversed cards

1. Enter Play Mode.
2. Click **Draw Card** several times. Random placeholder Arcana cards should appear and arrange horizontally.
3. Move the cursor over a card. It should grow slightly, then return to normal when the cursor leaves.
4. Click a card. It should move to PlayArea.
5. Confirm that the result says either:
   - `UPRIGHT / Normal Effect`, with the card upright, or
   - `REVERSED / Stronger Effect + Drawback`, with the card rotated 180 degrees.
6. After about two seconds, the card should disappear and the Play Area should become ready again.
7. Repeat a few times to see both random orientations.

### Environment conditions

1. Note that the first condition is Full Moon.
2. Click **Change Environment** repeatedly.
3. Confirm that the name and description cycle through Full Moon, Solar Flare, Twilight Fog, and back to Full Moon.

### Enemy Action Pool and reshuffle

1. Hover over Enemy. The ActionPoolPanel should appear with four AVAILABLE actions.
2. Move the cursor away. The panel should hide.
3. Click **End Turn** once. SelectedActionText should display the chosen action.
4. Hover over Enemy again. Exactly one action should read USED and three should read AVAILABLE.
5. Repeat End Turn until four turns have occurred. Each chosen action must be different, and after turn four all four entries should read USED.
6. Click End Turn a fifth time. The exhausted pool resets automatically before choosing; the hover panel should now show one USED action and three AVAILABLE actions for the new cycle.

If hover or clicks do not work, first check the EventSystem, the Canvas Graphic Raycaster, and the Image Raycast Target settings. If a button does nothing, recheck that button's On Click object and selected method.

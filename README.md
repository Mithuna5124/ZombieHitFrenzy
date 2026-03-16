# Zombie Hit Frenzy
**Mithuna K — IIT Guwahati**

---

## Build Info
- Unity 6.3 LTS, Android (Portrait), URP
- IL2CPP, ARM64 
- Min SDK: Android 8.0 (API 26)
- Target FPS: 60

---

## How to Play
Touch and drag anywhere to drive. Drag direction steers the car. Hit zombies to score. Restart from the end screen.

---

## Architecture

| Script | Responsibility |
|--------|---------------|
| `IDamageable.cs` | Interface for any object the car can hit |
| `PlayerController.cs` | Touch input + car driving in one place |
| `HitDetector.cs` | Detects collision, calls TakeDamage |
| `CameraRig.cs` | Smooth angled camera follow |
| `Zombie.cs` | Wandering movement + ragdoll on hit |
| `ZombieManager.cs` | Object pool, maintains active zombie count |
| `RoundManager.cs` | Timer, score, round state |
| `UIController.cs` | HUD display, end screen, restart |
| `MenuController.cs` | Title screen tap to start |
| `ShaderVariantStripper.cs` | Strips DOTS variants at build (Editor only) |


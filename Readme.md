# ⚔️ Unity Modular Ability & Character Architecture

[![Unity Version](https://img.shields.io/badge/Unity-6000.3.10f1-black.svg?style=for-the-badge&logo=unity&logoColor=white)](https://unity.com/)
[![Tests](https://img.shields.io/badge/Tests-Passing-success.svg?style=for-the-badge&logo=github-actions)](#)

A highly scalable, production-ready **Component-Based Ability and Entity System** for Unity built on SOLID principles. This project demonstrates a successful migration away from monolithic classes (**God Objects**) toward a decoupled, event-driven component architecture fully covered by automated Unit Tests.

---

## 🎮 Demo Controls & Abilities

Launch the demo scene and use the following layout to control the character and trigger your modular ability assets:

### Input Mapping
* **`W` `A` `S` `D`** — **Movement**: Smooth character movement and procedural rotation facing the directional velocity vector.
* **`1`** — **Slot 1: Dash**: High-velocity displacement action used for rapid re-positioning or dodging hazards.
* **`2`** — **Slot 2: Fire Attack**: Offensive damage-dealing action that applies delayed explosive firepower to targets.
* **`3`** — **Slot 3: Heal**: Defensive restoration utility that interacts with your `HealthComponent` to restore missing HP pools.
* **`4`** — **Slot 4: Ice Attack**: Offensive damage-dealing action that applies ice attack to targets

### Ability Asset Data Configurations
The system handles abilities natively as modular, interchangeable data configurations located within the `Assets/Data/` directory:

* ❄️ **`IceAttack.asset`** — Channeled/Delayed utility action using constraints to slow or freeze entity targets.
* 🔥 **`FireAttack.asset`** — Offensive bursting strike utilizing frame-accurate timeline delayed calculations.
* ⚡ **`Dash.asset`** — Short-duration instant kinetic displacement modification.
* 💚 **`Heal.asset`** — Restorative life-support module invoking contextual parameters on target actors.

---

## 🏗️ Architecture & Design Overview

Instead of stuffing movement, health tracking, audio triggers, and animation logic into a single giant script, the character acts as a lightweight registry hub (`BaseCharacterAdapter`). This hub automatically maps and resolves independent components via their abstraction interfaces at startup:


### Core Modules:
1. **Dynamic Service Registry (`BaseCharacterAdapter`)**: Provides loose, decoupled $O(1)$ lookup access to gameplay modules using the generic `GetComponentProvider<T>()` method pattern.
2. **Action Lifecycle Management (`DelayedActionBase` / `ContinuousDelayedActionBase`)**: Abstract base layers ensuring mathematically strict, frame-by-frame delay counting and active execution ticking without clock-drift accumulation.
3. **Multi-Owner State Constraints (`EntityRestrictionController`)**: A bitmask-like source registry allowing separate systems to place independent constraints simultaneously (e.g., Stun + Freeze) without overriding or prematurely clearing each other's effects.

---

## 🧪 Unit Testing

System stability and edge cases are secured using isolated **EditMode** tests within the Unity Test Runner. Every test block instantiates independent contexts to enforce the F.I.R.S.T properties of unit testing and avoid flaky execution dependencies.

### Covered Test Suites:
* `DelayedActionsTests`: Validates immediate triggers, delay execution offsets, and exact tick counter accumulation during active timelines.
* `RestrictionControllerTests`: Verifies correct masking behavior when managing input and movement constraints from overlapping multi-owner sources.
* `EntityComponentsTest`: A structural integrity test that guarantees mandatory module components remain properly attached and mapped to their contract interfaces.

To execute the automated test suites inside Unity, navigate to:  
`Window` ➔ `General` ➔ `Test Runner` ➔ `Run All (EditMode)`.
# StoryTracker — Skill Assessment

**Date:** 2026-06-29  
**Scope:** Backend C# / ASP.NET Core API (excluding intentionally skipped features: item Type filtering, full FVTT stat export)

---

## Overall verdict

**Rank: Solid Junior — closer to “strong junior” than “weak junior.”**

You are on par with a junior backend developer, with some areas already leaning toward early mid-level (architecture, shipping features). You are not mid-level yet — the gap is mostly discipline and production habits, not raw ability to build things.

---

## By area

| Area | Level | Notes |
|------|-------|-------|
| Getting something working | **Junior+** | End-to-end flow: API → Gemini → compendium → response |
| C# / ASP.NET basics | **Junior** | DI, controllers, services, HttpClient — used correctly |
| API design & structure | **Junior → early mid** | Clear layering; some inconsistencies hold you back |
| Attention to detail | **Junior** | Typos, unused code, interface mismatches |
| Production readiness | **Below junior bar** | No tests, weak errors, logging, lifecycle, docs drift |
| Problem-solving / initiative | **Junior+** | Real integration (AI + structured JSON + local dump) |

---

## What reads well (junior-or-better)

- **You ship a real vertical slice.** Not a tutorial — a working AI-backed API.
- **Architecture instincts are there.** Controllers → services → storage, interfaces, DI.
- **Modern .NET used reasonably.** HttpClient registration, JsonNode, OpenAPI/Scalar, nullable enabled.
- **Pragmatic product choices.** Template export, tag-based loot, compendium sync — sensible MVP thinking.

---

## What still feels junior

- **Inconsistency:** e.g. `ItemController` uses `IItemService`, `NpcController` injects concrete `NpcService`; interface return types don’t match implementation.
- **Rough edges:** naming typos (`IITemDataStorage`, `IGeneratePromts`), unused variables, dead DTOs.
- **Limited production mindset:** no tests, empty error responses, `Console.WriteLine` instead of logging, heavy data loaded per scoped request.
- **Config story unclear:** dev works via `appsettings.Development.json`; base config alone is not enough for other environments.

This does not mean “can’t code.” It means **can build features, not yet consistently build maintainable systems** — normal for junior level.

---

## One-line summary

> A junior who can actually build and wire things up. The next step is less “learn more syntax” and more tests, logging, naming, service lifetimes, and finishing small loose ends.

---

## Milestone to early mid-level

Pick **one service** and make it “production-shaped”:

1. Interface + implementation aligned  
2. Unit tests for core logic  
3. `ILogger` instead of `Console.WriteLine`  
4. Clear HTTP error messages  
5. Correct DI lifetime (e.g. Singleton for heavy data load)  
6. README matches reality  

Doing that once, then repeating the pattern, is the clearest path from **solid junior** to **early mid**.

---

## Context

Assessment based on code review of StoryTracker: build, run, endpoint tests, and static analysis. Intentionally deferred features (item Type matching, full export stat mapping) were excluded from the negative side of the evaluation.

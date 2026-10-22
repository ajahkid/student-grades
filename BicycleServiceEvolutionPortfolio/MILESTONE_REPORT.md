# Bicycle Service Milestone Report

This report records the project's development in order. It will be updated when each important step is tested and committed.

## Project Idea

The project tracks customers, their bicycles, and the service jobs requested for those bicycles. The idea stays the same in all five milestones; each version adds concepts from the course.

## Checkpoint 0: Project Setup

**Date:** 2026-10-20
**Version:** Portfolio structure and Milestone 1 scaffold
**What changed:** Selected Bicycle Service (#20), created five milestone folders, and created a .NET 10 console project for Milestone 1.
**Why:** The assignment requires inspectable versions and a consistent project idea.
**Concepts:** .NET console project, solution/portfolio layout.
**Verification:** The new console project built successfully.

## Checkpoint 1: Menu and Add Service Job

**Date:** 2026-10-20
**Version:** Milestone 1, early working step
**What changed:** Added a repeating menu, an Exit option, input prompts, and storage of a service job in a `List<string>`. Each job currently contains customer name, bicycle model, repair issue, and estimated cost.
**Why:** This makes the basic service workflow visible before adding more C# concepts.
**Concepts:** `bool`, `while`, `if/else`, `Console.ReadLine`, string interpolation, and `List<string>`.
**Current limitation:** The list exists only while the program runs. List/Find/Delete and JSON saving are not implemented yet.
**Verification:** The project built. A test run entered `Tina`, `Trek FX`, `Flat tire`, and `25` for the Add prompts; the app confirmed the job was added, returned to the menu, and exited cleanly after `0`.

## Checkpoint 2: List, Find, and Delete

**Date:** 2026-10-22
**Version:** Milestone 1, in-memory service job management
**What changed:** Added separate methods for adding, listing, finding, and deleting service jobs. The list displays numbered jobs, Find searches customer, bicycle, and issue text without case sensitivity, and Delete removes a selected job by its displayed number.
**Why:** These actions complete the basic service-job workflow before adding persistence.
**Concepts:** Methods, `List<string>`, `for`, `if`, `StringComparison`, and `int.TryParse`.
**Current limitation:** Jobs still disappear when the program closes. JSON loading and saving are the next Milestone 1 step.
**Verification:** The project built successfully. A scripted run added Tina's Trek FX flat-tire job, listed it, found it using lowercase `flat`, deleted job 1, confirmed the list was empty, and exited cleanly.

## Next Checkpoints

The next Milestone 1 checkpoint will add and test JSON loading and saving. Later entries will explain how the OOP, layered, database, and API versions evolve from this same workflow.
# Bicycle Service Milestone Report

This is a living report of my Bicycle Service project. It records what I built, the challenges I faced as a student, how I worked through them, and how I checked each milestone. I will add a reflection after completing and testing each milestone.

## Project Idea

The project tracks customers, their bicycles, and the service jobs requested for those bicycles. The idea stays the same in all five milestones; each version adds concepts from the course.

## Milestone 1 Reflection: Basic Console

### What I Built

For this milestone, I made a console program for a bicycle repair shop. It lets someone add a service job with a customer's name, bicycle model, repair issue, and estimated cost. They can also list, search for, and delete jobs. I added JSON saving so the jobs are still there the next time the program runs.

### Challenges I Faced and How I Overcame Them

- **Getting the menu options to work:** At first, the menu had options that were not connected to working actions yet. I added the actions one at a time and gave each one its own method. Testing them separately helped me keep track of what worked and what I still needed to finish.
- **Deleting the job I meant to delete:** The list counts positions starting at 0, but the menu shows job numbers starting at 1. I had to account for that difference. I also used `int.TryParse` and checked the number was valid before removing anything, so a typo would not crash the program.
- **Keeping jobs after closing the app:** My first version kept jobs only in a list, which meant they disappeared when the program stopped. I added JSON loading when the app starts and saving after a job is added or deleted. I checked this by closing and restarting the app, then checking that the job and its later deletion were both remembered.
- **Knowing how to test it properly:** A successful build only tells me that the code compiles. It does not prove the menu works as expected. I also ran through the main actions and tested the app again after restarting it.

### What I Learned

I learned that a larger program feels easier to manage when I build and test it in small steps. I also understand the difference between data held temporarily in a list and data saved in a file. Most importantly, I learned to check both that my code builds and that the program actually works when I use it.

### Milestone 1 Outcome

Milestone 1 is complete. The console app builds, its Add/List/Find/Delete workflow has been tested, and JSON persistence has been verified across restarts. The detailed implementation checkpoints and verification notes follow below.

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

## Checkpoint 3: JSON Persistence

**Date:** 2026-10-22
**Version:** Milestone 1, persistent service job storage
**What changed:** The app loads jobs from `service-jobs.json` at startup and saves the list after adding or deleting a job. If the existing JSON is malformed, the app leaves it untouched and disables saving to protect the file.
**Why:** Service records should remain available after the console app closes.
**Concepts:** `System.Text.Json`, file reading and writing, `File.Exists`, and exception handling.
**Current limitation:** The file stores the existing string representation of each job. Milestone 2 will replace it with `Customer`, `Bicycle`, and `ServiceJob` classes.
**Verification:** The project built successfully. A scripted run in a temporary working directory added a job, restarted and listed it, deleted it, then restarted again and confirmed the list was empty.

## Next Checkpoints

Milestone 1 is complete. The next checkpoint will begin Milestone 2 by modeling customers, bicycles, and service jobs as classes while retaining JSON persistence.

## Future Milestone Reflections

### Milestone 2: OOP and Persistence

_To be completed after implementation and testing._

### Milestone 3: Layered Multi-user Console

_To be completed after implementation and testing._

### Milestone 4: Database

_To be completed after implementation and testing._

### Milestone 5: Web API

_To be completed after implementation and testing._
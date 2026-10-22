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

## Checkpoint 4: OOP Models and Nested JSON

**Date:** 2026-10-22
**Version:** Milestone 2, object-oriented console application
**What changed:** Created a separate console project with `Customer`, `Bicycle`, and `ServiceJob` classes. Customers contain bicycles, and bicycles contain service jobs. The app supports Add/List/Find/Update/Delete and saves the nested objects to `service-customers.json`. Adding another job for the same customer and bicycle reuses those objects. Update lets the user change status and enter a final cost.
**Why:** This milestone practices representing the shop's real relationships with objects instead of storing each job as one formatted string.
**Concepts:** Classes, properties, constructors, object relationships, lists, decimal validation, and nested JSON serialization.
**Current limitation:** Jobs can be updated to Requested, In Progress, Completed, or Cancelled, but the app does not yet support editing customer or bicycle details.
**Verification:** The project built successfully. A scripted run added two jobs to the same customer and bicycle, found them, deleted one, restarted the app, and confirmed the remaining job was loaded from JSON. A separate run set a job to Completed, entered a final cost, restarted the app, and confirmed both values persisted.

## Milestone 2 Reflection: OOP and JSON Persistence

### What I Built

I made a second console app that keeps the same service-job actions as Milestone 1, but stores the information in three connected classes. A customer can have bicycles, and each bicycle can have several service jobs. The app saves those objects as nested JSON.

### Challenges I Faced and How I Overcame Them

- **Changing from one long string to related objects:** In Milestone 1, each job was stored as formatted text. This time, I had to decide which information belonged to the customer, bicycle, or service job. I used lists to represent the relationships: customers contain bicycles, and bicycles contain their service jobs.
- **Avoiding duplicate customer and bicycle records:** Adding a second job for the same customer could have created duplicate objects. I made the app look for a matching customer and bicycle first, then add the new job to the existing records.
- **Saving the new object structure:** The JSON format now has several levels instead of a simple list of strings. I tested it by adding multiple jobs for the same bicycle, restarting the program, and confirming the data loaded and a deletion stayed deleted.
- **Tracking a job through completion:** The model already had status and final-cost properties, but the menu did not let me change them. I added an update action with a short list of valid statuses and non-negative cost validation, then restarted the app to check the changes were saved.

### What I Learned

I learned how classes can represent parts of a real problem and how objects can be connected to each other. I also learned that a nested object model can make information easier to organize, but it means I need to test that the relationships survive saving and loading.

### Milestone 2 Outcome

Milestone 2 is complete. The separate app builds, supports Add/List/Find/Update/Delete, and saves related customer, bicycle, and service-job objects as JSON. Jobs start as `Requested` and can be updated with a status and final cost.

## Next Checkpoints

Milestones 1 and 2 are complete. The next checkpoint will begin Milestone 3 by separating the console presentation, service logic, and data access, then adding user registration and login.

## Future Milestone Reflections

### Milestone 3: Layered Multi-user Console

_To be completed after implementation and testing._

### Milestone 4: Database

_To be completed after implementation and testing._

### Milestone 5: Web API

_To be completed after implementation and testing._
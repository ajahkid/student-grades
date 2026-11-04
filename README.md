# Student Grade Calculator v0.2

An upgrade of v0.1. Same menu (options 1-4), plus:

- **Files.h / Files.cpp** – make, read, split and write student files.
- **StudentList.h / StudentList.cpp** – the container (`std::vector`) and sorting.
- **Timer.h** – a small stopwatch class.
- **Person.h / Person.cpp** – one student (same as v0.1).
- **Exceptions** – problems (missing file, bad row, bad input) are thrown with `throw` and caught with `try / catch` in `main.cpp`.

## New menu options

- **5** makes 5 files: `students_1000.txt` ... `students_10000000.txt` (`Name1 Surname1`, random scores).
- **6** runs the speed test. For each file it times: read, sort, split, write.
  Students with final grade < 5.0 go to `failed_N.txt`, the rest to `passed_N.txt`.

The 10 000 000 file needs a lot of memory and disk space and takes a while.

## Build

    make

or with CMake:

    cmake -S . -B build
    cmake --build build

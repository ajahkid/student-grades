# Student Grade Calculator v0.25

The same program as v0.2, but the list of students can be a `std::vector`, a `std::list` or a `std::deque`.
Only `StudentList.h` / `StudentList.cpp` know which one is used; the rest of the code does not change.

`make` builds three programs:

- `student-grades-vector`
- `student-grades-list`
- `student-grades-deque`

Run each one, choose **5** once (makes the data files) and then **6** (speed test) and compare the times.
Read, sort, split and write are timed for 1 000 up to 10 000 000 students.

Same files as v0.2: `Files.*` (make/read/split/write, with exceptions), `Timer.h`, `Person.*`.

## Build

    make

or with CMake:

    cmake -S . -B build
    cmake --build build

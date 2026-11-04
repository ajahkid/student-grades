# Student Grade Calculator v1.0

Final version. It reads a student file, sorts the students, splits them into
**passed** (final grade 5.0 or more) and **failed** (below 5.0), and measures how
fast that is for `std::vector`, `std::list` and `std::deque`.

## How to use

1. Build the program (from this folder):

       make

   or with CMake:

       cmake -S . -B build
       cmake --build build

2. Start it (`./student-grades`, or `student-grades.exe` on Windows).
3. Choose **1** once. It makes `students_1000.txt` ... `students_10000000.txt`
   (the biggest file needs a lot of memory and disk space).
4. Choose **2** to run one test: pick the container, file size, strategy and method.
   It writes `passed_N.txt` and `failed_N.txt` and prints read, sort, split and write times.
5. Choose **3** to compare all containers, strategies and methods for one file size.
6. Choose **0** to exit.

If a file is missing or has a bad row, the program prints `Problem: ...` and goes
back to the menu (exception handling).

## Strategies and methods

- **Strategy 1:** copy students into two new containers (`passed` and `failed`). The original keeps everyone.
- **Strategy 2:** one new container (`failed`). Failed students are moved out of the original, so only passed students stay. The original container is then made smaller (`shrink_to_fit`; `std::list` always uses only what it needs).
- **Plain loop:** the old way (a `for` / `while` loop).
- **Algorithms:** the same job with `<algorithm>` functions: `std::remove_copy_if`, `std::copy_if` (strategy 1) and `std::stable_partition` (strategy 2).

## Files

- `src/Person.h`, `src/Person.cpp` - one student and the final grade (average or median).
- `src/Files.h`, `src/Files.cpp` - makes the random student files.
- `src/Students.h` - reading, sorting, writing and both strategies for any container.
- `src/SpeedTest.h` - the timed tests.
- `src/Timer.h`, `src/Memory.h` - a stopwatch and a memory reader.
- `src/main.cpp` - the menu.

## Releases

- **v0.1** - console program: enter, random or file data; average and median.
- **v0.2** - classes in header files, exceptions, generated files, passed/failed split, timing with `std::vector`.
- **v0.25** - the same with `std::list` and `std::deque`, to compare containers.
- **v1.0** - two splitting strategies, plain loops compared with `<algorithm>` functions, a menu to choose.

## Results (100 000 students, split time in seconds, one run on my PC)

| Container | Strategy 1 loop | Strategy 1 algorithms | Strategy 2 loop | Strategy 2 algorithms |
|-----------|-----------------|-----------------------|-----------------|-----------------------|
| vector    | 0.036 | 0.055 | 24.853 | 0.030 |
| list      | 0.046 | 0.071 | 0.031  | 0.043 |
| deque     | 0.031 | 0.047 | 6.013  | 0.026 |

What the numbers show:

- Strategy 2 with a plain loop is very slow on `vector` and `deque`. Every `erase` in the middle shifts all the students after it. `list` does not have this problem, because removing from a list does not move anything.
- `std::stable_partition` fixes that: the same strategy 2 on `vector` goes from 24.8 s to 0.03 s, because it moves each student once.
- Strategy 1 is fast with all containers. The `<algorithm>` version is a little slower there, because it goes through the students twice (once for passed, once for failed).
- Strategy 2 uses less memory than strategy 1, because the students are not copied (measured below).
- **Fastest overall:** strategy 2 with `std::stable_partition` on `vector` or `deque` (about 0.03 s), and the plain loop on `list`.
- Reading the file (about 0.3-0.4 s) and sorting (about 0.05-0.08 s) take more time than the best splits.

## Memory: is strategy 1 really inefficient?

Yes. Test 1 000 000 students, algorithm method, one fresh program run per row
(menu option 2, then exit). Memory is what the program uses (Windows working set).
"Highest" is the peak since the program started, so it includes reading the file.

| Container | Strategy | In use before split | In use after split | Highest |
|-----------|----------|---------------------|--------------------|---------|
| vector | 1 | 112 MB | 220 MB | 220 MB |
| vector | 2 | 111 MB | 111 MB | 187 MB |
| list   | 1 | 142 MB | 280 MB | 280 MB |
| list   | 2 | 142 MB | 150 MB | 193 MB |
| deque  | 1 | 113 MB | 222 MB | 222 MB |
| deque  | 2 | 113 MB | 116 MB | 150 MB |

- Strategy 1 uses about **twice** the memory after the split (every student exists twice: in `students` and in `passed` or `failed`).
- Strategy 2 stays about the same, because the students are only moved between containers.
- The peak of strategy 2 is still higher than "in use", because reading the file and `stable_partition` need temporary memory.
- `list` uses the most memory: every student has two extra pointers.

Split time at 1 000 000 students (algorithm method): vector 0.61 s / 0.24 s, list 0.60 s / 0.38 s, deque 0.43 s / 0.22 s (strategy 1 / strategy 2).

Strategy 2 with a plain loop on `vector` and `deque` at 1 000 000 students and more was not tested; it would take hours.

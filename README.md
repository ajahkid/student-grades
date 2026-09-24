# Student Grade Calculator

Student Grade Calculator is a future-ready C++ console application for
students and teachers who need to collect homework and exam results and
compare final points. The program supports interactive entry, random test
data, variable-length homework lists, and import from `Students.txt`.

The `Person` class stores each student's first name, surname, homework scores,
exam score, and final grade. It also demonstrates the rule of three with an
explicit copy constructor, assignment operator, and destructor. Input and
output are implemented with overloaded `operator>>` and `operator<<`.

## Basic calculation formulas

The average calculation uses every homework result and the exam result:

```text
Final (Average) = (HW1 + HW2 + ... + HWn + Exam) / (n + 1)
```

The median calculation sorts the same collection of homework and exam scores.
For an even number of values, the two middle values are averaged.

All scores are expected to be in the range `0` to `10`. When entering data
manually, homework entry ends with `-1`, so the number of assignments does not
need to be known in advance.

## File format

`Students.txt` uses whitespace-separated columns. The last score on each row
is treated as the exam score; every score before it is treated as homework.

```text
Name        Surname    HW1   HW2   HW3   HW4   HW5   Exam
Name1       Surname1     8     9    10     6    10      9
Name2       Surname2     7    10     8     5     4      6
```

Students imported from the file are sorted by surname and then by first name.
The displayed table contains both `Final (Avg.)` and `Final (Med.)`, and the
selected calculation method is shown above the table.

## Build and run

With a compiler:

```bash
g++ -std=c++17 -Wall -Wextra -pedantic main.cpp -o student-grades
./student-grades
```

With CMake:

```bash
cmake -S . -B build
cmake --build build
./build/student-grades
```
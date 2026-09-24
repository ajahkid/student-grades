# Student Grade Calculator

Student Grade Calculator is a future-ready C++ console application for
collecting homework and exam results and presenting each student's final
point. The project will grow from interactive entry into a reusable student
results tool with multiple calculation methods, generated test data, and file
import.

## Basic calculation formula

For the average calculation, the final point is calculated from every
homework result and the exam result:

```text
Final (Average) = (HW1 + HW2 + ... + HWn + Exam) / (n + 1)
```

All scores are expected to be in the range `0` to `10`. Homework entry is
finished with `-1`, so the number of homework assignments does not need to be
known in advance.

## Current input format

```text
FirstName Surname HW1 HW2 ... HWn -1 Exam
```

Example:

```text
Alice Smith 8 9 10 6 -1 9
```

## Build and run

```bash
g++ -std=c++17 -Wall -Wextra -pedantic main.cpp -o student-grades
./student-grades
```
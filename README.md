# Student Grade Calculator v0.1

This C++17 console program calculates student grades using either the average
or median of homework and exam scores. You can enter student data, create
random data, or read the sample records from `Students.txt`.

## Source files

- `src/Person.h` stores the student information and names the two grade methods.
- `src/Person.cpp` works out the average or median grade.
- `src/main.cpp` shows the menu and handles student data.

## Build and run

With CMake:

```text
cmake -S . -B build
cmake --build build
```

With a C++17 compiler and Make:

```text
make
```

The executable is named `student-grades`. Run it from the project directory so
it can find `Students.txt`.

When entering a student by hand, type the first name, surname, homework
scores, `-1`, and then the exam score on one line. For example:

```text
Sam Lee 8 9 10 -1 7
```

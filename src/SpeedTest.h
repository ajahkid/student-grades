#ifndef SPEEDTEST_H
#define SPEEDTEST_H

#include "Person.h"
#include "Memory.h"
#include "Students.h"
#include "Timer.h"

#include <iomanip>
#include <iostream>
#include <string>

inline std::string fileNameFor(const std::string& prefix, int size) {
    return prefix + "_" + std::to_string(size) + ".txt";
}

// Reads, sorts, splits and writes ONE file with the chosen strategy and method.
// Prints how long each step took.
template <typename Container>
void runOneTest(const std::string& containerName, int size, int strategy,
                bool useAlgorithms) {
    Timer total;
    Timer step;

    Container students =
        readStudentsFromFile<Container>(fileNameFor("students", size));
    double readTime = step.seconds();

    step.reset();
    sortStudents(students);
    double sortTime = step.seconds();

    step.reset();
    Container passed;
    Container failed;
    double memoryBefore = currentMemoryMB();
    splitStudents(students, passed, failed, strategy, useAlgorithms);
    double splitTime = step.seconds();
    double memoryAfter = currentMemoryMB();

    // Strategy 2 leaves the passed students in "students".
    const Container& passedStudents = (strategy == 1) ? passed : students;

    step.reset();
    writeStudentsToFile(fileNameFor("passed", size), passedStudents);
    writeStudentsToFile(fileNameFor("failed", size), failed);
    double writeTime = step.seconds();

    std::cout << std::fixed << std::setprecision(3) << "\nContainer: "
              << containerName << ", strategy " << strategy << ", "
              << (useAlgorithms ? "algorithms" : "plain loop") << "\n"
              << "Students: " << size << "  (passed " << passedStudents.size()
              << ", failed " << failed.size() << ")\n"
              << "Read:  " << readTime << " s\n"
              << "Sort:  " << sortTime << " s\n"
              << "Split: " << splitTime << " s\n"
              << "Write: " << writeTime << " s\n"
              << "Total: " << total.seconds() << " s\n"
              << std::setprecision(1)
              << "Memory in use: " << memoryBefore << " MB before the split, "
              << memoryAfter << " MB after\n"
              << "Highest memory since the program started: " << peakMemoryMB()
              << " MB\n";
}

// Tests every strategy and method on one container. Only the split is timed here,
// because reading and sorting are the same for all of them.
template <typename Container>
void compareOneContainer(const std::string& containerName, int size) {
    Timer timer;
    Container students =
        readStudentsFromFile<Container>(fileNameFor("students", size));
    double readTime = timer.seconds();

    timer.reset();
    sortStudents(students);
    double sortTime = timer.seconds();

    std::cout << "\n" << containerName << ": read " << readTime << " s, sort "
              << sortTime << " s\n";

    for (int strategy = 1; strategy <= 2; ++strategy) {
        for (int method = 0; method <= 1; ++method) {
            bool useAlgorithms = (method == 1);

            // Strategy 2 changes its container, so every test gets a fresh copy.
            // The copying is not timed.
            Container copy = students;
            Container passed;
            Container failed;

            timer.reset();
            splitStudents(copy, passed, failed, strategy, useAlgorithms);
            double splitTime = timer.seconds();

            std::cout << "  strategy " << strategy << ", " << std::setw(10)
                      << std::left << (useAlgorithms ? "algorithms" : "plain loop")
                      << std::right << " split: " << splitTime << " s\n";
        }
    }
}

#endif

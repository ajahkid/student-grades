#ifndef STUDENTS_H
#define STUDENTS_H

// Everything here works with ANY container (vector, list or deque).
// "template <typename Container>" means: Container is a placeholder that is
// replaced by std::vector<Person>, std::list<Person> or std::deque<Person>.
// Templates must live in a header file, so there is no Students.cpp.

#include "Person.h"

#include <algorithm>
#include <fstream>
#include <iomanip>
#include <iterator>
#include <list>
#include <sstream>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

inline bool comesBefore(const Person& first, const Person& second) {
    if (first.surname != second.surname) {
        return first.surname < second.surname;
    }
    return first.firstName < second.firstName;
}

inline bool hasFailed(const Person& student) {
    return student.finalGrade(CalculationMethod::Average) < 5.0;
}

inline bool hasPassed(const Person& student) {
    return !hasFailed(student);
}

template <typename Container>
void sortStudents(Container& students) {
    std::sort(students.begin(), students.end(), comesBefore);
}

// std::list cannot use std::sort, so it gets its own version.
inline void sortStudents(std::list<Person>& students) {
    students.sort(comesBefore);
}

// Throws std::runtime_error if the file is missing or a row is bad.
template <typename Container>
Container readStudentsFromFile(const std::string& fileName) {
    std::ifstream file(fileName);
    if (!file) {
        throw std::runtime_error("Could not open " + fileName);
    }

    std::string line;
    std::getline(file, line); // Skip the column headings.

    Container students;
    while (std::getline(file, line)) {
        if (line.empty()) {
            continue;
        }

        std::stringstream row(line);
        Person student;
        if (!(row >> student.firstName >> student.surname)) {
            throw std::runtime_error("Could not read this row: " + line);
        }

        std::vector<int> scores;
        int score;
        while (row >> score) {
            if (score < 0 || score > 10) {
                throw std::runtime_error("Scores must be from 0 to 10: " + line);
            }
            scores.push_back(score);
        }

        if (!row.eof() || scores.size() < 2) {
            throw std::runtime_error(
                "This row needs at least one homework score and an exam: " + line);
        }

        student.exam = scores.back();
        scores.pop_back();
        student.homework = scores;
        students.push_back(student);
    }

    return students;
}

template <typename Container>
void writeStudentsToFile(const std::string& fileName, const Container& students) {
    std::ofstream file(fileName);
    if (!file) {
        throw std::runtime_error("Could not create " + fileName);
    }

    file << std::fixed << std::setprecision(2);
    file << "Name Surname Final(Avg.) Final(Med.)\n";
    for (const Person& student : students) {
        file << student.firstName << ' ' << student.surname << ' '
             << student.finalGrade(CalculationMethod::Average) << ' '
             << student.finalGrade(CalculationMethod::Median) << '\n';
    }
}

// ---------- Strategy 1: copy students into TWO new containers ----------
// "students" stays untouched.

// Version with a plain loop (the old way, like v0.25).
template <typename Container>
void strategy1WithLoop(const Container& students, Container& passed,
                       Container& failed) {
    for (const Person& student : students) {
        if (hasFailed(student)) {
            failed.push_back(student);
        } else {
            passed.push_back(student);
        }
    }
}

// Version with <algorithm> functions.
template <typename Container>
void strategy1WithAlgorithms(const Container& students, Container& passed,
                             Container& failed) {
    // remove_copy_if copies everything EXCEPT the failed ones.
    std::remove_copy_if(students.begin(), students.end(),
                        std::back_inserter(passed), hasFailed);
    // copy_if copies only the failed ones.
    std::copy_if(students.begin(), students.end(),
                 std::back_inserter(failed), hasFailed);
}

// ---------- Strategy 2: only ONE new container (failed) ----------
// Failed students are moved out of "students"; only passed ones stay in it.

// Version with a plain loop. erase() in the middle of a vector or deque is slow.
template <typename Container>
void strategy2WithLoop(Container& students, Container& failed) {
    typename Container::iterator it = students.begin();
    while (it != students.end()) {
        if (hasFailed(*it)) {
            failed.push_back(*it);
            it = students.erase(it);
        } else {
            ++it;
        }
    }
}

// Version with <algorithm> functions.
template <typename Container>
void strategy2WithAlgorithms(Container& students, Container& failed) {
    // stable_partition puts the passed students first and the failed ones last,
    // keeping the sorted order. It returns where the failed students start.
    typename Container::iterator firstFailed =
        std::stable_partition(students.begin(), students.end(), hasPassed);

    failed.assign(std::make_move_iterator(firstFailed),
                  std::make_move_iterator(students.end()));
    students.erase(firstFailed, students.end());
}

// Makes the container's memory as small as its content.
// Needed after strategy 2, because erase() does not give memory back.
template <typename Container>
void shrinkContainer(Container& students) {
    students.shrink_to_fit();
}

// std::list has no shrink_to_fit: it already uses only the memory it needs.
inline void shrinkContainer(std::list<Person>&) {
}

// Splits "students" with the chosen strategy (1 or 2).
// Strategy 1: "passed" and "failed" are filled, "students" keeps everyone.
// Strategy 2: "failed" is filled, and only the passed students remain in
// "students" (it is made smaller). "passed" is not used.
template <typename Container>
void splitStudents(Container& students, Container& passed, Container& failed,
                   int strategy, bool useAlgorithms) {
    if (strategy == 1) {
        if (useAlgorithms) {
            strategy1WithAlgorithms(students, passed, failed);
        } else {
            strategy1WithLoop(students, passed, failed);
        }
    } else {
        if (useAlgorithms) {
            strategy2WithAlgorithms(students, failed);
        } else {
            strategy2WithLoop(students, failed);
        }
        shrinkContainer(students);
    }
}

#endif

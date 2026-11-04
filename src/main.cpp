#include "Person.h"

#include <algorithm>
#include <fstream>
#include <iostream>
#include <random>
#include <sstream>
#include <string>
#include <vector>

bool comesBefore(const Person& first, const Person& second) {
    if (first.surname != second.surname) {
        return first.surname < second.surname;
    }
    return first.firstName < second.firstName;
}

void sortStudents(std::vector<Person>& students) {
    std::sort(students.begin(), students.end(), comesBefore);
}

void printStudents(const std::vector<Person>& students,
                   CalculationMethod method) {
    if (method == CalculationMethod::Average) {
        std::cout << "\nShowing grades with the average method.\n";
    } else {
        std::cout << "\nShowing grades with the median method.\n";
    }

    std::cout << "\nName          Surname        Final (Avg.) | Final (Med.)\n"
              << "------------------------------------------------------------\n";

    for (const Person& student : students) {
        std::cout << student.firstName << "  " << student.surname << "  "
                  << student.finalGrade(CalculationMethod::Average) << "  |  "
                  << student.finalGrade(CalculationMethod::Median) << '\n';
    }
}

bool readStudentFromConsole(Person& student) {
    std::string line;
    std::getline(std::cin >> std::ws, line);

    std::stringstream input(line);
    if (!(input >> student.firstName >> student.surname)) {
        std::cout << "Please enter the student's first and last names.\n";
        return false;
    }

    int score;
    bool foundEndMarker = false;

    while (input >> score) {
        if (score == -1) {
            foundEndMarker = true;
            break;
        }

        if (score < 0 || score > 10) {
            std::cout << "Scores must be from 0 to 10.\n";
            return false;
        }

        student.homework.push_back(score);
    }

    if (!foundEndMarker || !(input >> student.exam)) {
        std::cout << "End the homework scores with -1, then enter the exam score.\n";
        return false;
    }

    if (student.homework.empty() || student.exam < 0 || student.exam > 10) {
        std::cout << "Enter at least one homework score and an exam score from 0 to 10.\n";
        return false;
    }

    return true;
}

std::vector<Person> readStudentsFromConsole() {
    int studentCount;
    std::cout << "Number of students: ";

    if (!(std::cin >> studentCount) || studentCount < 1) {
        std::cout << "Enter a number greater than zero.\n";
        return {};
    }

    std::vector<Person> students;
    std::cout << "For each student, enter: FirstName Surname HW1 HW2 ... -1 Exam\n";

    for (int i = 0; i < studentCount; ++i) {
        Person student;
        std::cout << "Student " << i + 1 << ": ";

        if (!readStudentFromConsole(student)) {
            return {};
        }

        students.push_back(student);
    }

    sortStudents(students);
    return students;
}

std::vector<Person> readStudentsFromFile(const std::string& fileName) {
    std::ifstream file(fileName);
    if (!file) {
        std::cout << "Could not open " << fileName << ".\n";
        return {};
    }

    std::string line;
    std::getline(file, line); // Skip the column headings.

    std::vector<Person> students;
    while (std::getline(file, line)) {
        if (line.empty()) {
            continue;
        }

        std::stringstream row(line);
        Person student;
        if (!(row >> student.firstName >> student.surname)) {
            std::cout << "Could not read this row: " << line << '\n';
            return {};
        }

        std::vector<int> scores;
        int score;
        while (row >> score) {
            if (score < 0 || score > 10) {
                std::cout << "Scores in Students.txt must be from 0 to 10.\n";
                return {};
            }
            scores.push_back(score);
        }

        if (!row.eof() || scores.size() < 2) {
            std::cout << "This row needs at least one homework score and an exam: "
                      << line << '\n';
            return {};
        }

        student.exam = scores.back();
        scores.pop_back();
        student.homework = scores;
        students.push_back(student);
    }

    sortStudents(students);
    return students;
}

std::vector<Person> makeRandomStudents() {
    int studentCount;
    int homeworkCount;

    std::cout << "Number of students: ";
    if (!(std::cin >> studentCount) || studentCount < 1) {
        std::cout << "Enter a number greater than zero.\n";
        return {};
    }

    std::cout << "Homework assignments per student: ";
    if (!(std::cin >> homeworkCount) || homeworkCount < 1) {
        std::cout << "Enter a number greater than zero.\n";
        return {};
    }

    std::random_device randomDevice;
    std::mt19937 randomGenerator(randomDevice());
    std::uniform_int_distribution<int> scorePicker(0, 10);

    std::vector<Person> students;
    for (int i = 0; i < studentCount; ++i) {
        Person student;
        student.firstName = "Student" + std::to_string(i + 1);
        student.surname = "Generated" + std::to_string(i + 1);

        for (int homework = 0; homework < homeworkCount; ++homework) {
            student.homework.push_back(scorePicker(randomGenerator));
        }

        student.exam = scorePicker(randomGenerator);
        students.push_back(student);
    }

    sortStudents(students);
    return students;
}

int main() {
    std::vector<Person> students;

    while (true) {
        std::cout << "\nStudent Grade Calculator\n"
                  << "1. Enter student data\n"
                  << "2. Make random student data\n"
                  << "3. Read data from Students.txt\n"
                  << "4. Display grades\n"
                  << "0. Exit\n"
                  << "Choose: ";

        int choice;
        if (!(std::cin >> choice)) {
            std::cout << "Please enter a menu number.\n";
            return 1;
        }

        if (choice == 0) {
            return 0;
        }

        if (choice == 1) {
            students = readStudentsFromConsole();
        } else if (choice == 2) {
            students = makeRandomStudents();
        } else if (choice == 3) {
            students = readStudentsFromFile("Students.txt");
            if (!students.empty()) {
                std::cout << students.size() << " students loaded.\n";
            }
        } else if (choice == 4) {
            if (students.empty()) {
                std::cout << "There are no students to show yet.\n";
                continue;
            }

            std::cout << "1. Average\n2. Median\nChoose: ";
            int methodChoice;
            if (!(std::cin >> methodChoice) ||
                (methodChoice != 1 && methodChoice != 2)) {
                std::cout << "Choose 1 for average or 2 for median.\n";
                return 1;
            }

            CalculationMethod method = CalculationMethod::Average;
            if (methodChoice == 2) {
                method = CalculationMethod::Median;
            }
            printStudents(students, method);
        } else {
            std::cout << "That is not a menu option. Try again.\n";
        }
    }
}

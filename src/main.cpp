#include "Files.h"
#include "Person.h"
#include "StudentList.h"
#include "Timer.h"

#include <iomanip>
#include <iostream>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>

const int fileSizes[5] = {1000, 10000, 100000, 1000000, 10000000};

void printStudents(const StudentList& students, CalculationMethod method) {
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

void readStudentFromConsole(Person& student) {
    std::string line;
    std::getline(std::cin >> std::ws, line);

    std::stringstream input(line);
    if (!(input >> student.firstName >> student.surname)) {
        throw std::runtime_error("Please enter the student's first and last names.");
    }

    int score;
    bool foundEndMarker = false;

    while (input >> score) {
        if (score == -1) {
            foundEndMarker = true;
            break;
        }

        if (score < 0 || score > 10) {
            throw std::runtime_error("Scores must be from 0 to 10.");
        }

        student.homework.push_back(score);
    }

    if (!foundEndMarker || !(input >> student.exam)) {
        throw std::runtime_error(
            "End the homework scores with -1, then enter the exam score.");
    }

    if (student.homework.empty() || student.exam < 0 || student.exam > 10) {
        throw std::runtime_error(
            "Enter at least one homework score and an exam score from 0 to 10.");
    }
}

StudentList readStudentsFromConsole() {
    int studentCount;
    std::cout << "Number of students: ";

    if (!(std::cin >> studentCount) || studentCount < 1) {
        throw std::runtime_error("Enter a number greater than zero.");
    }

    StudentList students;
    std::cout << "For each student, enter: FirstName Surname HW1 HW2 ... -1 Exam\n";

    for (int i = 0; i < studentCount; ++i) {
        Person student;
        std::cout << "Student " << i + 1 << ": ";
        readStudentFromConsole(student);
        students.push_back(student);
    }

    sortStudents(students);
    return students;
}

StudentList makeRandomStudents() {
    int studentCount;
    int homeworkCount;

    std::cout << "Number of students: ";
    if (!(std::cin >> studentCount) || studentCount < 1) {
        throw std::runtime_error("Enter a number greater than zero.");
    }

    std::cout << "Homework assignments per student: ";
    if (!(std::cin >> homeworkCount) || homeworkCount < 1) {
        throw std::runtime_error("Enter a number greater than zero.");
    }

    std::random_device randomDevice;
    std::mt19937 randomGenerator(randomDevice());
    std::uniform_int_distribution<int> scorePicker(0, 10);

    StudentList students;
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

void makeAllFiles() {
    for (int size : fileSizes) {
        std::string fileName = "students_" + std::to_string(size) + ".txt";
        Timer timer;
        generateStudentFile(fileName, size);
        std::cout << fileName << " made in " << timer.seconds() << " s\n";
    }
}

// Does the whole job for one file and prints how long each step took.
void testOneFile(int size) {
    std::string number = std::to_string(size);

    Timer total;
    Timer step;

    StudentList students = readStudentsFromFile("students_" + number + ".txt");
    double readTime = step.seconds();

    step.reset();
    sortStudents(students);
    double sortTime = step.seconds();

    step.reset();
    StudentList passed;
    StudentList failed;
    splitStudents(students, passed, failed);
    double splitTime = step.seconds();

    step.reset();
    writeStudentsToFile("passed_" + number + ".txt", passed);
    writeStudentsToFile("failed_" + number + ".txt", failed);
    double writeTime = step.seconds();

    std::cout << std::fixed << std::setprecision(3)
              << std::setw(10) << size
              << std::setw(10) << readTime
              << std::setw(10) << sortTime
              << std::setw(10) << splitTime
              << std::setw(10) << writeTime
              << std::setw(10) << total.seconds() << '\n';
}

void runSpeedTest() {
    std::cout << "\nContainer: " << CONTAINER_NAME << " (times in seconds)\n"
              << std::setw(10) << "Students"
              << std::setw(10) << "Read"
              << std::setw(10) << "Sort"
              << std::setw(10) << "Split"
              << std::setw(10) << "Write"
              << std::setw(10) << "Total" << '\n';

    for (int size : fileSizes) {
        try {
            testOneFile(size);
        } catch (const std::exception& problem) {
            std::cout << std::setw(10) << size << "  Problem: " << problem.what()
                      << "\n";
        }
    }
}

int main() {
    StudentList students;

    while (true) {
        std::cout << "\nStudent Grade Calculator (" << CONTAINER_NAME << ")\n"
                  << "1. Enter student data\n"
                  << "2. Make random student data\n"
                  << "3. Read data from Students.txt\n"
                  << "4. Display grades\n"
                  << "5. Make the 5 student files (1 000 to 10 000 000 students)\n"
                  << "6. Run the speed test\n"
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

        try {
            if (choice == 1) {
                students = readStudentsFromConsole();
            } else if (choice == 2) {
                students = makeRandomStudents();
            } else if (choice == 3) {
                students = readStudentsFromFile("Students.txt");
                sortStudents(students);
                std::cout << students.size() << " students loaded.\n";
            } else if (choice == 4) {
                if (students.empty()) {
                    throw std::runtime_error("There are no students to show yet.");
                }

                std::cout << "1. Average\n2. Median\nChoose: ";
                int methodChoice;
                if (!(std::cin >> methodChoice) ||
                    (methodChoice != 1 && methodChoice != 2)) {
                    return 1;
                }

                CalculationMethod method = CalculationMethod::Average;
                if (methodChoice == 2) {
                    method = CalculationMethod::Median;
                }
                printStudents(students, method);
            } else if (choice == 5) {
                makeAllFiles();
            } else if (choice == 6) {
                runSpeedTest();
            } else {
                std::cout << "That is not a menu option. Try again.\n";
            }
        } catch (const std::exception& problem) {
            std::cout << "Problem: " << problem.what() << '\n';
        }
    }
}

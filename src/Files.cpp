#include "Files.h"

#include <fstream>
#include <iomanip>
#include <random>
#include <sstream>
#include <stdexcept>
#include <utility>

void generateStudentFile(const std::string& fileName, int studentCount) {
    std::ofstream file(fileName);
    if (!file) {
        throw std::runtime_error("Could not create " + fileName);
    }

    std::random_device randomDevice;
    std::mt19937 randomGenerator(randomDevice());
    std::uniform_int_distribution<int> scorePicker(0, 10);

    file << "Name Surname HW1 HW2 HW3 HW4 HW5 Exam\n";
    for (int i = 1; i <= studentCount; ++i) {
        file << "Name" << i << " Surname" << i;
        for (int homework = 0; homework < 5; ++homework) {
            file << ' ' << scorePicker(randomGenerator);
        }
        file << ' ' << scorePicker(randomGenerator) << '\n';
    }
}

StudentList readStudentsFromFile(const std::string& fileName) {
    std::ifstream file(fileName);
    if (!file) {
        throw std::runtime_error("Could not open " + fileName);
    }

    std::string line;
    std::getline(file, line); // Skip the column headings.

    StudentList students;
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

void splitStudents(StudentList& all, StudentList& passed, StudentList& failed) {
    for (Person& student : all) {
        // std::move hands the student over instead of copying it. It saves memory.
        if (student.finalGrade(CalculationMethod::Average) < 5.0) {
            failed.push_back(std::move(student));
        } else {
            passed.push_back(std::move(student));
        }
    }
    all.clear();
}

void writeStudentsToFile(const std::string& fileName, const StudentList& students) {
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

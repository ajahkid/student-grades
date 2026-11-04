#include "Files.h"
#include "Person.h"
#include "SpeedTest.h"
#include "Timer.h"

#include <deque>
#include <iomanip>
#include <iostream>
#include <list>
#include <stdexcept>
#include <string>
#include <vector>

const int fileSizes[5] = {1000, 10000, 100000, 1000000, 10000000};

// Asks for a whole number between low and high. Throws an error if it is bad.
int askNumber(const std::string& question, int low, int high) {
    std::cout << question;
    int answer;
    if (!(std::cin >> answer) || answer < low || answer > high) {
        std::cin.clear();
        std::cin.ignore(10000, '\n');
        throw std::runtime_error("Please enter a number from " +
                                 std::to_string(low) + " to " +
                                 std::to_string(high) + ".");
    }
    return answer;
}

int askFileSize() {
    std::cout << "File size:\n";
    for (int i = 0; i < 5; ++i) {
        std::cout << "  " << i + 1 << ". " << fileSizes[i] << " students\n";
    }
    return fileSizes[askNumber("Choose: ", 1, 5) - 1];
}

void makeAllFiles() {
    for (int size : fileSizes) {
        std::string fileName = fileNameFor("students", size);
        Timer timer;
        generateStudentFile(fileName, size);
        std::cout << fileName << " made in " << timer.seconds() << " s\n";
    }
}

void runOne() {
    int container = askNumber("Container (1 = vector, 2 = list, 3 = deque): ", 1, 3);
    int size = askFileSize();
    int strategy = askNumber("Strategy (1 = two new containers, 2 = one new container): ", 1, 2);
    int method = askNumber("Method (1 = plain loop, 2 = <algorithm> functions): ", 1, 2);
    bool useAlgorithms = (method == 2);

    if (container == 1) {
        runOneTest<std::vector<Person> >("vector", size, strategy, useAlgorithms);
    } else if (container == 2) {
        runOneTest<std::list<Person> >("list", size, strategy, useAlgorithms);
    } else {
        runOneTest<std::deque<Person> >("deque", size, strategy, useAlgorithms);
    }
}

void compareAll() {
    int size = askFileSize();
    if (size >= 1000000) {
        std::cout << "Warning: strategy 2 with a plain loop on vector and deque "
                     "can take hours at this size. Be patient or press Ctrl+C.\n";
    }
    std::cout << std::fixed << std::setprecision(3)
              << "\nSplit times for " << size << " students:\n";

    compareOneContainer<std::vector<Person> >("vector", size);
    compareOneContainer<std::list<Person> >("list", size);
    compareOneContainer<std::deque<Person> >("deque", size);
}

int main() {
    while (true) {
        std::cout << "\nStudent Grade Calculator v1.0\n"
                  << "1. Make the 5 student files\n"
                  << "2. Run one test (choose container, strategy and method)\n"
                  << "3. Compare everything for one file size\n"
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
                makeAllFiles();
            } else if (choice == 2) {
                runOne();
            } else if (choice == 3) {
                compareAll();
            } else {
                std::cout << "That is not a menu option. Try again.\n";
            }
        } catch (const std::exception& problem) {
            std::cout << "Problem: " << problem.what() << '\n';
        }
    }
}

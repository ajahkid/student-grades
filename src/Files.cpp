#include "Files.h"

#include <fstream>
#include <random>
#include <stdexcept>

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

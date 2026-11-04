#ifndef PERSON_H
#define PERSON_H

#include <string>
#include <vector>

enum class CalculationMethod {
    Average,
    Median
};

class Person {
public:
    std::string firstName;
    std::string surname;
    std::vector<int> homework;
    int exam = 0;

    double finalGrade(CalculationMethod method) const;
};

#endif

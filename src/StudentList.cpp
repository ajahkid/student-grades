#include "StudentList.h"

#include <algorithm>

bool comesBefore(const Person& first, const Person& second) {
    if (first.surname != second.surname) {
        return first.surname < second.surname;
    }
    return first.firstName < second.firstName;
}

void sortStudents(StudentList& students) {
    std::sort(students.begin(), students.end(), comesBefore);
}

#include "StudentList.h"

#include <algorithm>

bool comesBefore(const Person& first, const Person& second) {
    if (first.surname != second.surname) {
        return first.surname < second.surname;
    }
    return first.firstName < second.firstName;
}

void sortStudents(StudentList& students) {
#ifdef USE_LIST
    // std::list cannot use std::sort, it has its own sort.
    students.sort(comesBefore);
#else
    std::sort(students.begin(), students.end(), comesBefore);
#endif
}

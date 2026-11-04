#ifndef STUDENTLIST_H
#define STUDENTLIST_H

#include "Person.h"

#include <vector>

// In v0.2 the list of students is a std::vector.
typedef std::vector<Person> StudentList;
#define CONTAINER_NAME "vector"

void sortStudents(StudentList& students);

#endif

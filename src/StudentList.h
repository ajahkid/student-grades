#ifndef STUDENTLIST_H
#define STUDENTLIST_H

#include "Person.h"

// The container is chosen when compiling:
//   -DUSE_LIST  -> std::list
//   -DUSE_DEQUE -> std::deque
//   nothing     -> std::vector
#if defined(USE_LIST)
#include <list>
typedef std::list<Person> StudentList;
#define CONTAINER_NAME "list"
#elif defined(USE_DEQUE)
#include <deque>
typedef std::deque<Person> StudentList;
#define CONTAINER_NAME "deque"
#else
#include <vector>
typedef std::vector<Person> StudentList;
#define CONTAINER_NAME "vector"
#endif

void sortStudents(StudentList& students);

#endif

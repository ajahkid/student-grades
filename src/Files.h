#ifndef FILES_H
#define FILES_H

#include "StudentList.h"

#include <string>

// Every function here throws std::runtime_error if something goes wrong.

// Makes a file with names like Name1 Surname1 and random scores.
void generateStudentFile(const std::string& fileName, int studentCount);

StudentList readStudentsFromFile(const std::string& fileName);

// Puts students with a final grade below 5.0 in failed, the rest in passed.
void splitStudents(StudentList& all, StudentList& passed, StudentList& failed);

void writeStudentsToFile(const std::string& fileName, const StudentList& students);

#endif

#ifndef FILES_H
#define FILES_H

#include <string>

// Makes a file with names like Name1 Surname1 and random scores.
// Throws std::runtime_error if the file cannot be created.
void generateStudentFile(const std::string& fileName, int studentCount);

#endif

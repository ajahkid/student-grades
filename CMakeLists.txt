cmake_minimum_required(VERSION 3.16)
project(student_grades VERSION 0.2 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if (NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

add_executable(student-grades
    src/main.cpp src/Person.cpp src/StudentList.cpp src/Files.cpp)

if (MSVC)
    target_compile_options(student-grades PRIVATE /W4)
else()
    target_compile_options(student-grades PRIVATE -Wall -Wextra -pedantic)
endif()

configure_file(Students.txt Students.txt COPYONLY)

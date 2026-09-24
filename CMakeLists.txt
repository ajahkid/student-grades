cmake_minimum_required(VERSION 3.16)
project(student_grades LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

add_executable(student-grades main.cpp)

if (MSVC)
    target_compile_options(student-grades PRIVATE /W4)
else()
    target_compile_options(student-grades PRIVATE -Wall -Wextra -pedantic)
endif()

configure_file(Students.txt Students.txt COPYONLY)
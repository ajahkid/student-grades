cmake_minimum_required(VERSION 3.16)
project(student_grades VERSION 0.25 LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

if (NOT CMAKE_BUILD_TYPE)
    set(CMAKE_BUILD_TYPE Release)
endif()

set(SOURCES src/main.cpp src/Person.cpp src/StudentList.cpp src/Files.cpp)

add_executable(student-grades-vector ${SOURCES})

add_executable(student-grades-list ${SOURCES})
target_compile_definitions(student-grades-list PRIVATE USE_LIST)

add_executable(student-grades-deque ${SOURCES})
target_compile_definitions(student-grades-deque PRIVATE USE_DEQUE)

foreach(target student-grades-vector student-grades-list student-grades-deque)
    if (MSVC)
        target_compile_options(${target} PRIVATE /W4)
    else()
        target_compile_options(${target} PRIVATE -Wall -Wextra -pedantic)
    endif()
endforeach()

configure_file(Students.txt Students.txt COPYONLY)

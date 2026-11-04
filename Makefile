CXX ?= g++
CXXFLAGS ?= -std=c++17 -O2 -Wall -Wextra -pedantic
FILES = src/main.cpp src/Person.cpp src/StudentList.cpp src/Files.cpp
HEADERS = src/Person.h src/StudentList.h src/Files.h src/Timer.h

all: student-grades-vector student-grades-list student-grades-deque

student-grades-vector: $(FILES) $(HEADERS)
	$(CXX) $(CXXFLAGS) $(FILES) -o student-grades-vector

student-grades-list: $(FILES) $(HEADERS)
	$(CXX) $(CXXFLAGS) -DUSE_LIST $(FILES) -o student-grades-list

student-grades-deque: $(FILES) $(HEADERS)
	$(CXX) $(CXXFLAGS) -DUSE_DEQUE $(FILES) -o student-grades-deque

.PHONY: all clean
clean:
	rm -f student-grades-vector student-grades-list student-grades-deque students_*.txt passed_*.txt failed_*.txt

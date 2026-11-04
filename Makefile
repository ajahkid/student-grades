CXX ?= g++
CXXFLAGS ?= -std=c++17 -O2 -Wall -Wextra -pedantic
FILES = src/main.cpp src/Person.cpp src/StudentList.cpp src/Files.cpp

student-grades: $(FILES) src/Person.h src/StudentList.h src/Files.h src/Timer.h
	$(CXX) $(CXXFLAGS) $(FILES) -o student-grades

.PHONY: clean
clean:
	rm -f student-grades students_*.txt passed_*.txt failed_*.txt

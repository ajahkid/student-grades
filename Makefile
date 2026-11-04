CXX ?= g++
CXXFLAGS ?= -std=c++17 -Wall -Wextra -pedantic

student-grades: src/main.cpp src/Person.cpp src/Person.h
	$(CXX) $(CXXFLAGS) src/main.cpp src/Person.cpp -o student-grades

.PHONY: clean
clean:
	rm -f student-grades

CXX ?= g++
CXXFLAGS ?= -std=c++17 -O2 -Wall -Wextra -pedantic
FILES = src/main.cpp src/Person.cpp src/Files.cpp
ifeq ($(OS),Windows_NT)
LIBS = -lpsapi
endif

student-grades: $(FILES) src/Person.h src/Files.h src/Students.h src/SpeedTest.h src/Timer.h src/Memory.h
	$(CXX) $(CXXFLAGS) $(FILES) $(LIBS) -o student-grades

.PHONY: clean
clean:
	rm -f student-grades students_*.txt passed_*.txt failed_*.txt

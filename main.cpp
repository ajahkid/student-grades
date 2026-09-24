#include <algorithm>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

enum class CalculationMethod {
    Average,
    Median,
};

class Person {
public:
    Person() = default;

    Person(std::string firstName, std::string surname,
           std::vector<int> homework, int exam)
        : firstName_(std::move(firstName)),
          surname_(std::move(surname)),
          homework_(std::move(homework)),
          exam_(exam) {
        calculateFinalGrade(CalculationMethod::Average);
    }

    // Rule of three: copy constructor, assignment operator, and destructor.
    Person(const Person& other)
        : firstName_(other.firstName_),
          surname_(other.surname_),
          homework_(other.homework_),
          exam_(other.exam_),
          finalGrade_(other.finalGrade_),
          selectedMethod_(other.selectedMethod_) {}

    Person& operator=(const Person& other) {
        if (this != &other) {
            firstName_ = other.firstName_;
            surname_ = other.surname_;
            homework_ = other.homework_;
            exam_ = other.exam_;
            finalGrade_ = other.finalGrade_;
            selectedMethod_ = other.selectedMethod_;
        }
        return *this;
    }

    ~Person() = default;

    void calculateFinalGrade(CalculationMethod method) {
        finalGrade_ = finalGrade(method);
        selectedMethod_ = method;
    }

    [[nodiscard]] double finalGrade(CalculationMethod method) const {
        std::vector<int> results = homework_;
        results.push_back(exam_);

        if (method == CalculationMethod::Average) {
            double total = 0.0;
            for (const int score : results) {
                total += score;
            }
            return total / static_cast<double>(results.size());
        }

        std::sort(results.begin(), results.end());
        const std::size_t middle = results.size() / 2;
        if (results.size() % 2 == 0) {
            return (results[middle - 1] + results[middle]) / 2.0;
        }
        return results[middle];
    }

    static Person randomPerson(const std::string& firstName,
                               const std::string& surname,
                               std::size_t homeworkCount,
                               std::mt19937& generator) {
        std::uniform_int_distribution<int> scoreDistribution(0, 10);
        std::vector<int> homework;
        homework.reserve(homeworkCount);
        for (std::size_t index = 0; index < homeworkCount; ++index) {
            homework.push_back(scoreDistribution(generator));
        }
        return Person(firstName, surname, homework,
                      scoreDistribution(generator));
    }

    [[nodiscard]] const std::string& firstName() const { return firstName_; }
    [[nodiscard]] const std::string& surname() const { return surname_; }

    friend std::istream& operator>>(std::istream& in, Person& person);
    friend std::ostream& operator<<(std::ostream& out, const Person& person);

private:
    std::string firstName_;
    std::string surname_;
    std::vector<int> homework_;
    int exam_ = 0;
    double finalGrade_ = 0.0;
    CalculationMethod selectedMethod_ = CalculationMethod::Average;
};

std::istream& operator>>(std::istream& in, Person& person) {
    std::string line;
    if (!std::getline(in >> std::ws, line)) {
        return in;
    }

    std::istringstream row(line);
    Person parsed;
    row >> parsed.firstName_ >> parsed.surname_;
    if (row.fail()) {
        in.setstate(std::ios::failbit);
        return in;
    }

    int score = 0;
    bool homeworkEnded = false;
    while (row >> score) {
        if (score == -1) {
            homeworkEnded = true;
            break;
        }
        if (score < 0 || score > 10) {
            in.setstate(std::ios::failbit);
            return in;
        }
        parsed.homework_.push_back(score);
    }

    if (!homeworkEnded || !(row >> parsed.exam_) ||
        parsed.exam_ < 0 || parsed.exam_ > 10) {
        in.setstate(std::ios::failbit);
        return in;
    }

    parsed.calculateFinalGrade(CalculationMethod::Average);
    person = parsed;
    return in;
}

std::ostream& operator<<(std::ostream& out, const Person& person) {
    out << std::left << std::setw(14) << person.firstName_
        << std::setw(14) << person.surname_ << std::right << std::setw(13)
        << std::fixed << std::setprecision(2)
        << person.finalGrade(CalculationMethod::Average) << " | "
        << std::setw(12) << person.finalGrade(CalculationMethod::Median);
    return out;
}

std::string methodName(CalculationMethod method) {
    return method == CalculationMethod::Average ? "Average" : "Median";
}

void sortStudents(std::vector<Person>& students) {
    std::sort(students.begin(), students.end(),
              [](const Person& left, const Person& right) {
                  if (left.surname() != right.surname()) {
                      return left.surname() < right.surname();
                  }
                  return left.firstName() < right.firstName();
              });
}

void printStudents(const std::vector<Person>& students,
                   CalculationMethod selectedMethod) {
    std::cout << "\nSelected final grade calculation: "
              << methodName(selectedMethod) << "\n\n"
              << "Name          Surname        Final (Avg.) | Final (Med.)\n"
              << "------------------------------------------------------------\n";
    for (const Person& student : students) {
        std::cout << student << '\n';
    }
}

std::vector<Person> readStudentsFromConsole() {
    std::size_t studentCount = 0;
    std::cout << "Number of students: ";
    if (!(std::cin >> studentCount)) {
        throw std::runtime_error("Invalid number of students.");
    }

    std::vector<Person> students;
    students.reserve(studentCount);
    std::cout << "Enter: FirstName Surname HW1 ... HWn -1 Exam\n";
    for (std::size_t index = 0; index < studentCount; ++index) {
        std::cout << "Student " << index + 1 << ": ";
        Person student;
        if (!(std::cin >> student)) {
            throw std::runtime_error("Invalid student data.");
        }
        students.push_back(student);
    }
    sortStudents(students);
    return students;
}

std::vector<Person> readStudentsFromFile(const std::string& fileName) {
    std::ifstream input(fileName);
    if (!input) {
        throw std::runtime_error("Could not open " + fileName + ".");
    }

    std::string line;
    std::getline(input, line);  // Skip the column header.
    std::vector<Person> students;

    while (std::getline(input, line)) {
        if (line.empty()) {
            continue;
        }

        std::istringstream row(line);
        std::string firstName;
        std::string surname;
        row >> firstName >> surname;
        if (firstName.empty() || surname.empty()) {
            throw std::runtime_error("Invalid row in " + fileName + ": " +
                                     line);
        }

        std::vector<int> scores;
        int score = 0;
        while (row >> score) {
            if (score < 0 || score > 10) {
                throw std::runtime_error("Score outside 0..10 in " + fileName +
                                         ": " + line);
            }
            scores.push_back(score);
        }

        if (scores.size() < 2) {
            throw std::runtime_error(
                "Each file row needs at least one homework score and an exam.");
        }

        const int exam = scores.back();
        scores.pop_back();
        students.emplace_back(firstName, surname, scores, exam);
    }

    sortStudents(students);
    return students;
}

std::vector<Person> generateRandomStudents() {
    std::size_t studentCount = 0;
    std::size_t homeworkCount = 0;
    std::cout << "Number of students: ";
    if (!(std::cin >> studentCount)) {
        throw std::runtime_error("Invalid number of students.");
    }
    std::cout << "Homework assignments per student: ";
    if (!(std::cin >> homeworkCount) || homeworkCount == 0) {
        throw std::runtime_error("Homework count must be greater than zero.");
    }

    std::random_device device;
    std::mt19937 generator(device());
    std::vector<Person> students;
    students.reserve(studentCount);
    for (std::size_t index = 0; index < studentCount; ++index) {
        students.push_back(Person::randomPerson(
            "Student" + std::to_string(index + 1),
            "Generated" + std::to_string(index + 1), homeworkCount, generator));
    }
    sortStudents(students);
    return students;
}

int main() {
    std::vector<Person> students;
    CalculationMethod selectedMethod = CalculationMethod::Average;

    while (true) {
        std::cout << "\nStudent final grade calculator\n"
                  << "1. Enter student data\n"
                  << "2. Generate random student data\n"
                  << "3. Read data from Students.txt\n"
                  << "4. Display final grades\n"
                  << "0. Exit\n"
                  << "Choice: ";

        int choice = 0;
        if (!(std::cin >> choice)) {
            std::cerr << "Invalid menu choice.\n";
            return 1;
        }

        try {
            if (choice == 0) {
                return 0;
            }
            if (choice == 1) {
                students = readStudentsFromConsole();
                selectedMethod = CalculationMethod::Average;
                printStudents(students, selectedMethod);
            } else if (choice == 2) {
                students = generateRandomStudents();
                selectedMethod = CalculationMethod::Average;
                printStudents(students, selectedMethod);
            } else if (choice == 3) {
                students = readStudentsFromFile("Students.txt");
                std::cout << students.size()
                          << " student records loaded from Students.txt.\n";
                selectedMethod = CalculationMethod::Average;
                printStudents(students, selectedMethod);
            } else if (choice == 4) {
                if (students.empty()) {
                    std::cout << "No student data is available.\n";
                    continue;
                }
                std::cout << "1. Average\n2. Median\nChoice: ";
                int methodChoice = 0;
                if (!(std::cin >> methodChoice)) {
                    throw std::runtime_error("Invalid calculation method.");
                }
                selectedMethod = methodChoice == 2
                                     ? CalculationMethod::Median
                                     : CalculationMethod::Average;
                printStudents(students, selectedMethod);
            } else {
                std::cout << "Unknown menu choice.\n";
            }
        } catch (const std::exception& error) {
            std::cerr << error.what() << '\n';
            return 1;
        }
    }
}
#include <iomanip>
#include <iostream>
#include <sstream>
#include <string>
#include <utility>
#include <vector>

class Person {
public:
    Person() = default;

    Person(std::string firstName, std::string surname,
           std::vector<int> homework, int exam)
        : firstName_(std::move(firstName)),
          surname_(std::move(surname)),
          homework_(std::move(homework)),
          exam_(exam) {
        calculateAverage();
    }

    Person(const Person& other)
        : firstName_(other.firstName_),
          surname_(other.surname_),
          homework_(other.homework_),
          exam_(other.exam_),
          finalGrade_(other.finalGrade_) {}

    Person& operator=(const Person& other) {
        if (this != &other) {
            firstName_ = other.firstName_;
            surname_ = other.surname_;
            homework_ = other.homework_;
            exam_ = other.exam_;
            finalGrade_ = other.finalGrade_;
        }
        return *this;
    }

    ~Person() = default;

    void calculateAverage() {
        double total = exam_;
        for (const int score : homework_) {
            total += score;
        }
        finalGrade_ = total / static_cast<double>(homework_.size() + 1);
    }

    [[nodiscard]] const std::string& firstName() const { return firstName_; }
    [[nodiscard]] const std::string& surname() const { return surname_; }
    [[nodiscard]] double finalGrade() const { return finalGrade_; }

    friend std::istream& operator>>(std::istream& in, Person& person);
    friend std::ostream& operator<<(std::ostream& out, const Person& person);

private:
    std::string firstName_;
    std::string surname_;
    std::vector<int> homework_;
    int exam_ = 0;
    double finalGrade_ = 0.0;
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

    parsed.calculateAverage();
    person = parsed;
    return in;
}

std::ostream& operator<<(std::ostream& out, const Person& person) {
    out << std::left << std::setw(14) << person.firstName_
        << std::setw(14) << person.surname_
        << std::right << std::setw(18) << std::fixed
        << std::setprecision(2) << person.finalGrade_;
    return out;
}

int main() {
    std::cout << "Student final grade calculator\n"
              << "Enter homework scores from 0 to 10. Finish each student "
                 "with -1, then enter the exam score.\n\n";

    std::size_t studentCount = 0;
    std::cout << "Number of students: ";
    if (!(std::cin >> studentCount)) {
        std::cerr << "Invalid number of students.\n";
        return 1;
    }

    std::vector<Person> students;
    students.reserve(studentCount);

    for (std::size_t index = 0; index < studentCount; ++index) {
        std::cout << "Student " << index + 1
                  << " (FirstName Surname HW1 ... HWn -1 Exam): ";
        Person person;
        if (!(std::cin >> person)) {
            std::cerr << "Invalid student data.\n";
            return 1;
        }
        students.push_back(person);
    }

    std::cout << "\nName          Surname        Final_Point(Aver.)\n"
              << "------------------------------------------------\n";
    for (const Person& student : students) {
        std::cout << student << '\n';
    }
}
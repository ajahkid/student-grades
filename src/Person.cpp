#include "Person.h"

#include <algorithm>

double Person::finalGrade(CalculationMethod method) const {
    std::vector<int> scores = homework;
    scores.push_back(exam);

    if (method == CalculationMethod::Average) {
        int total = 0;
        for (int score : scores) {
            total += score;
        }
        return static_cast<double>(total) / scores.size();
    }

    std::sort(scores.begin(), scores.end());
    int middle = static_cast<int>(scores.size()) / 2;

    if (scores.size() % 2 == 0) {
        return (scores[middle - 1] + scores[middle]) / 2.0;
    }

    return scores[middle];
}

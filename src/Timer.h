#ifndef TIMER_H
#define TIMER_H

#include <chrono>

// A stopwatch. It starts when it is created.
class Timer {
public:
    Timer() {
        reset();
    }

    void reset() {
        start = std::chrono::steady_clock::now();
    }

    double seconds() const {
        std::chrono::duration<double> passed =
            std::chrono::steady_clock::now() - start;
        return passed.count();
    }

private:
    std::chrono::steady_clock::time_point start;
};

#endif

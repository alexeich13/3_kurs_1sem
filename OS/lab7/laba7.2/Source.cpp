#include <iostream>
#include <thread>
#include <chrono>
#include "Windows.h"
using namespace std;

atomic<bool> flag(false);
atomic<long long>  iteration = 0;

int freeCycle()
{
    while (true)
    {
        iteration++;
        if (flag.load())
        {
            return 0;
        }
    }
    return 0;
}

int main() 
{
    bool flag5 = true;
    bool flag10 = true;
    thread f1(freeCycle);
    if (!f1.joinable())
    {
        cerr << "Error: " << GetLastError() << " - thread was not created" << endl;
    }

    auto startTime = chrono::high_resolution_clock::now();

    while (true)
    {
        auto currentTime = chrono::high_resolution_clock::now();
        auto elapsedTime = chrono::duration_cast<chrono::seconds>(currentTime - startTime).count();

        if (elapsedTime == 5 && flag5)
        {
            cout << "5 seconds: " << iteration.load() << endl;
            flag5 = false;
        }

        if (elapsedTime == 10 && flag10)
        {
            cout << "10 seconds: " << iteration.load() << endl;
            flag10 = false;
        }

        if (elapsedTime == 15)
        {
            cout << "15 seconds: " << iteration.load() << endl;
            flag.store(true);
            break;
        }
    }
    f1.join();

    return 0;
}

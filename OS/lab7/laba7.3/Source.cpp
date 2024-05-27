#include <iostream>
#include <thread>
#include <chrono>
#include "Windows.h"
using namespace std;

atomic<bool> flag(false);
atomic<long long> iteration = 0;
const int TIMER_PERIOD = 3000;
const int SECOND_TIMER_PERIOD = 0;

int freeCycle()
{
    while (!flag.load())
    {
        iteration++;
    }
    return 0;
}

int CloseTimer()
{
    HANDLE hTimerClose = OpenWaitableTimer(TIMER_ALL_ACCESS, FALSE, L"Global\\TMRC");
    while (!flag.load())
    {
        if (WaitForSingleObject(hTimerClose, INFINITE) != WAIT_OBJECT_0)
        {
            cerr << "Error: " << GetLastError() << " - wait 'CloseTimer' failed" << endl;
            return -1;
        }
        flag.store(true);
    }
    CloseHandle(hTimerClose);
    return 0;
}



int main()
{
    HANDLE hTimer = CreateWaitableTimer(NULL, FALSE, L"Global\\TMR");
    HANDLE hTimerClose = CreateWaitableTimer(NULL, FALSE, L"Global\\TMRC");
    if (hTimer == NULL)
    {
        cerr << "Error: " << GetLastError() << " - timer was not created" << endl;
        return -1;
    }
    if (hTimerClose == NULL)
    {
        cerr << "Error: " << GetLastError() << " - timer 'close' was not created" << endl;
        return -1;
    }

    LARGE_INTEGER dueTime;
    dueTime.QuadPart = -30000000LL;

    LARGE_INTEGER dueTimeClose;
    dueTimeClose.QuadPart = -150000000LL;

    clock_t start = clock();

    if(!SetWaitableTimer(hTimer, &dueTime, TIMER_PERIOD, NULL, NULL, FALSE))
    {
        cerr << "Error: " << GetLastError() << " - timer not set" << endl;
        return -1;
    }

    if (!SetWaitableTimer(hTimerClose, &dueTimeClose, SECOND_TIMER_PERIOD, NULL, NULL, FALSE))
    {
        cerr << "Error: " << GetLastError() << " - timer 'close' not set" << endl;
        return -1;
    }

    thread f1(freeCycle);
    if (!f1.joinable())
    {
        cerr << "Error: " << GetLastError() << " - thread was not created" << endl;
    }

    thread f2(CloseTimer);
    if (!f2.joinable())
    {
        cerr << "Error: " << GetLastError() << " - thread 'CloseTimer' was not created" << endl;
        return -1;
    }

    while (!flag.load())
    {
        cout << "Value: " << iteration << endl;

        if (WaitForSingleObject(hTimer, INFINITE) != WAIT_OBJECT_0)
        {
            cerr << "Error: " << GetLastError() << " - wait failed" << endl;
            return -1;
        }
    }

    cout << "Terminating the application. Final value: " << iteration << endl;

    f1.join();
    f2.join();

    CloseHandle(hTimer);
    CloseHandle(hTimerClose);

    return 0;
}

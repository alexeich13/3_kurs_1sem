#include <iostream>
#include <chrono>
#include <ctime>
#include <time.h>
using namespace std;

int main()
{
    auto now = chrono::system_clock::now();
    time_t now_c = chrono::system_clock::to_time_t(now);
    tm localTime;
    if (localtime_s(&localTime, &now_c) == 0)
    {
        cout << localTime.tm_mday << "." << 1 + localTime.tm_mon << "." << 1900 + localTime.tm_year << " " << localTime.tm_hour << ":" << localTime.tm_min << ":" << localTime.tm_sec << endl;
    }
    else
    {
        cerr << "Error: localtime_r failed." << endl;
    }
}

#include <iostream>
#include <Windows.h>
#include <thread>
#include <bitset>
using namespace std;

int CountSetBits(DWORD_PTR bitMask)
{
    int count = 0;
    while (bitMask) {
        count += bitMask & 1;
        bitMask >>= 1;
    }
    return count;
}

int main()
{
    DWORD currentProcessId = GetCurrentProcessId();
    cout << "Current process ID: " << currentProcessId << endl;
    DWORD currentThreadId = GetCurrentThreadId();
    cout << "Current thread ID: " << currentThreadId << endl;
    int processPriority = GetPriorityClass(GetCurrentProcess());
    cout << "Priority of the current process: " << processPriority << endl;
    int threadPriority = GetThreadPriority(GetCurrentThread());
    cout << "Priority of the current thread: " << threadPriority << endl;
    DWORD_PTR processAffinityMask, systemAffinityMask;
    GetProcessAffinityMask(GetCurrentProcess(), &processAffinityMask, &systemAffinityMask);
    cout << "Affinity mask of available processors for the process: " << bitset<sizeof(DWORD_PTR) * 1>(processAffinityMask) << std::endl;
    int processorCount = CountSetBits(processAffinityMask);
    cout << "Number of processors available to the process: " << processorCount << endl;
    int currentThreadProcessor = GetCurrentProcessorNumber();
    cout << "Processor assigned to the current thread: " << currentThreadProcessor << endl;
    return 0;
}
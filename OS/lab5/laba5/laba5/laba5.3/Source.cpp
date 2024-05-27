#include <iostream>
#include <Windows.h>
#include <mutex>
using namespace std;
#define THREAD_BASE_PRIORITY_LOWRT  15  // value that gets a thread to LowRealtime-1
#define THREAD_BASE_PRIORITY_MAX    2   // maximum thread base priority boost
#define THREAD_BASE_PRIORITY_MIN    (-2)  // minimum thread base priority boost
#define THREAD_BASE_PRIORITY_IDLE   (-15) // value that gets a thread to idle
mutex mtx;

DWORD_PTR CountToAffinityMask(int affinityMask)
{
	DWORD_PTR affinityMaskBit = 0;
	for (int i = 0; i < affinityMask; i++) {
		affinityMaskBit |= (static_cast<unsigned long long>(1) << i);
	}
	return affinityMaskBit;
}

int CountSetBits(DWORD_PTR bitMask)
{
	int count = 0;
	while (bitMask) {
		count += bitMask & 1;
		bitMask >>= 1;
	}
	return count;
}

void func1()
{
	for (int i = 0; i < 1000000; i++)
	{
		DWORD idProccess = GetCurrentProcessId();
		DWORD idThread = GetCurrentThreadId();
		DWORD_PTR priorityProccess = GetPriorityClass(GetCurrentProcess());
		DWORD_PTR priorityThread = GetThreadPriority(GetCurrentThread());

		DWORD_PTR processAffinityMask, systemAffinityMask;
		GetProcessAffinityMask(GetCurrentProcess(), &processAffinityMask, &systemAffinityMask);
		int processorCount = CountSetBits(processAffinityMask);
		int currentThreadProcessor = GetCurrentProcessorNumber();

		if (i % 5000 == 0)
		{
			//mtx.lock();
			cout << "First " << i << " - " << "PID: " << idProccess << " - " << "TID: " << idThread << " - " << "Proccess Priority: " << priorityProccess << " - " << "Thread Priority: " << priorityThread << " - " << "Currrent Proccessor: " << currentThreadProcessor << endl;
			//mtx.unlock();
		}
	}
}

void func2()
{
	for (int i = 0; i < 1000000; i++)
	{
		DWORD idProccess = GetCurrentProcessId();
		DWORD idThread = GetCurrentThreadId();
		DWORD_PTR priorityProccess = GetPriorityClass(GetCurrentProcess());
		DWORD_PTR priorityThread = GetThreadPriority(GetCurrentThread());

		DWORD_PTR processAffinityMask, systemAffinityMask;
		GetProcessAffinityMask(GetCurrentProcess(), &processAffinityMask, &systemAffinityMask);
		int processorCount = CountSetBits(processAffinityMask);
		int currentThreadProcessor = GetCurrentProcessorNumber();
		if (i % 5000 == 0)
		{
			//mtx.lock();
			cout << "Second " << i << " - " << "PID: " << idProccess << " - " << "TID: " << idThread << " - " << "Proccess Priority: " << priorityProccess << " - " << "Thread Priority: " << priorityThread << " - " << "Currrent Proccessor: " << currentThreadProcessor << endl;
			//mtx.unlock();
		}
	}
}

int StartThread(int firstThreadPriority, int secondThreadPriority)
{
	HANDLE firstThread = CreateThread(NULL, 0, LPTHREAD_START_ROUTINE(func1), NULL, CREATE_SUSPENDED, NULL);
	if (firstThread == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the first thread was not started" << endl;
	}
	cout << "First thread started" << endl;

	HANDLE secondThread = CreateThread(NULL, 0, LPTHREAD_START_ROUTINE(func2), NULL, CREATE_SUSPENDED, NULL);
	if (secondThread == NULL)
	{
		cerr << "Error: " << GetLastError() << " - the second thread was not started" << endl;
	}
	cout << "Second thread started" << endl;

	if (SetThreadPriority(firstThread, firstThreadPriority) == 0)
	{
		cerr << "Error: " << GetLastError() << " - set first thread priority was not processed" << endl;
	}

	if (SetThreadPriority(secondThread, secondThreadPriority) == 0)
	{
		cerr << "Error: " << GetLastError() << " - set second thread priority was not processed" << endl;
	}

	int fPriority = GetThreadPriority(firstThread);
	cout << "First thread priority: " << fPriority << endl;
	int sPriority = GetThreadPriority(secondThread);
	cout << "Second thread priority: " << sPriority << endl;

	ResumeThread(firstThread);
	ResumeThread(secondThread);

	WaitForSingleObject(firstThread, INFINITE);
	WaitForSingleObject(secondThread, INFINITE);

	CloseHandle(firstThread);
	CloseHandle(secondThread);

	return 0;
}

int main()
{
	int affinityMask;
	int proccessPriority;
	int firstThreadPriority;
	int secondThreadPriority;

	DWORD_PTR maxProccessCount = GetMaximumProcessorCount(ALL_PROCESSOR_GROUPS);

	cout << "Number of available processors: " << maxProccessCount << endl;
	cout << "Enter the number of available processors for the process: ";
	cin >> affinityMask;
	cout << "Enter proccess priority: ";
	cin >> proccessPriority;
	cout << "Enter first thread priority: ";
	cin >> firstThreadPriority;
	cout << "Enter second thread priority: ";
	cin >> secondThreadPriority;

	DWORD_PTR affinityMaskDWORD = CountToAffinityMask(affinityMask);
	if (!SetProcessAffinityMask(GetCurrentProcess(), affinityMaskDWORD))
	{
		cerr << "Error: " << GetLastError() << " - the affinity mask was not processed" << endl;
	}
	
	if(SetPriorityClass(GetCurrentProcess(), proccessPriority) == 0)
	{
		cerr << "Error: " << GetLastError() << " - set proccess priority was not processed" << endl;
	}

	DWORD_PTR priorityProccess = GetPriorityClass(GetCurrentProcess());
	cout << "Current thread priority: " << priorityProccess << endl;

	DWORD_PTR processAffinityMask, systemAffinityMask;
	GetProcessAffinityMask(GetCurrentProcess(), &processAffinityMask, &systemAffinityMask);
	int processorCount = CountSetBits(processAffinityMask);
	cout << "Number of available processors: " << processorCount << endl;

	if (StartThread(firstThreadPriority, secondThreadPriority) != 0)
	{
		cerr << "Error: " << GetLastError() << " - Start thread failed" << endl;
	}
	else
	{
		cout << "Start Thread completed" << endl;
	}

	return 0;
}
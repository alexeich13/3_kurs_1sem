#include <iostream>
#include <windows.h>
#include <process.h>
#include <thread>
using namespace std;

CRITICAL_SECTION crit;

void func()
{
	thread::id this_id = this_thread::get_id();
	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			EnterCriticalSection(&crit);
		}
		if (i == 60)
		{
			LeaveCriticalSection(&crit);
		}
		this_thread::sleep_for(chrono::milliseconds(100));
		cout << i << "Main Thread: " << this_id << endl;
	}
}

void func1()
{
	thread::id this_id = this_thread::get_id();
	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			EnterCriticalSection(&crit);
		}
		else if (i == 60)
		{
			LeaveCriticalSection(&crit);
		}
		this_thread::sleep_for(chrono::milliseconds(100));
		cout << i << "First Child Thread: " << this_id << endl;
	}	
}

void func2()
{
	thread::id this_id = this_thread::get_id();
	for (int i = 0; i < 90; i++)
	{
		if (i == 30)
		{
			EnterCriticalSection(&crit);
		}
		else if (i == 60)
		{
			LeaveCriticalSection(&crit);
		}
		this_thread::sleep_for(chrono::milliseconds(100));
		cout << i << "Second Child Thread: " << this_id << endl;
	}
}

int main()
{
	InitializeCriticalSection(&crit);
	thread f1(func1);
	thread f2(func2);
	func();
	if (!f1.joinable())
	{
		cerr << "Error: " << GetLastError() << " - firsrt thread was not started" << endl;
	}
	if (!f2.joinable())
	{
		cerr << "Error: " << GetLastError() << " - second thread was not started" << endl;
	}
	f1.join();
	f2.join();
	DeleteCriticalSection(&crit);
	return 0;
}
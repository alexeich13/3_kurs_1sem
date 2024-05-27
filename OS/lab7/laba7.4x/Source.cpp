#include <iostream>
#include <Windows.h>
#include <thread>
using namespace std;

atomic<bool> flag(false);

int freeCycle()
{
	for (int number = 0; !flag.load(); number++)
	{
		if (number <= 1)
		{
		}
		else if (number <= 3)
		{
			cout << number << endl;
			
		}
		else if (number % 2 == 0 || number % 3 == 0)
		{
		}
		else
		{
			bool isPrime = true;
			for (int i = 5; i * i <= number; i += 6)
			{
				if (number % i == 0 || number % (i + 2) == 0)
				{
					isPrime = false;
					break;
				}
			}
			if (isPrime)
			{
				cout << number << endl;
			}
		}
	}
	return 0;
}

int main()
{
	thread fThread(freeCycle);

	if (!fThread.joinable())
	{
		cerr << "Error: " << GetLastError() << " - thread was not created" << endl;
	}
	HANDLE hTimer = OpenWaitableTimer(TIMER_ALL_ACCESS, FALSE, L"Global\\TMRF");
	if (WaitForSingleObject(hTimer, INFINITE) != WAIT_OBJECT_0)
	{
		cerr << "Error: " << GetLastError() << " - wait failed" << endl;
	}
	flag.store(true);
	CloseHandle(hTimer);
	fThread.join();

	return 0;
}
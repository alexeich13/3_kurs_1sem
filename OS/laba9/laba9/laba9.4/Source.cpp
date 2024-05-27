#include <iostream>
#include <windows.h>
#include <fstream>
#include <string>
#include <sstream>
#include <thread>
using namespace std;

bool printWatchRowFileTxt(LPCWSTR FileNameIn, DWORD mlsec)
{
    if (FileNameIn == nullptr)
    {
        cerr << "Error: " << GetLastError() << " - Invalid file name." << endl;
        cout << "Scan file terminated." << endl << endl;
        return false;
    }

    HANDLE hFile = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    if (hFile == INVALID_HANDLE_VALUE)
    {
        cerr << "Error: " << GetLastError() << " - can't open file" << endl << endl;
        cout << "Scan file terminated." << endl << endl;
        return false;
    }

    DWORD fileSize = GetFileSize(hFile, NULL);
    if (fileSize == INVALID_FILE_SIZE)
    {
        cerr << "Error: " << GetLastError() << " - can't get file size" << endl << endl;
        cout << "Scan file terminated." << endl << endl;
        CloseHandle(hFile);
        return false;
    }

    char* fileContent = new char[fileSize + 1];
    DWORD bytesRead = 0;

    if (!ReadFile(hFile, fileContent, fileSize, &bytesRead, NULL))
    {
        cerr << "Error: " << GetLastError() << " - error reading file" << endl << endl;
        cout << "Scan file terminated." << endl << endl;
        CloseHandle(hFile);
        return false;
    }

    CloseHandle(hFile);

    fileContent[bytesRead] = '\0';
    string bufFileContent(fileContent);
    delete[] fileContent;

    istringstream file_in(bufFileContent);
    int i_number_line = 0;
    string line;
    string tempFileContent;

    while (getline(file_in, line))
    {
        i_number_line++;
    }

    cout << "The tracking program has started." << endl;
    wcout << L"Current file name: " << FileNameIn << endl;
    cout << "Current number of rows: " << i_number_line << endl;

    DWORD startTickCount = GetTickCount64();
    bool changeDetected = false;

    while ((GetTickCount64() - startTickCount) < mlsec)
    {
        HANDLE hFileCheck = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

        if (hFileCheck == INVALID_HANDLE_VALUE)
        {
            cerr << "Error: " << GetLastError() << " - can't open file" << endl << endl;
            cout << "Scan file terminated." << endl << endl;
            return false;
        }

        DWORD fileSize = GetFileSize(hFileCheck, NULL);
        if (fileSize == INVALID_FILE_SIZE)
        {
            cerr << "Error: " << GetLastError() << " - can't get file size" << endl << endl;
            cout << "Scan file terminated." << endl << endl;
            CloseHandle(hFileCheck);
            return false;
        }

        char* fileContent = new char[fileSize + 1];
        DWORD bytesRead = 0;

        if (!ReadFile(hFileCheck, fileContent, fileSize, &bytesRead, NULL))
        {
            cerr << "Error: " << GetLastError() << " - error reading file" << endl << endl;
            cout << "Scan file terminated." << endl << endl;
            CloseHandle(hFileCheck);
            return false;
        }

        CloseHandle(hFileCheck);

        fileContent[bytesRead] = '\0';
        string bufFileContent(fileContent);
        delete[] fileContent;

        istringstream file_in(bufFileContent);
        int i_number_line_now = 0;
        string line;
        string tempFileContent;

        while (getline(file_in, line))
        {
            i_number_line_now++;
        }

        if (i_number_line != i_number_line_now)
        {
            cout << "Number of lines changed. Current number of rows: " << i_number_line_now << endl;
            i_number_line = i_number_line_now;
            changeDetected = true;
        }
        this_thread::sleep_for(chrono::milliseconds(500));
    }

    if (!changeDetected)
    {
        cout << "No changes detected in " << mlsec << " milliseconds.\n";
    }

    return true;
}

int main()
{
    LPCWSTR fileName = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\laba9\\laba92.txt"; 
    DWORD mlsec = 60000;

    if (!printWatchRowFileTxt(fileName, mlsec))
    {
        cerr << "Error: " << GetLastError() << " - error scanning file" << endl << endl;
        return 2;
    }

    return 0;
}

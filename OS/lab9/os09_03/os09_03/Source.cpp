#define _CRT_SECURE_NO_WARNINGS
#pragma warning(disable : 4996)

#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <Windows.h>
#include <vector>
using namespace std;

bool printFileTxt(LPCWSTR FileNameIn)
{
    cout << "File info:" << endl << endl;
    HANDLE hFile = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    DWORD fileSize = GetFileSize(hFile, NULL);
    char* fileBuffer = new char[fileSize + 1];
    DWORD bytesRead = 0;
    DWORD  dwBytesRead = 0;
    if (!ReadFile(hFile, fileBuffer, fileSize, &bytesRead, NULL))
    {
        cerr << "Error: " << GetLastError() << " - error reading file." << endl;
        cout << "Reading data from file was terminated." << endl;
        delete[] fileBuffer;
        CloseHandle(hFile);
        return false;
    }

    fileBuffer[bytesRead] = '\0';

    if (bytesRead == 0)
    {
        cout << "Warning: File empty." << endl << endl;
    }
    else
    {
        cout << endl << fileBuffer << endl;
    }

    delete[] fileBuffer;
    CloseHandle(hFile);
    return true;
}


bool insRowFileTxt(LPCWSTR FileNameIn, LPCWSTR str, DWORD row)
{
    cout << "Insert row in file..." << endl;

    if (FileNameIn == nullptr)
    {
        cerr << "Error: " << GetLastError() << " - Invalid file name." << endl;
        cout << "Insert row terminated." << endl;
        return false;
    }

    HANDLE hFile = CreateFileW(FileNameIn, FILE_GENERIC_WRITE | FILE_GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    if (hFile == INVALID_HANDLE_VALUE)
    {
        cerr << "Error: " << GetLastError() << " - can't open file." << endl << endl;
        cout << "Insert row terminated." << endl << endl;
        return false;
    }

    DWORD shiftPointer = 0;

    if (row == -1)
    {
        DWORD dwPointer = SetFilePointer(hFile, shiftPointer, NULL, FILE_END);
        if (dwPointer == INVALID_SET_FILE_POINTER)
        {
            cerr << "Error: " << GetLastError() << " - set pointer file to end failed." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }

        int strBufferSize = WideCharToMultiByte(CP_UTF8, 0, str, -1, NULL, 0, NULL, NULL);
        if (strBufferSize == 0)
        {
            cerr << "Error: " << GetLastError() << " - string is not defined." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }

        char* strBuffer = new char[strBufferSize + 1];
        if (strBuffer == nullptr)
        {
            cerr << "Error: Memory allocation failed." << endl;
            CloseHandle(hFile);
            return false;
        }

        strBuffer[0] = '\n';
        WideCharToMultiByte(CP_UTF8, 0, str, -1, strBuffer + 1, strBufferSize, NULL, NULL);

        DWORD strBytesWritten;
        DWORD strBytesToWrite = static_cast<DWORD>(strlen(strBuffer));
        if (!WriteFile(hFile, strBuffer, strBytesToWrite, &strBytesWritten, NULL))
        {
            cerr << "Error: " << GetLastError() << " - writing to file failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            delete[] strBuffer;
            return false;
        }
        delete[] strBuffer;
    }
    else if (row == 0)
    {
        DWORD fileSize = GetFileSize(hFile, NULL);
        if (fileSize == INVALID_FILE_SIZE)
        {
            cerr << "Error: " << GetLastError() << " - can't get file size." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }

        char* fileBuffer = new char[fileSize + 1];
        if (fileBuffer == nullptr)
        {
            cerr << "Error: Memory allocation failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }

        DWORD fileBufferSize = 0;

        if (!ReadFile(hFile, fileBuffer, fileSize, &fileBufferSize, NULL))
        {
            cerr << "Error: " << GetLastError() << " - error reading file." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] fileBuffer;
            CloseHandle(hFile);
            return false;
        }

        fileBuffer[fileBufferSize] = '\0';

        DWORD dwPointer = SetFilePointer(hFile, shiftPointer, NULL, FILE_BEGIN);
        if (dwPointer == INVALID_SET_FILE_POINTER)
        {
            cerr << "Error: " << GetLastError() << " - set pointer file to end failed." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] fileBuffer;
            return false;
        }

        int strBufferSize = WideCharToMultiByte(CP_UTF8, 0, str, -1, NULL, 0, NULL, NULL);
        if (strBufferSize == 0)
        {
            cerr << "Error: " << GetLastError() << " - string is not defined." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] fileBuffer;
            return false;
        }

        char* strBuffer = new char[strBufferSize + 2];
        if (strBuffer == nullptr)
        {
            cerr << "Error: Memory allocation failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] fileBuffer;
            CloseHandle(hFile);
            return false;
        }

        WideCharToMultiByte(CP_UTF8, 0, str, -1, strBuffer, strBufferSize, NULL, NULL);

        strBuffer[strBufferSize - 1] = '\n';
        strBuffer[strBufferSize] = '\0';

        DWORD strBytesWritten;
        DWORD strBytesToWrite = static_cast<DWORD>(strlen(strBuffer));
        if (!WriteFile(hFile, strBuffer, strBytesToWrite, &strBytesWritten, NULL))
        {
            cerr << "Error: " << GetLastError() << " - writing to file failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            delete[] fileBuffer;
            delete[] strBuffer;
            return false;
        }
        delete[] strBuffer;

        DWORD fileBytesWritten;
        DWORD fileBytesToWrite = static_cast<DWORD>(strlen(fileBuffer));
        if (!WriteFile(hFile, fileBuffer, fileBytesToWrite, &fileBytesWritten, NULL))
        {
            cerr << "Error: " << GetLastError() << " - writing to file failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            delete[] fileBuffer;
            return false;
        }
        delete[] fileBuffer;
    }
    else
    {
        int strBufferSize = WideCharToMultiByte(CP_UTF8, 0, str, -1, NULL, 0, NULL, NULL);
        if (strBufferSize == 0)
        {
            cerr << "Error: " << GetLastError() << " - string is not defined." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            return false;
        }

        char* strBuffer = new char[strBufferSize + 2];
        if (strBuffer == nullptr)
        {
            cerr << "Error: Memory allocation failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }

        WideCharToMultiByte(CP_UTF8, 0, str, -1, strBuffer, strBufferSize, NULL, NULL);

        strBuffer[strBufferSize - 1] = '\n';
        strBuffer[strBufferSize] = '\0';

        DWORD fileSize = GetFileSize(hFile, NULL);
        if (fileSize == INVALID_FILE_SIZE)
        {
            cerr << "Error: " << GetLastError() << " - can't get file size" << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] strBuffer;
            CloseHandle(hFile);
            return false;
        }

        char* tempFileBuffer = new char[fileSize + 1];
        if (tempFileBuffer == nullptr)
        {
            cerr << "Error: Memory allocation failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] strBuffer;
            CloseHandle(hFile);
            return false;
        }

        DWORD tempFileBufferSize = 0;

        if (!ReadFile(hFile, tempFileBuffer, fileSize, &tempFileBufferSize, NULL))
        {
            cerr << "Error: " << GetLastError() << " - error reading file" << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            delete[] tempFileBuffer;
            delete[] strBuffer;
            CloseHandle(hFile);
            return false;
        }

        tempFileBuffer[tempFileBufferSize] = '\0';

        string fileBuffer(tempFileBuffer);
        delete[] tempFileBuffer;

        int pos = 0;
        for (int currentRow = 1; currentRow < row; currentRow++)
        {
            pos = (int)fileBuffer.find('\n', pos);
            if (pos == string::npos)
            {
                cerr << "Error: Specified row exceeds the number of lines in the file." << endl << endl;
                cout << "Insert row terminated." << endl << endl;
                delete[] strBuffer;
                CloseHandle(hFile);
                return false;
            }
            pos++;
        }

        fileBuffer.insert(pos, strBuffer);
        delete[] strBuffer;

        DWORD dwPointer = SetFilePointer(hFile, shiftPointer, NULL, FILE_BEGIN);
        if (dwPointer == INVALID_SET_FILE_POINTER)
        {
            cerr << "Error: " << GetLastError() << " - set pointer file to end failed." << endl << endl;
            cout << "Insert row terminated." << endl << endl;
            return false;
        }

        DWORD fileBytesWritten;
        DWORD fileBytesToWrite = static_cast<DWORD>(fileBuffer.length());
        if (!WriteFile(hFile, fileBuffer.c_str(), fileBytesToWrite, &fileBytesWritten, NULL))
        {
            cerr << "Error: " << GetLastError() << " - writing to file failed." << endl;
            cout << "Insert row terminated." << endl << endl;
            CloseHandle(hFile);
            return false;
        }
    }
    CloseHandle(hFile);
    return true;
}

int main()
{
    LPCWSTR fileName = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab9\\lab91.txt";
    LPCWSTR str = L"Insert";
    vector<int> rowsToIns = { -1, 7, 5, 0 };
    for (int i = (int)rowsToIns.size() - 1; i >= 0; i--)
    {
        if (!insRowFileTxt(fileName, str, rowsToIns[i]))
        {
            cout << "Program interrupted: ";
            cerr << "Error: " << GetLastError() << " - edit file failed. " << endl;
            return 2;
        }
    }

    if (!printFileTxt(fileName))
    {
        cout << "Program interrupted: ";
        cerr << "Error: " << GetLastError() << " - read file text failed. " << endl;
        return 2;
    }
    return 0;
}
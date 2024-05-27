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
    cout << "File information:" << endl << endl;
    HANDLE hFile = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    DWORD fileSize = GetFileSize(hFile, NULL);
    if (fileSize == INVALID_FILE_SIZE)
    {
        cerr << "Error: " << GetLastError() << " - can't get file size." << endl;
        cout << "Reading data from file was terminated." << endl;
        CloseHandle(hFile);
        return false;
    }
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
        cout << fileBuffer << endl;
    }
    delete[] fileBuffer;
    CloseHandle(hFile);
    return true;
}


bool delRowFileTxt(LPCWSTR FileNameIn, DWORD row)
{
    cout << "Delete from file." << endl;

    if (FileNameIn == nullptr)
    {
        cerr << "Error: " << GetLastError() << " - Invalid file name." << endl;
        cout << "Edit row terminated." << endl;
        return false;
    }

    HANDLE hFile = CreateFile(FileNameIn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
    DWORD fileSize = GetFileSize(hFile, NULL);
    if (fileSize == INVALID_FILE_SIZE)
    {
        cerr << "Error: " << GetLastError() << " - can't get file size." << endl << endl;
        cout << "Edit row terminated." << endl << endl;
        CloseHandle(hFile);
        return false;
    }
    char* fileContent = new char[fileSize + 1];
    DWORD bytesRead = 0;
    if (!ReadFile(hFile, fileContent, fileSize, &bytesRead, NULL))
    {
        cerr << "Error: " << GetLastError() << " - error reading file." << endl << endl;
        cout << "Edit row terminated." << endl << endl;
        CloseHandle(hFile);
        return false;
    }
    CloseHandle(hFile);

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
        if (!(i_number_line_now == row))
        {
            tempFileContent.insert(tempFileContent.size(), line + '\n');
        }
    }
    char* newFileContent = new char[size(tempFileContent) + 1];
    strcpy(newFileContent, tempFileContent.c_str());
    HANDLE wFile = CreateFileW(FileNameIn, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
    DWORD dwBytesToWrite = (DWORD)strlen(newFileContent);
    DWORD dwBytesWritten = 0;
    if (!WriteFile(wFile, newFileContent, dwBytesToWrite, &dwBytesWritten, NULL))
    {
        cerr << "Error: " << GetLastError() << " - error write file." << endl << endl;
        cout << "Edit row terminated." << endl << endl;
        delete[] newFileContent;
        CloseHandle(wFile);
        return false;
    }
    delete[] newFileContent;
    CloseHandle(wFile);
    return true;
}

int main()
{
    LPCWSTR fileName = L"C:\\BSTU\\3 kurs\\3 kurs 1 sem\\OS\\lab9\\lab91.txt";

    vector<int> rowsToDelete = { 1, 3, 8, 10 };
    for (int i = (int)rowsToDelete.size() - 1; i >= 0; i--)
    {
        if (!delRowFileTxt(fileName, rowsToDelete[i]))
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
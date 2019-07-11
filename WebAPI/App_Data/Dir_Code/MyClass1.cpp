#include <iostream>
#include <fstream>
 
using namespace std;

int ArraySum(int array[], int length){
    //write your code here
    int res = 0;
    for(int i = 0; i < length; i++){
        res += array[i];
    }
    return res;
 }
 
int main() {
	ifstream myfile("E:/Backup/Lap Trinh/Đồ án tốt ngiệp/datn/WebAPI/App_Data/Dir_TestCase/challenge_1_input_0.txt");
	if (myfile.is_open())
	{
		int arrSize = 0;
		int length;
		myfile >> length;
		int arr[length];
		
		while (!myfile.eof())
		{
			int x;
			myfile >> x;
			arr[arrSize] = x;
			arrSize++;
		}
		int res = ArraySum(arr, length);
		cout << res;
		myfile.close();
	}
	else
	{
		cout << "Unable to open file";
	}
	return 0;
}

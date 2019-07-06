# PhoneBookProject
To do this project I have decided to use binary file to store the entries.
I have decided to use binary file because it was one of the choices that I was allowed to do in
my project.

In this project I built a class PhoneEntryModel where I stored:

id -> int
firstname -> string
lastname -> string
entrytype -> enum
phonenumber -> string

I built a class Constants where I stored some constants variables that I was going
to use in my project:

filename -> the name of the file that I was going to use.
filelocation -> the current directory where the file it will be stored.
filepath -> the full path where my file it will be stored.
filemaxsize -> the maximum length of the file.

I built an interface with the functions which I will use for my project:

GetAll function where I would get all the entries of the file and stored it in a list of objects.
Add function where I would store an instance of object to the binary file.
Edit function where I would edit an entry from the binary file.
Delete function where I would delete an entry from the binary file.
Iterate function where I would itereate the list of objects, order by firstname or lastname and store it in the binary file.

I built a BinaryFileManager class where I managed the functions used for my project.

Create file function used for creating the file and if the file is created just opened it.
WriteToBinaryFile function for writing entries into the binary file by using the method of serialize objects.
ReadFromBinaryFile function for reading entries from the binary file and store it into a list of ojects by using the method of deserialize the file.
Iterate function for iterating the list of objects obtained by the binary file and sorted by firstname if the orderbyfirstname variable was true or by lastname if the orderbyfirstname variable was false.
Edit function for editing an entry of the instance object passed as a parameter by iterating the list of objects obtained by the binary file and if the id property of our new entry was matched with the id property on the list of object, replace that entry with our new entry and write again our new list of object into the file.
Delete function for deleting an entry of the instance object passed as parameter by iterating the list of objects obtained by the binary file and if the id property of our new entry was matched with the id property on the list of object, delete that entry on the list of objects and write again our new list of object into the file.
Add function for adding an entry of the instance object passed as parameter by stored into a list of objects and write it our list of object into the file.

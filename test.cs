using System;
using BCrypt.Net;

class Program {
    static void Main() {
        bool result = BCrypt.Net.BCrypt.Verify("123456", "$2a$11$CIQHeQAWiaFZtYDvA6a92O8yr48moWQbuV1XZG3WVIOuMa8qsuIi2");
        Console.WriteLine(result);
    }
}

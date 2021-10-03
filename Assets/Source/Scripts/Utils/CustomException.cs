using System;

namespace Utils
{
    public class InvalidHenryIsABitchException : Exception
    {
        public InvalidHenryIsABitchException() { }
        public InvalidHenryIsABitchException(string e) : base(e) { }
        
    }
}
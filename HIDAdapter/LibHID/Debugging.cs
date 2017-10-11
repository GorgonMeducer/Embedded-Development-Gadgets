using System;
using System.Runtime.InteropServices;

///  <summary>
///  Used only in Debug.Write statements.
///  </summary>
namespace HID
{    
    internal sealed class Debugging  
    {
        //
        // Declarations
        //
        internal const Int16 FORMAT_MESSAGE_FROM_SYSTEM = 0X1000;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Int32 FormatMessage(Int32 dwFlags, ref Int64 lpSource, Int32 dwMessageId, Int32 dwLanguageZId, String lpBuffer, Int32 nSize, Int32 Arguments);        


        
        
        ///  <summary>
        ///  Get text that describes the result of an API call.
        ///  </summary>
        ///  
        ///  <param name="FunctionName"> the name of the API function. </param>
        ///  
        ///  <returns>
        ///  The text.
        ///  </returns>
        internal String ResultOfAPICall( String functionName ) 
        {             
            Int32 bytes         = 0; 
            Int32 resultCode    = 0; 
            String resultString = ""; 
            
            resultString = new String(Convert.ToChar( 0 ), 129 ); 
            
            // Returns the result code for the last API call.
            resultCode = System.Runtime.InteropServices.Marshal.GetLastWin32Error(); 
            
            // Get the result message that corresponds to the code.
            Int64 temp = 0;          
            bytes = FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, ref temp, resultCode, 0, resultString, 128, 0); 
            
            // Subtract two characters from the message to strip the CR and LF.
            if ( bytes > 2 ) 
            { 
                resultString = resultString.Remove( bytes - 2, 2 ); 
            }             

            // Create the String to return.
            resultString = Environment.NewLine + functionName + Environment.NewLine + "Result = " + resultString + Environment.NewLine; 
            
            return resultString;
          
        }// ResultOfAPICall()

    }// class Debugging

}// namespace HidLib

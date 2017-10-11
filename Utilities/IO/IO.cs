using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ESnail.Utilities.IO
{
    static public class PathEx
    {
        //! \note get each node in a path string
        //! \param tPathString a reference to a path string
        //! \return an array of nodes in a path
        static public String[] Separate(String tPathString)
        {
            return Separate(tPathString, Path.DirectorySeparatorChar);
        }

        static public String Left(String tPathString, Char tSeperator, Int32 tLength)
        {
            String[] tTarget = PathEx.Separate(tPathString, tSeperator);
            String tResult = "";

            tLength = Math.Min(tTarget.Length, tLength);
            do
            {
                if (0 == tLength)
                {
                    break;
                }
                for (Int32 i = 0; i < tLength; i++)
                {
                    tResult = PathEx.Combine(' ', tTarget[i], tResult);
                }
            } while (false);

            return tResult;
        }

        static public String Left(String tPathString, Int32 tLenght)
        {
            return Left(tPathString, Path.DirectorySeparatorChar, tLenght);
        }

        static public String Right(String tPathString, Char tSeperator, Int32 tLength)
        {
            String[] tTarget = PathEx.Separate(tPathString, tSeperator);
            String tResult = "";

            tLength = Math.Min(tTarget.Length, tLength);
            do
            {
                if (0 == tLength)
                {
                    break;
                }
                for (Int32 i = 0; i < tLength; i++)
                {
                    tResult = PathEx.Combine(' ',  tTarget[tTarget.Length - i - 1], tResult);
                }
            } while (false);

            return tResult;
        }

        static public String Right(String tPathString, Int32 tLenght)
        {
            return Right(tPathString, Path.DirectorySeparatorChar, tLenght);
        }

        static public String[] Separate(String tPathString, String tSeperators)
        {
            if (null == tSeperators)
            {
                return new String[] { tPathString };
            }
            else if ("" == tSeperators)
            {
                return new String[] { tPathString };
            }

            return Separate(tPathString, tSeperators.ToCharArray());
        }

        static public String[] Separate(String tPathString, Char[] tSeperators)
        {
            if (null == tPathString)
            {
                return new String[] { };
            }
            else if ("" == tPathString.Trim())
            {
                return new String[] { };
            }
            if (null == tSeperators)
            {
                return new String[] { tPathString };
            }
            else if (0 == tSeperators.Length)
            {
                return new String[] { tPathString };
            }

            List<String> m_ResultList = new List<string>();

            do
            {
                Int32 tPosition = tPathString.Length ;

                foreach (Char tSeperator in tSeperators)
                {
                    Int32 tLocation = tPathString.IndexOf(tSeperator);
                    if (tLocation >= 0) {
                        tPosition = Math.Min(tPosition, tLocation);
                    }
                }
                
                if (tPathString.Length == tPosition)
                {
                    m_ResultList.Add(tPathString);
                    break;
                }
                String tNode = tPathString.Substring(0, tPosition);
                if ("" != tNode)
                {
                    m_ResultList.Add(tNode);
                }

                tPathString = tPathString.Substring(tPosition + 1, tPathString.Length - tPosition - 1);
            }
            while ("" != tPathString);

            if (0 == m_ResultList.Count)
            {
                return new String[] { };
            }
            return m_ResultList.ToArray();
        }

        static public String[] Separate(String tPathString, Char tSeparator)
        {
#if false
            if (null == tPathString)
            {
                return new String[]{};
            }
            else if ("" == tPathString.Trim())
            {
                return new String[] { };
            }


            List<String> m_ResultList = new List<string>();
            
            do
            {
                Int32 tPosition = tPathString.IndexOf(tSeparator);
                if (-1 == tPosition)
                {
                    m_ResultList.Add(tPathString);
                    break;
                }
                String tNode = tPathString.Substring(0, tPosition);
                if ("" != tNode)
                {
                    m_ResultList.Add(tNode);
                }
               
                tPathString = tPathString.Substring(tPosition + 1, tPathString.Length - tPosition - 1);
            }
            while ("" != tPathString);

            if (0 == m_ResultList.Count)
            {
                return new String[] { };
            }
            return m_ResultList.ToArray();
#else
            return Separate(tPathString, new Char[] { tSeparator });
#endif
        }

        //! \note same to System.IO.Path.Combine() method
        //! \param tStringA path string A
        //! \param tStringB path string B
        //! \return result path string
        static public String Combine(String tStringA, String tStringB)
        {
            return System.IO.Path.Combine(tStringA, tStringB);
        }

        static public String Combine(Char tSeperator, String tStringA, String tStringB)
        { 
            return CombineEx(tSeperator, new String[] { tStringA, tStringB });
        }

        //! \note combine a set of path string
        //! \param tStringA path string A
        //! \param tStringB path string B
        //! \return result path string
        static public String Combine(String[] tNodes)
        {
            if (null == tNodes)
            {
                return null;
            }
            else if (0 == tNodes.Length)
            {
                return "";
            }

            String tResult = "";
            foreach (String tItem in tNodes)
            {
                if (null == tItem)
                {
                    continue;
                }
                tResult = System.IO.Path.Combine(tResult, tItem);
            }

            return tResult;
        }

        //! \note combine a few path strings
        //! \param tStringA path string A
        //! \param tStringB path string B
        //! \return result path string
        static public String CombineEx(params String[] tNodes)
        {
            return Combine(tNodes);
        }

        static public Boolean CheckExtensionList(String tPath, String tExtensionList)
        {
            if (null == tPath)
            {
                return false;
            }

            String tTargetExtension = Path.GetExtension(tPath).ToLower().Trim();

            foreach (String tExtension in PathEx.Separate(tExtensionList, '|'))
            {
                if (tExtension.ToLower().Trim() == ("*" + tTargetExtension))
                {
                    return true;
                }
            }

            return false;
        }

        static public String CombineEx(Char tSeperator,params String[] tNodes )
        {
            do
            {
                if (null == tNodes)
                {
                    break;
                }
                else if (0 == tNodes.Length)
                {
                    break;
                }
                StringBuilder tBuilder = new StringBuilder();
                tBuilder.Append(tNodes[0]);
                for (Int32 n = 1; n < tNodes.Length;n++) 
                {
                    tBuilder.Append(tSeperator);
                    tBuilder.Append(tNodes[n]);
                }

                return tBuilder.ToString();

            } while (false);

            return "";
        }

        static public String RelativePath(String tPathSource, String tPathTarget)
        {
            if ((null == tPathSource) || (null == tPathTarget))
            {
                return null;
            }
            else if (("" == tPathSource.Trim()) || ("" == tPathTarget.Trim()))
            {
                return "";
            }
           

            String tPathA = Path.GetFullPath(tPathSource);
            String tPathB = Path.GetFullPath(tPathTarget);

            if ((null == tPathA) || (null == tPathB))
            {
                return null;
            }
            else if (("" == tPathA.Trim()) || ("" == tPathB.Trim()))
            {
                return "";
            }

            if (Path.GetPathRoot(tPathA) != Path.GetPathRoot(tPathB))
            {
                return tPathTarget;
            }

            String[] tPathNodeSrc = PathEx.Separate(tPathA);
            String[] tPathNodeDes = PathEx.Separate(tPathB);

            if ((null == tPathNodeSrc) || (null == tPathNodeDes))
            {
                return null;
            }

            Queue<String> tPathNodeQueueSrc = new Queue<String>();
            foreach (String tItem in tPathNodeSrc)
            {
                tPathNodeQueueSrc.Enqueue(tItem);
            }
            Queue<String> tPathNodeQueueDes = new Queue<String>();
            foreach (String tItem in tPathNodeDes)
            {
                tPathNodeQueueDes.Enqueue(tItem);
            }

            Queue<String> tResultQueue = new Queue<String>();

            while(tPathNodeQueueDes.Count > 0 && tPathNodeQueueSrc.Count > 0)
            {
                String tItemA = tPathNodeQueueSrc.Peek();
                String tItemB = tPathNodeQueueDes.Peek();

                if (tItemA != tItemB)
                {
                    break;
                }

                tPathNodeQueueSrc.Dequeue();
                tPathNodeQueueDes.Dequeue();
            }

            //tResultQueue.Enqueue(".");

            while (tPathNodeQueueSrc.Count > 0)
            {
                tPathNodeQueueSrc.Dequeue();
                tResultQueue.Enqueue("..");
            }
            while (tPathNodeQueueDes.Count > 0)
            {
                tResultQueue.Enqueue(tPathNodeQueueDes.Dequeue());
            }

            return Combine(tResultQueue.ToArray());
        }
    }

    static public class Error
    {
        public static Boolean WriteErrorLog(String tFile, String tError)
        {
            do
            {
                try
                {
                    using (StreamWriter tWriter = File.AppendText(Path.Combine(
                        System.Windows.Forms.Application.StartupPath,
                        tFile)))
                    {
                        if (null != tWriter)
                        {
                            tWriter.WriteLine("==========================================");
                            tWriter.Write(DateTime.Now.ToLongDateString());
                            tWriter.Write("  ");
                            tWriter.Write(DateTime.Now.ToLongTimeString());
                            tWriter.Write("\r\n");
                            tWriter.WriteLine("==========================================");
                            tWriter.WriteLine("Error Info:");
                            try
                            {
                                tWriter.Write(tError);
                            }
                            catch (Exception) { }
                            tWriter.Write("\r\n\r\n\r\n");

                        }
                    }
                }
                catch (Exception) 
                {
                    return false;
                }

                return true;
            }
            while (false);

            return false;
        }
    }
}

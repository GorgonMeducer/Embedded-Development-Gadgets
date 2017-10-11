using System;
using System.Collections.Generic;
using System.Text;
using ESnail.CommunicationSet.Commands;

namespace ESnail.Device.Telegraphs
{
    public class STK500Telegraph : SinglePhaseTelegraph
    {
        //! constructor
        public STK500Telegraph(ESCommand Command)
            : base(Command)
        { 
            
        }

        //! Decoder
        public override System.Int32 Decode(ref Queue<System.Byte> InputQueue, ref Boolean tRequestDrop)
        {
            //! add code here
            return 0;
        }

        //! encoder
        public override System.Byte[] Encode()
        {
            //! add code here
            return null;
        }


        public override String Type
        {
            get { return "ATSTK500-Compliant Telegraph"; }
        }

        public override Telegraph GetTestTelegraph()
        {
            return null;
        }

        public override Telegraph CreateTelegraph(params object[] Args)
        {
            if (null == Args)
            {
                return null;
            }
            if (0 == Args.Length)
            {
                return null;
            }
            if (!(Args[0] is ESCommand))
            {
                return null;
            }

            return new STK500Telegraph(Args[0] as ESCommand);
        }
    }
}

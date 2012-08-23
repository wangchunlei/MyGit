using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.NotifierDeploy
{
    [Serializable]
    public enum MessageType
    {
        ToOutPut,ToApprove,ToNotify,ToUpdate,ToReset
    }
}

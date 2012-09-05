using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domas.DAP.ADF.NotifierDeploy
{
    [Serializable]
    public enum MessageType
    {
        ToSubmit, ToApprove, ToOutPut, ToRecycle, ApprovePassed, ApproveRefused, ToTransfer, ToRegister,
        ToNotify, ToUpdate, ToReset, ToLogOff
    }
}

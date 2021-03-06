/*
   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.

   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.opensource.org/licenses/ms-rl

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/


using CodeOwls.StudioShell.Paths.Items.Debugger;
using EnvDTE80;

namespace CodeOwls.StudioShell.Provider.Variables
{
    public class CurrentStackFrameVariable : DTEPSVariable
    {
        public CurrentStackFrameVariable(DTE2 dte) : base(dte, "currentStackFrame")
        {
        }

        public override object Value
        {
            get { 
                var o = _dte.Debugger.CurrentStackFrame;
                if( null == o )
                {
                    return null;
                }

                return new ShellStackFrame(o);
            }
        }
    }

    public class CurrentThreadVariable : DTEPSVariable
    {
        public CurrentThreadVariable(DTE2 dte) : base(dte, "currentThread")
        {
        }

        public override object Value
        {
            get 
            {
                if (null == _dte.Debugger)
                {
                    return null;
                }

                var o = _dte.Debugger.CurrentThread;
                if( null == o )
                {
                    return null;
                }

                return new ShellThread(o);
            }
        }
    }

    public class CurrentProcessVariable : DTEPSVariable
    {
        public CurrentProcessVariable(DTE2 dte) : base(dte, "currentProcess")
        {
            
        }

        public override object Value
        {
            get
            {
                if (null == _dte.Debugger)
                {
                    return null;
                }

                var o = _dte.Debugger.CurrentProcess as Process2;
                if (null == o)
                {
                    return null;
                }

                return new ShellProcess(o);
            }
        }
    }

    public class CurrentDebugModeVariable : DTEPSVariable
    {
        public CurrentDebugModeVariable(DTE2 dte) : base(dte, "currentDebugMode")
        {
            
        }

        public override object Value
        {
            get
            {
                if( null == _dte.Debugger)
                {
                    return null;
                }
                
                return _dte.Debugger.CurrentMode;
            }
        }
    }
}

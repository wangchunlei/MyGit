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


using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Items.Debugger
{
    public class ShellDebugger
    {
        private readonly Debugger2 _debugger;

        internal ShellDebugger(Debugger2 debugger)
        {
            _debugger = debugger;
        }

        public IEnumerable<ShellBreakpoint> Breakpoints
        {
            get
            {
                if (null != _debugger.Breakpoints)
                {
                    foreach (Breakpoint2 bp in _debugger.Breakpoints)
                    {
                        yield return new ShellBreakpoint(bp);
                    }
                }
            }
        }

        public Languages Languages
        {
            get { return _debugger.Languages; }
        }

        public dbgDebugMode CurrentMode
        {
            get { return _debugger.CurrentMode; }
        }

        public ShellProcess CurrentProcess
        {
            get { return new ShellProcess(_debugger.CurrentProcess); }
            set { _debugger.CurrentProcess = value.AsProcess(); }
        }

        public ShellThread CurrentThread
        {
            get { return new ShellThread(_debugger.CurrentThread); }
            set { _debugger.CurrentThread = value.AsThread(); }
        }

        public ShellStackFrame CurrentStackFrame
        {
            get { return new ShellStackFrame(_debugger.CurrentStackFrame); }
            set { _debugger.CurrentStackFrame = value.AsStackFrame(); }
        }

        public bool HexDisplayMode
        {
            get { return _debugger.HexDisplayMode; }
            set { _debugger.HexDisplayMode = value; }
        }

        public bool HexInputMode
        {
            get { return _debugger.HexInputMode; }
            set { _debugger.HexInputMode = value; }
        }

        public dbgEventReason LastBreakReason
        {
            get { return _debugger.LastBreakReason; }
        }

        public ShellBreakpoint BreakpointLastHit
        {
            get { return new ShellBreakpoint(_debugger.BreakpointLastHit as Breakpoint2); }
        }

        public IEnumerable<ShellBreakpoint> AllBreakpointsLastHit
        {
            get
            {
                if (null != _debugger.AllBreakpointsLastHit)
                {
                    foreach (Breakpoint2 bp in _debugger.AllBreakpointsLastHit)
                    {
                        yield return new ShellBreakpoint(bp);
                    }
                }
            }
        }

        public IEnumerable<ShellProcess> DebuggedProcesses
        {
            get
            {
                if (null != _debugger.DebuggedProcesses)
                {
                    foreach (Process p in _debugger.DebuggedProcesses)
                    {
                        yield return new ShellProcess(p);
                    }
                }
            }
        }

        public IEnumerable<ShellProcess> LocalProcesses
        {
            get
            {
                if (null != _debugger.LocalProcesses)
                {
                    foreach (Process p in _debugger.LocalProcesses)
                    {
                        yield return new ShellProcess(p);
                    }
                }
            }
        }

        public Transports Transports
        {
            get { return _debugger.Transports; }
        }

        public ShellExpression GetExpression(string ExpressionText, bool UseAutoExpandRules, int Timeout)
        {
            return new ShellExpression(_debugger.GetExpression(ExpressionText, UseAutoExpandRules, Timeout));
        }

        public void DetachAll()
        {
            _debugger.DetachAll();
        }

        public void StepInto(bool WaitForBreakOrEnd)
        {
            _debugger.StepInto(WaitForBreakOrEnd);
        }

        public void StepOver(bool WaitForBreakOrEnd)
        {
            _debugger.StepOver(WaitForBreakOrEnd);
        }

        public void StepOut(bool WaitForBreakOrEnd)
        {
            _debugger.StepOut(WaitForBreakOrEnd);
        }

        public void Go(bool WaitForBreakOrEnd)
        {
            _debugger.Go(WaitForBreakOrEnd);
        }

        public void Break(bool WaitForBreakMode)
        {
            _debugger.Break(WaitForBreakMode);
        }

        public void Stop(bool WaitForDesignMode)
        {
            _debugger.Stop(WaitForDesignMode);
        }

        public void SetNextStatement()
        {
            _debugger.SetNextStatement();
        }

        public void RunToCursor(bool WaitForBreakOrEnd)
        {
            _debugger.RunToCursor(WaitForBreakOrEnd);
        }

        public void ExecuteStatement(string Statement, int Timeout, bool TreatAsExpression)
        {
            _debugger.ExecuteStatement(Statement, Timeout, TreatAsExpression);
        }

        public void TerminateAll()
        {
            _debugger.TerminateAll();
        }

        public void WriteMinidump(string FileName, dbgMinidumpOption Option)
        {
            _debugger.WriteMinidump(FileName, Option);
        }

        public ShellExpression GetExpression2(string ExpressionText, bool UseAutoExpandRules, bool TreatAsStatement,
                                              int Timeout)
        {
            return
                new ShellExpression(_debugger.GetExpression2(ExpressionText, UseAutoExpandRules, TreatAsStatement,
                                                             Timeout));
        }
    }
}
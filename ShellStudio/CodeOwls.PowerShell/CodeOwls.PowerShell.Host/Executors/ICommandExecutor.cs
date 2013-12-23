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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace CodeOwls.PowerShell.Host.Executors
{
    public interface ICommandExecutor
    {
        CommandExecutorState CurrentState { get; }
        Collection<PSObject> Execute(string command);
        Collection<PSObject> Execute(string command, Dictionary<string, object> parameters);

        IAsyncResult BeginExecute(string command, Dictionary<string, object> parameters, ExecutionOptions options,
                                  AsyncCallback callback, object asyncState);

        Collection<PSObject> EndExecute(IAsyncResult ar);
        bool CancelCurrentExecution(int timeoutInMilliseconds);
    }
}

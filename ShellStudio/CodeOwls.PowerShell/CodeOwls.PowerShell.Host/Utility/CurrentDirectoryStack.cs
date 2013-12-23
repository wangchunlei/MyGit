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

namespace CodeOwls.PowerShell.Host.Utility
{
    public static class CurrentDirectoryStack
    {
        public static IDisposable Push(string path)
        {
            return new Session(path);
        }

        #region Nested type: Session

        private class Session : IDisposable
        {
            private readonly string _newCurrentDirectory;
            private readonly string _oldCurrentDirectory;

            public Session(string newCurrentDirectory)
            {
                _newCurrentDirectory = newCurrentDirectory;
                _oldCurrentDirectory = Environment.CurrentDirectory;
                Environment.CurrentDirectory = _newCurrentDirectory;
            }

            #region IDisposable Members

            public void Dispose()
            {
                Environment.CurrentDirectory = _oldCurrentDirectory;
            }

            #endregion
        }

        #endregion
    }
}

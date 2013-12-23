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
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Provider.Variables
{
    public class DTEEventSource
    {
        private DebuggerEvents _debuggerEvents;
        private DebuggerExpressionEvaluationEvents _debuggerExpressionEvaluationEvents;
        private BuildEvents _buildEvents;
        private DTEEvents _dteEvents;
        private FindEvents _findEvents;
        private ProjectItemsEvents _miscFileEvents;
        private ProjectItemsEvents _projectItemsEvents;
        private ProjectsEvents _projectEvents;
        private PublishEvents _publishEvents;
        private SelectionEvents _selectionEvents;
        private SolutionEvents _solutionEvents;
        private ProjectItemsEvents _solutionItemEvents;
        private DebuggerProcessEvents _debuggerProcessEvents;

        public DTEEventSource( Events2 events)
        {
            _buildEvents = events.BuildEvents;
            _dteEvents = events.DTEEvents;
            _debuggerEvents = events.DebuggerEvents;
            _debuggerProcessEvents = events.DebuggerProcessEvents;
            _debuggerExpressionEvaluationEvents = events.DebuggerExpressionEvaluationEvents;
            _findEvents = events.FindEvents;
            _miscFileEvents = events.MiscFilesEvents;
            _projectItemsEvents = events.ProjectItemsEvents;
            _projectEvents = events.ProjectsEvents;
            _publishEvents = events.PublishEvents;
            _selectionEvents = events.SelectionEvents;
            _solutionEvents = events.SolutionEvents;
            _solutionItemEvents = events.SolutionItemsEvents;            
        }

        public DebuggerProcessEvents DebuggerProcessEvents
        {
            get { return _debuggerProcessEvents; }
        }

        public ProjectItemsEvents SolutionItemEvents
        {
            get { return _solutionItemEvents; }
        }

        public SolutionEvents SolutionEvents
        {
            get { return _solutionEvents; }
        }

        public SelectionEvents SelectionEvents
        {
            get { return _selectionEvents; }
        }

        public PublishEvents PublishEvents
        {
            get { return _publishEvents; }
        }

        public ProjectsEvents ProjectEvents
        {
            get { return _projectEvents; }
        }

        public ProjectItemsEvents ProjectItemsEvents
        {
            get { return _projectItemsEvents; }
        }

        public ProjectItemsEvents MiscFileEvents
        {
            get { return _miscFileEvents; }
        }

        public FindEvents FindEvents
        {
            get { return _findEvents; }
        }

        public DTEEvents DteEvents
        {
            get { return _dteEvents; }
        }

        public BuildEvents BuildEvents
        {
            get { return _buildEvents; }
        }

        public DebuggerExpressionEvaluationEvents DebuggerExpressionEvaluationEvents
        {
            get { return _debuggerExpressionEvaluationEvents; }
        }

        public DebuggerEvents DebuggerEvents
        {
            get { return _debuggerEvents; }
        }
    }
}

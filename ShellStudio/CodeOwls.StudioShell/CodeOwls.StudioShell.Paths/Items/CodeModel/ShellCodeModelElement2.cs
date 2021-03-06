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
using System.Linq;
using CodeOwls.StudioShell.Paths.Items.ProjectModel;
using EnvDTE;
using EnvDTE80;

namespace CodeOwls.StudioShell.Paths.Items.CodeModel
{
    public class ShellCodeModelElement2 : IShellCodeModelElement2
    {
        private readonly CodeElement _element;

        internal ShellCodeModelElement2(CodeElement element)
        {
            _element = element;
        }

        public virtual bool IsContainer
        {
            get { return true; }
        }

        public virtual string Name
        {
            get { return _element.Name; }
            set { _element.Name = value; }
        }

        public virtual string FullName
        {
            get { return _element.FullName; }
        }

        public ShellProjectItem ProjectItem
        {
            get { return new ShellProjectItem(_element.ProjectItem); }
        }

        public vsCMElement Kind
        {
            get { return _element.Kind; }
        }

        public bool IsCodeType
        {
            get { return _element.IsCodeType; }
        }

        public vsCMInfoLocation InfoLocation
        {
            get { return _element.InfoLocation; }
        }

        public string Language
        {
            get { return _element.Language; }
        }

        public TextPoint StartPoint
        {
            get { return _element.StartPoint; }
        }

        public TextPoint EndPoint
        {
            get { return _element.EndPoint; }
        }

        protected IEnumerable<IShellCodeModelElement2> GetEnumerator(CodeElements codeElements)
        {
            return from CodeElement ce in codeElements
                   select ShellObjectFactory.CreateFromCodeElement(ce);
        }

        public TextPoint GetStartPoint(vsCMPart Part)
        {
            return _element.GetStartPoint(Part);
        }

        public TextPoint GetEndPoint(vsCMPart Part)
        {
            return _element.GetEndPoint(Part);
        }

        public void RenameSymbol(string NewName)
        {
            var e2 = _element as CodeElement2;
            if (null == e2)
            {
                return;
            }
            e2.RenameSymbol(NewName);
        }

        public void Activate()
        {
            var point = _element.StartPoint;
            point.Parent.Parent.Activate();
            var offset = point.AbsoluteCharOffset;
            point.Parent.Selection.MoveToAbsoluteOffset( offset, false );
        }
    }
}
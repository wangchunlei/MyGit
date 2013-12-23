#
#   Copyright (c) 2011 Code Owls LLC, All Rights Reserved.
#
#   Licensed under the Microsoft Reciprocal License (Ms-RL) (the "License");
#   you may not use this file except in compliance with the License.
#   You may obtain a copy of the License at
#
#     http://www.opensource.org/licenses/ms-rl
#
#   Unless required by applicable law or agreed to in writing, software
#   distributed under the License is distributed on an "AS IS" BASIS,
#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#   See the License for the specific language governing permissions and
#   limitations under the License.
#
. ./helpers.ps1

Describe "Solution/CodeModel/<project>/<file>" {
    
    new-solution;
    new-item dte:/solution/projects/P -type classlibrary -language csharp | out-null;
    new-item dte:/solution/projects/P/C.cs -type class | out-null;
    
    $root = "dte:/solution/codemodel/P/C.cs";

    It "new-item namespace" {
        
        verify { new-item -path $root/N -type namespace }
        
        assert { test-path $root/N }    
    
    } 

    It "new-item using" {
        verify { new-item -path $root -name 'System.IO' -position 0 -type import }
        assert{ test-path "$root/Import System.IO" }
    }

}

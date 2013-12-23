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

Describe "Solution/CodeModel/../<namespace>" {
    
    new-solution;
    new-item dte:/solution/projects/P -type classlibrary -language csharp | out-null;
    new-item dte:/solution/projects/P/C.cs -type class | out-null;
    new-item dte:/solution/codemodel/P/C.cs/N -type namespace | out-null;

    $root = "dte:/solution/codemodel/P/C.cs/N";

    It "new-item namespace" {
        
        verify { new-item -path $root/N1 -type namespace }
        
        assert { test-path $root/N1 }        
    } 

    It "new-item internal class" {
        verify { new-item -path $root -name "MyClass" -type class }

        assert { test-path "$root/MyClass" }
    }

    It "new-item public class" {
        verify { new-item -path $root -name "EveryonesClass" -access public -type class }

        assert { test-path "$root/EveryonesClass" }
    }

    It "new-item delegate" {
        verify { new-item -path $root -name "MyDelegate" -memberType 'void' -type delegate }

        assert { test-path "$root/MyDelegate" }
    }
    
    It "errors on new-item using" {
        assert-error { get-asdfwae } #new-item -path $root -name 'System.IO' -position 0 -type import }
    }    
}

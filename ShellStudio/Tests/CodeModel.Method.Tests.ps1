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

Describe "Solution/CodeModel/../<method>" {
    
    new-solution;
    new-item dte:/solution/projects/P -type classlibrary -language csharp | out-null;
    new-item dte:/solution/projects/P/C.cs -type class | out-null;
    new-item dte:/solution/codemodel/P/C.cs/N -type namespace | out-null;
    new-item dte:/solution/codemodel/P/C.cs/N/C -type class | out-null;
    new-item dte:/solution/codemodel/p/c.cs/N/C/M -type method | out-null;
    $root = "dte:/solution/codemodel/P/C.cs/N/C/M";

    It "new-item parameter" {
        verify { new-item -path $root/A -type parameter -membertype 'string' }
        assert { test-path -path $root/A }
    }

    It "new-item positional parameter" {
        verify { new-item -path $root/B -type parameter -membertype 'string' }
        verify { new-item -path $root/C -type parameter -position 1 -membertype int }
        assert { test-path -path $root/C }
    }
    
    It "new-item attribute" {
        verify { new-item -path $root -type attribute -name 'Serializable' }
        assert { test-path $root/Serializable }
    }

    It "errors on new-item method" {
        assert-error { new-item -path $root/MyMethod -membertype 'string' -type Method }
    }

    It "errors on new-item property" {
        assert-error { new-item -path $root/MyItem -membertype 'string' -get -set -type Property }
    }

    It "errors on new-item event" {
        assert-error { new-item -path $root/MyPublicEvent -type event -access public -membertype 'EventHandler' }
    }
 
    It "error on new-item class" {
        assert-error { new-item -path $root -name "MyClass" -type class }
    }

 
    It "errors on new-item interface" {
        assert-error { new-item -path $root -name "IInterface" -access private -type interface }
    }

    It "errors on new-item delegate" {
        assert-error { new-item -path $root -name "MyDelegate" -memberType 'void' -type delegate }
    }
    
    It "errors on new-item using" {
        assert-error { new-item -path $root -name 'System.IO' -position 0 -type import }
    }
    
    It "errors on new-item namespace" {        
        assert-error { new-item -path $root/N1 -type namespace }
    }     
}

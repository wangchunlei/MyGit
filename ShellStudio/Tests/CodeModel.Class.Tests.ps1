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

Describe "Solution/CodeModel/../<class>" {
    
    new-solution;
    new-item dte:/solution/projects/P -type classlibrary -language csharp | out-null;
    new-item dte:/solution/projects/P/C.cs -type class | out-null;
    new-item dte:/solution/codemodel/P/C.cs/N -type namespace | out-null;
    new-item dte:/solution/codemodel/P/C.cs/N/C -type class | out-null;

    $root = "dte:/solution/codemodel/P/C.cs/N/C";

    It "new-item void method" {
        verify { new-item -path $root/MyVoidMethod -type Method -membertype 'void' }
        assert { test-path -path $root/MyVoidMethod }
    }

    It "new-item typed method" {
        verify { new-item -path $root/MyMethod -membertype 'string' -type Method }
        assert { test-path -path $root/MyMethod }
    }

    It "new-item property" {
        verify { new-item -path $root/MyItem -membertype 'string' -get -set -type Property }
        assert { test-path -path $root/MyItem }
    }

    It "new-item internal property" {
        verify { new-item -path $root/MyInternalItem -membertype 'string' -access internal -get -set -type Property }
        assert { test-path -path $root/MyInternalItem }
    }

    It "new-item settable property" {
        verify { new-item -path $root/MySetItem -membertype 'int' -set -type Property }
        assert { 
            ( test-path -path $root/MySetItem ) -and (( gi $root/mysetitem ).canset) -and -not (( gi $root/mysetitem ).canget) 
        }
    }

    It "new-item gettable property" {
        verify { new-item -path $root/MyGetItem -membertype 'System.IO.IStream' -get -type Property }
        assert { 
            ( test-path -path $root/MyGetItem ) -and ( gi $root/mygetitem ).canget -and -not ( gi $root/mygetitem ).canset 
        }
    }
    
    It "new-item private event" {
        verify { new-item -path $root/MyEvent -type event -membertype 'EventHandler' }

        assert { test-path $root/MyEvent }
    }

    It "new-item internal event" {
        verify { new-item -path $root/MyInternalEvent -type event -access internal -membertype 'EventHandler' }

        assert { test-path $root/MyInternalEvent }
    }

    It "new-item public event" {
        verify { new-item -path $root/MyPublicEvent -type event -access public -membertype 'EventHandler' }

        assert { test-path $root/MyPublicEvent }
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
    
    It "errors on new-item interface" {
        assert-error { new-item -path $root -name "IInterface" -access private -type interface }
    }

    It "errors on new-item using" {
        assert-error { new-item -path $root -name 'System.IO' -position 0 -type import }
    }
    
    It "errors on new-item namespace" {
        
        assert-error { new-item -path $root/N1 -type namespace }
    }     
}

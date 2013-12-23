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

Describe "Special Help Items" {
    It "commandbars do not promote combobox" {
        $h = get-help -path dte:/commandbars/menubar new-item -full | out-string
        
        assert { $h -notmatch 'combobox' }
    }
}

Describe "Provider Help" {

    $newitem = @{
        'dte:/solution/projects' = 'new project or solution folder';
        'dte:/commandbars' = 'new command bar';
        'dte:/commandbars/menubar' = 'new control';
        'dte:/debugger/breakpoints' = 'new breakpoint';
        'dte:/Commands' = 'new command';
        'dte:/errors' = 'new error';
        'dte:/outputpanes' = 'new output pane';
        'dte:/Solution\CodeModel' = 'new project or solution folder';
        'dte:/windowconfigurations' = 'new window configuration'

    };

    $newitem.keys | %{
        It "new-item $_" {
            $path = $_;
            $h = get-help -path $_ new-item;
            assert { $h.synopsis -match $newitem[$_]}            
        }
    }

    $setitem = @{
        'WindowConfigurations\Design'= 'updates.+window configuration'

    };

    $setitem.keys | %{
        It "set-item $_" {
            $path = $_;
            $h = get-help -path $_ set-item;
            assert { $h.synopsis -match $setitem[$_]}            
        }
    }

    $invokeitem = @{
    'OutputPanes\Build' = '';
    'WindowConfigurations\Debug' = '';
    'Windows\Output' = ''
    }

    $invokeitem.keys | %{
        It "invoke-item $_" {
            $path = $_;
            $h = get-help -path $_ invoke-item;
            assert { $h.synopsis -match $setitem[$_]}            
        }
    }


    <#
    $sh = get-help new-item;    
    ls dte: -rec | where { $_.ssitemmode -match '\+' } | where { $_ -notmatch 'commandbars' } | select -exp PSPath | %{
        $path = $_ -replace '.+::','';
        It "new item $path" {
            assert{ $sh -ne ( get-help -path $path new-item ) }
        }
    }
    #>
}

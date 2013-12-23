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

Describe "WindowConfigurations" {
    
    delete-solution;

    $root = 'dte:/windowconfigurations'
    
    It "new-item create new configuration" {
        $name = (get-randomname 'a');
        
        verify {
            new-item -path $root/$name 
        }
        
        assert {
            test-path "$root/$name"
        }
    }

    It "invoke-item recalls existing configuration" {
        $name = get-randomname 'W';

        arrange { ( get-item dte:/windows/output ).Visible = $true; }
        verify { new-item -path $root/$name }

        arrange { 
            ( get-item dte:/windows/output ).Visible = $false; 
            invoke-item -path $root/$name 
        }
        
        assert { ( get-item dte:/windows/output ).Visible }
    }

    It "set-item updates existing configuration" {
        $name = get-randomname 'W';

        arrange { ( get-item dte:/windows/output ).Visible = $true; }
        verify { new-item -path $root/$name }

        arrange { 
            ( get-item dte:/windows/output ).Visible = $false; 
            set-item -path $root/$name;
            ( get-item dte:/windows/output ).Visible = $true;
            invoke-item -path $root/$name;
        }
        
        assert { -not( ( get-item dte:/windows/output ).Visible ) }            
    }

    It "set-item -force creates nonexisting configuration" {
        $name = get-randomname 'r';
        verify { set-item $root/$name -force }
        assert { test-path $root/$name }
    }

    It "remove-item deletes configuration" {
        $name = get-randomname 'zceaef'
        arrange{
            new-item $root/$name;
            test-path $root/$name;
        }

        verify { remove-item $root/$name }

        assert { -not (test-path $root/$name) }
    }

   
    It "errors on set-item for nonexisting configuration" {
        assert-error { new-item "dte:/windowconfigurations/$(get-randomname 'lasf')" } 
    }

    It "errors on new-item for existing configuration" {
        $name = get-randomname 'z';
        arrange { new-item dte:/windowconfigurations/$name }

        assert-error { new-item dte:/windowconfigurations/$name }
    }

    It "errors on remove-item for nonexistent configuration" {
        assert-error { remove-item "dte:/windowconfigurations/$(new-randomname 'CzCz')" }
    }
    
}

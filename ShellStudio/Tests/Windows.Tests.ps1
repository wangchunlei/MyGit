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

    $root = 'dte:/windows'
    
    It "errors on new-item" {
        $name = (get-randomname 'a');
        
        assert-error { new-item -path $root/$name }
    }

    It "invoke-item focuses window" {
        arrange { 
            ( get-item dte:/windows/output ).Visible = $false; 
            invoke-item dte:/windows/output 
        }
        assert { ( get-item dte:/windows/output ).Visible -eq $true;  }
    }
   
}

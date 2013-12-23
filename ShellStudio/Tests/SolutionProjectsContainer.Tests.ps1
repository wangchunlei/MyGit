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

Describe "Solution/Projects" {

    new-solution;

    It "new-item create new projects" {
        $name = get-randomname
        new-item -path dte:/solution/projects/$name -type ClassLibrary | out-null
        test-path dte:/solution/projects/$name
    }
    
    It "new-item create new folders" {
        $name = get-randomname
        new-item dte:/solution/projects/$name -type folder | out-null
        test-path dte:/solution/projects/$name
    }

    It "remove-item existing projects" {
        $name = get-randomname
        new-item dte:/solution/projects -type classlibrary -language csharp -name $name | out-null
        
        assert { test-path dte:/solution/projects/$name } | out-null;
        
        remove-item dte:/solution/projects/$name -recurse -force;
        
        -not( test-path dte:/solution/projects/$name )
    }

    It "remove-item existing folders" {
        $name = get-randomname
        new-item dte:/solution/projects/a -type folder | out-null
        
        remove-item dte:/solution/projects/a -recurse -force;
        -not( test-path dte:/solution/projects/a )
    }

    It "get-childitem list project items" {
        $name = get-randomname
        new-item -path dte:/solution/projects/$name -type ClassLibrary | out-null
        $items = dir dte:/solution/projects/$name;
        $items.length -ne 0;        
    }       

}

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

Describe "Solution/Projects/<folder>" {
    
    new-solution;
    function new-folder {
        $fn = get-randomname;
        new-item -path "dte:/solution/projects/$fn" -type folder | out-null;
        "dte:/solution/projects/$fn"
    }
    
    It "new-item create new subprojects" {
        $name = (get-randomname);
        $folder = new-folder;
        write-debug $folder;    
        arrange {
            new-item -path $folder/$name -type classlibrary
        }
        
        assert {
            test-path "$folder/$name"
        }

    }

    It "new-item create new folders" {
        $name = (get-randomname);
        $folder = new-folder;
        arrange {
            new-item -path $folder -name $name -type folder
        }
        
        assert {
            test-path "$folder/$name"
        }
    }
    
    It "new-item create new class" {
        $name = (get-randomname);
        $folder = new-folder;
        verify {
            new-item -path $folder -name $name -type 'class'
        }
        
        assert {
            test-path "$folder/$name"
        }
    }

    It "new-item add existing file" {
        $folder = new-folder;
        $f = ls | where { ! $_.psIsContainer } | select -first 1 | select -exp fullname;
        $n = $f.name;
        verify { new-item -path $folder -itemfilepath $f }
        assert { test-path $folder/$n }
    }
    
    It "remove-item project items" {
        $name = (get-randomname);
        $folder = new-folder;
        arrange {
            new-item -path $folder -name $name -type 'textfile'
            assert { test-path "$folder/$name" }
            remove-item -path $folder/$name -recurse -force;
        }
        
        assert {
            -not( test-path "$folder/$name" )
        }
    }

    <#
    this one throws 
    It "should support remove-item for existing folders" {
        
        
        arrange {
            new-item -path dte:/solution/projects/a -type folder | out-null
            new-item -path dte:/solution/projects/a/b -type folder | out-null
            #assert { test-path dte:/solution/projects/a/b }
            remove-item -path dte:/solution/projects/a/b -recurse -force;
        }
        
        assert {
            -not( test-path dte:/solution/projects/a/b )
        }
    }#>
    
}

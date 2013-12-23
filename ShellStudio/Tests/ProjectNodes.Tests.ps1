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

Describe "Solution/Projects/<project>" {

    new-solution;
    $project = ( new-project ).pspath;

    "Class","ClassDiagram","CodeFile","TextFile","XMLFile","XMLSchema","XSLTFile" | %{
        It "new-item $($_)" {
            $name = get-randomname;
            verify { new-item -path $project/$_ -type $_ -language csharp }
            
            assert{ test-path $project/$_ } 
        }

        sleep -secon 2
    }
       
    It "new-item folder" {
        $name = get-randomname
        new-item -path $project/$name -type folder | out-null
        assert{test-path $project/$name}
    }
    
    It "new-item has custom help" {
        $h = get-help -path $project -full| out-string;
        $sh = get-help new-item -full;
        assert { $h -ne $sh }
    }
        
    It "remove-item folder -force" {
        $name = get-randomname
        new-item -path $project/$name -type folder | out-null
        assert{ test-path $project/$name} | out-null
        
        remove-item -path $project/$name -recurse -force;
        
        assert{ -not( test-path $project/$name ) } 
    }   
    
    It "remove-item folder" {
        $name = get-randomname
        new-item -path $project/$name -type folder | out-null
        assert{ test-path $project/$name} | out-null
        
        remove-item -path $project/$name -recurse;
        
        assert{ -not( test-path $project/$name ) } 
    }   
    
    It "remove-item code file" {
        $name = get-randomname
        assert{ new-item -path $project/$name.cs -type codefile -language csharp } | out-null
        
        remove-item $project/$name.cs -recurse;
        assert-false {test-path $project/$name.cs}
    }
    
    It "rename-item project" {
        $name = get-randomname
        
        rename-item $project -newname $name;
        assert { -not( test-path $project ) -and ( test-path dte:/solution/projects/$name ) }
        
    }
    

}


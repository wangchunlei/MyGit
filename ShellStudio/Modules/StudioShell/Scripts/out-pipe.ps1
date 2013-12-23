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


[CmdletBinding()]
param(
	[string] 
	[parameter( Mandatory=$false )]
	# the name of the output pipe where output will be sent; defaults to 'StudioShell'
	$name = 'StudioShell',
	
	[string] 
	[parameter( Mandatory=$false )]
	# the name of the output pipe where output will be sent; defaults to 'StudioShell'
	$endOfStream = 'EOS',

	[string]
	[parameter( mandatory=$true, valueFromPipeline=$true )]
	# the string to write to the output pane
	$inputObject
);

$script:pipe;
$script:writer;
begin
{
	add-type -assembly system.core | out-null; 
	$script:pipe = new-object System.IO.Pipes.NamedPipeClientStream($name)
	$script:pipe.Connect();

	$script:writer = new-object System.IO.StreamWriter -arg $script:pipe;
}
process
{
	$script:writer.WriteLine( $inputObject.ToString() );
}
end
{
	$script:writer.WriteLine( $endOfStream );
	$script:writer.Dispose();
	
	$script:pipe.Close();
	$script:pipe.Dispose();
	$script:pipe = $null;
}
